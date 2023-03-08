using Delta.Api.Model;

namespace Delta.Api.IDal
{
    public interface IRelationInfoDal : IGenDal<RelationInfo>
    {
        Task<List<RelationInfo>> GetRelationsAsync(string sourceCollectionName, string relationshipCollection, string? destinationCollectionName = null);
    }
}
