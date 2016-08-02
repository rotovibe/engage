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
            double version = 1.0;
            string token = "530376c0d6a4850698fb3bb1";
            string patientId = "52e26f0b072ef7191c111c4d";
            string patientGoalId = "52e26f0b072ef7191c111234";
            IRestClient client = new JsonServiceClient();

            GetInitializeTaskResponse response = client.Get<GetInitializeTaskResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Goal/{3}/Task/initialize/?Token={4}",
                version,
                contractNumber,
                patientId,
                patientGoalId,
                token));

        }
    }
}
