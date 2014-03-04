using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.Common.Audit;
using Phytel.API.Common.Format;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Configuration;
using System.Runtime.CompilerServices;
using Phytel.API.Common;
using ServiceStack.ServiceHost;
using System.Web;

namespace Phytel.API.AppDomain.NG
{
    public abstract class ManagerBase
    {
        protected static readonly string ADSecurityServiceURL = ConfigurationManager.AppSettings["ADSecurityServiceUrl"];

        public ValidateTokenResponse IsUserValidated(string version, string token, HttpRequest httpRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                    throw new ArgumentException("Token is null or empty.");

                string additionalToken = string.Format("{0}-{1}", httpRequest.UserAgent, httpRequest.Browser.Version);

                IRestClient client = new JsonServiceClient();

                JsonServiceClient.HttpWebRequestFilter = x =>
                    x.Headers.Add(string.Format("x-Phytel-Security: {0}",
                    additionalToken));

                ValidateTokenResponse response = client.Post<ValidateTokenResponse>(string.Format("{0}/{1}/{2}/token", ADSecurityServiceURL, "NG", version),
                    new ValidateTokenRequest { Token = token } as object);

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
