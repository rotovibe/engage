using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.Data.ETL.BulkCopy
{
    public class EMode
    {
        public int ContactId { get; set; }
        public string MongoContactId { get; set; }
        public int CommModeLookUpId { get; set; }
        public string MongoCommModeId { get; set; }
        public string Preferred { get; set; }
        public string OptOut { get; set; }
        public int Version { get; set; }
        public string MongoUpdatedBy { get; set; }
        public int UpdatedById { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string MongoRecordCreatedBy { get; set; }
        public int RecordCreatedById { get; set; }
        public DateTime? RecordCreatedOn { get; set; }
        public DateTime? TTLDate { get; set; }
    }
}
