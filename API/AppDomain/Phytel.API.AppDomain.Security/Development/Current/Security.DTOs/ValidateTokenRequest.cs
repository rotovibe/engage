using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.Security.DTO
{
    [Route("/api/security/Token")]
    public class ValidateTokenRequest
    {
        public string Token { get; set; }
    }
}
