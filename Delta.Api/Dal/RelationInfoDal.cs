using ArangoDBNetStandard;
using Delta.Api.DbContext;
using Delta.Api.IDal;
using Delta.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Delta.Api.Dal
{
    public class RelationInfoDal : GenDal<RelationInfo>, IRelationInfoDal
    {
        private readonly IArangoDBContext _dbContext;
        public RelationInfoDal(IArangoDBContext dBContext):base(dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task<List<RelationInfo>> GetRelationsAsync(string sourceCollectionName, string relationshipCollection,string? destinationCollectionName=null)
        {
            Dictionary<string, object> bindValues = new Dictionary<string, object>()
            {
                {"@relationshipCollection",relationshipCollection },
                {"sourceCollectionName",sourceCollectionName }
            };
            //string qry = "for doc in relationInfo "+Environment.NewLine;
            string qry = "for doc in @@relationshipCollection " + Environment.NewLine;
            qry += " filter doc.sourceTable==@sourceCollectionName " + Environment.NewLine;
            if (destinationCollectionName != null)
            {
                qry += " && doc.destinationTable==@destinationCollectionName" + Environment.NewLine;
                bindValues.Add("destinationCollectionName", destinationCollectionName);
            }
            qry += " return doc";
            
            var response = await _dbContext.GetDataBase<ArangoDBClient>().Cursor.PostCursorAsync<RelationInfo>(qry, bindValues);
            return response.Result.ToList();
        }

    }
}
