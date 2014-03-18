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
            double version = 1.0;
            string token = "5327104ad6a4850adcc085ce";

            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("Token: {0}", token));

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
        public void Get_Program_Details_summary_for_display_Tests()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string priority = "3";
            double version = 1.0;
            string token = "5307b137d6a4850cd4abdc3c";
            string patientProgramId = "52f56d9fd6a4850fd025fb67";
            string patientId = "52f55876072ef709f84e6944";
            IRestClient client = new JsonServiceClient();

            GetPatientProgramDetailsSummaryResponse response = client.Get<GetPatientProgramDetailsSummaryResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Program/{3}/?Token={4}",
                version,
                contractNumber,
                patientId,
                patientProgramId,
                token) );
        }

        /// <summary>
        /// Gets assigned program details for patient.
        /// </summary>
        [TestMethod]
        public void Get_patient_program_details_Tests()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string token = "5307d950d6a4850cd4abe657";
            string patientId = "52f55881072ef709f84e6d80";
            IRestClient client = new JsonServiceClient();

            GetPatientProgramsResponse response = client.Get<GetPatientProgramsResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Programs/?Token={3}",
                version,
                contractNumber,
                patientId,
                token));
        }
    }
}
