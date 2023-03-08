using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delta.Api.Model
{
    [Serializable]
    public class ClientInfo
    {
        public string _id { get; set; }
        public int _key { get; set; }
        public string name { get; set; }
        public string clientid { get; set; }

        public ClientInfo() { }
    }
}
