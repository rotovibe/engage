using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Patient_NG_Priority_Test
    {
        [TestMethod]
        public void Update_Patient_Priority_By_PatientID()
        {
            string patientID = "528f6dc2072ef708ecd90e3a";
            string contractNumber = "InHealth001";
            string context = "NG";
            string priority = "3";
            string version = "v1";
            string token = "52af293ad6a4850da8845c20";
            IRestClient client = new JsonServiceClient();

            PutPatientDetailsUpdateResponse response = client.Put<PutPatientDetailsUpdateResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/patient/{2}/priority/{3}?Token={4}",
                version, 
                contractNumber, 
                patientID,
                priority,
                token),
                new PutPatientDetailsUpdateRequest { } as object);
        }
    }
}
