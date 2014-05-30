using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Diagnostics;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Assign_Patient_To_Program_Tests
    {
        [TestMethod]
        public void Assign_Program_toPatient_Tests()
        {
            //string patientID = "5325db50d6a4850adcbba8e6";
            //string ContractProgramID = "5330920da38116ac180009d2";
            //string userID = "5325c821072ef705080d3488";
            //string contractNumber = "InHealth001";
            //string context = "NG";
            //double version = 1.0;

            string contractNumber = "InHealth001";
            string context = "NG";
            string priority = "3";
            double version = 1.0;
            string token = "533d7d41d6a48508c45b4772";
            string programId = "5330920da38116ac180009d2";  //valid one
            string patientId = "5325d9e7d6a4850adcbba4ad";

            Stopwatch st = new Stopwatch();
            st.Start();
            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("Token: {0}", token));

            IRestClient client = new JsonServiceClient();

            PostPatientToProgramsResponse response = client.Post<PostPatientToProgramsResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Programs/?ContractProgramId={3}&Token={4}",
                version,
                contractNumber,
                patientId,
                programId,
                token), new PostPatientToProgramsRequest() as object);

            st.Stop();
            st.ElapsedMilliseconds.ToString();

            //PostPatientToProgramsResponse response = client.Post<PostPatientToProgramsResponse>(
            //    string.Format(@"http://azurephyteldev.cloudapp.net:59900/Nightingale/{0}/{1}/Patient/{2}/Programs/?ContractProgramId={3}&Token={4}",
            //    version,
            //    contractNumber,
            //    patientId,
            //    programId,
            //    token), new PostPatientToProgramsRequest() as object);
        }
    }
}
