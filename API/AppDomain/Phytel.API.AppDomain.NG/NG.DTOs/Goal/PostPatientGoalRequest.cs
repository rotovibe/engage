using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Api(Description = "A Request object to update a patient goal.")]
    [Route("/{Version}/{ContractNumber}/Patient/{PatientId}/Goal/{PatientGoalId}/Update", "POST")]
    public class PostPatientGoalRequest : IAppDomainRequest
    {
        [ApiMember(Name = "PatientId", Description = "Id of the patient for whom a goal is being created.", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        [ApiMember(Name = "PatientGoalId", Description = "PatientGoalId of the goal associated", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string PatientGoalId { get; set; }

        [ApiMember(Name = "Goal", Description = "PatientGoal being updated", ParameterType = "body", DataType = "PatientGoal", IsRequired = true)]
        public PatientGoal Goal { get; set; }
        
        [ApiMember(Name = "UserId", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "QueryString", DataType = "string", IsRequired = true)]
        public string Token { get; set; }
    }
}

     