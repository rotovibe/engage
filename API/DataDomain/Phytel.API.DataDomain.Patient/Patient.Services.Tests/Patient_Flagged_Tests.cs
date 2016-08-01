using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Patient.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Patient_Flagged_Tests
    {
        [TestMethod]
        public void Update_Patient_Flagged_By_PatientID()
        {
            // http://localhost:8888/Patient/NG/1.0/InHealth001/patient/999/flagged/2?UserId=12345
            string patientID = "528f6dc2072ef708ecd90e87";
            string userId = "BB241C64-A0FF-4E01-BA5F-4246EF50780E";
            string contractNumber = "InHealth001";
            string context ="NG";
            string flagged = "1";
            double version = 1.0;
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetPatientDataResponse response = client.Put<GetPatientDataResponse>(
                string.Format(@"http://localhost:8888/Patient/{0}/{1}/{2}/patient/{3}/flagged/{4}?UserId={5}", context, version, contractNumber, patientID, flagged, userId),
                new GetPatientDataRequest { } as object);
        }

        [TestMethod]
        public void Update_Patient_Flagged_By_PatientID_duplicate()
        {
            // http://localhost:8888/Patient/NG/1.0/InHealth001/patient/999/flagged/2?UserId=12345
            string patientID = "528f6dc2072ef708ecd90e3a";
            string userId = "BB241C64-A0FF-4E01-BA5F-4246EF50780E";
            string contractNumber = "InHealth001";
            string context = "NG";
            string flagged = "1";
            double version = 1.0;
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetPatientDataResponse response = client.Put<GetPatientDataResponse>(
                string.Format(@"http://localhost:8888/Patient/{0}/{1}/{2}/patient/{3}/flagged/{4}?UserId={5}", context, version, contractNumber, patientID, flagged, userId),
                new GetPatientDataRequest { } as object);
        }

        [TestMethod]
        public void Update_Patient_Flagged_By_PatientID_Remove_User()
        {
            // http://localhost:8888/Patient/NG/1.0/InHealth001/patient/999/flagged/2?UserId=12345
            string patientID = "528f6dc2072ef708ecd90e3a";
            string userId = "BB241C64-A0FF-4E01-BA5F-4246EF50780E";
            string contractNumber = "InHealth001";
            string context = "NG";
            string flagged = "0";
            double version = 1.0;
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                                        x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetPatientDataResponse response = client.Put<GetPatientDataResponse>(
                string.Format(@"http://localhost:8888/Patient/{0}/{1}/{2}/patient/{3}/flagged/{4}?UserId={5}", context, version, contractNumber, patientID, flagged, userId),
                new GetPatientDataRequest { } as object);
        }
    }
}
