using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class ProgramAttributeData
    {
        public string Id { get; set; }
        public string PlanElementId { get; set; }
        //public DateTime? AttrStartDate { get; set; }
        //public DateTime? AttrEndDate { get; set; }
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
        //public DateTime? AssignedOn { get; set; }
        //public string AssignedBy { get; set; }
        //public string AssignedTo { get; set; }
        public string CompletedBy { get; set; }
        public int Completed { get; set; }
        public DateTime? DateCompleted { get; set; }
    }
}
