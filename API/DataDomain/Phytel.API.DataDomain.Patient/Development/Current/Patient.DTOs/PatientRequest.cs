using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Patient.DTO
{
    [Route("/[context]/data/patient")]
    public class PatientRequest
    {
        public string PatientID { get; set; }
    }
}
