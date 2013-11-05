using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Route("/v1/{Product}/Contract/{ContractNumber}/patient")]
    public class PatientRequest
    {
        public string ID { get; set; }
        public string Product { get; set; }
        public string ContractNumber { get; set; }
        public string Token { get; set; }
    }
}
