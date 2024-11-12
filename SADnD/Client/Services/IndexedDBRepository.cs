using BlazorDB;
using SADnD.Shared;
using System.Linq.Expressions;
using System.Reflection;

namespace SADnD.Client.Services
{
    public class IndexedDBRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        IBlazorDbFactory _dbFactory;
        string _dbName = "";
        //string _primaryKeyName = "";
        bool _autoGenerateKey;

        IndexedDbManager manager;
        string storeName = "";
        Type entityType;
        PropertyInfo primaryKey;

        public IndexedDBRepository(string dbName, bool autoGenerateKey, IBlazorDbFactory dbFactory)
        {
            _dbName = dbName;
            _dbFactory = dbFactory;
            //_primaryKeyName = primaryKeyName;
            _autoGenerateKey = autoGenerateKey;

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

        public async Task<bool> Delete(TEntity entityToDelete)
        {
            await EnsureManager();
            var Id = primaryKey.GetValue(entityToDelete);
            return await Delete(Id);
        }

        public async Task<bool> Delete(object id)
        {
            await EnsureManager();
            try
            {
                await manager.DeleteRecordAsync(storeName, id);
                return true;
            }
            catch (Exception ex)
            {
                // TODO: Log Exception
                return false;
            }
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            await EnsureManager();
            var array = await manager.ToArray<TEntity>(storeName);
            if (array == null)
                return new List<TEntity>();
            else
                return array.ToList();
        }

        public async Task<TEntity> GetByID(object id)
        {
            await EnsureManager();
            var items = await manager.Where<TEntity>(storeName, "Id", id);
            if (items.Any())
                return items.First();
            else
                return null;
        }

        public async Task<TEntity> Insert(TEntity entity)
        {
            await EnsureManager();
            if (_autoGenerateKey)
            {
                primaryKey.SetValue(entity, 0);
            }

            try
            {
                var record = new StoreRecord<TEntity>()
                {
                    StoreName = storeName,
                    Record = entity
                };
                await manager.AddRecordAsync<TEntity>(record);
                var allItems = await GetAll();
                var last = allItems.Last();
                return last;
            }
            catch (Exception ex)
            {
                // TODO: Log Exception
                return null;
            }
        }

        public async Task<TEntity> Update(TEntity entityToUpdate)
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
                return entityToUpdate;
            }
            catch (Exception ex)
            {
                // TODO: Log exception
                return null;
            }
        }
    }
}
