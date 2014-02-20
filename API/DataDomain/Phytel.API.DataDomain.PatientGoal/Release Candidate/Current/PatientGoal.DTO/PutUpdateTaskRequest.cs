using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Goal/{PatientGoalId}/Task/{TaskId}/Update", "PUT")]
    public class PutUpdateTaskRequest : IDataDomainRequest
    {
        [ApiMember(Name = "TaskId", Description = "TaskId of the task to update", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string TaskId { get; set; }

        [ApiMember(Name = "PatientGoalId", Description = "Id of the PatientGoal.", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientGoalId { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = false)]
        public PatientTaskData Task { get; set; }

        [ApiMember(Name = "TaskIdsList", Description = "taskid list to reference", ParameterType = "body", DataType = "string", IsRequired = false)]
        public List<string> TaskIdsList { get; set; }

        [ApiMember(Name = "PatientId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string PatientId { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the PatientGoal", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Version { get; set; }
    }
}
