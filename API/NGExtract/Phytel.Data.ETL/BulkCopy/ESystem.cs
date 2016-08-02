using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Data.ETL.BulkCopy
{
    public class ESystem
    {
        public string MongoId { get; set; }
        public string Name { get; set; }
        public string Field { get; set; }
        public string DisplayLabel { get; set; }
        public string Status { get; set; }
        public string Primary { get; set; }
        public double Version { get; set; }
        public string UpdatedBy { get; set; }
        public string DeleteFlag { get; set; }
        public DateTime? TTLDate { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string RecordCreatedBy { get; set; }
        public DateTime RecordCreatedOn { get; set; }
    }
}
