using ArangoDBNetStandard;
using Delta.Api.DbContext;
using Delta.Api.IDal;
using Delta.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delta.Api.Dal
{
    public class PropertyInfoDal : IPropertyInfoDal
    {
        private readonly IArangoDBContext _dbContext;
        public PropertyInfoDal(IArangoDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<PropertyInfo> GetByIdAsync()
        {
            try
            {
                string qry = @"for rec in propertyInfo
                                filter rec.propid==""PD-001""
                                return rec";
                //var coll = await _dbContext.GetDb().Cursor.PostAdvanceCursorAsync<IEnumerable<MdmInfo>>(qry);
                //return coll as IEnumerable<MdmInfo>;

                var coll = await _dbContext.GetDataBase<ArangoDBClient>().Document.GetDocumentAsync<PropertyInfo>("propertyInfo", "3");
                return coll;

                //var response = await _dbContext.GetDb().Cursor.PostCursorAsync<MdmInfo>(qry);
                //return response.Result.ToList();

                //return coll.Result.AsEnumerable();
                //var coll = await _dbContext.GetDb().Document.GetDocumentsAsync<MdmInfo>("mdmInfo",null);
                //return coll as IEnumerable<MdmInfo>;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                //throw;
            }
            return null;
        }

        public async Task<List<PropertyInfo>> GetByListAsync()
        {
            try
            {
                string qry = @"for rec in propertyInfo
                                return rec";
                var response = await _dbContext.GetDataBase<ArangoDBClient>().Cursor.PostCursorAsync<PropertyInfo>(qry);
                return response.Result.ToList();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                //throw;
            }
            return null;
        }

    }
}
