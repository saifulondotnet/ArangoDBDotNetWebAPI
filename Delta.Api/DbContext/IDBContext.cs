namespace Delta.Api.DbContext
{
    public interface IDBContext
    {
        T GetDataBase<T>();
    }
}
