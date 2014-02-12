using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Task_Tests
    {
        [TestMethod]
        public void Get_Initial_Task_Tests()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            string token = "52fba33cd6a4850aa450d8f1";
            string patientId = "52e26f0b072ef7191c111c4d";
            IRestClient client = new JsonServiceClient();

            GetInitializeTaskResponse response = client.Get<GetInitializeTaskResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Task/initialize/?Token={3}",
                version,
                contractNumber,
                patientId,
                token));
        }
    }
}
