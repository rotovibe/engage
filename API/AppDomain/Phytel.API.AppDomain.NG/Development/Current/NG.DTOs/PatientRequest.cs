using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/api/ng/patient")]
    public class PatientRequest
    {
        public int ID { get; set; }
    }
}
