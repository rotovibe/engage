using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.Common
{
    public class AppData : IAppData
    {
       public  string Id { get; set; }
        public string DataSource { get; set; }
   //     public DateTime RecordCreatedOn { get; set; }
        public string ExternalRecordId { get; set; }
    }
}
