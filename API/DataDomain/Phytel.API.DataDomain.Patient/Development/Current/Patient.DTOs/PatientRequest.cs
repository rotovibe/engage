using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Patient.DTO
{
    [Route("/api/Data/login")]
    public class PatientRequest
    {
        public string UserToken { get; set; }
    }
}
