using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.Data.ETL.BulkCopy
{
    public class EContact
    {
        public int ContactId { get; set; }
        public int PatientId { get; set; }
        public string MongoPatientId { get; set; }
        public string MongoId { get; set; }
        public int Version { get; set; }
        public string MongoUpdatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string MongoRecordCreatedBy { get; set; }
        public int RecordCreatedById { get; set; }
        public DateTime? RecordCreatedOn { get; set; }
        public string ResourceId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PreferredName { get; set; }
        public string Gender { get; set; }
        public string MongoTimeZone { get; set; }
        public int TimeZone { get; set; }
        public DateTime? TTLDate { get; set; }
        public string Delete { get; set; }
        public string ExtraElements { get; set; }
    }
}
