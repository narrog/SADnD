using BlazorDB;
using Microsoft.JSInterop;
using SADnD.Client.Shared;
using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class NoteSyncManager : SyncRepository<Note>
    {
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private readonly NoteApiManager _apiRepository;
        private readonly IJSRuntime _jsruntime;
        private readonly Dictionary<Type, object> _repositories;
        private readonly IndexedDBRepository<NoteStory> _storyDBRepository;
        private readonly IndexedDBRepository<NotePerson> _personDBRepository;
        private readonly IndexedDBRepository<NoteLocation> _locationDBRepository;
        private readonly IndexedDBRepository<NoteQuest> _questDBRepository;
        private readonly IndexedDBRepository<NoteHint> _hintDBRepository;
        public NoteSyncManager(
            IBlazorDbFactory dbFactory, 
            NoteApiManager noteManager, 
            IJSRuntime jsRuntime, 
            IndexedDBRepository<NoteStory> storyDBRepository, 
            IndexedDBRepository<NotePerson> personDBRepository, 
            IndexedDBRepository<NoteLocation> locationDBRepository, 
            IndexedDBRepository<NoteQuest> questDBRepository, 
            IndexedDBRepository<NoteHint> hintDBRepository)
            : base(noteManager,null, jsRuntime)
        {
            _apiRepository = noteManager;
            _jsruntime = jsRuntime;

            _storyDBRepository = storyDBRepository;
            _personDBRepository = personDBRepository;
            _locationDBRepository = locationDBRepository;
            _questDBRepository = questDBRepository;
            _hintDBRepository = hintDBRepository;
            _repositories = new Dictionary<Type, object>()
            {
                { typeof(NoteStory), _storyDBRepository},
                { typeof(NotePerson), _personDBRepository},
                { typeof(NoteLocation), _locationDBRepository},
                { typeof(NoteQuest), _questDBRepository},
                { typeof(NoteHint), _hintDBRepository}
            };
        }
        public override async Task<bool> Delete(Note entityToDelete)
        {
            bool deleted = false;
            switch (entityToDelete.GetType().Name)
            {
                case nameof(NoteStory):
                    if (IsOnline)
                    {
                        deleted = await _apiRepository.Delete(entityToDelete);
                        deleted = await _storyDBRepository.Delete((NoteStory)entityToDelete) || deleted;
                        await _storyDBRepository.DeleteKey(entityToDelete.Id);
                    }
                    else
                    {
                        await _storyDBRepository.RecordDelete(entityToDelete.Id);
                        deleted = await _storyDBRepository.Delete((NoteStory)entityToDelete);
                    }
                    break;
                case nameof(NotePerson):
                    if (IsOnline)
                    {
                        deleted = await _apiRepository.Delete(entityToDelete);
                        deleted = await _personDBRepository.Delete((NotePerson)entityToDelete) || deleted;
                        await _personDBRepository.DeleteKey(entityToDelete.Id);
                    }
                    else
                    {
                        await _personDBRepository.RecordDelete(entityToDelete.Id);
                        deleted = await _personDBRepository.Delete((NotePerson)entityToDelete);
                    }
                    break;
                case nameof(NoteLocation):
                    if (IsOnline)
                    {
                        deleted = await _apiRepository.Delete(entityToDelete);
                        deleted = await _locationDBRepository.Delete((NoteLocation)entityToDelete) || deleted;
                        await _locationDBRepository.DeleteKey(entityToDelete.Id);
                    }
                    else
                    {
                        await _locationDBRepository.RecordDelete(entityToDelete.Id);
                        deleted = await _locationDBRepository.Delete((NoteLocation)entityToDelete);
                    }
                    break;
                case nameof(NoteQuest):
                    if (IsOnline)
                    {
                        deleted = await _apiRepository.Delete(entityToDelete);
                        deleted = await _questDBRepository.Delete((NoteQuest)entityToDelete) || deleted;
                        await _questDBRepository.DeleteKey(entityToDelete.Id);
                    }
                    else
                    {
                        await _questDBRepository.RecordDelete(entityToDelete.Id);
                        deleted = await _questDBRepository.Delete((NoteQuest)entityToDelete);
                    }
                    break;
                case nameof(NoteHint):
                    if (IsOnline)
                    {
                        deleted = await _apiRepository.Delete(entityToDelete);
                        deleted = await _hintDBRepository.Delete((NoteHint)entityToDelete) || deleted;
                        await _hintDBRepository.DeleteKey(entityToDelete.Id);
                    }
                    else
                    {
                        await _hintDBRepository.RecordDelete(entityToDelete.Id);
                        deleted = await _hintDBRepository.Delete((NoteHint)entityToDelete);
                    }
                    break;
                default:
                    break;
            }
            return deleted;
        }
        public override async Task<bool> Delete(object id)
        {
            bool deleted = false;
            var entity = await GetByID(id);
            if (entity != null)
            {
                return await Delete(entity);
            }
            else
            {
                if (await _storyDBRepository.DeleteKey(id) == true)
                    return true;
                if (await _personDBRepository.DeleteKey(id) == true)
                    return true;
                if (await _locationDBRepository.DeleteKey(id) == true)
                    return true;
                if (await _questDBRepository.DeleteKey(id) == true)
                    return true;
                if (await _hintDBRepository.DeleteKey(id) == true)
                    return true;
            }
            return false;
        }
        public override async Task<IEnumerable<Note>> GetAll()
        {
            if (IsOnline)
            {
                await semaphore.WaitAsync();
                try
                {
                    var list = await _apiRepository.GetAll();
                    if (list != null)
                    {
                        var keys = await _storyDBRepository.GetKeys();
                        foreach (var item in await _personDBRepository.GetKeys())
                        {
                            keys.Add(item);
                        }
                        foreach (var item in await _locationDBRepository.GetKeys())
                        {
                            keys.Add(item);
                        }
                        foreach (var item in await _questDBRepository.GetKeys())
                        {
                            keys.Add(item);
                        }
                        foreach (var item in await _hintDBRepository.GetKeys())
                        {
                            keys.Add(item);
                        }
                        foreach (var entry in list)
                        {
                            switch (entry.GetType().Name)
                            {
                                case nameof(NoteStory):
                                    var key = keys.FirstOrDefault(k => k.OnlineId.ToString() == entry.Id.ToString());
                                    if (key != null)
                                    {
                                        await _storyDBRepository.Update((NoteStory)entry);
                                        keys.Remove(key);
                                    }
                                    else
                                    {
                                        await _storyDBRepository.Insert((NoteStory)entry);
                                    }
                                    break;
                                case nameof(NotePerson):
                                    key = keys.FirstOrDefault(k => k.OnlineId.ToString() == entry.Id.ToString());
                                    if (key != null)
                                    {
                                        await _personDBRepository.Update((NotePerson)entry);
                                        keys.Remove(key);
                                    }
                                    else
                                    {
                                        await _personDBRepository.Insert((NotePerson)entry);
                                    }
                                    break;
                                case nameof(NoteLocation):
                                    key = keys.FirstOrDefault(k => k.OnlineId.ToString() == entry.Id.ToString());
                                    if (key != null)
                                    {
                                        var result = await _locationDBRepository.Update((NoteLocation)entry);
                                        keys.Remove(key);
                                    }
                                    else
                                    {
                                        await _locationDBRepository.Insert((NoteLocation)entry);
                                    }
                                    break;
                                case nameof(NoteQuest):
                                    key = keys.FirstOrDefault(k => k.OnlineId.ToString() == entry.Id.ToString());
                                    if (key != null)
                                    {
                                        await _questDBRepository.Update((NoteQuest)entry);
                                        keys.Remove(key);
                                    }
                                    else
                                    {
                                        await _questDBRepository.Insert((NoteQuest)entry);
                                    }
                                    break;
                                case nameof(NoteHint):
                                    key = keys.FirstOrDefault(k => k.OnlineId.ToString() == entry.Id.ToString());
                                    if (key != null)
                                    {
                                        await _hintDBRepository.Update((NoteHint)entry);
                                        keys.Remove(key);
                                    }
                                    else
                                    {
                                        await _hintDBRepository.Insert((NoteHint)entry);
                                    }
                                    break;
                                default:
                                    break;
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
                var list = new List<Note>();
                foreach (var item in await _storyDBRepository.GetAll())
                {
                    list.Add(item);
                }
                foreach (var item in await _personDBRepository.GetAll())
                {
                    list.Add(item);
                }
                foreach (var item in await _locationDBRepository.GetAll())
                {
                    list.Add(item);
                }
                foreach (var item in await _questDBRepository.GetAll())
                {
                    list.Add(item);
                }
                foreach (var item in await _hintDBRepository.GetAll())
                {
                    list.Add(item);
                }
                return list;
            }
        }
        public override async Task<Note> GetByID(object id)
        {
            if (IsOnline)
                return await _apiRepository.GetByID(id);
            else
            {
                Note result;
                result = await _apiRepository.GetByID(id);
                if (result != null)
                    return result;
                result = await _personDBRepository.GetByID(id);
                if (result != null)
                    return result;
                result = await _locationDBRepository.GetByID(id);
                if (result != null)
                    return result;
                result = await _questDBRepository.GetByID(id);
                if (result != null)
                    return result;
                result = await _hintDBRepository.GetByID(id);
                if (result != null)
                    return result;
                return null;
            }
        }
        public override async Task<Note> Insert(Note entity)
        {
            Note returnValue;
            switch (entity.GetType().Name)
            {
                case nameof(NoteStory):
                    if (IsOnline)
                    {
                        returnValue = await _apiRepository.Insert(entity);
                        await _storyDBRepository.Insert((NoteStory)returnValue);
                    }
                    else
                    {
                        returnValue = await _storyDBRepository.Insert((NoteStory)entity);
                        await _storyDBRepository.RecordInsert((NoteStory)returnValue);
                    }
                    break;
                case nameof(NotePerson):
                    if (IsOnline)
                    {
                        returnValue = await _apiRepository.Insert(entity);
                        await _personDBRepository.Insert((NotePerson)returnValue);
                    }
                    else
                    {
                        returnValue = await _personDBRepository.Insert((NotePerson)entity);
                        await _personDBRepository.RecordInsert((NotePerson)returnValue);
                    }
                    break;
                case nameof(NoteLocation):
                    if (IsOnline)
                    {
                        returnValue = await _apiRepository.Insert(entity);
                        await _locationDBRepository.Insert((NoteLocation)returnValue);
                    }
                    else
                    {
                        returnValue = await _locationDBRepository.Insert((NoteLocation)entity);
                        await _locationDBRepository.RecordInsert((NoteLocation)returnValue);
                    }
                    break;
                case nameof(NoteQuest):
                    if (IsOnline)
                    {
                        returnValue = await _apiRepository.Insert(entity);
                        await _questDBRepository.Insert((NoteQuest)returnValue);
                    }
                    else
                    {
                        returnValue = await _questDBRepository.Insert((NoteQuest)entity);
                        await _questDBRepository.RecordInsert((NoteQuest)returnValue);
                    }
                    break;
                case nameof(NoteHint):
                    if (IsOnline)
                    {
                        returnValue = await _apiRepository.Insert(entity);
                        await _hintDBRepository.Insert((NoteHint)returnValue);
                    }
                    else
                    {
                        returnValue = await _hintDBRepository.Insert((NoteHint)entity);
                        await _hintDBRepository.RecordInsert((NoteHint)returnValue);
                    }
                    break;
                default:
                    returnValue = null;
                    break;
            }
            return returnValue;
        }
        public override async Task<Note> Update(Note entityToUpdate)
        {
            Note returnValue;
            switch (entityToUpdate.GetType().Name)
            {
                case nameof(NoteStory):
                    if (IsOnline)
                    {
                        returnValue = await _apiRepository.Update(entityToUpdate);
                        await _storyDBRepository.Update((NoteStory)returnValue);
                    }
                    else
                    {
                        returnValue = await _storyDBRepository.Update((NoteStory)entityToUpdate);
                        await _storyDBRepository.RecordUpdate((NoteStory)returnValue);
                    }
                    break;
                case nameof(NotePerson):
                    if (IsOnline)
                    {
                        returnValue = await _apiRepository.Update(entityToUpdate);
                        await _personDBRepository.Update((NotePerson)returnValue);
                    }
                    else
                    {
                        returnValue = await _personDBRepository.Update((NotePerson)entityToUpdate);
                        await _personDBRepository.RecordUpdate((NotePerson)returnValue);
                    }
                    break;
                case nameof(NoteLocation):
                    if (IsOnline)
                    {
                        returnValue = await _apiRepository.Update(entityToUpdate);
                        await _locationDBRepository.Update((NoteLocation)returnValue);
                    }
                    else
                    {
                        returnValue = await _locationDBRepository.Update((NoteLocation)entityToUpdate);
                        await _locationDBRepository.RecordUpdate((NoteLocation)returnValue);
                    }
                    break;
                case nameof(NoteQuest):
                    if (IsOnline)
                    {
                        returnValue = await _apiRepository.Update(entityToUpdate);
                        await _questDBRepository.Update((NoteQuest)returnValue);
                    }
                    else
                    {
                        returnValue = await _questDBRepository.Update((NoteQuest)entityToUpdate);
                        await _questDBRepository.RecordUpdate((NoteQuest)returnValue);
                    }
                    break;
                case nameof(NoteHint):
                    if (IsOnline)
                    {
                        returnValue = await _apiRepository.Update(entityToUpdate);
                        await _hintDBRepository.Update((NoteHint)returnValue);
                    }
                    else
                    {
                        returnValue = await _hintDBRepository.Update((NoteHint)entityToUpdate);
                        await _hintDBRepository.RecordUpdate((NoteHint)returnValue);
                    }
                    break;
                default:
                    returnValue = null;
                    break;
            }
            return returnValue;
        }
        public override async Task<bool> SyncLocalToServer()
        {
            if (!IsOnline)
                return false;
            var transactionsStory = await _storyDBRepository.GetTransactions();
            if (transactionsStory != null || transactionsStory.Count > 0)
            {
                foreach (var transaction in transactionsStory)
                {
                    object oldOnlineId = transaction.Entity.Id;
                    switch (transaction.Action)
                    {

                        case LocalTransactionTypes.Insert:
                            if (transaction.Entity.Id < 0)
                            {
                                transaction.Entity.Id = 0;
                            }
                            var insertedEntity = await _apiRepository.Insert(transaction.Entity);
                            await _storyDBRepository.AddUpdatedKey(oldOnlineId, insertedEntity.Id);
                            break;
                        case LocalTransactionTypes.Update:
                            if (Convert.ToInt32(oldOnlineId.ToString()) < 0)
                            {
                                transaction.Entity.Id = Convert.ToInt32(await _storyDBRepository.GetUpdatedKey(oldOnlineId));
                            }
                            await _apiRepository.Update(transaction.Entity);
                            break;
                        case LocalTransactionTypes.Delete:
                            if (Convert.ToInt32(oldOnlineId.ToString()) < 0)
                            {
                                transaction.Entity.Id = Convert.ToInt32(await _storyDBRepository.GetUpdatedKey(oldOnlineId));
                            }
                            await _apiRepository.Delete(transaction.Entity);
                            break;
                        default:
                            break;
                    }
                }
                await _storyDBRepository.DeleteAllTransactions();
                await _storyDBRepository.CleanupKeys();
            }
            var transactionsPerson = await _personDBRepository.GetTransactions();
            if (transactionsPerson != null || transactionsPerson.Count > 0)
            {
                foreach (var transaction in transactionsPerson)
                {
                    object oldOnlineId = transaction.Entity.Id;
                    switch (transaction.Action)
                    {

                        case LocalTransactionTypes.Insert:
                            if (transaction.Entity.Id < 0)
                            {
                                transaction.Entity.Id = 0;
                            }
                            var insertedEntity = await _apiRepository.Insert(transaction.Entity);
                            await _personDBRepository.AddUpdatedKey(oldOnlineId, insertedEntity.Id);
                            break;
                        case LocalTransactionTypes.Update:
                            if (Convert.ToInt32(oldOnlineId.ToString()) < 0)
                            {
                                transaction.Entity.Id = Convert.ToInt32(await _personDBRepository.GetUpdatedKey(oldOnlineId));
                            }
                            await _apiRepository.Update(transaction.Entity);
                            break;
                        case LocalTransactionTypes.Delete:
                            if (Convert.ToInt32(oldOnlineId.ToString()) < 0)
                            {
                                transaction.Entity.Id = Convert.ToInt32(await _personDBRepository.GetUpdatedKey(oldOnlineId));
                            }
                            await _apiRepository.Delete(transaction.Entity);
                            break;
                        default:
                            break;
                    }
                }
                await _personDBRepository.DeleteAllTransactions();
                await _personDBRepository.CleanupKeys();
            }
            var transactionsLocation = await _locationDBRepository.GetTransactions();
            if (transactionsLocation != null || transactionsLocation.Count > 0)
            {
                foreach (var transaction in transactionsLocation)
                {
                    object oldOnlineId = transaction.Entity.Id;
                    switch (transaction.Action)
                    {

                        case LocalTransactionTypes.Insert:
                            if (transaction.Entity.Id < 0)
                            {
                                transaction.Entity.Id = 0;
                            }
                            var insertedEntity = await _apiRepository.Insert(transaction.Entity);
                            await _locationDBRepository.AddUpdatedKey(oldOnlineId, insertedEntity.Id);
                            break;
                        case LocalTransactionTypes.Update:
                            if (Convert.ToInt32(oldOnlineId.ToString()) < 0)
                            {
                                transaction.Entity.Id = Convert.ToInt32((await _locationDBRepository.GetUpdatedKey(oldOnlineId)).ToString());
                            }
                            await _apiRepository.Update(transaction.Entity);
                            break;
                        case LocalTransactionTypes.Delete:
                            if (Convert.ToInt32(oldOnlineId.ToString()) < 0)
                            {
                                transaction.Entity.Id = Convert.ToInt32((await _locationDBRepository.GetUpdatedKey(oldOnlineId)).ToString());
                            }
                            await _apiRepository.Delete(transaction.Entity);
                            break;
                        default:
                            break;
                    }
                }
                await _locationDBRepository.DeleteAllTransactions();
                await _locationDBRepository.CleanupKeys();
            }
            var transactionsQuest = await _questDBRepository.GetTransactions();
            if (transactionsQuest != null || transactionsQuest.Count > 0)
            {
                foreach (var transaction in transactionsQuest)
                {
                    object oldOnlineId = transaction.Entity.Id;
                    switch (transaction.Action)
                    {

                        case LocalTransactionTypes.Insert:
                            if (transaction.Entity.Id < 0)
                            {
                                transaction.Entity.Id = 0;
                            }
                            var insertedEntity = await _apiRepository.Insert(transaction.Entity);
                            await _questDBRepository.AddUpdatedKey(oldOnlineId, insertedEntity.Id);
                            break;
                        case LocalTransactionTypes.Update:
                            if (Convert.ToInt32(oldOnlineId.ToString()) < 0)
                            {
                                transaction.Entity.Id = Convert.ToInt32(await _questDBRepository.GetUpdatedKey(oldOnlineId));
                            }
                            await _apiRepository.Update(transaction.Entity);
                            break;
                        case LocalTransactionTypes.Delete:
                            if (Convert.ToInt32(oldOnlineId.ToString()) < 0)
                            {
                                transaction.Entity.Id = Convert.ToInt32(await _questDBRepository.GetUpdatedKey(oldOnlineId));
                            }
                            await _apiRepository.Delete(transaction.Entity);
                            break;
                        default:
                            break;
                    }
                }
                await _questDBRepository.DeleteAllTransactions();
                await _questDBRepository.CleanupKeys();
            }
            var transactionsHint = await _hintDBRepository.GetTransactions();
            if (transactionsHint != null || transactionsHint.Count > 0)
            {
                foreach (var transaction in transactionsHint)
                {
                    object oldOnlineId = transaction.Entity.Id;
                    switch (transaction.Action)
                    {

                        case LocalTransactionTypes.Insert:
                            if (transaction.Entity.Id < 0)
                            {
                                transaction.Entity.Id = 0;
                            }
                            var insertedEntity = await _apiRepository.Insert(transaction.Entity);
                            await _hintDBRepository.AddUpdatedKey(oldOnlineId, insertedEntity.Id);
                            break;
                        case LocalTransactionTypes.Update:
                            if (Convert.ToInt32(oldOnlineId.ToString()) < 0)
                            {
                                transaction.Entity.Id = Convert.ToInt32(await _hintDBRepository.GetUpdatedKey(oldOnlineId));
                            }
                            await _apiRepository.Update(transaction.Entity);
                            break;
                        case LocalTransactionTypes.Delete:
                            if (Convert.ToInt32(oldOnlineId.ToString()) < 0)
                            {
                                transaction.Entity.Id = Convert.ToInt32(await _hintDBRepository.GetUpdatedKey(oldOnlineId));
                            }
                            await _apiRepository.Delete(transaction.Entity);
                            break;
                        default:
                            break;
                    }
                }
                await _hintDBRepository.DeleteAllTransactions();
                await _hintDBRepository.CleanupKeys();
            }
            await GetAll();
            return true;
        }
    }
}