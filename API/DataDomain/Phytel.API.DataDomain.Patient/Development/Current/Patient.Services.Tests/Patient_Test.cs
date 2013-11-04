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
        public void GetPatientByID()
        {
            string controlValue = "Tony";
            string sampleValue;
            IRestClient client = new JsonServiceClient();

            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("APIKey:{0}", "12345"));

            PatientResponse response = client.Post<PatientResponse>("http://localhost:9999/api/Data/User",
                new PatientRequest { PatientID = "5"} as object);

            sampleValue = response.FirstName;

            Assert.AreEqual(controlValue, sampleValue);
        }
    }
}
