using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Interview_Action_Processing_Tests
    {
        [TestMethod]
        public void Get_Program_Details_summary_for_display_Tests()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string priority = "3";
            string version = "v1";
            string token = "52d81857d6a4850860d2b501";
            string programId = "52d7fc5bd6a48508602923d9";
            string patientId = "528f6c94072ef708ecd5bc1c";
            string actionID = "52a0f33bd43323141c9eb274";
            IRestClient client = new JsonServiceClient();

            PostProcessActionResponse response = client.Post<PostProcessActionResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Program/Module/Action/Process/?ProgramId={3}&Token={4}",
                version,
                contractNumber,
                patientId,
                programId,
                token), new PostProcessActionRequest() { Program = GenProgram(), ActionId = actionID });
        }

        private static Program GenProgram()
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
                    new Response { Id = "52a641f1d433231824878c90", Nominal = true, Order = 1, NextStepId = "52a641f1d433231824878c90", StepId = "52a641e8d433231824878c8f" },
                    new Response { Id = "52a641f1d433231824878c90", Nominal = true, Order = 2, NextStepId = "52a641f1d433231824878c90", StepId = "52a641e8d433231824878c8f" }
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
                    new Response { Id = "52a641f1d433231824878c90", Nominal = true, Order = 1, NextStepId = "52a641f1d433231824878c90", StepId = "52a641f1d433231824878c90" },
                    new Response { Id = "52a641f1d433231824878c90", Nominal = true, Order = 2, NextStepId = "52a641f1d433231824878c90", StepId = "52a641f1d433231824878c90" }
                }
            };

            Actions act = new Actions
            {
                Enabled = true,
                Id = "52a0f33bd43323141c9eb274",
                ModuleId = "52a0a775fe7a5915485bdfd1",
                Name = "act Verify P4H Eligibility",
                Description = "act - Assess whether individual is eligible for the program",
                CompletedBy = "Care Manager",
                Text = "text",
                Completed = true,
                Order = 1,
                Status = 1,
                Steps = new List<Step> { s,s1 }
            };

            Actions act1 = new Actions
            {
                Enabled = false,
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
                Enabled = true,
                Id = "aaaaf33bd43323141c9eb274",
                ModuleId = "52a0a775fe7a5915485bdfd1",
                Name = "act2 Verify P4H Eligibility",
                Description = "dependent on act1 blah",
                CompletedBy = "Care Manager",
                Text = "text",
                Completed = false,
                Order = 3,
                Status = 1,
                Previous = "5555f33bd43323141c9eb275",
                Steps = new List<Step> { s, s1 }
            };

            SpawnElement se = new SpawnElement { ElementId = "5555f33bd43323141c9eb275", ElementType = 2 };
            Module mod = new Module
            {
                Id = "52a0a775fe7a5915485bdfd1",
                Enabled = true,
                ProgramId = "52c71fd7d6a4850a1cf69494",
                Text = "THIS IS A TEST PROGRAM ALL IT'S HIERARCHY WILL BE BROKEN MOD2",
                Description = "THIS IS A TEST PROGRAM ALL IT'S HIERARCHY WILL BE BROKEN MOD2",
                Name = "Low Carb Diet Module",
                Completed = false,
                Order = 1,
                Status = 1,
                 SpawnElement = new List<SpawnElement>{se},
                Actions = new List<Actions> { act, act1, act2 }
            };

            Module mod1 = new Module
            {
                Id = "52a0a775fe7a5915485bdfd1",
                Enabled = true,
                ProgramId = "52c71fd7d6a4850a1cf69494",
                Text = "THIS IS A TEST PROGRAM ALL IT'S HIERARCHY WILL BE BROKEN MOD1",
                Description = "THIS IS A TEST PROGRAM ALL IT'S HIERARCHY WILL BE BROKEN MOD2",
                Name = "Low Carb Diet Module",
                Completed = false,
                Order = 1,
                Status = 1,
                Actions = new List<Actions> { act, act1, act2 }
            };

            Program pMap = new Program()
            {
                Enabled = true,
                Id = "52d7fc5bd6a48508602923d9",
                PatientId = "1234",
                 Description = "THIS IS A TEST PROGRAM ALL IT'S HIERARCHY WILL BE BROKEN PROGRAM!",
                Text = "THIS IS A TEST PROGRAM ALL IT'S HIERARCHY WILL BE BROKEN",
                Modules = new List<Module> { mod, mod1 },
                Completed = false,
                Order = 1,
                Status = 1
            };

            return pMap;
        }
    }
}
