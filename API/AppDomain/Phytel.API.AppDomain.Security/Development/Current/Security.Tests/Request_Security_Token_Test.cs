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
            // need to make sure that there is a registration in the apiusertoken table in the c3 DB for this user.
            string controlValue = "inhealthadmin";
            string sampleValue;
            IRestClient client = new JsonServiceClient();

            AuthenticateResponse response = client.Post<AuthenticateResponse>("http://localhost:999/api/security/login",
                new AuthenticateRequest { APIKey = "12345", Product = "NG", Token = "1234" } as object);
            sampleValue = response.UserName;
            Assert.AreEqual(controlValue, sampleValue);
        }
    }
}
