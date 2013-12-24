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
            string token = "52b875aed6a4850ab047d601";
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
            string token = "52af293ad6a4850da8845c20";
            string programId = "52b6304afe7a590654430378";
            string patientId = "528f6dc2072ef708ecd90e56";
            IRestClient client = new JsonServiceClient();

            PostPatientToProgramsResponse response = client.Post<PostPatientToProgramsResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}//Patient/{2}/Programs/?ContractProgramId={3}&Token=52b868fad6a4850ab047d5c9",
                version,
                contractNumber,
                patientId,
                programId), new PostPatientToProgramsRequest() as object);
        }
    }
}
