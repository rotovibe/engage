using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetPatientProgramDetailsSummaryResponse : IDomainResponse
    {
        public ProgramDetailNG Program { get; set; }
        public ResponseStatus Status { get; set; }
        public string Version { get; set; }
    }

    public class ProgramDetailNG
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
        public List<ObjectivesDetailNG> ObjectivesInfo { get; set; }
        public List<ModuleDetailNG> Modules { get; set; }
        public string Version { get; set; }
    }

    public class ModuleDetailNG
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ObjectivesDetailNG> Objectives { get; set; }
        public List<ActionsDetailNG> Actions { get; set; }
        public int Status { get; set; }
    }

    public class ActionsDetailNG
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CompletedBy { get; set; }
        public List<ObjectivesDetailNG> Objectives { get; set; }
        public List<StepsDetailNG> Steps { get; set; }
        public int Status { get; set; }
    }

    public class StepsDetailNG
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

    public class ObjectivesDetailNG
    {
        public string Id { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public int Status { get; set; }
    }
}
