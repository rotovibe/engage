﻿using System;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.PatientNote.DTO
{
    public class PatientUtilizationData
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string VisitTypeId { get; set; }
        public string OtherType { get; set; }
        public DateTime? AdmitDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string LocationId { get; set; }
        public string OtherLocation { get; set; }
        public string Reason { get; set; }
        public string DispositionId { get; set; }
        public string OtherDisposition { get; set; }
        public string SourceId { get; set; }
        public List<string> ProgramIds { get; set; }
        public string SystemSource { get; set; }
        public string TypeId { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Admitted { get; set; }

        public PatientUtilizationData(){}
    }
}
