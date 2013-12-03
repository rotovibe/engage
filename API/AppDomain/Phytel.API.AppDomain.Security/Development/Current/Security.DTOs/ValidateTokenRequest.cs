using ServiceStack;

namespace Phytel.API.AppDomain.Security.DTO
{
    [Route("/{Context}/{Version}/token", "POST")]
    public class ValidateTokenRequest //we don't inherit from IAppDomainRequest in the Security as that interface does not apply
    {
        [ApiMember(Name = "Token", Description = "Authentication Token to permit caller to API", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Version { get; set; }
    }
}
