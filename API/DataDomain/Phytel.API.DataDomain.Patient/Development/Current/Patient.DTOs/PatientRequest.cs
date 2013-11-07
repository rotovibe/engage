using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Patient.DTO
{
    [Route("/{Context}/{Version}/Contract/{ContractNumber}/patient", "POST")]
    [Route("/{Context}/{Version}/Contract/{ContractNumber}/patient/{PatientID}", "GET")]
    public class PatientRequest : IDataDomainRequest
    {
        public string PatientID { get; set; }
        public string Context { get; set; }
        public string ContractNumber { get; set; }
        public string Version { get; set; }
    }
}
