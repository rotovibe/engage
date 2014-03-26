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
            request.UserId = "531f2df4072ef727c4d2a3b2"; 
            request.Version = 1;
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
            request.Version = 1;
            request.Id = "5303a5ccd4332316b4a69449";
            request.PatientId = "52f55877072ef709f84e69b0";
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
            request.Version = 1;
            request.PatientId = "52f55877072ef709f84e69b0";
            request.UserId = "Snehal";

            GoalsManager gManager = new GoalsManager();
            GetAllPatientGoalsResponse response = gManager.GetAllPatientGoals(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void SavePatientGoal_Test()
        {
            PostPatientGoalRequest request = new PostPatientGoalRequest();
            request.ContractNumber = "InHealth001";
            request.UserId = "AD_TestHarness";
            request.Version = 1;
            request.PatientId = "52f55874072ef709f84e68c5";
            request.UserId = "Snehal";
            request.Goal = new PatientGoal { Name =  "my name", SourceId = "my source"};

            GoalsManager gManager = new GoalsManager();
            PostPatientGoalResponse response = gManager.SavePatientGoal(request);

            Assert.IsNotNull(response);
        }


        
        [TestMethod]
        public void InitializePatientGoal_Test()
        {
            GetInitializeGoalRequest request = new GetInitializeGoalRequest();
            request.ContractNumber = "InHealth001";
            request.UserId = "AD_TestHarness";
            request.Version = 1;
            request.PatientId = "531f2dcd072ef727c4d29fb0";
            request.UserId = "531f2df5072ef727c4d2a3bc";

            GoalsManager gManager = new GoalsManager();
            GetInitializeGoalResponse response = gManager.GetInitialGoalRequest(request);

            Assert.IsNotNull(response);
        }
    }
}
