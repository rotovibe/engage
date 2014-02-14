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
            string token = "52fd3964d6a4850f681dd269";
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

            //GetInitializeTaskResponse response = client.Get<GetInitializeTaskResponse>(@"http://azurephyteldev.cloudapp.net:59900/Nightingale/v1/InHealth001/Patient/52f55873072ef709f84e6810/Goal/52fd2d6cd433231c845e7d25/Task/Initialize/?Token=52fe461ad6a4850b2c8f7a9d");
        }
    }
}
