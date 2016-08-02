using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.Data.ETL.BulkCopy
{
    public class EPatientSystem
    {
        public string MongoPatientId { get; set; }
        public string MongoId { get; set; }
        public string Label { get; set; }
        public string SystemId { get; set; }
        public string SystemName { get; set; }
        public string MongoUpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string MongoRecordCreatedBy { get; set; }
        public DateTime RecordCreatedOn { get; set; }
        public int Version { get; set; }
        public DateTime? TTLDate { get; set; }
        public string Delete { get; set; }
        public string ExtraElements { get; set; }
        public string Value { get; set; }
        public string DataSource { get; set; }
        public string Status { get; set; }
        public string Primary { get; set; }
        public string SysId { get; set; }
    }
}
