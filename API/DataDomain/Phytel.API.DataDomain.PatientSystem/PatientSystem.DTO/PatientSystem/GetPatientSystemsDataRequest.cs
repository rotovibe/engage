using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.PatientSystem.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/PatientSystems", "GET")]
    public class GetPatientSystemsDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "PatientId", Description = "ID of the patient to retrieve Patient System Information", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }
        
        [ApiMember(Name = "Context", Description = "Product Context requesting the PatientSystem", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = false)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }
    }
}
