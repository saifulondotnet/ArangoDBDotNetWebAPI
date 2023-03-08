using ArangoDBNetStandard;
using Delta.Api.DbContext;
using Delta.Api.IDal;
using Delta.Api.Model;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delta.Api.Dal
{
    public class GenDal<T> : IGenDal<T> where T : class
    {
        private readonly IArangoDBContext _dbContext;
        public GenDal(IArangoDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<List<T>> GetAllAsync(string collectionName)
        {
            string qry = "for rec in @collectionName" + Environment.NewLine;
            qry += " return rec";
            Dictionary<string, object> bindValues = new Dictionary<string, object>() { { "collectionName", collectionName } };
            var response = await _dbContext.GetDataBase<IArangoDBClient>().Cursor.PostCursorAsync<T>(qry);
            return response.Result.ToList();
        }

        public async Task<T> GetFirstAsync(string collectionName, string key, string value)
        {
            collectionName = collectionName.Replace("'", string.Empty);
            string qry = "for doc in " + collectionName + " " + Environment.NewLine;
            qry += " filter doc.@key==@value " + Environment.NewLine;
            qry += " limit 1" + Environment.NewLine;
            qry += " return doc";
            Dictionary<string, object> bindValues = new Dictionary<string, object>()
            {
                {"key", key },
                {"value", value }
            };
            var response = await _dbContext.GetDataBase<IArangoDBClient>().Cursor.PostCursorAsync<T>(qry, bindValues);
            return response.Result.FirstOrDefault();
        }

        public async Task<bool> DeleteByQueryAsync(string collectionName, string columnName, IList<string> selectors)
        {
            collectionName = collectionName.Replace("'", string.Empty);
            string filter = string.Empty;
            foreach (var item in selectors)
            {
                filter += filter == string.Empty ? " doc.@columnName=='" + item + "' " : " or doc.@columnName=='" + item + "' ";
            }
            string qry = "for doc in " + collectionName + " " + Environment.NewLine;
            qry += " filter " + filter + Environment.NewLine;
            qry += " remove doc in " + collectionName + " ";
            Dictionary<string, object> bindValues = new Dictionary<string, object>()
            {
                {"columnName", columnName }
            };
            try
            {
                await _dbContext.GetDataBase<IArangoDBClient>().Cursor.PostCursorAsync<T>(qry, bindValues);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<T> GetByKeyAsync(string collectionName, string key)
        {
            var coll = await _dbContext.GetDataBase<IArangoDBClient>().Document.GetDocumentAsync<T>(collectionName, key);
            return coll;
        }
        public async Task<bool> DeleteAsync(string collectionName, string key)
        {
            var response = await _dbContext.GetDataBase<IArangoDBClient>().Document.DeleteDocumentAsync(collectionName, key);
            return true;
        }

        public async Task<bool> DeleteMatchedAsync(string collectionName, IList<string> selectors)
        {
            var response = await _dbContext.GetDataBase<IArangoDBClient>().Document.DeleteDocumentsAsync(collectionName, selectors);
            if (response[0].Error == true)
            {
                return false;
            }
            return true;
        }

    }
}
