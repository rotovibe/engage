using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Complete_Action_Tests
    {
        [TestMethod]
        public void Complete_Single_Action_Tests()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string priority = "3";
            double version = 1.0;
            string token = "52d949d1d6a4850b882a783b";
            string programId = "52d9639dd6a4850b88206a3c";
            string patientId = "528f6c7c072ef708ecd51581";
            string actionID = "52d9639dd6a4850b88206a54";
            IRestClient client = new JsonServiceClient();

            PostProcessActionResponse response = client.Post<PostProcessActionResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Program/Module/Action/Process/?ProgramId={3}&Token={4}",
                version,
                contractNumber,
                patientId,
                programId,
                token), new PostProcessActionRequest() { ProgramId = programId, Action = GenAction()});
        }

        private static Actions GenAction()
        {
            List<SpawnElement> ser = new List<SpawnElement>();
            ser.Add(new SpawnElement { ElementType = 101, ElementId = "528a66f4d4332317acc5095f" });

            Step s1 = new Step()
            {
                SourceId = "52cb2bdb1e601522209c44ba",
                Order = 1,
                Enabled = true,
                ElementState = 0,
                Completed = false,
                Id = "52d9639dd6a4850b88206a55",
                ActionId = "52d9639dd6a4850b88206a54",
                StepTypeId = 1,
                SelectType = 0,
                ControlType = 0,
                IncludeTime = false,
                Question = "Is patient a Diabetic?",
                Status = 1,
                Responses = new List<Response> { 
                    new Response { 
                        Id = "52d9639dd6a4850b88206a56", 
                        Order = 1, 
                        Text = "Yes",
                        StepId = "52d9639dd6a4850b88206a55",
                    Nominal = false,
                    Required = false,
                    NextStepId = "52d9639dd6a4850b88206a58",
                    SpawnElement = ser,
                    },
                    new Response { 
                        Id = "52d9639dd6a4850b88206a57", 
                        Order = 2, 
                         Text = "No",
                        StepId = "52d9639dd6a4850b88206a55",
                    Nominal = false,
                    Required = false,
                    NextStepId = "52d9639dd6a4850b88206a58"}
                }
            };

            Step s2 = new Step()
            {
                SourceId = "52cf3ade1e6015397c1bcc2c",
                Order = 2,
                Enabled = true,
                ElementState = 0,
                Completed = false,
                Id = "52d9639dd6a4850b88206a58",
                ActionId = "52d9639dd6a4850b88206a54",
                StepTypeId = 7,
                ControlType = 0,
                SelectType = 0,
                IncludeTime = false,
                Description = "complete - final step in action",
                Text = "complete",
                Status = 1
            };

            List<SpawnElement> se = new List<SpawnElement>();
            se.Add(new SpawnElement
            {
                ElementId = "52d9639dd6a4850b88206a59",
                ElementType = 2
            });

            List<ObjectiveInfo> obj = new List<ObjectiveInfo> { new ObjectiveInfo{ Id = "52a0bf97d43323141c9eb271",
             Status = 5, Unit = "%", Value = "75"}};

            Actions act = new Actions
            {
                SourceId = "52cb205d1e601522209c44a8",
                Order = 1,
                Enabled = true,
                ElementState = 0,
                Completed = true,
                SpawnElement = se,
                Id = "52d9639dd6a4850b88206a54",
                ModuleId = "52d9639dd6a4850b88206a53",
                Name = "Diabetic screening",
                Description = "Diabetic screening for initial assesment",
                CompletedBy = "Care Manager",
                Objectives = obj,
                Status = 1,
                Steps = new List<Step> { s1, s2 }
            };

            return act;
        }

        private static Program GenTestProgram()
        {
            Step s = new Step()
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
                Responses = new List<Response> { 
                    new Response { Id = "1", Nominal = true, Order = 1, NextStepId = "52a641f1d433231824878c90", StepId = "52a641e8d433231824878c8f" },
                    new Response { Id = "2", Nominal = true, Order = 2, NextStepId = "0", StepId = "52a641e8d433231824878c8f" }
                }
            };

            Step s1 = new Step()
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
                Responses = new List<Response> { 
                    new Response { Id = "1", Nominal = true, Order = 1, NextStepId = "1", StepId = "52a641f1d433231824878c90" },
                    new Response { Id = "2", Nominal = true, Order = 2, NextStepId = "1", StepId = "52a641f1d433231824878c90" }
                }
            };

            Step s2 = new Step()
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

            List<SpawnElement> se = new List<SpawnElement>();
            se.Add(new SpawnElement { ElementId = "aaaaf33bd43323141c9eb274", ElementType = 3 } );
            se.Add(new SpawnElement { ElementId = "52a0a775fe7a5915485b8866", ElementType = 2 });
            se.Add(new SpawnElement { ElementId = "52a0a775fe7a5915485b1212", ElementType = 2 });

            Actions act = new Actions
            {
                Enabled = true,
                Id = "52a0f33bd43323141c9eb274",
                ModuleId = "52a0a775fe7a5915485bdfd1",
                Name = "act Verify P4H Eligibility",
                Description = "act - Assess whether individual is eligible for the program",
                CompletedBy = "Care Manager",
                Text = "text",
                Completed = false,
                Order = 1,
                Status = 1,
                SpawnElement =  se,
                Steps = new List<Step> { s, s1, s2 }
            };

            Actions act1 = new Actions
            {
                Enabled = true,
                Id = "5555f33bd43323141c9eb275",
                ModuleId = "52a0a775fe7a5915485bdfd1",
                Name = "act1 Verify P4H Eligibility",
                Description = "act1 Assess whether individual is eligible for the program",
                CompletedBy = "Care Manager",
                Text = "text",
                Completed = false,
                Order = 2,
                Status = 1,
                Previous = "52a0f33bd43323141c9eb274",
                Steps = new List<Step> { s, s1 }
            };

            Actions act2 = new Actions
            {
                Enabled = false,
                Id = "aaaaf33bd43323141c9eb274",
                ModuleId = "52a0a775fe7a5915485bdfd1",
                Name = "act2 Verify P4H Eligibility",
                Description = "dependent on act1",
                CompletedBy = "Care Manager",
                Text = "text",
                Completed = false,
                Order = 3,
                Status = 1,
                Previous = "5555f33bd43323141c9eb275",
                Steps = new List<Step> { s, s1 }
            };

            List<SpawnElement> sem = new List<SpawnElement>();
            sem.Add(new SpawnElement { ElementId = "9990a775fe7a5915485b1212", ElementType = 2 });

            Module mod = new Module
            {
                Id = "52a0a775fe7a5915485bdfd1",
                Enabled = true,
                ProgramId = "52c71fd7d6a4850a1cf69494",
                Text = "Reduce the amount of crabs in the diet",
                Description = "Reduce the amount of crabs in the diet",
                Name = "Low Carb Diet Module",
                Completed = false,
                Order = 1,
                Status = 1,
                Actions = new List<Actions> { act, act1, act2 },
                SpawnElement = sem
            };

            Module mod1 = new Module
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

            Module mod2 = new Module
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

            Module mod3 = new Module
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

            Program pMap = new Program()
            {
                Enabled = true,
                Id = "52c71fd7d6a4850a1cf69494",
                PatientId = "1234",
                Text = "THIS IS THE PROGRAM",
                Modules = new List<Module> { mod, mod1, mod2, mod3 },
                Completed = false,
                Order = 1,
                Status = 1
            };

            return pMap;
        }
    }
}
