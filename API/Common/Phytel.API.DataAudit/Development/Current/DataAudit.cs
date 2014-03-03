using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataAudit
{
    [Serializable]
    public class DataAudit
    {
        public string UserId { get; set; }
        public string Type { get; set; }  //Insert, Update, Delete
        public string EntityType { get; set; }
        public string EntityID { get; set; }
        public object Entity { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Contract { get; set; }


    }
}
