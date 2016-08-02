using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.Data.ETL.BulkCopy
{
    public class EObservationResponse
    {
        public int PatientId { get; set; }
        public string MongoPatientId { get; set; }
        public string MongoId { get; set; }
        public string MongoObservationId { get; set; }
        public int ObservationId { get; set; }
        public string Display { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal NumericValue { get; set; }
        public string NonNumericValue { get; set; }
        public string Source { get; set; }
        public string State { get; set; }
        public string Type { get; set; }
        public string Units { get; set; }
        public string AdministeredBy { get; set; }
        public string MongoUpdatedBy { get; set; }
        public int UpdatedById { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string MongoRecordCreatedBy { get; set; }
        public int RecordCreatedById { get; set; }
        public DateTime? RecordCreatedOn { get; set; }
        public int Version { get; set; }
        public DateTime? TTLDate { get; set; }
        public string Delete { get; set; }
    }
}
