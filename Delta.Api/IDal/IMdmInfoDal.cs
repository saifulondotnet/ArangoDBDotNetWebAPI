using Delta.Api.Model;

namespace Delta.Api.IDal
{
    public interface IMdmInfoDal
    {
        Task<int> GetMdmInfosAsync();
        Task<IEnumerable<MdmInfo>> GetMdmInfoDocAsync();
        Task<MdmInfo> GetByKeyAsync();
    }
}
