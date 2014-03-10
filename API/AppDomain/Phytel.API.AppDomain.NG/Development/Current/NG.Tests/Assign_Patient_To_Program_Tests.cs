using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Assign_Patient_To_Program_Tests
    {
        [TestMethod]
        public void Assign_Program_toPatient_Tests()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string priority = "3";
            string version = "v1";
            string token = "531e187cd6a48509f401ca5f";
            string programId = "52e024f91e601512a8f03789";  //valid one
            string patientId = "52f5585b072ef709f84e5f01";

            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("Token: {0}", token));

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
    }
}
