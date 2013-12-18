using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Runtime.Serialization;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/cohorts", "GET")]
    public class GetAllCohortsRequest : IAppDomainRequest
    {
        [ApiMember(Name = "ContractNumber", Description = "Contract number", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "path", DataType = "string", IsRequired = true)]        
        public string Token { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the Request", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Version { get; set; }

        public GetAllCohortsRequest() { }
    }
}
