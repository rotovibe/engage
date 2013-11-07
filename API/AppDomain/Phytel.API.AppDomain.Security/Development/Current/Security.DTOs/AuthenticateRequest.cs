using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.Security.DTO
{
    [Route("/{Version}/login", "POST")]
    public class AuthenticateRequest //we don't inherit from IAppDomainRequest in the Security as that interface does not apply
    {
        [ApiMember(Name = "Token", Description = "User One-Time Token permitting access to the API", ParameterType = "body", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "APIKey", Description = "API License Key", ParameterType = "body", DataType = "string", IsRequired = true)]
        public string APIKey { get; set; }

        [ApiMember(Name = "Product", Description = "Product Code Requesting Access", ParameterType = "body", DataType = "string", IsRequired = true)]
        public string Product { get; set; }
        
        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "string", IsRequired = false)]
        public string Version { get; set; }
    }
}
