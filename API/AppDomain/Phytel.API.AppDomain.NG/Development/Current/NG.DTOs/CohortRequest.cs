using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Runtime.Serialization;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/cohort", "POST")]
    public class CohortRequest : IAppDomainRequest
    {
        [ApiMember(Name = "Context", Description = "Product parameter will be defined in the route.", ParameterType = "path", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract parameter will be defined in the route.", ParameterType = "path", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "header", DataType = "string", IsRequired = false)]        
        public string Token { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the Request", ParameterType = "path", DataType = "string", IsRequired = false)]
        public string Version { get; set; }
    }
}
