using Phytel.API.Interface;
using ServiceStack;

namespace Phytel.API.DataDomain.Patient.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/patientdetails", "POST")]
    public class GetPatientsDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "PatientIDs", Description = "Array of PatientIDs", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string[] PatientIDs { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the patient", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Version { get; set; }
    }
}
