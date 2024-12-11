using BlazorDB;
using Microsoft.JSInterop;
using SADnD.Shared;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SADnD.Client.Services;
using System.Xml;
using SADnD.Shared.Models;
namespace SADnD.Client.Shared
{
    public class SyncRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1,1);
        private readonly APIRepository<TEntity> _apiRepository;
        private readonly IndexedDBRepository<TEntity> _indexedDBRepository;
        private readonly IJSRuntime _jsruntime;
        Type entityType;
        PropertyInfo primaryKey;
        public bool IsOnline { get; set; } = false;

        public delegate void OnlineStatusEventHandler(object sender, OnlineStatusEventArgs e);
        public event OnlineStatusEventHandler OnlineStatusChanged;

        public SyncRepository(APIRepository<TEntity> apiRepository, IndexedDBRepository<TEntity> indexedDBRepository, IJSRuntime jsRuntime)
        {
            _apiRepository = apiRepository;
            _indexedDBRepository = indexedDBRepository;
            _jsruntime = jsRuntime;

            entityType = typeof(TEntity);
            primaryKey = entityType.GetProperty("Id");

            _ = _jsruntime.InvokeVoidAsync("connectivity.initialize", DotNetObjectReference.Create(this));
        }
        [JSInvokable("ConnectivityChanged")]
        public virtual async void OnConnectivityChanged(bool isOnline)
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
        public virtual async Task<bool> Delete(TEntity entityToDelete)
        {
            bool deleted = false;
            if (IsOnline)
            {
                deleted = await _apiRepository.Delete(entityToDelete);
                deleted = await _indexedDBRepository.Delete(entityToDelete) || deleted;
                await _indexedDBRepository.DeleteKey(primaryKey.GetValue(entityToDelete));
            }
            else
            {
                await _indexedDBRepository.RecordDelete(primaryKey.GetValue(entityToDelete));
                deleted = await _indexedDBRepository.Delete(entityToDelete);
            }
            return deleted;
        }
        public virtual async Task<bool> Delete(object id)
        {
            bool deleted = false;
            var entity = await GetByID(id);
            if (entity != null)
                return await Delete(entity);
            return await _indexedDBRepository.DeleteKey(id);
        }
        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            if (IsOnline)
            {
                await semaphore.WaitAsync();
                try
                {
                    var list = await _apiRepository.GetAll();
                    if (list != null)
                    {
                        var keys = await _indexedDBRepository.GetKeys();
                        foreach (var entry in list)
                        {
                            var key = keys.FirstOrDefault(k => k.OnlineId.ToString() == primaryKey.GetValue(entry).ToString());
                            if (key != null)
                            {
                                await _indexedDBRepository.Update(entry);
                                keys.Remove(key);
                            }
                            else
                            {
                                await _indexedDBRepository.Insert(entry);
                            }
                        }
                        foreach (var ke in keys)
                        {
                            await Delete(ke.OnlineId);
                        }
                        return list;
                    }
                }
                finally
                {
                    semaphore.Release();
                }
                return null;
            }
            else
            {
                return await _indexedDBRepository.GetAll();
            }
        }
        public virtual async Task<TEntity> GetByID(object id)
        {
            if (IsOnline)
                return await _apiRepository.GetByID(id);
            else
                return await _indexedDBRepository.GetByID(id);
        }
        public virtual async Task<TEntity> Insert(TEntity entity)
        {
            TEntity returnValue;
            if (IsOnline)
            {
                returnValue = await _apiRepository.Insert(entity);
                await _indexedDBRepository.Insert(returnValue);
            }
            else
            {
                returnValue = await _indexedDBRepository.Insert(entity);
                await _indexedDBRepository.RecordInsert(returnValue);
            }
            return returnValue;
        }
        public virtual async Task<TEntity> Update(TEntity entityToUpdate)
        {
            TEntity returnValue;
            if (IsOnline)
            {
                returnValue = await _apiRepository.Update(entityToUpdate);
                await _indexedDBRepository.Update(returnValue);
            }
            else
            {
                returnValue = await _indexedDBRepository.Update(entityToUpdate);
                await _indexedDBRepository.RecordUpdate(returnValue);
            }
            return returnValue;
        }
        public virtual async Task<bool> SyncLocalToServer()
        {
            if (!IsOnline)
                return false;
            var transactions = await _indexedDBRepository.GetTransactions();
            if (transactions == null || transactions.Count == 0)
                return true;
            else
            {
                foreach (var localTransaction in transactions)
                {
                    object oldOnlineId = primaryKey.GetValue(localTransaction.Entity);
                    switch (localTransaction.Action)
                    {
                        case LocalTransactionTypes.Insert:
                            if (Convert.ToInt32(oldOnlineId) < 0)
                            {
                                primaryKey.SetValue(localTransaction.Entity, 0);
                            }
                            var insertedEntity = await _apiRepository.Insert(localTransaction.Entity);
                            await _indexedDBRepository.AddUpdatedKey(oldOnlineId, primaryKey.GetValue(insertedEntity));
                            break;
                        case LocalTransactionTypes.Update:
                            if (Convert.ToInt32(oldOnlineId) < 0)
                            {
                                primaryKey.SetValue(localTransaction.Entity, Convert.ToInt32((await _indexedDBRepository.GetUpdatedKey(oldOnlineId)).ToString()));
                            }
                            await _apiRepository.Update(localTransaction.Entity);
                            break;
                        case LocalTransactionTypes.Delete:
                            if (Convert.ToInt32(oldOnlineId) < 0)
                            {
                                primaryKey.SetValue(localTransaction.Entity, Convert.ToInt32((await _indexedDBRepository.GetUpdatedKey(oldOnlineId)).ToString()));
                            }
                            await _apiRepository.Delete(localTransaction.Entity);
                            break;
                        default:
                            break;
                    }
                }
                await _indexedDBRepository.DeleteAllTransactions();
                await _indexedDBRepository.CleanupKeys();
                await GetAll();
                return true;
            }
        }
    }
}
