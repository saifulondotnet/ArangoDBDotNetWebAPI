using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delta.Api.Model
{
    [Serializable]
    public class MdmInfo
    {
        public string _id { get; set; }
        public int _key { get; set; }
        public string collName { get; set; }
        public Schema[] schema { get; set; }

        public MdmInfo() { }
    }
}
