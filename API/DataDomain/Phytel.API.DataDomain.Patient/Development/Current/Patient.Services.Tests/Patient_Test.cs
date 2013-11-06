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
            string patientID = "52781cd8fe7a5925fcee5bf3";
            string contractNumber = "InHealth001";
            string context ="NG";
            IRestClient client = new JsonServiceClient();

            //JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("APIKey:{0}", "12345"));

            PatientResponse response = client.Post<PatientResponse>("http://localhost:8888/NG/data/patient",
                new PatientRequest { PatientID = patientID, ContractNumber = contractNumber, Context = context } as object);

            sampleValue = response.FirstName;

            Assert.AreEqual(controlValue, sampleValue);
        }
    }
}
