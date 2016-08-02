using System;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class ProgramAttribute
    {
            public string Id { get; set; }
            public string PlanElementId { get; set; }
            public int Status { get; set; }
            public string AuthoredBy { get; set; }
            public int Locked { get; set; }
            public string IneligibleReason { get; set; }
            public int Eligibility { get; set; }
            public int Enrollment { get; set; }
            public int GraduatedFlag { get; set; }
            public bool OptOut { get; set; }
            public string Population { get; set; }
            public string RemovedReason { get; set; }
            public string DidNotEnrollReason { get; set; }
            public string OverrideReason { get; set; }
            public string CompletedBy { get; set; }
            public int Completed { get; set; }
            public DateTime? DateCompleted { get; set; }

            //public DateTime? AttrEndDate { get; set; }  Sprint 12
            //public DateTime? AttrStartDate { get; set; } Sprint 12
            // public DateTime? AssignedOn { get; set; } Sprint 12
            // public string AssignedBy { get; set; } Sprint 12
            // public string AssignedTo { get; set; } Sprint 12
    }
}
