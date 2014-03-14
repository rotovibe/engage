using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Patient_NG_Flagged_Tests
    {
        [TestMethod]
        public void Update_Patient_Flagged_By_PatientID()
        {
            // http://localhost:8888/Patient/NG/1.0/InHealth001/patient/999/flagged/2?UserId=12345
            string patientID = "528f6d09072ef708ecd6d08e";
            string userId = "ba9b277d-4b53-4a53-a2c5-15d4969423ec";
            string contractNumber = "InHealth001";
            string context = "NG";
            string flagged = "1";
            double version = 1.0;
            string token = "52cc3dcbd6a4850cf4c0ce58";

            IRestClient client = new JsonServiceClient();

            PutPatientFlaggedUpdateResponse response = client.Post<PutPatientFlaggedUpdateResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/patient/{2}/flagged/{3}?UserId={4}&Token={5}",
                version,
                contractNumber,
                patientID,
                flagged,
                userId,
                token),
                new PutPatientFlaggedUpdateRequest { } as object);
        }
    }
}
