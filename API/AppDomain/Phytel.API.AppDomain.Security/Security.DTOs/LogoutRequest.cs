using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.Security.DTO
{
    [Api(Description="Request posted when a user attempts to log out of the application.")]
    [Route("/{Version}/{ContractNumber}/logout", "POST")]
    public class LogoutRequest //we don't inherit from IAppDomainRequest in the Security as that interface does not apply
    {
        [ApiMember(Name = "Token", Description = "One-Time Token given by the User Application Login process permitting access to the API", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }
        
        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "path", DataType = "double", IsRequired = true)]
        public double Version { get; set; }
    }
}
