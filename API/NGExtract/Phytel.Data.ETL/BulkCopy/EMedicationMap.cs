using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.Data.ETL.BulkCopy
{
    public class EMedicationMap
    {
        public string MongoId { get; set; }
        public string FullName { get; set; }
        public string SubstanceName { get; set; }
        public string Route { get; set; }
        public string Form { get; set; }
        public string Strength { get; set; }
        public double Version { get; set; }
        public bool DeleteFlag { get; set; }
        public bool Custom { get; set; }
        public bool Verified { get; set; }
        public DateTime? TTLDate { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string MongoRecordCreatedBy { get; set; }
        public DateTime RecordCreatedOn { get; set; }
        public string MongoUpdatedBy { get; set; }
    }
}
