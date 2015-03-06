using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Runtime.Serialization;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/Patient/{PatientId}/Program/Module/Action/Repeat", "POST")]
    public class PostRepeatActionRequest : IAppDomainRequest, IProcessActionRequest
    {
        [ApiMember(Name = "UserID", Description = "UserID of the user making the request (Internally used ONLY)", ParameterType = "body", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "ProgramId", Description = "ProgramId", ParameterType = "body", DataType = "String", IsRequired = true)]
        public string ProgramId { get; set; }

        [ApiMember(Name = "Action", Description = "Action entitiy", ParameterType = "body", DataType = "Actions", IsRequired = true)]
        public Actions Action { get; set; }

        [ApiMember(Name = "PatientId", Description = "PatientId", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "QueryString", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }
    }
}
