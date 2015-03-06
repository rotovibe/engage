using ServiceStack.ServiceHost;
using Phytel.API.Interface;

namespace Phytel.API.AppDomain.NG.DTO
{
     [Route("/{Version}/{ContractNumber}/lookup/languages", "GET")]
    public class GetAllLanguagesRequest : IAppDomainRequest
    {
        [ApiMember(Name = "ContractNumber", Description = "Contract parameter will be defined in the route.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Token parameter in Header", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the Request", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        public GetAllLanguagesRequest() { }
    }
}
