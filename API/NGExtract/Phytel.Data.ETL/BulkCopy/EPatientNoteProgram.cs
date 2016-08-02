using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.Data.ETL.BulkCopy
{
    public class EPatientNoteProgram
    {
        public string MongoId { get; set; }
        public string PatientNoteMongoId { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string RecordCreatedBy { get; set; }
        public DateTime? RecordCreatedOn { get; set; }
        public int Version { get; set; }
    }
}
