using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/patient/{PatientId}/flagged/{Flagged}", "POST")]
    public class PutPatientFlaggedUpdateRequest : IAppDomainRequest
    {
        [ApiMember(Name = "UserId", Description = "UserId of the user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "PatientId", Description = "Id of the patient being updated", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        [ApiMember(Name = "Flagged", Description = "Flagged value of the patient being updated", ParameterType = "property", DataType = "string", IsRequired = true)]
        public int Flagged { get; set; }

        [ApiMember(Name = "Token", Description = "Token parameter", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        public PutPatientFlaggedUpdateRequest() { }
    }
}