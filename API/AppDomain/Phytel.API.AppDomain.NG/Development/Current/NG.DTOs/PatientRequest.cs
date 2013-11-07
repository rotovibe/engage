using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Runtime.Serialization;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Context}/{Version}/Contract/{ContractNumber}/patient", "POST")]
    [Route("/{Context}/{Version}/Contract/{ContractNumber}/patient/{PatientID}", "GET")]
    public class PatientRequest : IAppDomainRequest
    {
        [ApiMember(Name = "PatientID", Description = "ID parameter", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientID { get; set; }

        [ApiMember(Name = "Context", Description = "Product parameter will be defined in the route.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract parameter will be defined in the route.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Token parameter in Header", ParameterType = "Header Information", DataType = "string", IsRequired = true)]        
        public string Token { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the Request", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Version { get; set; }
    }
}
