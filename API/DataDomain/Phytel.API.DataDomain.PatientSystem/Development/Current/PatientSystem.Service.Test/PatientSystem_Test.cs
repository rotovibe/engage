using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientSystem.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.PatientSystem.Services.Test
{
    [TestClass]
    public class User_Services_Test
    {
        [TestMethod]
        public void GetPatientSystems()
        {
            string controlValue = "Tony";
            string sampleValue;
            string patientID = "52781cd8fe7a5925fcee5bf3";
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetAllPatientSystemsDataResponse response = client.Post<GetAllPatientSystemsDataResponse>("http://localhost:8888/NG/data/PatientSystem",
                new GetAllPatientSystemsDataRequest { PatientID = patientID, ContractNumber = contractNumber, Context = context, Version = 1 } as object);

            sampleValue = string.Empty;

            Assert.AreEqual(controlValue, sampleValue);
        }

        [TestMethod]
        public void GetPatientSystemByID()
        {
            string controlValue = "Tony";
            string sampleValue;
            string patientSystemID  = "52781cd8fe7a5925fcee5bf3";
            string contractNumber = "InHealth001";
            string context ="NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetPatientSystemDataResponse response = client.Post<GetPatientSystemDataResponse>("http://localhost:8888/NG/data/PatientSystem",
                new GetPatientSystemDataRequest { PatientSystemID = patientSystemID, ContractNumber = contractNumber, Context = context, Version = 1 } as object);

            sampleValue = string.Empty;

            Assert.AreEqual(controlValue, sampleValue);
        }
    }
}
