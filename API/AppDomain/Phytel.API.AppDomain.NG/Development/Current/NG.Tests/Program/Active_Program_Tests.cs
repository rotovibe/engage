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
            string token = "532763f5d6a4850720b45543";

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
            string token = "534ee052d6a48504b03b4a9a";
            string patientProgramId = "534d9bffd6a48504b058a2cf";
            string patientId = "5325dad4d6a4850adcbba776";
            IRestClient client = new JsonServiceClient();

            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("Token: {0}", token));

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
            string token = "5356f171d6a485044c289330";
            string patientId = "5325dacdd6a4850adcbba75e";
            IRestClient client = new JsonServiceClient();

            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("Token: {0}", token));

            GetPatientProgramsResponse response = client.Get<GetPatientProgramsResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Programs/?Token={3}",
                version,
                contractNumber,
                patientId,
                token));
        }
    }
}
