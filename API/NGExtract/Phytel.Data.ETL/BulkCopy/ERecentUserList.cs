using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.Data.ETL.BulkCopy
{
    public class ERecentUserList
    {
        public int UserId { get; set; }
        public string MongoUserId { get; set; }
        public string MongoId { get; set; }
        public int Version { get; set; }
        public string MongoUpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string MongoRecordCreatedBy { get; set; }
        public DateTime? RecordCreatedOn { get; set; }
    }
}
