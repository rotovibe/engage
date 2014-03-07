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
            string token = "1cc93a7a-dde8-44a3-a534-a568d02e5ff0";

            IRestClient client = new JsonServiceClient();

            AuthenticateResponse response = client.Post<AuthenticateResponse>("http://localhost:888/security/v1/login",
                new AuthenticateRequest { APIKey = "12345", Context = "NG", Token = token } as object);
            sampleValue = response.UserName;
            Assert.AreEqual(controlValue, sampleValue);
        }

        [TestMethod]
        public void Logout_Test()
        {
            IRestClient client = new JsonServiceClient();
            LogoutResponse response = client.Post<LogoutResponse>("http://localhost:888/security/v1/logout",
                new LogoutRequest { Context = "NG", Token = "53162017072ef71b5c3d8e4f" } as object);
            Assert.AreEqual(response.SuccessfulLogout, true);
        }
    }
}
