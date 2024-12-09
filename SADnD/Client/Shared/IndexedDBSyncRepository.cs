using BlazorDB;
using Microsoft.JSInterop;
using SADnD.Shared;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SADnD.Client.Services;
using System.Xml;
namespace SADnD.Client.Shared
{
    public class IndexedDBSyncRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        IBlazorDbFactory _dbFactory;
        private readonly APIRepository<TEntity> _apiRepository;
        private readonly IJSRuntime _jsruntime;
        string _dbName = "";

        IndexedDbManager manager;
        string storeName = "";
        Type entityType;
        PropertyInfo primaryKey;
        public bool IsOnline { get; set; } = false;

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
                deleted = await _apiRepository.Delete(entityToDelete);
                await DeleteOffline(entityToDelete);
            }
            else
                deleted = await DeleteOffline(entityToDelete);
            return deleted;
        }
        public async Task<bool> DeleteOffline(TEntity entityToDelete)
        {
            await EnsureManager();
            return await DeleteOffline(primaryKey.GetValue(entityToDelete));
        }
        public async Task<bool> Delete(object id)
        {
            bool deleted = false;
            if (IsOnline)
            {
                deleted = await _apiRepository.Delete(id);
                await DeleteOffline(id);
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
                var localId = await GetLocalId(id);
                var result = await manager.DeleteRecordAsync(storeName, localId);
                if (result.Failed)
                    return false;
                RecordDelete(id);

                if (IsOnline)
                {
                    var keys = await GetKeys();
                    if (keys.Count > 0)
                    {
                        var key = keys.Where(x => x.LocalId.ToString() == localId.ToString()).FirstOrDefault();
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
            var entity = await GetByIDOffline(id);
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
                        var keys = await GetKeys();
                        foreach (var entry in list)
                        {
                            var key = keys.FirstOrDefault(k => k.OnlineId.ToString() == primaryKey.GetValue(entry).ToString());
                            if (key != null)
                            {
                                await manager.UpdateRecordAsync(new UpdateRecord<TEntity>()
                                {
                                    StoreName = storeName,
                                    Record = entry,
                                    Key = key.LocalId
                                });
                                keys.Remove(key);
                            }
                            else
                            {
                                await InsertOffline(entry);
                            }
                        }
                        foreach (var ke in keys)
                        {
                            await DeleteOffline(ke.OnlineId);
                        }
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
                if (Convert.ToInt32(onlineId) == 0)
                {
                    onlineId = -1;
                    primaryKey.SetValue(entity, onlineId);
                }
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

                RecordInsert(entity);

                return entity;
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
                await UpdateOffline(entityToUpdate);
            }
            else
                await UpdateOffline(entityToUpdate);
            return entityToUpdate;
        }
        public async Task<TEntity> UpdateOffline(TEntity entityToUpdate)
        {
            await EnsureManager();
            object localId = await GetLocalId(primaryKey.GetValue(entityToUpdate));
            try
            {
                await manager.UpdateRecord(new UpdateRecord<TEntity>()
                {
                    StoreName = storeName,
                    Record = entityToUpdate,
                    Key = localId
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
                List<OnlineOfflineKey> keys = new();
                foreach (var localTransaction in array.ToList())
                {
                    try
                    {
                        object oldOnlineId = new();
                        switch (localTransaction.Action)
                        {
                            case LocalTransactionTypes.Insert:
                                oldOnlineId = primaryKey.GetValue(localTransaction.Entity);
                                if (Convert.ToInt32(oldOnlineId) < 0)
                                {
                                    primaryKey.SetValue(localTransaction.Entity, 0);
                                }
                                var insertedEntity = await _apiRepository.Insert(localTransaction.Entity);
                                var onlineId = primaryKey.GetValue(insertedEntity);
                                keys = await GetKeys();
                                var key = keys.FirstOrDefault(k => k.OnlineId.ToString() == oldOnlineId.ToString());
                                key.OnlineId = primaryKey.GetValue(insertedEntity);
                                await manager.AddRecordAsync(new StoreRecord<OnlineOfflineKey>()
                                {
                                    StoreName = KeyStoreName,
                                    Record = key
                                });
                                break;
                            case LocalTransactionTypes.Update:
                                oldOnlineId = primaryKey.GetValue(localTransaction.Entity);
                                if (Convert.ToInt32(oldOnlineId.ToString()) < 0)
                                {
                                    keys = await GetKeys();
                                    var oldKey = keys.FirstOrDefault(k => k.OnlineId.ToString() == oldOnlineId.ToString());
                                    var newKey = keys.FirstOrDefault(k => k.LocalId.ToString() == oldKey.LocalId.ToString() && k.Id != oldKey.Id);
                                    primaryKey.SetValue(localTransaction.Entity, Convert.ToInt32(newKey.OnlineId.ToString()));
                                }
                                await _apiRepository.Update(localTransaction.Entity);
                                break;
                            case LocalTransactionTypes.Delete:
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
                keys = await GetKeys();
                foreach (var key in keys)
                {
                    if (Convert.ToInt32(key.OnlineId.ToString()) < 0)
                    {
                        await manager.DeleteRecordAsync(KeyStoreName, key.Id);
                        var localEntity = (await manager.Where<TEntity>(storeName, "Id", key.LocalId)).FirstOrDefault();
                        var newKey = keys.FirstOrDefault(k => k.LocalId.ToString() == key.LocalId.ToString() && k.Id != key.Id);
                        primaryKey.SetValue(localEntity, Convert.ToInt32(newKey.OnlineId.ToString()));
                        await manager.UpdateRecordAsync(new UpdateRecord<TEntity>()
                        {
                            StoreName = storeName,
                            Record = localEntity,
                            Key = newKey.LocalId
                        });
                    }
                }
                await GetAll();
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
