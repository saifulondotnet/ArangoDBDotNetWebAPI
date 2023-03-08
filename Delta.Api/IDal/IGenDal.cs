namespace Delta.Api.IDal
{
    public interface IGenDal<T> where T : class
    {
        //IGenDal<T> Create(T entity);
        //IGenDal<T> Update(T entity);
        //IGenDal<T> Delete(T entity);
        //IGenDal<T> Get(string key);
        //IGenDal<T> Set(string key, T entity);
        Task<bool> DeleteAsync(string collectionName, string key);
        Task<bool> DeleteMatchedAsync(string collectionName, IList<string> selectors);
        Task<bool> DeleteByQueryAsync(string collectionName, string columnName, IList<string> selectors);
        Task<List<T>> GetAllAsync(string collectionName);
        Task<T> GetByKeyAsync(string collectionName, string key);
        Task<T> GetFirstAsync(string collectionName, string key, string value);
    }
}
