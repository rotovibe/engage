using System;

namespace Phytel.Data.ETL.BulkCopy
{
    public class EPatientMedicationFrequency
    {
        public string MongoId { get; set; }
        public string LookUpType { get;set;}
        public string Name { get; set; }
        public string MongoPatientId { get; set; }
        public double Version { get; set; }
        public string MongoUpdatedBy { get; set; }
        public bool DeleteFlag { get; set; }
        public DateTime? TTLDate { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string MongoRecordCreatedBy { get; set; }
        public DateTime RecordCreatedOn { get; set; }
    }
}
