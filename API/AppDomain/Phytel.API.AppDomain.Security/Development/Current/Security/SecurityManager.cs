using Phytel.API.AppDomain.Security.DTOs;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.Security
{
    public static class SecurityManager
    {
        public static AuthenticateResponse GetToken(string username, string password, string apikey)
        {
            AuthenticateResponse response = null;

            IRestClient client = new JsonServiceClient();

            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("APIKey:{0}", "12345"));

            response = client.Post<AuthenticateResponse>("http://localhost:999/api/login",
                new AuthenticateRequest { Password = "Testing", Product = "NG", UserName = "NGMel" } as object);

            return response;
        }
    }
}
