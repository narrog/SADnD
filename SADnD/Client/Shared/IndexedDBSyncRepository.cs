using BlazorDB;
using Microsoft.JSInterop;
using SADnD.Shared;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SADnD.Client.Services;

namespace SADnD.Client.Shared
{
    public class IndexedDBSyncRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        IBlazorDbFactory _dbFactory;
        private readonly APIRepository<TEntity> _apiRepository;
        private readonly IJSRuntime _jsruntime;
        string _dbName = "";
        //string _primaryKeyName = "";
        //bool _autoGenerateKey;

        IndexedDbManager manager;
        string storeName = "";
        Type entityType;
        PropertyInfo primaryKey;
        public bool IsOnline { get; set; } = true;

        public delegate void OnlineStatusEventHandler(object sender, OnlineStatusEventArgs e);
        public event OnlineStatusEventHandler OnlineStatusChanged;

        public IndexedDBSyncRepository(string dbName, IBlazorDbFactory dbFactory,
            APIRepository<TEntity> apiRepository, IJSRuntime jsRuntime)
        {
            _dbName = dbName;
            _dbFactory = dbFactory;
            _apiRepository = apiRepository;
            _jsruntime = jsRuntime;

            entityType = typeof(TEntity);
            storeName = entityType.Name;
            primaryKey = entityType.GetProperty("Id");

            _ = _jsruntime.InvokeVoidAsync("connectivity.initialize", DotNetObjectReference.Create(this));
        }

        public string KeyStoreName
        {
            get { return $"{storeName}{Globals.KeysSuffix}"; }
        }
        public string LocalStoreName
        {
            get { return $"{storeName}{Globals.LocalTransactionsSuffix}"; }
        }

        [JSInvokable("ConnectivityChanged")]
        public async void OnConnectivityChanged(bool isOnline)
        {
            IsOnline = isOnline;

            if (!isOnline)
                OnlineStatusChanged?.Invoke(this, new OnlineStatusEventArgs { IsOnline = false });
            else
            {
                await SyncLocalToServer();
                OnlineStatusChanged?.Invoke(this, new OnlineStatusEventArgs { IsOnline = true });
            }
        }

        private async Task EnsureManager()
        {
            if (manager == null)
            {
                manager = await _dbFactory.GetDbManager(_dbName);
                await manager.OpenDb();
            }
        }

        public async Task<bool> Delete(TEntity entityToDelete)
        {
            bool deleted = false;
            if (IsOnline)
            {
                var onlineId = primaryKey.GetValue(entityToDelete);
                deleted = await _apiRepository.Delete(entityToDelete);
                var localEntity = await UpdateKeyToLocal(entityToDelete);
                await DeleteOffline(localEntity);
            }
            else
                deleted = await DeleteOffline(entityToDelete);
            return deleted;
        }
        public async Task<bool> DeleteOffline(TEntity entityToDelete)
        {
            await EnsureManager();
            var Id = primaryKey.GetValue(entityToDelete);
            return await Delete(Id);
        }
        public async Task<bool> Delete(object id)
        {
            bool deleted = false;
            if (IsOnline)
            {
                var localId = await GetLocalId(id);
                await DeleteOffline(localId);
                deleted = await _apiRepository.Delete(id);
            }
            else
                deleted = await DeleteOffline(id);
            return deleted;
        }

        public async Task<bool> DeleteOffline(object id)
        {
            await EnsureManager();
            try
            {
                RecordDelete(id);
                var result = await manager.DeleteRecordAsync(storeName, id);
                if (result.Failed)
                    return false;

                if (IsOnline)
                {
                    var keys = await GetKeys();
                    if (keys.Count > 0)
                    {
                        var key = keys.Where(x => x.LocalId.ToString() == id.ToString()).FirstOrDefault();
                        if (key != null)
                            await manager.DeleteRecordAsync(KeyStoreName, key.Id);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                // TODO: Log exception
                return false;
            }
        }
        public async void RecordDelete(object id)
        {
            if (IsOnline)
                return;
            var action = LocalTransactionTypes.Delete;
            var entity = await GetByID(id);
            var record = new StoreRecord<LocalTransaction<TEntity>>()
            {
                StoreName = LocalStoreName,
                Record = new LocalTransaction<TEntity>()
                {
                    Entity = entity,
                    Action = action,
                    ActionName = action.ToString(),
                    Id = int.Parse(id.ToString())
                }
            };
            await manager.AddRecordAsync(record);
        }
        private async Task ClearLocalDB()
        {
            await EnsureManager();
            await manager.ClearTableAsync(KeyStoreName);
            await manager.ClearTableAsync(storeName);
        }
        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await GetAll(false);
        }
        public async Task<IEnumerable<TEntity>> GetAll(bool dontSync = false)
        {
            if (IsOnline)
            {
                var list = (await _apiRepository.GetAll()).ToList();
                if (list != null)
                {
                    if (!dontSync)
                    {
                        await ClearLocalDB();
                        var result = await manager.BulkAddRecordAsync(storeName, list);
                        var localList = (await GetAllOffline(false)).ToList();
                        var keys = new List<OnlineOfflineKey>();
                        for (int i = 0; i < list.Count(); i++)
                        {
                            var localId = primaryKey.GetValue(localList[i]);
                            keys.Add(new OnlineOfflineKey()
                            {
                                Id = Convert.ToInt32(localId),
                                OnlineId = primaryKey.GetValue(list[i]),
                                LocalId = localId
                            });
                        }
                        await manager.ClearTableAsync(KeyStoreName);
                        result = await manager.BulkAddRecordAsync(KeyStoreName, keys);
                    }
                    return list;
                }
                return null;
            }
            else
                return await GetAllOffline(true);
        }
        public async Task<IEnumerable<TEntity>> GetAllOffline(bool onlineKeys)
        {
            await EnsureManager();
            var array = await manager.ToArray<TEntity>(storeName);
            //Console.WriteLine(JsonConvert.SerializeObject(array));
            if (array == null)
                return new List<TEntity>();
            else
            {
                if (onlineKeys)
                {
                    foreach (var entity in array)
                    {
                        await UpdateKeyFromLocal(entity);
                    }
                }
                return array.ToList();
            }
        }
        public async Task<TEntity> GetByID(object id)
        {
            if (IsOnline)
                return await _apiRepository.GetByID(id);
            else
                return await GetByIDOffline(id);
        }
        public async Task<TEntity> GetByIDOffline(object id)
        {
            await EnsureManager();
            var localId = await GetLocalId(id);
            var items = await manager.Where<TEntity>(storeName, "Id", localId);
            if (items.Any())
                return items.First();
            else
                return null;
        }
        public async Task<TEntity> Insert(TEntity entity)
        {
            TEntity returnValue;
            if (IsOnline)
            {
                returnValue = await _apiRepository.Insert(entity);
                await InsertOffline(returnValue);
            }
            else
                returnValue = await InsertOffline(entity);
            return returnValue;
        }
        public async Task<TEntity> InsertOffline(TEntity entity)
        {
            await EnsureManager();

            try
            {
                var onlineId = primaryKey.GetValue(entity);
                var record = new StoreRecord<TEntity>()
                {
                    StoreName = storeName,
                    Record = entity
                };
                var result = await manager.AddRecordAsync(record);
                var allItems = await GetAllOffline(false);
                var last = allItems.Last();
                var localId = primaryKey.GetValue(last);

                var key = new OnlineOfflineKey()
                {
                    Id = Convert.ToInt32(localId),
                    OnlineId = onlineId,
                    LocalId = localId,
                };
                var storeRecord = new StoreRecord<OnlineOfflineKey>()
                {
                    DbName = _dbName,
                    StoreName = KeyStoreName,
                    Record = key
                };
                await manager.AddRecordAsync(storeRecord);

                RecordInsert(last);

                return last;
            }
            catch (Exception ex)
            {
                // TODO: Log Exception
                return null;
            }
        }
        public async void RecordInsert(TEntity entity)
        {
            if (IsOnline)
                return;
            try
            {
                var action = LocalTransactionTypes.Insert;
                var record = new StoreRecord<LocalTransaction<TEntity>>()
                {
                    StoreName = LocalStoreName,
                    Record = new LocalTransaction<TEntity>()
                    {
                        Entity = entity,
                        Action = action,
                        ActionName = action.ToString()
                    }
                };
                await manager.AddRecordAsync(record);
            }
            catch (Exception ex)
            {
                // TODO: Log exception
            }
        }
        public async Task<TEntity> Update(TEntity entityToUpdate)
        {
            TEntity localEntity;
            if (IsOnline)
            {
                entityToUpdate = await _apiRepository.Update(entityToUpdate);
                localEntity = JsonConvert.DeserializeObject<TEntity>(JsonConvert.SerializeObject(entityToUpdate));
                localEntity = await UpdateKeyToLocal(localEntity);
                await UpdateOffline(localEntity);
            }
            else
                await UpdateOffline(entityToUpdate);
            return entityToUpdate;
        }
        public async Task<TEntity> UpdateOffline(TEntity entityToUpdate)
        {
            await EnsureManager();
            object id = primaryKey.GetValue(entityToUpdate);
            try
            {
                await manager.UpdateRecord(new UpdateRecord<TEntity>()
                {
                    StoreName = storeName,
                    Record = entityToUpdate,
                    Key = id
                });
                RecordUpdate(entityToUpdate);
                return entityToUpdate;
            }
            catch (Exception ex)
            {
                // TODO: Log exception
                return null;
            }
        }
        public async void RecordUpdate(TEntity entity)
        {
            if (IsOnline)
                return;
            try
            {
                var action = LocalTransactionTypes.Update;
                var record = new StoreRecord<LocalTransaction<TEntity>>()
                {
                    StoreName = LocalStoreName,
                    Record = new LocalTransaction<TEntity>()
                    {
                        Entity = entity,
                        Action = action,
                        ActionName = action.ToString()
                    }
                };
                await manager.AddRecordAsync(record);
            }
            catch (Exception ex)
            {
                // TODO: Log exception
            }
        }
        public async Task<bool> SyncLocalToServer()
        {
            if (!IsOnline)
                return false;
            await EnsureManager();
            var array = await manager.ToArray<LocalTransaction<TEntity>>(LocalStoreName);
            if (array == null || array.Count == 0)
                return true;
            else
            {
                foreach (var localTransaction in array.ToList())
                {
                    try
                    {
                        switch (localTransaction.Action)
                        {
                            case LocalTransactionTypes.Insert:
                                var insertedEntity = await _apiRepository.Insert(localTransaction.Entity);
                                var localId = primaryKey.GetValue(localTransaction.Entity);
                                var onlineId = primaryKey.GetValue(insertedEntity);
                                var key = new OnlineOfflineKey()
                                {
                                    Id = Convert.ToInt32(localId),
                                    OnlineId = onlineId,
                                    LocalId = localId
                                };
                                await manager.AddRecordAsync(new StoreRecord<OnlineOfflineKey>()
                                {
                                    StoreName = KeyStoreName,
                                    Record = key
                                });
                                break;
                            case LocalTransactionTypes.Update:
                                localTransaction.Entity = await UpdateKeyFromLocal(localTransaction.Entity);
                                await _apiRepository.Update(localTransaction.Entity);
                                break;
                            case LocalTransactionTypes.Delete:
                                localTransaction.Entity = await UpdateKeyFromLocal(localTransaction.Entity);
                                await _apiRepository.Delete(localTransaction.Entity);
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        // TODO: Log exception
                    }
                }
                await DeleteAllTransactions();
                // TODO: Get all new records since last online, Clear and GetAll is wildly ineficient in bigger DBs
                ClearLocalDB();
                GetAll();
                return true;
            }
        }
        private async Task DeleteAllTransactions()
        {
            await EnsureManager();
            await manager.ClearTableAsync(LocalStoreName);
        }
        private async Task<object> GetLocalId(object onlineId)
        {
            var keys = await GetKeys();
            var key = keys.Where(x => x.OnlineId.ToString() == onlineId.ToString()).FirstOrDefault();
            return key.LocalId;
        }
        private async Task<object> GetOnlineId(object localId)
        {
            var keys = await GetKeys();
            var key = keys.Where(x => x.LocalId.ToString() == localId.ToString()).FirstOrDefault();
            return key.OnlineId;
        }
        private async Task<List<OnlineOfflineKey>> GetKeys()
        {
            await EnsureManager();
            return (await manager.ToArray<OnlineOfflineKey>(KeyStoreName)).ToList();
        }
        private async Task<TEntity> UpdateKeyToLocal(TEntity entity)
        {
            var onlineId = primaryKey.GetValue(entity);
            var keys = await GetKeys();
            if (keys == null)
                return null;
            var item = keys.Where(x => x.OnlineId.ToString() == onlineId.ToString()).FirstOrDefault();
            if (item == null)
                return null;
            var key = item.LocalId;
            key = JsonConvert.DeserializeObject<object>(key.ToString());
            var typeName = key.GetType().Name;
            if (typeName == nameof(Int64))
            {
                if (primaryKey.PropertyType.Name == nameof(Int32))
                    key = Convert.ToInt32(key);
            }
            else if (typeName == "String")
            {
                if (primaryKey.PropertyType.Name != "String")
                    key = key.ToString();
            }
            primaryKey.SetValue(entity, key);
            return entity;
        }
        private async Task<TEntity> UpdateKeyFromLocal(TEntity entity)
        {
            var localId = primaryKey.GetValue(entity);
            var keys = await GetKeys();
            if (keys == null)
                return null;
            var item = keys.Where(x => x.LocalId.ToString() == localId.ToString()).FirstOrDefault();
            if (item == null)
                return null;
            var key = item.OnlineId;
            key = JsonConvert.DeserializeObject<object>(key.ToString());
            var typeName = key.GetType().Name;
            if (typeName == nameof(Int64))
            {
                if (primaryKey.PropertyType.Name == nameof(Int32))
                    key = Convert.ToInt32(key);
            }
            else if (typeName == "String")
            {
                if (primaryKey.PropertyType.Name != "String")
                    key = key.ToString();
            }
            primaryKey.SetValue(entity, key);
            return entity;
        }
    }
}
