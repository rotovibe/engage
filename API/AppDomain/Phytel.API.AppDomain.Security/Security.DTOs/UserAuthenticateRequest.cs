using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.Security.DTO
{
    [Route("/{Version}/user/login", "GET")]
    [Route("/{Version}/{ContractNumber}/user/login", "GET")]
    public class UserAuthenticateRequest //we don't inherit from IAppDomainRequest in the Security as that interface does not apply
    {
        [ApiMember(Name = "UserName", Description = "API Username requesting access", ParameterType = "Header", DataType = "string", IsRequired = true)]
        public string UserName { get; set; }

        [ApiMember(Name = "Password", Description = "API User Password", ParameterType = "Header", DataType = "string", IsRequired = true)]
        public string Password { get; set; }

        [ApiMember(Name = "APIKey", Description = "API License Key", ParameterType = "Header", DataType = "string", IsRequired = true)]
        public string APIKey { get; set; }

        [ApiMember(Name = "Context", Description = "Context Requesting Access", ParameterType = "Header", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to access", ParameterType = "Path", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "Path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }
    }
}
