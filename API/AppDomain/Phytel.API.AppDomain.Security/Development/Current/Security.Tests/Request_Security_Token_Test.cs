using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.Security.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.Security.Services.Test
{
    [TestClass]
    public class Request_Security_Token_Test
    {
        [TestMethod]
        public void Get_Token_With_User_Password_APIKey()
        {
            string controlValue = "rbobadilla";
            string sampleValue;
            IRestClient client = new JsonServiceClient();

            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("APIKey:{0}", "12345"));

            AuthenticateResponse response = client.Post<AuthenticateResponse>("http://localhost:999/api/security/login",
                new AuthenticateRequest { Password = "Testing", Product = "NG", UserName = "NGMel" } as object);

            sampleValue = response.Validated;

            Assert.AreEqual(controlValue, sampleValue);
        }
    }
}
