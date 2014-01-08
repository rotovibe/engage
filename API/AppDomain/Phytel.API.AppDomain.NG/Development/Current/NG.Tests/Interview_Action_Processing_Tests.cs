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
            string token = "52cadc50d6a4850db4de0827";
            string programId = "52c5b8d71e601540b017e6d3";
            string patientId = "528f6dc2072ef708ecd90e56";
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
                    new Response { Id = "1", Nominal = true, Order = 1, ResponsePathId = "52a641f1d433231824878c90", StepID = "52a641e8d433231824878c8f" },
                    new Response { Id = "2", Nominal = true, Order = 2, ResponsePathId = "0", StepID = "52a641e8d433231824878c8f" }
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
                    new Response { Id = "1", Nominal = true, Order = 1, ResponsePathId = "1", StepID = "52a641f1d433231824878c90" },
                    new Response { Id = "2", Nominal = true, Order = 2, ResponsePathId = "1", StepID = "52a641f1d433231824878c90" }
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
                Description = "dependent on act1",
                CompletedBy = "Care Manager",
                Text = "text",
                Completed = false,
                Order = 3,
                Status = 1,
                Previous = "5555f33bd43323141c9eb275",
                Steps = new List<Step> { s, s1 }
            };

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
                Actions = new List<Actions> { act, act1, act2 }
            };

            Program pMap = new Program()
            {
                Enabled = true,
                Id = "52c71fd7d6a4850a1cf69494",
                PatientId = "1234",
                Text = "THIS IS THE PROGRAM",
                Modules = new List<Module> { mod },
                Completed = false,
                Order = 1,
                Status = 1
            };

            return pMap;
        }
    }
}
