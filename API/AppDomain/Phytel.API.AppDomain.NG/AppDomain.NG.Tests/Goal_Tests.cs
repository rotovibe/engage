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
            request.Id = "543c2eb284ac0509803de12a";
            request.PatientId = "543c2e6b84ac050980224c50";
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
            request.UserId = "531f2df5072ef727c4d2a3bc";
            request.Version = 1;
            request.PatientId = "543c2e6b84ac050980224c50";

            GoalsManager gManager = new GoalsManager();
            GetAllPatientGoalsResponse response = gManager.GetAllPatientGoals(request);

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
           // request.AssignedToId = "5325c821072ef705080d3488";
          // request.CreatedById = "5325c821072ef705080d3488";
            request.StatusIds = new List<int>{1, 2, 3};
            request.PatientId = "5481dbbd231e250160a0e9d1";
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
            request.StatusIds = new List<int> { 1, 2, 3 };
            request.PatientId = "543c2e6b84ac050980224c50";
            request.UserId = "531f2df5072ef727c4d2a3bc";

            GoalsManager gManager = new GoalsManager();
            GetTasksResponse response = gManager.GetTasks(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void SavePatientGoal_Test()
        {
            PostPatientGoalRequest request = new PostPatientGoalRequest();
            request.ContractNumber = "InHealth001";
            request.Version = 1;
            request.UserId = "531f2df5072ef727c4d2a3bc";
            request.PatientId = "5325db62d6a4850adcbba91e";
            request.PatientGoalId = "5438563484ac050d1cb15617";
            request.Goal = new PatientGoal
            {
                Id = "5438563484ac050d1cb15617",
                //FocusAreaIds = new List<string> { "532be4771e60150ce42f8a30" },
                Name = "Snehal 3",
                PatientId = "5325db62d6a4850adcbba91e",
                SourceId = "52fa57c9d433231dd0775011",
                StatusId = 2,
                StartDate = DateTime.UtcNow,
            };

            GoalsManager gManager = new GoalsManager();
            PostPatientGoalResponse response = gManager.SavePatientGoal(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void SavePatientIntervention_Test()
        {
            PostPatientInterventionRequest request = new PostPatientInterventionRequest();
            request.ContractNumber = "InHealth001";
            request.Version = 1;
            request.UserId = "531f2df5072ef727c4d2a3bc";
            request.Id = "54403a1e84ac05070c5685a2";
            request.Intervention = new PatientIntervention
            {
                Id = "54403a1e84ac05070c5685a2",
                AssignedToId = "5325c821072ef705080d3488",
                //BarrierIds = new List<string> { "53308ee0d6a4850998d7fc63" },
                //CategoryId = "52fa624ad433231dd077501f",
                Description = "int2",
                PatientGoalId = "5440392884ac05070c5682ae",
                StartDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(10),
                StatusDate = DateTime.UtcNow,
                StatusId = 1,
                ClosedDate = DateTime.Parse("2014-10-16T21:50:47.057Z"),
                Details = "my intervention details"
            };
            request.PatientGoalId = "5440392884ac05070c5682ae";
            request.PatientId = "5325dae6d6a4850adcbba7ae";

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
            request.Id = "543c2f3f84ac0509803de5d2";
            request.Task = new PatientTask
            {
                Id = "543c2f3f84ac0509803de5d2",
               // BarrierIds = new List<string> { "53308ee3d6a4850998d7fc65" },
              // CustomAttributes = new List<CustomAttribute> { };
                Description = "task 3",
                PatientGoalId = "543c2eb284ac0509803de12a",
                StartDate = DateTime.UtcNow,
                StatusDate = DateTime.UtcNow,
                StatusId = 3,
                ClosedDate = DateTime.UtcNow,
                TargetDate = DateTime.UtcNow,
                TargetValue = "new value",
                Details = "123 my task details"
            };
            request.PatientGoalId = "543c2eb284ac0509803de12a";
            request.PatientId = "543c2e6b84ac050980224c50";

            GoalsManager gManager = new GoalsManager();
            PostPatientTaskResponse response = gManager.SavePatientTask(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void SavePatientbarrier_Test()
        {
            PostPatientBarrierRequest request = new PostPatientBarrierRequest();
            request.ContractNumber = "InHealth001";
            request.Version = 1;
            request.UserId = "531f2df5072ef727c4d2a3bc";
            request.Id = "543853ba84ac050d1cb13261";
            request.Barrier = new PatientBarrier
            {
                Id = "543853ba84ac050d1cb13261",
                CategoryId = "52fa61bed433231dd077501c",
                Name = "barr 4",
                PatientGoalId = "543853b184ac050d1cb13183",
                StatusDate = DateTime.UtcNow,
                StatusId =  2,
                Details = "my barrier details 123"
            };
            request.PatientGoalId = "543853b184ac050d1cb13183";
            request.PatientId = "5325db62d6a4850adcbba91e";

            GoalsManager gManager = new GoalsManager();
            PostPatientBarrierResponse response = gManager.SavePatientBarrier(request);

            Assert.IsNotNull(response);
        }
    }
}
