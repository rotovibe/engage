using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.Security.DTO
{
    [Route("/{Version}/login", "POST")]
    public class AuthenticateRequest
    {
        public string Token { get; set; }
        public string APIKey { get; set; }
        public string Product { get; set; }
    }
}
