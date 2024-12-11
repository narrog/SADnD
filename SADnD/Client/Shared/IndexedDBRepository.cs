using BlazorDB;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using SADnD.Shared;
using System.Net.Http.Headers;
using System.Reflection;

namespace SADnD.Client.Shared
{
    public class IndexedDBRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IBlazorDbFactory _dbFactory;
        private readonly string _dbName = "SADnD.IndexedDB";
        private int offlineId = 0;

        public IndexedDbManager manager;
        string storeName = "";
        Type entityType;
        PropertyInfo primaryKey;
        private string KeyStoreName => $"{storeName}{Globals.KeysSuffix}";
        private string LocalStoreName => $"{storeName}{Globals.LocalTransactionsSuffix}";

        public IndexedDBRepository(IBlazorDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
            entityType = typeof(TEntity);
            storeName = entityType.Name;
            primaryKey = entityType.GetProperty("Id");
        }
        private async Task EnsureManager()
        {
            if (manager == null)
            {
                manager = await _dbFactory.GetDbManager(_dbName);
                await manager.OpenDb();
            }
        }
        public virtual async Task<bool> Delete(TEntity entityToDelete)
        {
            Console.WriteLine($"ID for delete: {primaryKey.GetValue(entityToDelete)}");
            return await Delete(primaryKey.GetValue(entityToDelete));
        }

        public virtual async Task<bool> Delete(object id)
        {
            await EnsureManager();
            var localId = await GetLocalId(id);
            Console.WriteLine($"localId for delete: {localId}");
            var result = await manager.DeleteRecordAsync(storeName, localId);
            if (result.Failed)
                return false;
            return true;
        }
        public virtual async Task RecordDelete(object id)
        {
            var action = LocalTransactionTypes.Delete;
            var entity = await GetByID(id);
            Console.WriteLine($"in record: {JsonConvert.SerializeObject(entity)}");
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
        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await GetAll(true);
        }
        public virtual async Task<IEnumerable<TEntity>> GetAll(bool onlineKeys = true)
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
                        primaryKey.SetValue(entity, await GetOnlineId(primaryKey.GetValue(entity)));
                    }
                }
                return array.ToList();
            }
        }

        public virtual async Task<TEntity> GetByID(object id)
        {
            await EnsureManager();
            var localId = await GetLocalId(id);
            var items = await manager.Where<TEntity>(storeName, "Id", localId);
            if (items.Any())
            {
                Console.WriteLine($"items: {JsonConvert.SerializeObject(items.First())}");
                var result = items.First();
                Console.WriteLine($"result before id change: {JsonConvert.SerializeObject(result)}");
                var onlineId = (await GetOnlineId(primaryKey.GetValue(result))).ToString();
                if (primaryKey.PropertyType == typeof(Int32))
                {
                    primaryKey.SetValue(result, Convert.ToInt32(onlineId));
                    Console.WriteLine($"id changed (int): {JsonConvert.SerializeObject(result)}");
                    return result;
                }
                primaryKey.SetValue(result, onlineId);
                Console.WriteLine($"id changed (string): {JsonConvert.SerializeObject(result)}");
                return result;
            }
            else
                return null;
        }

        public virtual async Task<TEntity> Insert(TEntity entity)
        {
            await EnsureManager();
            var onlineId = primaryKey.GetValue(entity);
            if (Convert.ToInt32(onlineId) == 0)
            {
                onlineId = --offlineId;
                primaryKey.SetValue(entity, onlineId);
            }
            var record = new StoreRecord<TEntity>()
            {
                StoreName = storeName,
                Record = entity
            };
            await manager.AddRecordAsync(record);
            var allItems = await GetAll(false);
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
            return entity;
        }
        public virtual async Task RecordInsert(TEntity entity)
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
        public virtual async Task<TEntity> Update(TEntity entityToUpdate)
        {
            await EnsureManager();
            object localId = await GetLocalId(primaryKey.GetValue(entityToUpdate));
            await manager.UpdateRecord(new UpdateRecord<TEntity>()
            {
                StoreName = storeName,
                Record = entityToUpdate,
                Key = localId
            });
            return entityToUpdate;
        }
        public virtual async Task RecordUpdate(TEntity entity)
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
        public virtual async Task<List<LocalTransaction<TEntity>>> GetTransactions()
        {
            await EnsureManager();
            var array = await manager.ToArray<LocalTransaction<TEntity>>(LocalStoreName);
            if (array != null)
                return array.ToList();
            return null;
        }
        public virtual async Task DeleteAllTransactions()
        {
            await EnsureManager();
            await manager.ClearTableAsync(LocalStoreName);
        }
        public virtual async Task<bool> DeleteKey(object id)
        {
            await EnsureManager();
            var keys = await GetKeys();
            if (keys.Count > 0)
            {
                var key = keys.Where(x => x.OnlineId.ToString() == id.ToString()).FirstOrDefault();
                if (key != null)
                {
                    await manager.DeleteRecordAsync(KeyStoreName, key.Id);
                    return true;
                }
            }
            return false;
        }
        public virtual async Task<bool> AddUpdatedKey(object oldId, object newId)
        {
            var keys = await GetKeys();
            var key = keys.FirstOrDefault(k => k.OnlineId.ToString() == oldId.ToString());
            key.OnlineId = newId;
            await manager.AddRecordAsync(new StoreRecord<OnlineOfflineKey>()
            {
                StoreName = KeyStoreName,
                Record = key
            });
            return true;
        }
        public virtual async Task<object> GetUpdatedKey(object id)
        {
            var keys = await GetKeys();
            var oldKey = keys.FirstOrDefault(k => k.OnlineId.ToString() == id.ToString());
            var newKey = keys.FirstOrDefault(k => k.LocalId.ToString() == oldKey.LocalId.ToString() && k.Id != oldKey.Id);
            return newKey.OnlineId;
        }
        public virtual async Task CleanupKeys()
        {
            var keys = await GetKeys();
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
        }
        public async Task<List<OnlineOfflineKey>> GetKeys()
        {
            await EnsureManager();
            return (await manager.ToArray<OnlineOfflineKey>(KeyStoreName)).ToList();
        }
        private async Task<object> GetLocalId(object onlineId)
        {
            var keys = await GetKeys();
            var key = keys.Where(x => x.OnlineId.ToString() == onlineId.ToString()).FirstOrDefault();
            Console.WriteLine($"GetLocalId: {onlineId.ToString()} {key.LocalId.ToString()} - {key.LocalId.GetType()}");
            return key.LocalId;
        }
        private async Task<object> GetOnlineId(object localId)
        {
            var keys = await GetKeys();
            var key = keys.Where(x => x.LocalId.ToString() == localId.ToString()).FirstOrDefault();
            Console.WriteLine($"GetOnlineId: {localId.ToString()} {key.LocalId.ToString()} - {key.LocalId.GetType()}");
            return key.OnlineId;
        }
    }
}
