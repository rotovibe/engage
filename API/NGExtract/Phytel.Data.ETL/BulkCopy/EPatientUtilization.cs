using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Data.ETL.BulkCopy
{
    public class EPatientUtilization
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string NoteTypeId { get; set; }
        public string Reason { get; set; }
        public string VisitTypeId { get; set; }
        public string OtherVisitType { get; set; }
        public DateTime? AdmitDate { get; set; }
        public string Admitted { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string LocationId { get; set; }
        public string OtherLocation { get; set; }
        public string DispositionId { get; set; }
        public string OtherDisposition { get; set; }
        public string UtilizationSourceId { get; set; }
        public string DataSource { get; set; }
        // Standard fields
        public double Version { get; set; }
        public string UpdatedBy { get; set; }
        public string DeleteFlag { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string RecordCreatedBy { get; set; }
        public DateTime RecordCreatedOn { get; set; }
    }
}
