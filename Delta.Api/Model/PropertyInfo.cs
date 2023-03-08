using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delta.Api.Model
{
    [Serializable]
    public class PropertyInfo
    {
        public string _id { get; set; }
        public int _key { get; set; }
        public string name { get; set; }
        public string propid { get; set; }

        public PropertyInfo() { }
    }
}
