using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Patient.DTO
{
    [Route("/{Context}/data/patient")]
    public class DataPatientRequest
    {
        public string PatientID { get; set; }
        public string Context { get; set; }
        public string ContractID { get; set; }
    }
}
