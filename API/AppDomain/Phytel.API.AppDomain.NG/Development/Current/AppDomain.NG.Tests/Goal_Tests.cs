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


        [TestMethod]
        public void GetInterventions_Test()
        {
            GetInterventionsRequest request = new GetInterventionsRequest();
            request.ContractNumber = "InHealth001";
            request.Version = 1;
            request.AssignedToId = "5325c821072ef705080d3488";
          // request.CreatedById = "5325c821072ef705080d3488";
            request.StatusIds = new List<int>{1, 2};
          //  request.PatientId = "5325db97d6a4850adcbba9ba";
            request.UserId = "531f2df5072ef727c4d2a3bc";

            GoalsManager gManager = new GoalsManager();
            GetInterventionsResponse response = gManager.GetInterventions(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void GetTasks_Test()
        {
            GetTasksRequest request = new GetTasksRequest();
            request.ContractNumber = "InHealth001";
            request.Version = 1;
            request.StatusIds = new List<int> { 1, 2 };
           // request.PatientId = "5325db97d6a4850adcbba9ba";
            request.UserId = "531f2df5072ef727c4d2a3bc";

            GoalsManager gManager = new GoalsManager();
            GetTasksResponse response = gManager.GetTasks(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void SavePatientIntervention_Test()
        {
            PostPatientInterventionRequest request = new PostPatientInterventionRequest();
            request.ContractNumber = "InHealth001";
            request.Version = 1;
            request.UserId = "531f2df5072ef727c4d2a3bc";
            request.Id = "53308ee8d6a4850998d7fc69";
            request.Intervention = new PatientIntervention
            {
                Id = "53308ee8d6a4850998d7fc69",
                AssignedToId = "5325c821072ef705080d3488",
                BarrierIds = new List<string> { "53308ee0d6a4850998d7fc63" },
                CategoryId = "52fa624ad433231dd077501f",
                Description = "testing save",
                PatientGoalId = "53308e94d6a4850998d7fc5f",
                StartDate = DateTime.UtcNow,
                StatusDate = DateTime.UtcNow,
                StatusId = 2,
                ClosedDate = DateTime.UtcNow
            };
            request.PatientGoalId = "53308e94d6a4850998d7fc5f";
            request.PatientId = "5325da9ad6a4850adcbba6c2";

            GoalsManager gManager = new GoalsManager();
            PostPatientInterventionResponse response = gManager.SavePatientIntervention(request);

            Assert.IsNotNull(response);
        }


        [TestMethod]
        public void SavePatientTask_Test()
        {
            PostPatientTaskRequest request = new PostPatientTaskRequest();
            request.ContractNumber = "InHealth001";
            request.Version = 1;
            request.UserId = "531f2df5072ef727c4d2a3bc";
            request.Id = "53308ee5d6a4850998d7fc67";
            request.Task = new PatientTask
            {
                Id = "53308ee5d6a4850998d7fc67",
               // BarrierIds = new List<string> { "53308ee3d6a4850998d7fc65" },
              // CustomAttributes = new List<CustomAttribute> { };
                Description = "task 2",
                PatientGoalId = "53308e94d6a4850998d7fc5f",
                StartDate = DateTime.UtcNow,
                StatusDate = DateTime.UtcNow,
                StatusId = 3,
                ClosedDate = DateTime.UtcNow,
                TargetDate = DateTime.UtcNow,
                TargetValue = "new value"
            };
            request.PatientGoalId = "53308e94d6a4850998d7fc5f";
            request.PatientId = "5325da9ad6a4850adcbba6c2";

            GoalsManager gManager = new GoalsManager();
            PostPatientTaskResponse response = gManager.SavePatientTask(request);

            Assert.IsNotNull(response);
        }
    }
}
