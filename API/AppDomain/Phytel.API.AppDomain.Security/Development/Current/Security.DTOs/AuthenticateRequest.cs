using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.Security.DTO
{
    [Api(Description="Request posted when a product needs to authenticate itself to the API and receive a usage token")]
    [Route("/{Version}/login", "POST")]
    public class AuthenticateRequest //we don't inherit from IAppDomainRequest in the Security as that interface does not apply
    {
        [ApiMember(Name = "Token", Description = "One-Time Token given by the User Application Login process permitting access to the API", ParameterType = "body", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "APIKey", Description = "API License Key unique to the product requesting access", ParameterType = "body", DataType = "string", IsRequired = true)]
        public string APIKey { get; set; }

        [ApiMember(Name = "Product", Description = "Product Code requesting access to the API", ParameterType = "body", DataType = "string", IsRequired = true)]
        public string Product { get; set; }
        
        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "string", IsRequired = false)]
        public string Version { get; set; }
    }
}
