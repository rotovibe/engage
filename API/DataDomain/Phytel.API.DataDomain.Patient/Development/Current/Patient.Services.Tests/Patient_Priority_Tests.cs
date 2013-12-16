using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Patient.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Patient_Priority_Test
    {
        [TestMethod]
        public void GetPatientByID()
        {
            // http://localhost:8888/Patient/NG/v1/InHealth001/patient/999/priority/?priority=2
            string patientID = "528f6dc2072ef708ecd90e3a";
            string contractNumber = "InHealth001";
            string context ="NG";
            string priority = "2";
            string version = "v1";
            IRestClient client = new JsonServiceClient();

            GetPatientDataResponse response = client.Put<GetPatientDataResponse>(
                string.Format(@"http://localhost:8888/Patient/{0}/{1}/{2}/patient/{3}/priority/?priority={4}", context, version, contractNumber, patientID, priority ),
                new GetPatientDataRequest { } as object);
        }
    }
}
