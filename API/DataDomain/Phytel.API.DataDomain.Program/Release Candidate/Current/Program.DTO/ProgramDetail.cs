using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using System;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class ProgramDetail : PlanElementDetail
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string ContractProgramId { get; set; }
        public int ProgramState { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string Client { get; set; }
        public int Eligibility { get; set; }
        public string EligibilityRequirements { get; set; }
        public DateTime? EligibilityStartDate { get; set; }
        public DateTime? EligibilityEndDate { get; set; }
        public int Status { get; set; }
        public List<ObjectiveInfoData> ObjectivesData { get; set; }
        public List<ModuleDetail> Modules { get; set; }
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

        public ProgramAttributeData Attributes { get; set; }
    }
}
