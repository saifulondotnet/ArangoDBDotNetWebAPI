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
    public class MdmInfoDal : IMdmInfoDal
    {
        private readonly IArangoDBContext _dbContext;
        public MdmInfoDal(IArangoDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<int> GetMdmInfosAsync()
        {
            try
            {
                //var coll = await _dbContext.GetDataBase<IArangoDBClient>().Collection.GetCollectionAsync("MdmInfo");
                //return coll;
                var coll = await _dbContext.GetDataBase<ArangoDBClient>().Collection.GetCollectionAsync("mdmInfo");
                if (coll != null)
                {
                    return 1;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                //throw;
            }
            return 0;
        }

        public async Task<IEnumerable<MdmInfo>> GetMdmInfoDocAsync()
        {
            try
            {
                string qry = @"for mdm in mdmInfo
                                return mdm";

                var response = await _dbContext.GetDataBase<ArangoDBClient>().Cursor.PostCursorAsync<MdmInfo>(qry);
                return response.Result.ToList();

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

        public async Task<MdmInfo> GetByKeyAsync()
        {
            try
            {
                string qry = @"for mdm in mdmInfo
                                mdm._key==""1""
                                return mdm";

                var coll = await _dbContext.GetDataBase<ArangoDBClient>().Document.GetDocumentAsync<MdmInfo>("mdmInfo", "3");
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

        
    }
}
