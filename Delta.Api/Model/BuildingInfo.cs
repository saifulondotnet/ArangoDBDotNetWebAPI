using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delta.Api.Model
{
    [Serializable]
    public class BuildingInfo
    {
        public string _id { get; set; }
        public int _key { get; set; }
        public string name { get; set; }
        public string buildingId { get; set; }
        public DateTime buildDate { get; set; }
        public string locationtype { get; set; }
        public string propid { get; set; }
        public string clientid { get; set; }

        public BuildingInfo() { }
    }
}
