using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class Program : PlanElement
    {
        public string PatientId { get; set; }
        public string ContractProgramId { get; set; }
        public int ProgramState { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string Client { get; set; }
        public int Eligibility { get; set; } // remove : duplicate
        public string EligibilityRequirements { get; set; }
        public DateTime? EligibilityStartDate { get; set; }
        public DateTime? EligibilityEndDate { get; set; }
        public int Status { get; set; }
        public List<ObjectiveInfo> Objectives { get; set; }
        public List<Module> Modules { get; set; }
        public double Version { get; set; }
        public string Text { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string AuthoredBy { get; set; }
        public string TemplateName { get; set; }
        public string TemplateVersion { get; set; }
        public string ProgramVersion { get; set; }
        public DateTime? ProgramVersionUpdatedOn { get; set; }

        //public int EligibilityOverride { get; set; }
        //public int Enrollment { get; set; }
        //public bool GraduatedFlag { get; set; }
        //public string IneligibleReason { get; set; }
        //public string OptOut { get; set; }
        //public string OptOutReason { get; set; }
        //public DateTime? OptOutDate { get; set; }
        //public string RemovedReason { get; set; }
        //public string DidNotEnrollReason { get; set; }
        //public string DisEnrollReason { get; set; }
        //public string OverrideReason { get; set; }

        public ProgramAttribute Attributes { get; set; }
    }
}
