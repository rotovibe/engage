using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Patient.DTO
{
    [Route("/{Context}/{Version}/Contract/{ContractNumber}/patientlist", "POST")]
    [Route("/{Context}/{Version}/Contract/{ContractNumber}/patientlist/caremanager/{CareManagerID}", "GET")]
    public class PatientListRequest : IDataDomainRequest
    {
        [ApiMember(Name = "CareManagerID", Description = "ID of the care manager to retrieve patients for", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string CareManagerID { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the patient", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Version { get; set; }
    }
}
