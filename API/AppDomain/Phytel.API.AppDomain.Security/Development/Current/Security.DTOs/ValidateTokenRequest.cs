using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.Security.DTO
{
    [Route("/{Version}/token", "POST")]
    public class ValidateTokenRequest : IAppDomainRequest
    {
        public string Token { get; set; }

        public string Version { get; set; }

        public string ContractNumber { get; set; }

        public string Context { get; set; }
    }
}
