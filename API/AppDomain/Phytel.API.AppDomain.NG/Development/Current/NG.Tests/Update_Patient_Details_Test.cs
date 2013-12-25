using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Update_Patient_Details_Test
    {
        [TestMethod]
        public void Update_Patient_Details_By_PatientID()
        {
            string patientID = "528f6dc2072ef708ecd90e3a";
            string contractNumber = "InHealth001";
            string context = "NG";
            string priority = "3";
            string version = "v1";
            string token = "52ba6780d6a48507dcbe609f";
            IRestClient client = new JsonServiceClient();

            PutPatientDetailsUpdateResponse response = client.Post<PutPatientDetailsUpdateResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/patient/Update/?Id={2}",
                version,
                contractNumber,
                patientID),
                new PutPatientDetailsUpdateRequest
                {
                    DOB = "12-12-2013",
                    Priority = 3,
                    PreferredName = "Samuel",
                    Token = token
                } as object);
        }
    }
}
