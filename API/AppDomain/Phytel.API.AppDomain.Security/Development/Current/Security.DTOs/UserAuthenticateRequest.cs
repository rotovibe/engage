using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.Security.DTO
{
    [Route("/{Version}/login", "POST")]
    public class UserAuthenticateRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string APIKey { get; set; }
        public string Product { get; set; }
    }
}
