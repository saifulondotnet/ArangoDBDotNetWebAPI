using ArangoDBNetStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delta.Api.DbContext
{
    public interface IArangoDBContext
    {
        T GetDataBase<T>();
        //IArangoDBClient GetDb();
    }
}
