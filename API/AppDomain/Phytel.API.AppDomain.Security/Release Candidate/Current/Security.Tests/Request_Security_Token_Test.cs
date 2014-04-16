using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.Security.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;

namespace Phytel.API.AppDomain.Security.Services.Test
{
    [TestClass]
    public class Request_Security_Token_Test
    {
        [TestMethod]
        public void ValidateCredential_Test()
        {
            // need to make sure that there is a registration in the apiusertoken table in the DB for this user.
            string controlValue = "inhealthadmin";
            string sampleValue;
            string token = "BB241C64-A0FF-4E01-BA5F-4246EF50780E";

            IRestClient client = new JsonServiceClient();

            AuthenticateResponse response = client.Post<AuthenticateResponse>("http://localhost:888/security/1.0/login",
                new AuthenticateRequest { APIKey = "12345", Context = "NG", Token = token } as object);
            sampleValue = response.UserName;
            Assert.AreEqual(controlValue, sampleValue);
        }

        [TestMethod]
        public void Logout_Test()
        {
            IRestClient client = new JsonServiceClient();
            LogoutResponse response = client.Post<LogoutResponse>("http://localhost:888/security/1.0/logout",
                new LogoutRequest { Context = "NG", Token = "53162017072ef71b5c3d8e4f" } as object);
            Assert.AreEqual(response.SuccessfulLogout, true);
        }

        [TestMethod]
        public void ValidateToken_Test()
        {
            string PhytelSecurityHeaderKey = "x-Phytel-Security";
            string additionalToken = "Engineer";
            
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
            x.Headers.Add(string.Format("{0}: {1}", PhytelSecurityHeaderKey, additionalToken));

            ValidateTokenResponse response = client.Post<ValidateTokenResponse>(string.Format("{0}/{1}/{2}/{3}/token",
                "http://localhost:888/Security",
                "NG",
                1,
                "InHealth001"),
                new ValidateTokenRequest { Token = "53162017072ef71b5c3d8e4f" } as object);

            Assert.IsNotNull(response);
        }
    }
}
