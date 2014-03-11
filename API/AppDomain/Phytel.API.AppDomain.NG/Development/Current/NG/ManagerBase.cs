using Phytel.API.AppDomain.Security.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Configuration;

namespace Phytel.API.AppDomain.NG
{
    public abstract class ManagerBase
    {
        protected static readonly string ADSecurityServiceURL = ConfigurationManager.AppSettings["ADSecurityServiceUrl"];
        protected static readonly string PhytelSecurityHeaderKey = "x-Phytel-Security";

        public ValidateTokenResponse IsUserValidated(double version, string token, string contractNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                    throw new UnauthorizedAccessException("Token is null or empty.");

                string additionalToken = BuildSecurityToken();

                IRestClient client = new JsonServiceClient();

                JsonServiceClient.HttpWebRequestFilter = x =>
                    x.Headers.Add(string.Format("{0}: {1}", PhytelSecurityHeaderKey, additionalToken));

                //[Route("/{Context}/{Version}/{ContractNumber}/token", "POST")]
                ValidateTokenResponse response = client.Post<ValidateTokenResponse>(string.Format("{0}/{1}/{2}/{3}/token", 
                    ADSecurityServiceURL,
                    "NG",
                    version,
                    contractNumber),
                    new ValidateTokenRequest { Token = token } as object);

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string BuildSecurityToken()
        {
            string securityToken = "Unknown";
            try
            {
#if(DEBUG)
                securityToken = "Engineer";
#else
                securityToken = string.Format("{0}-{1}",
                    HttpContext.Current.Request.UserAgent,
                    HttpContext.Current.Request.Params["REMOTE_ADDR"]);
#endif
            }
            catch
            {
                securityToken = "Unknown";
            }
            return securityToken;
        }
    }
}
