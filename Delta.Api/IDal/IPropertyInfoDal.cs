using Delta.Api.Model;

namespace Delta.Api.IDal
{
    public interface IPropertyInfoDal
    {
        Task<PropertyInfo> GetByIdAsync();
        Task<List<PropertyInfo>> GetByListAsync();
    }
}
