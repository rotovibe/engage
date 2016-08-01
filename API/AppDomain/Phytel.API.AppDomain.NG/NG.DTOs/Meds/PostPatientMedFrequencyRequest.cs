using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/PatientMedSupp/Frequency/Insert", "POST")]
    public class PostPatientMedFrequencyRequest : IAppDomainRequest
    {
        [ApiMember(Name = "PatientMedFrequency", Description = "PatientMedFrequency details that need to be inserted.", ParameterType = "property", DataType = "PatientMedFrequency", IsRequired = true)]
        public PatientMedFrequency PatientMedFrequency { get; set; }

        [ApiMember(Name = "Token", Description = "Token parameter", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        public PostPatientMedFrequencyRequest() { }
    }
}