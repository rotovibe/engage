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
            double version = 1.0;
            string token = "52cc3dcbd6a4850cf4c0ce58";
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

        [TestMethod]
        public void DeletePatient_Test()
        {
            string contractNumber = "InHealth001";
            double version = 1.0;
            string token = "53b2b4d6d6a4850facf303d1";
            string id = "5325d9fad6a4850adcbba4e2";
            string userId = "53b2b4d6d6a4850facf303d1";
            IRestClient client = new JsonServiceClient();

            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("Token: {0}", token));

            PostDeletePatientResponse response = client.Post<PostDeletePatientResponse>(
                string.Format(@"http://localhost:56176/{0}/{1}/Patient/{2}/Delete",
                version,
                contractNumber,
                id), new PostDeletePatientRequest
                {
                    ContractNumber = contractNumber, 
                    Id = id, 
                    Token = token,
                    UserId = userId,
                    Version = version
                } as object);

            Assert.IsNotNull(response);
        }


        [TestMethod]
        public void GetRecentPatientsForAContact_Test()
        {
            string contractNumber = "InHealth001";
            double version = 1.0;
            string token = "53750ca2d6a4850854d33c42";
            string contactId = "5325c821072ef705080d3488";

            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("Token: {0}", token));

            IRestClient client = new JsonServiceClient();
            //[Route("/{Version}/{ContractNumber}/Patient/{Id}/Delete", "POST")]
            GetRecentPatientsResponse response = client.Get<GetRecentPatientsResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Contact/{2}/RecentPatients",
                version,
                contractNumber,
                contactId));

            Assert.IsNotNull(response.Limit);
        }
    }
}
