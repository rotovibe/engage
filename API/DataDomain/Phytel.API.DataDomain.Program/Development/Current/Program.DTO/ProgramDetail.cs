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
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string EligibilityRequirements { get; set; }
        public DateTime? EligibilityStartDate { get; set; }
        public DateTime? EligibilityEndDate { get; set; }
        public int Status { get; set; }
        public List<ObjectivesDetail> ObjectivesInfo { get; set; }
        public List<ModuleDetail> Modules { get; set; }
        public string Version { get; set; }
        public string Text { get; set; }
    }
}
