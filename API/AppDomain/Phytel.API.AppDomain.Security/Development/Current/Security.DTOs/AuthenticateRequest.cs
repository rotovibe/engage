using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.Security.DTO
{
    [Route("/api/security/login", "POST")]
    public class AuthenticateRequest
    {
        public string Token { get; set; }
        public string APIKey { get; set; }
        public string Product { get; set; }
    }

    [Route("/api/security/login", "GET")]
    public class AuthenticateGetRequest
    {
        public string Token { get; set; }
        public string APIKey { get; set; }
        public string Product { get; set; }
        public string Unknown { get; set; }
    }
}
