using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.DataDomain.Security.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;

namespace Phytel.API.AppDomain.Security
{
    public static class SecurityManager
    {
        public static AuthenticateResponse ValidateCredentials(string username, string password, string apikey)
        {

            IRestClient client = new JsonServiceClient();

            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("APIKey:{0}", "12345"));

            ValidateResponse wsResponse = client.Post<ValidateResponse>("http://localhost:9999/api/Data/Login",
                new ValidateRequest { APIKey=apikey, Password = password, Product = "NG", UserName = username } as object);

            // translate to appdomain response object
            AuthenticateResponse authResponse = new AuthenticateResponse();
            authResponse.Validated = wsResponse.Validated.ToString();

            return authResponse;
        }

        public static bool IsTokenExpired(string tempToken)
        {
            bool valid = false;
            IRestClient client = new JsonServiceClient();

            // might not need this
            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("APIKey:{0}", "12345"));

            ValidateTokenResponse wsResponse = client.Post<ValidateTokenResponse>("http://localhost:9999/api/Data/Token",
                new ValidateTokenRequest { Token = tempToken } as object);

            ValidateTokenResponse authResponse = new ValidateTokenResponse();
            valid = Convert.ToBoolean(wsResponse.Validated);
            return valid;
        }
    }
}
