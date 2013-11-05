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
            string controlValue = "tdigiorgio@phytel.com";
            string sampleValue;
            IRestClient client = new JsonServiceClient();

            AuthenticateResponse response = client.Post<AuthenticateResponse>("http://localhost:999/api/security/login",
                new AuthenticateRequest { APIKey = "12345", Product = "NG", Token = "abcxyz" } as object);
            sampleValue = response.UserName;
            Assert.AreEqual(controlValue, sampleValue);
        }
    }
}
