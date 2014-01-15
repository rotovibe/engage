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
            string token = "52c24bd0d6a4850ef80555ea";
            IRestClient client = new JsonServiceClient();

            GetActiveProgramsResponse response = client.Get<GetActiveProgramsResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Programs/Active?Token={2}",
                version,
                contractNumber,
                token));

            //GetActiveProgramsResponse response = client.Get<GetActiveProgramsResponse>(
            //    string.Format(@"http://azurephyteldev.cloudapp.net:59900/Nightingale/{0}/{1}/Programs/Active?Token={2}",
            //    version,
            //    contractNumber,
            //    token));
        }

        [TestMethod]
        public void Assign_Program_toPatient_Tests()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string priority = "3";
            string version = "v1";
            string token = "52cc3dcbd6a4850cf4c0ce58";
            //string programId = "52b6304afe7a590654430378";
            string programId = "52b6304afe7a590654430378";  //valid one
            //string programId = "52b6304afe7a590654431111";
            //string patientId = "528f6dc2072ef708ecd90e41";
            //string patientId = "528f6c42072ef708ecd43f59";
            string patientId = "528f6dc2072ef708ecd90fbb";
            IRestClient client = new JsonServiceClient();

            PostPatientToProgramsResponse response = client.Post<PostPatientToProgramsResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Programs/?ContractProgramId={3}&Token={4}",
                version,
                contractNumber,
                patientId,
                programId,
                token), new PostPatientToProgramsRequest() as object);

            //PostPatientToProgramsResponse response = client.Post<PostPatientToProgramsResponse>(
            //    string.Format(@"http://azurephyteldev.cloudapp.net:59900/Nightingale/{0}/{1}/Patient/{2}/Programs/?ContractProgramId={3}&Token={4}",
            //    version,
            //    contractNumber,
            //    patientId,
            //    programId,
            //    token), new PostPatientToProgramsRequest() as object);
        }

        [TestMethod]
        public void Get_Program_Details_summary_for_display_Tests()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string priority = "3";
            string version = "v1";
            string token = "52d442a3d6a4850dac4a0b8f";
            string programId = "52d449ddd6a4850dace2a71b";
            string patientId = "528f6bdb072ef708ecd316b9";
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
