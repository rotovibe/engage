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
        public void Post_PatientGoal_Initialize_Test()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string token = "530376c0d6a4850698fb3bb1";
            string patientId = "52e26f0b072ef7191c111c4d";
            string patientGoalId = "52fd2d6cd433231c845e7d25";
            IRestClient client = new JsonServiceClient();

            GetInitializeGoalResponse response = client.Get<GetInitializeGoalResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Goal/Initialize/?Token={3}",
                version,
                contractNumber,
                patientId,
                token));
        }
    }
}
