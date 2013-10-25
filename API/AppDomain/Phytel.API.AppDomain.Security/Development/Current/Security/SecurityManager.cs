using Phytel.API.AppDomain.Security.DTOs;
using Phytel.API.DataDomain.Security.DTOs;
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

            TokenResponse wsResponse = client.Post<TokenResponse>("http://localhost:9999/api/login",
                new TokenRequest { APIKey="test12345", Password = "Testing", Product = "NG", UserName = "NGMel" } as object);

            response.Token  = wsResponse.Token;

            return response;
        }
    }
}
