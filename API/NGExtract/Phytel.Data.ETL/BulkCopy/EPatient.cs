using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.Data.ETL.BulkCopy
{
    public class EPatient
    {
        public string MongoPatientSystemId { get; set; }
        public string MongoId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PreferredName { get; set; }
        public string Suffix { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Priority { get; set; }
        public string Background { get; set; }
        public int Version { get; set; }
        public string MongoUpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string MongoRecordCreatedBy { get; set; }
        public DateTime? RecordCreatedOn { get; set; }
        public DateTime? TTLDate { get; set; }
        public string Delete { get; set; }
        public string ExtraElements { get; set; }
        public string FSSN { get; set; }
        //   public int? LSSN { get; set; }
        public string LSSN { get; set; }
        public string DataSource { get; set; }
        public string MongoMaritalStatusId { get; set; }
        public string Protected { get; set; }
        public string Deceased { get; set; }
        public string Status { get; set; }
        public string MongoReasonId { get; set; }
        public string StatusDataSource { get; set; }
        public string ClinicalBackGround { get; set; }
    }
}
