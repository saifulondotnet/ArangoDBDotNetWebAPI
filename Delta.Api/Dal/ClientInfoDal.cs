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
    public class ClientInfoDal : GenDal<ClientInfo>, IClientInfoDal
    {
        private readonly IArangoDBContext _dbContext;
        public ClientInfoDal(IArangoDBContext dBContext):base(dBContext)
        {
            _dbContext = dBContext;
        }


    }
}
