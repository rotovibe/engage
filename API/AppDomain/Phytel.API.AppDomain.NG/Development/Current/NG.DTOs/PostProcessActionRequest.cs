using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Runtime.Serialization;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/Patient/{PatientId}/Program/Module/Action/Process", "POST")]
    public class PostProcessActionRequest : IAppDomainRequest
    {
        [ApiMember(Name = "UserID", Description = "UserID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "Program", Description = "Program entitiy", ParameterType = "body", DataType = "Program", IsRequired = true)]
        public Program Program { get; set; }

        [ApiMember(Name = "PatientId", Description = "PatientId", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        [ApiMember(Name = "ActionId", Description = "Id of action being processed", ParameterType = "body", DataType = "string", IsRequired = true)]
        public string ActionId { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "QueryString", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Version { get; set; }
    }
}
