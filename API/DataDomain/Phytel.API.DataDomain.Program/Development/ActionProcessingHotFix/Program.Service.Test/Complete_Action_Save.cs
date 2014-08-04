using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Complete_Action_Save_Tests
    {
        [TestMethod]
        public void Complete_Single_Action_Tests()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string priority = "3";
            double version = 1.0;
            string token = "52d58da0d6a4850e90240706";
            string programId = "52deedd0d6a4850fac29d83a";
            string patientId = "528f6cfa072ef708ecd68c94";
            string actionID = "52a0f33bd43323141c9eb274";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            PutProgramActionProcessingResponse response = client.Put<PutProgramActionProcessingResponse>(
                string.Format(@"http://localhost:8888/Program/{0}/{1}/{2}/Patient/{3}/Programs/{4}/Update",
                context,
                version,
                contractNumber,
                patientId,
                programId,
                token), new PutProgramActionProcessingRequest { Program = GenTestProgram(), UserId = "roumel"  });
        }

        private static ProgramDetail GenTestProgram()
        {
            StepsDetail s = new StepsDetail()
            {
                Id = "52a641e8d433231824878c8f",
                ActionId = "52a0f33bd43323141c9eb274",
                ControlType = 1,
                Description = "description",
                Enabled = true,
                Question = "Are you an ABC Employee?",
                Notes = "Example notes",
                Completed = true,
                Order = 1,
                Status = 1,
                Responses = new List<ResponseDetail> { 
                    new ResponseDetail { Id = "52a641f1d433231824878c90", Nominal = true, Order = 1, NextStepId = "52a641f1d433231824878c90", StepId = "52a641e8d433231824878c8f" },
                    new ResponseDetail { Id = "52a641f1d433231824878c90", Nominal = true, Order = 2, NextStepId = "52a641f1d433231824878c90", StepId = "52a641e8d433231824878c8f" }
                }
            };

            StepsDetail s1 = new StepsDetail()
            {
                Id = "52a641f1d433231824878c90",
                ActionId = "52a0f33bd43323141c9eb274",
                ControlType = 1,
                Description = "description",
                Enabled = true,
                Question = "Are you a spouse of an ABC Employee?",
                Notes = "Example notes",
                Completed = true,
                Order = 2,
                Status = 1,
                Responses = new List<ResponseDetail> { 
                    new ResponseDetail { Id = "52a641f1d433231824878c90", Nominal = true, Order = 1, NextStepId = "52a641f1d433231824878c90", StepId = "52a641f1d433231824878c90" },
                    new ResponseDetail { Id = "52a641f1d433231824878c90", Nominal = true, Order = 2, NextStepId = "52a641f1d433231824878c90", StepId = "52a641f1d433231824878c90" }
                }
            };

            StepsDetail s2 = new StepsDetail()
            {
                Id = "52a641f1d433231824871234",
                ActionId = "52a0f33bd43323141c9eb274",
                ControlType = 1,
                Description = "description",
                Enabled = true,
                Question = "Are you a spouse of an ABC Employee?",
                Notes = "Example notes",
                Completed = true,
                Order = 2,
                Status = 1,
                StepTypeId = 7
            };

            List<SpawnElementDetail> se = new List<SpawnElementDetail>();
            se.Add(new SpawnElementDetail { ElementId = "aaaaf33bd43323141c9eb274", ElementType = 3 });
            se.Add(new SpawnElementDetail { ElementId = "52a0a775fe7a5915485b8866", ElementType = 2 });
            se.Add(new SpawnElementDetail { ElementId = "52a0a775fe7a5915485b1212", ElementType = 2 });

            ActionsDetail act = new ActionsDetail
            {
                Enabled = true,
                Id = "52a0f33bd43323141c9eb274",
                ModuleId = "52a0a775fe7a5915485bdfd1",
                Name = "act Verify P4H Eligibility",
                Description = "act - Assess whether individual is eligible for the program",
                CompletedBy = "Care Manager",
                Completed = false,
                Order = 1,
                 Text = "testtest",
                Status = 1,
                SpawnElement = se,
                Steps = new List<StepsDetail> { s, s1, s2 }
            };

            ActionsDetail act1 = new ActionsDetail
            {
                Enabled = true,
                Id = "5555f33bd43323141c9eb275",
                ModuleId = "52a0a775fe7a5915485bdfd1",
                Name = "act1 Verify P4H Eligibility",
                Description = "act1 Assess whether individual is eligible for the program",
                CompletedBy = "Care Manager",
                Completed = false,
                Order = 2,
                Text = "testtest",
                Status = 1,
                Previous = "52a0f33bd43323141c9eb274",
                Steps = new List<StepsDetail> { s, s1 }
            };

            ActionsDetail act2 = new ActionsDetail
            {
                Enabled = false,
                Id = "aaaaf33bd43323141c9eb274",
                ModuleId = "52a0a775fe7a5915485bdfd1",
                Name = "act2 Verify P4H Eligibility",
                Description = "dependent on act1",
                CompletedBy = "Care Manager",
                Completed = false,
                Order = 3,
                Text = "testtest",
                Status = 1,
                Previous = "5555f33bd43323141c9eb275",
                Steps = new List<StepsDetail> { s, s1 }
            };

            List<SpawnElementDetail> sem = new List<SpawnElementDetail>();
            sem.Add(new SpawnElementDetail { ElementId = "9990a775fe7a5915485b1212", ElementType = 2 });

            ModuleDetail mod = new ModuleDetail
            {
                Id = "52a0a775fe7a5915485bdfd1",
                Enabled = true,
                ProgramId = "52c71fd7d6a4850a1cf69494",
                Description = "Reduce the amount of crabs in the diet",
                Name = "Low Carb Diet Module",
                Completed = false,
                Order = 1,
                Text = "testtest",
                Status = 1,
                Actions = new List<ActionsDetail> { act, act1, act2 },
                SpawnElement = sem
            };

            ModuleDetail mod1 = new ModuleDetail
            {
                Id = "52a0a775fe7a5915485b8866",
                Enabled = false,
                ProgramId = "52c71fd7d6a4850a1cf69494",
                Text = "Reduce the amount of crabs in the diet",
                Description = "Reduce the amount of crabs in the diet",
                Name = "Low Carb Diet Module",
                Completed = false,
                Order = 1,
                Status = 1
            };

            ModuleDetail mod2 = new ModuleDetail
            {
                Id = "52a0a775fe7a5915485b1212",
                Enabled = false,
                ProgramId = "52c71fd7d6a4850a1cf69494",
                Text = "Reduce the amount of crabs in the diet",
                Description = "Reduce the amount of crabs in the diet",
                Name = "Low Carb Diet Module",
                Completed = false,
                Order = 1,
                Status = 1
            };

            ModuleDetail mod3 = new ModuleDetail
            {
                Id = "9990a775fe7a5915485b1212",
                Enabled = false,
                ProgramId = "52c71fd7d6a4850a1cf69494",
                Text = "Testgen module number 3",
                Description = "module number 3",
                Name = "Low Carb Diet Module",
                Completed = false,
                Order = 1,
                Status = 1
            };

            ProgramDetail pMap = new ProgramDetail()
            {
                Enabled = true,
                Id = "52c71fd7d6a4850a1cf69494",
                PatientId = "1234",
                Text = "THIS IS THE PROGRAM",
                Modules = new List<ModuleDetail> { mod, mod1, mod2, mod3 },
                Completed = false,
                Order = 1,
                Status = 1
            };

            return pMap;
        }
    }
}
