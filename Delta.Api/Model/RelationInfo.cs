using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delta.Api.Model
{
    [Serializable]
    public class RelationInfo
    {
        public string _id { get; set; }
        public int _key { get; set; }
        public string sourceTable { get; set; }
        public string sourceId { get; set; }
        public string destinationTable { get; set; }
        public string destinationId { get; set; }
        public string type { get; set; }

        public RelationInfo() { }
    }
}
