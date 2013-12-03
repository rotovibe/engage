using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.Security.DTO
{
    [Route("/{Version}/login", "POST")]
    public class UserAuthenticateRequest //we don't inherit from IAppDomainRequest in the Security as that interface does not apply
    {
        [ApiMember(Name = "UserName", Description = "API Username requesting access", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserName { get; set; }

        [ApiMember(Name = "Password", Description = "API User Password", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Password { get; set; }

        [ApiMember(Name = "APIKey", Description = "API License Key", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string APIKey { get; set; }

        [ApiMember(Name = "Product", Description = "Product Code Requesting Access", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Product { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Version { get; set; }
    }
}
