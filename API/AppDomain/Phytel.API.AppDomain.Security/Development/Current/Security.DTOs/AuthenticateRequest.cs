using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.Security.DTO
{
    [Route("/{Version}/login", "POST")]
    public class AuthenticateRequest : IAppDomainRequest
    {
        public string Token { get; set; }
        public string APIKey { get; set; }
        public string Product { get; set; }

        public string Version { get; set; }

        public string ContractNumber { get; set; }

        public string Context { get; set; }
    }
}
