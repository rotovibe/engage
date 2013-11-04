using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Patient.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Patient.Services.Test
{
    [TestClass]
    public class User_Services_Test
    {
        [TestMethod]
        public void Get_Token_From_DataStore()
        {
            string controlValue = "tdigiorgio@phytel.com";
            string sampleValue;
            IRestClient client = new JsonServiceClient();

            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("APIKey:{0}", "12345"));

            PatientDataResponse response = client.Post<PatientDataResponse>("http://localhost:9999/api/Data/User",
                new PatientDataRequest { UserToken = "abcxyz" } as object);

            sampleValue = response.UserName;

            Assert.AreEqual(controlValue, sampleValue);
        }
    }
}
