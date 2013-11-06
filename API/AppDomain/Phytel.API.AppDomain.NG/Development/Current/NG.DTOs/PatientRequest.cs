using ServiceStack.ServiceHost;
using System.Runtime.Serialization;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/v1/{Product}/Contract/{ContractNumber}/patient")]
    public class PatientRequest
    {
           [ApiMember(Name = "ID", Description = "ID parameter",
               ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ID { get; set; }

           [ApiMember(Name = "Product", Description = "Product parameter will be defined in the route.",
               ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Product { get; set; }

           [ApiMember(Name = "ContractNumber", Description = "Contract parameter will be defined in the route.",
               ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

           [ApiMember(Name = "Token", Description = "Token parameter in Header",
               ParameterType = "Header Information", DataType = "string", IsRequired = true)]        
        public string Token { get; set; }
    }
}
