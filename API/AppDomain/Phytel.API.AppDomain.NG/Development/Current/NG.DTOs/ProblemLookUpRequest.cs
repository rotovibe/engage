using ServiceStack.ServiceHost;
using Phytel.API.Interface;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
     [Route("/{Context}/{Version}/{ContractNumber}/problemslookup", "GET")]
    public class ProblemLookUpRequest : IAppDomainRequest
    {
        [ApiMember(Name = "Context", Description = "Product parameter will be defined in the route.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract parameter will be defined in the route.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Token parameter in Header", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the Request", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Version { get; set; }
    }
}
