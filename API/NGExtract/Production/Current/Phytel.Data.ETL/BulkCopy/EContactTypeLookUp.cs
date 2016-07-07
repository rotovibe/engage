using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Data.ETL.BulkCopy
{
    public class EContactTypeLookUp
    {
        public string MongoId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string ParentId { get; set; }
        public string Group { get; set; }
        public bool Active { get; set; }
        // Standard fields
        public double Version { get; set; }
        public bool DeleteFlag { get; set; }
        public DateTime? TTLDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string RecordCreatedBy { get; set; }
        public DateTime RecordCreatedOn { get; set; }
    }
}
