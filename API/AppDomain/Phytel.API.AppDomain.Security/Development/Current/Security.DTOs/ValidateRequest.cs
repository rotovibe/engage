using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Security.DTO
{
    [Route("/api/Data/User")]
    public class ValidateRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string APIKey { get; set; }
        public string Product { get; set; }
    }
}
