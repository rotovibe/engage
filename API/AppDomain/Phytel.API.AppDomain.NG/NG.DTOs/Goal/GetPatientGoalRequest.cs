using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/Patient/{PatientId}/Goal/{Id}", "GET")]
    public class GetPatientGoalRequest : IAppDomainRequest
    {
        [ApiMember(Name = "PatientId", Description = "Id of the patient for whom a goal is being created.", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        [ApiMember(Name = "Id", Description = "Id of the PatientGoal being requested", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Id { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the PatientGoal", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "QueryString", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        public GetPatientGoalRequest() { }
    }
}
