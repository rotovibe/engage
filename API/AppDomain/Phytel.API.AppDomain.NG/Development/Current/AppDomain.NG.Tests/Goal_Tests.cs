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

        [TestMethod]
        public void GetPaitentGoal_Test()
        {
            GetPatientGoalRequest request = new GetPatientGoalRequest();
            request.ContractNumber = "InHealth001";
            request.UserId = "AD_TestHarness";
            request.Version = "v1";
            request.Id = "53011e8ed4332316c093952a";
            request.PatientId = "52f55874072ef709f84e68c5";
            request.UserId = "Snehal";

            GoalsManager gManager = new GoalsManager();
            GetPatientGoalResponse response = gManager.GetPatientGoal(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void GetAllPatientGoal_Test()
        {
            GetAllPatientGoalsRequest request = new GetAllPatientGoalsRequest();
            request.ContractNumber = "InHealth001";
            request.UserId = "AD_TestHarness";
            request.Version = "v1";
            request.PatientId = "52f55874072ef709f84e68c5";
            request.UserId = "Snehal";

            GoalsManager gManager = new GoalsManager();
            GetAllPatientGoalsResponse response = gManager.GetAllPatientGoals(request);

            Assert.IsNotNull(response);
        }
    }
}
