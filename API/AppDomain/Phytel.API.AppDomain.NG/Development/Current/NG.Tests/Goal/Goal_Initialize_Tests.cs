using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Goal_Tests
    {
        [TestMethod]
        public void Post_PatientGoal_Update_Test()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            string token = "53025640d6a4850e20091ecf";
            string patientId = "52e26f0b072ef7191c111c4d";
            string patientGoalId = "52fd2d6cd433231c845e7d25";
            IRestClient client = new JsonServiceClient();

            GetInitializeGoalResponse response = client.Get<GetInitializeGoalResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Goal/Initialize/?Token={4}",
                version,
                contractNumber,
                patientId,
                patientGoalId,
                token));
        }
    }
}
