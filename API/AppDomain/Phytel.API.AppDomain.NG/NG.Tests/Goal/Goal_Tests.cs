using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Goal_Initialize_Tests
    {
        [TestMethod]
        public void Post_PatientGoal_Update_Test()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string token = "5303ddbdd6a4850f14a7a83c";
            string patientId = "52e26f0b072ef7191c111c4d";
            string patientGoalId = "53011f1ed4332316c093952b";
            IRestClient client = new JsonServiceClient();

            PostPatientGoalResponse response = client.Post<PostPatientGoalResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Goal/{3}/Update/?Token={4}",
                version,
                contractNumber,
                patientId,
                patientGoalId,
                token), new PostPatientGoalRequest
                {
                     Goal = PostInitializeGoalRequest()
                } as object);
        }

        private PatientGoal PostInitializeGoalRequest()
        {
            PatientGoal goal = new PatientGoal
            {
                Id = "52e26f0b072ef7191c111234",
                EndDate = System.DateTime.UtcNow.AddDays(30),
                Interventions = GetInterventions(),
                Name = "Remember cat facts.",
                PatientId = "52f55852072ef709f84e5be1",
                SourceId = "Source of all cat knowledge",
                StartDate = System.DateTime.UtcNow,
                StatusId = 1,
                TargetDate = System.DateTime.UtcNow,
                TargetValue = "Cats have nine lives.",
                Tasks = GetPatientTasks(),
                TypeId = 1,
                ProgramIds = GetProgramsList(),
                //Barriers = GetBarriers(),
                CustomAttributes = GetAttributes(),
                FocusAreaIds = GetFocusAreas()
            };


            return goal;
        }

        private List<string> GetFocusAreas()
        {
            List<string> focusAreas = new List<string>();
            return focusAreas;
        }

        private List<CustomAttribute> GetAttributes()
        {
            List<CustomAttribute> attr = new List<CustomAttribute>();
            attr.Add(new CustomAttribute
            {
                ControlType = 2,
                Id = "52fd4d29fe7a5912b0979dee",
                Order = 1,
                Values = new List<string>{"200"}
            });
            return attr;
        }

        private List<PatientTask> GetPatientTasks()
        {
            List<PatientTask> tasks = new List<PatientTask>();
            tasks.Add(new PatientTask
            {
                Id = "530389e3fe7a592258ffc234",
                CustomAttributes = new List<CustomAttribute> { new CustomAttribute { Values = new List<string> { "200" }, Order = 1, Id = "52fd96c0fe7a5913503f1c64", Name = "task attribute", ControlType = 1 } },
                BarrierIds = new List<string> { "52fd96c0fe7a5913503f1c64" },
                PatientGoalId = "52e26f0b072ef7191c111234",
                Description = "test description",
                StartDate = System.DateTime.UtcNow,
                StatusId = 1,
                StatusDate = System.DateTime.UtcNow,
                TargetDate = System.DateTime.UtcNow,
                TargetValue = "target value"
            });
            return tasks;
        }

        private List<string> GetProgramsList()
        {
            List<string> progs = new List<string>();
            progs.Add("52f70839d6a4850aa4fb7afa");
            return progs;
        }

        private List<PatientIntervention> GetInterventions()
        {
            List<PatientIntervention> interventions = new List<PatientIntervention>();
            interventions.Add(new PatientIntervention
            {
                Id = "52fd3fcefe7a5912b0149acd",
                AssignedToId = "assigned to me",
                BarrierIds = new List<string> { "52fd96c0fe7a5913503f1c64" },
                CategoryId = null,
                Description = "This is a description of interventions.",
                PatientGoalId = "53011f1ed4332316c093952b",
                StartDate = System.DateTime.UtcNow,
                StatusId = 1,
                StatusDate = System.DateTime.UtcNow
            });
            return interventions;
        }

        private List<PatientBarrier> GetBarriers()
        {
            List<PatientBarrier> barriers = new List<PatientBarrier>();
            barriers.Add(new PatientBarrier
            {
                CategoryId = "category value",
                Id = "52fd96c0fe7a5913503f1c64",
                Name = "Barrier name",
                PatientGoalId = "52fd2d6cd433231c845e7d25",
                StatusId = 1,
                StatusDate = System.DateTime.UtcNow
            });
            return barriers;
        }
    }
}
