using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.C3User.DTO
{
    [Route("/api/Data/login")]
    public class C3UserDataRequest
    {
        public string UserToken { get; set; }
    }
}
