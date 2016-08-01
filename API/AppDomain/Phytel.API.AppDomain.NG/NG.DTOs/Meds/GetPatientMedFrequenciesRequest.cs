using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/PatientMedSupp/Frequency/{PatientId}", "GET")]
    public class GetPatientMedFrequenciesRequest : IAppDomainRequest
    {
        [ApiMember(Name = "PatientId", Description = "ID of the patient whose frequencies are being requested", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }
                
        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "QueryString", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        public GetPatientMedFrequenciesRequest() { }
    }
}
