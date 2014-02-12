using System;
using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Goal/{PatientGoalId}/Update", "PUT")]
    public class PutGoalDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "PatientGoalId", Description = "PatientGoalId", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientGoalId { get; set; }

        [ApiMember(Name = "FocusAreas", Description = "Focus Areas", ParameterType = "property", DataType = "List of string", IsRequired = false)]
        public List<string> FocusAreas { get; set; }

        [ApiMember(Name = "Name", Description = "Goal name", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Name { get; set; }

        [ApiMember(Name = "Source", Description = "Source of goal", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Source { get; set; }

        [ApiMember(Name = "Programs", Description = "Programs available for the patient", ParameterType = "property", DataType = "List of string", IsRequired = false)]
        public List<string> Programs { get; set; }

        [ApiMember(Name = "Type", Description = "Long term or short term goals", ParameterType = "property", DataType = "String", IsRequired = false)]
        public string Type { get; set; }

        [ApiMember(Name = "Status", Description = "Status of the goal", ParameterType = "property", DataType = "String", IsRequired = false)]
        public string Status { get; set; }

        [ApiMember(Name = "StartDate", Description = "Start date of the goal", ParameterType = "property", DataType = "DateTime", IsRequired = false)]
        public DateTime StartDate { get; set; }

        [ApiMember(Name = "EndDate", Description = "End date of the goal", ParameterType = "property", DataType = "DateTime", IsRequired = false)]
        public DateTime EndDate { get; set; }

        [ApiMember(Name = "TargetValue", Description = "Target Value of the goal", ParameterType = "property", DataType = "String", IsRequired = false)]
        public string TargetValue { get; set; }

        [ApiMember(Name = "TargetDate", Description = "Target date of the goal", ParameterType = "property", DataType = "DateTime", IsRequired = false)]
        public DateTime TargetDate { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the PatientGoal", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Version { get; set; }
    }
}
