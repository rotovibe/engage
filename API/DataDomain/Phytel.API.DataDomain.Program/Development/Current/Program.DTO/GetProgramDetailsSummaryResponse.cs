using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using System;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class GetProgramDetailsSummaryResponse : IDomainResponse
    {
        public ProgramDetail Program { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class ProgramDetail
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
        public string ProgramStatus { get; set; }
        public List<ObjectivesDetail> ObjectivesInfo { get; set; }
        public List<ModuleDetail> Modules { get; set; }
        public string Version { get; set; }
    }

    public class ModuleDetail
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ObjectivesDetail> Objectives { get; set; }
        public List<ActionsDetail> Actions { get; set; }
        public int Status { get; set; }
    }

    public class ActionsDetail
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CompletedBy { get; set; }
        public List<ObjectivesDetail> Objectives { get; set; }
        public List<StepsDetail> Steps { get; set; }
        public int Status { get; set; }
    }

    public class StepsDetail
    {
        public string Id { get; set; }
        public int Type { get; set; }
        public string Question { get; set; }
        public string T { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string Text { get; set; }
        public string Ex { get; set; }
        public int Status { get; set; }
    }

    public class ObjectivesDetail
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public int Status { get; set; }
    }
}
