using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Active_Programs_Tests
    {
        [TestMethod]
        public void Get_Active_Program_Tests()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string priority = "3";
            string version = "v1";
            string token = "52bdac26d6a4850d485c62ef";
            IRestClient client = new JsonServiceClient();

            //GetActiveProgramsResponse response = client.Get<GetActiveProgramsResponse>(
            //    string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Programs/Active?Token={2}",
            //    version,
            //    contractNumber,
            //    token));

            GetActiveProgramsResponse response = client.Get<GetActiveProgramsResponse>(
                string.Format(@"http://azurephyteldev.cloudapp.net:59900/Nightingale/{0}/{1}/Programs/Active?Token={2}",
                version,
                contractNumber,
                token));
        }

        [TestMethod]
        public void Assign_Program_toPatient_Tests()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string priority = "3";
            string version = "v1";
            string token = "52bdb1c0d6a4850d485c633d";
            //string programId = "52b6304afe7a590654430378";
            string programId = "52b6304afe7a590654430378";
            //string patientId = "528f6dc2072ef708ecd90e41";
            //string patientId = "528f6c42072ef708ecd43f59";
            string patientId = "528f6b2f072ef708eccf98b8";
            IRestClient client = new JsonServiceClient();

        //http://azurephyteldev.cloudapp.net:59900/Nightingale/v1/InHealth001//Patient/528f6b2f072ef708eccf98b8/Programs/?ContractProgramId=52b6304afe7a590654430378&Token=52bdb7b8d6a4850d485c636b

            //PostPatientToProgramsResponse response = client.Post<PostPatientToProgramsResponse>(
            //    string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Programs/?ContractProgramId={3}&Token={4}",
            //    version,
            //    contractNumber,
            //    patientId,
            //    programId,
            //    token), new PostPatientToProgramsRequest() as object);

            PostPatientToProgramsResponse response = client.Post<PostPatientToProgramsResponse>(
                string.Format(@"http://azurephyteldev.cloudapp.net:59900/Nightingale/{0}/{1}/Patient/{2}/Programs/?ContractProgramId={3}&Token={4}",
                version,
                contractNumber,
                patientId,
                programId,
                token), new PostPatientToProgramsRequest() as object);
        }

        [TestMethod]
        public void Get_Program_Details_summary_for_display_Tests()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string priority = "3";
            string version = "v1";
            string token = "52bc9c3ed6a4850d485c6137";
            string programId = "52bca85bd6a4850d4843f92a";
            string patientId = "528f6dc2072ef708ecd90e56";
            IRestClient client = new JsonServiceClient();

            GetPatientProgramDetailsSummaryResponse response = client.Get<GetPatientProgramDetailsSummaryResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Program/Details/?ProgramId={3}&Token={4}",
                version,
                contractNumber,
                patientId,
                programId,
                token) );
        }
    }
}
