using ArangoDBNetStandard;
using ArangoDBNetStandard.Transport.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delta.Api.DbContext
{
    public class ArangoDBContext : IArangoDBContext
    {
        private readonly string host = "http://127.0.0.1:8529";
        private readonly string db = "_system";
        private readonly string uid = "root";
        private readonly string pwd = "";

        private readonly IArangoDBClient _arangoDbClient;
        public ArangoDBContext() 
        {
            var transport = HttpApiTransport.UsingBasicAuth(new Uri(host), db, uid, pwd);
            _arangoDbClient = new ArangoDBClient(transport);
        }
        public T GetDataBase<T>()
        {
            return (T)_arangoDbClient;
        }

        //public IArangoDBClient GetDb()
        //{
        //    return _arangoDbClient;
        //}
    }
}
