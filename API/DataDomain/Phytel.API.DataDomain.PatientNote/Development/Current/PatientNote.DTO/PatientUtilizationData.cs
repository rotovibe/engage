using System;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.PatientNote.DTO
{
    public class PatientUtilizationData
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string VisitType { get; set; }
        public string OtherType { get; set; }
        public DateTime? AdmitDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string Location { get; set; }
        public string OtherLocation { get; set; }
        public string Reason { get; set; }
        public string Disposition { get; set; }
        public string OtherDisposition { get; set; }
        public string SourceId { get; set; }
        public List<string> ProgramIds { get; set; }
        public string PSystem { get; set; }
        public string NoteType { get; set; }
        public string RecordCreatedBy { get; set; }
        public DateTime RecordCreatedOn { get; set; }
        public bool Admitted { get; set; }

        public PatientUtilizationData(){}
    }
}
