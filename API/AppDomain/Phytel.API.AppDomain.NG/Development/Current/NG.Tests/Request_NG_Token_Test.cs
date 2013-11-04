using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Services.Test
{
    [TestClass]
    public class Request_NG_Token_Test
    {
        [TestMethod]
        public void GetPatientByID_Test()
        {
            string controlValue = "DiGiorgio";
            string sampleValue;
            IRestClient client = new JsonServiceClient();

            PatientResponse response = client.Post<PatientResponse>("http://localhost:888/api/ng/patient",
                new PatientRequest { ID = 5  } as object);

            sampleValue = response.LastName;

            Assert.AreEqual(controlValue, sampleValue);
        }
    }
}
