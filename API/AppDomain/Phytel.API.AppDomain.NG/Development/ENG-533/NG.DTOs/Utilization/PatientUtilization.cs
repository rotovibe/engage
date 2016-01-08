using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO.Utilization
{
    public class PatientUtilization
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string VisitTypeId { get; set; }
        public string OtherType { get; set; }
        public DateTime? AdmitDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string LocationId { get; set; }
        public string OtherLocation { get; set; }
        public string Text { get; set; }
        public string DispositionId { get; set; }
        public string OtherDisposition { get; set; }
        public string UtilizationSourceId { get; set; }
        public List<string> ProgramIds { get; set; }
        public string DataSource { get; set; }
        public string TypeId { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Admitted { get; set; }
        public string UpdatedById { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public PatientUtilization()
        {
        }
    }
}
