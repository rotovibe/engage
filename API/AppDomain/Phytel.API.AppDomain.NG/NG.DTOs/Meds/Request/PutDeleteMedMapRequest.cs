using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/{Version}/{ContractNumber}/MedicationMap/Delete", "PUT")]
    public class PutDeleteMedMapRequest : IAppDomainRequest
    {
        [ApiMember(Name = "MedicationMaps", Description = "MedicationMaps that need to be deleted.", ParameterType = "property", DataType = "PatientMedSupp", IsRequired = true)]
        public List<MedicationMap> MedicationMaps { get; set; }

        [ApiMember(Name = "Token", Description = "Token parameter", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }
    }
}