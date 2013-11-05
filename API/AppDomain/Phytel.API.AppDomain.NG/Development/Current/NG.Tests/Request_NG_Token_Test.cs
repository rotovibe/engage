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
            // check to see if this id is registered in APISession in mongodb.
            string token = "52792478fe7a592338e990a0";
            string controlValue = "DiGiorgio";
            string sampleValue;
            IRestClient client = new JsonServiceClient();

            PatientResponse response = client.Post<PatientResponse>("http://localhost:888/v1/NG/Contract/InHealth001/patient",
                new PatientRequest { ID = "5", Token= token } as object);

            sampleValue = response.LastName;

            Assert.AreEqual(controlValue, sampleValue);
        }
    }
}
