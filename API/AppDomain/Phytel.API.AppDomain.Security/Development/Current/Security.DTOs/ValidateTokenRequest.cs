using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Security.DTO
{
    [Route("/api/Data/Token")]
    public class ValidateTokenRequest
    {
        public string Token { get; set; }
    }
}
