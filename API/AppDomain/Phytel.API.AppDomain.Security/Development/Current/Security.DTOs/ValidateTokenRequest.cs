using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.Security.DTO
{
    [Route("/{Version}/token", "POST")]
    public class ValidateTokenRequest
    {
        public string Token { get; set; }
    }
}
