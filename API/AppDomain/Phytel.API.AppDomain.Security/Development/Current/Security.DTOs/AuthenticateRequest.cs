using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.Security.DTOs
{
    [Route("/api/security/login")]
    public class AuthenticateRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string APIKey { get; set; }
        public string Product { get; set; }
    }
}
