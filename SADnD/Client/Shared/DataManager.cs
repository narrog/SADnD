using SADnD.Shared;
using System.Linq.Expressions;

namespace SADnD.Client.Shared
{
    public class DataManager<TEntity> where TEntity : class
    {
        private readonly APIRepository<TEntity> _apiRepository;
        private List<TEntity> list;
        public DataManager(APIRepository<TEntity> apiRepository)
        {
            _apiRepository = apiRepository;
            list = new List<TEntity>();
            GetAll();
        }
        public IReadOnlyList<TEntity> List => list.AsReadOnly();
        private async Task EnsureList() {
            if (list == null)
                await GetAll();
        }
        private async Task GetAll()
        {
            list = (await _apiRepository.GetAll()).ToList();
        }
        public async Task Add(TEntity entity)
        {
            await _apiRepository.Insert(entity);
            await GetAll();
        }
        public async Task Update(TEntity entityToUpdate)
        {
            await _apiRepository.Update(entityToUpdate);
            var propertyInfo = typeof(TEntity).GetProperty("Id");
            if (propertyInfo == null)
                throw new Exception("Id nicht vorhanden");
            var index = list.FindIndex(x => propertyInfo.GetValue(x) == propertyInfo.GetValue(entityToUpdate));
            if (index == -1)
                throw new Exception("Entity nicht in Liste");
            list[index] = entityToUpdate;
        }
        public async Task Remove(TEntity entityToDelete)
        {
            if (await _apiRepository.Delete(entityToDelete))
                list.Remove(entityToDelete);
        }

        //public async Task<TEntity> GetByID(object id)
        //{
        //    await EnsureList();
        //    var propertyInfo = typeof(TEntity).GetProperty("Id");
        //    if (propertyInfo == null)
        //    {
        //        throw new InvalidOperationException($"Type {typeof(TEntity).Name} does not have a property named 'Id'.");
        //    }
        //    return List.FirstOrDefault(x => propertyInfo.GetValue(x) == id);
        //}
    }
}