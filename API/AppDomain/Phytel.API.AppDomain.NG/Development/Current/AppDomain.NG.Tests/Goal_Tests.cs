using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.Test
{
    [TestClass]
    public class Goal_Tests
    {
        [TestMethod]
        public void UpdatePatient_Test()
        {
            GetInitializeGoalRequest request = new GetInitializeGoalRequest();
            request.ContractNumber = "InHealth001";
            request.UserId = "AD_TestHarness"; 
            request.Version = "v1";
            request.PatientId = "52f55874072ef709f84e68c5";

            GoalsManager gManager = new GoalsManager();
            GetInitializeGoalResponse response = gManager.GetInitialGoalRequest(request);

            Assert.IsNotNull(response);
        }
    }
}
