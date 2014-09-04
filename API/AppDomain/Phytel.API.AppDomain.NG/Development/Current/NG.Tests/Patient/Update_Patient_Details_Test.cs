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
                   Token = token
                } as object);

        }

        [TestMethod]
        public void DeletePatient_Test()
        {
            string contractNumber = "InHealth001";
            double version = 1.0;
            string token = "54089e71d6a48518403ba173";
            string id = "54087eb2d6a48509407d6967";
            string userId = "5325c821072ef705080d3488";
            IRestClient client = new JsonServiceClient();

            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("Token: {0}", token));

            PostDeletePatientResponse response = client.Post<PostDeletePatientResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Delete",
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

        [TestMethod]
        public void SavePatient_Test()
        {
            string contractNumber = "InHealth001";
            double version = 1.0;
            string token = "53f23c70d6a4850d189307d9";
            string userId = "53b2b4d6d6a4850facf303d1";
            IRestClient client = new JsonServiceClient();

            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("Token: {0}", token));

            Phytel.API.AppDomain.NG.DTO.Patient p = new AppDomain.NG.DTO.Patient {
                Id = "5325d9fcd6a4850adcbba4ea",
               //FirstName = "Alex", 
               //MiddleName = "M",
                LastName = "Wilcox",
               //Suffix = "Jr.",
               //PreferredName = "Alex Rowland",
               //DOB = "01/01/2008",
               //FullSSN = "123456789",
               Gender = "F",
               Priority = 3
            };

            //[Route("/{Version}/{ContractNumber}/patient/Update/", "POST")]
            PutPatientDetailsUpdateResponse response = client.Post<PutPatientDetailsUpdateResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/Update",
                version,
                contractNumber), new PutPatientDetailsUpdateRequest
                {
                    ContractNumber = contractNumber,
                    //Insert = true,
                    Patient = p,
                    Token = token,
                    UserId = userId,
                    Version = version
                } as object);

            Assert.IsNotNull(response);
        }
    }
}
