using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Data.ETL.BulkCopy
{
    public class EPatientUtilizationProgram
    {
        public string Id { get; set; }
        public string PatientUtilizationId { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string RecordCreatedBy { get; set; }
        public DateTime? RecordCreatedOn { get; set; }
        public double Version { get; set; }
    }
}
