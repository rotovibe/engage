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
            string token = "52dd7de7d6a4850ea84c2894";
            string programId = "52dd5c5bd6a4850ea8b75381";
            string patientId = "528f6bdf072ef708ecd326f0";
            string actionID = "52a0f33bd43323141c9eb274";
            IRestClient client = new JsonServiceClient();

            PostProcessActionResponse response = client.Post<PostProcessActionResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Patient/{2}/Program/Module/Action/Process/?ProgramId={3}&Token={4}",
                version,
                contractNumber,
                patientId,
                programId,
                token), new PostProcessActionRequest() {  Action = GenAction(), ProgramId = programId });
        }

        private static Actions GenAction()
        {
            Step s1 = new Step()
            {
                Id = "52dd5c5bd6a4850ea8b7539a",
                ActionId = "52dd5c5bd6a4850ea8b75399",
                SourceId = "52cb2bdb1e601522209c44ba",
                Order = 1,
                Enabled = true,
                ElementState = 0,
                Completed = true,
                StepTypeId = 1,
                ControlType = 0,
                SelectType = 0,
                IncludeTime = false,
                Header = string.Empty,
                Description = "",
                Question = "Is patient a Diabetic?",
                Notes = "Example notes",
                Status = 1,
                SelectedResponseId = "52dd5c5bd6a4850ea8b7539b",
                Responses = new List<Response> { 
                    new Response { Id = "52dd5c5bd6a4850ea8b7539b", Nominal = false, Text="Yes", Required=false, Value=string.Empty, Order = 1, NextStepId = "52dd5c5bd6a4850ea8b7539d", StepId = "52dd5c5bd6a4850ea8b7539a" },
                    new Response { Id = "52dd5c5bd6a4850ea8b7539c", Nominal = false, Text ="No", Required=false,  Order = 2, NextStepId = "52dd5c5bd6a4850ea8b7539d", StepId = "52dd5c5bd6a4850ea8b7539a" }
                }
            };

            // complete step action
            Step s2 = new Step()
            {
                SourceId = "52cf3ade1e6015397c1bcc2c",
                Order = 2,
                Enabled = true,
                ElementState = 0,
                Completed = false,
                Id = "52dd5c5bd6a4850ea8b7539d",
                ActionId = "52dd5c5bd6a4850ea8b75399",
                StepTypeId = 7,
                Header = string.Empty,
                ControlType = 0,
                SelectType = 0,
                IncludeTime = false,
                Question = string.Empty,
                Description = "complete - final step in action",
                 Text = "complete",
                Notes = "Example notes",
                Status = 1
            };

            List<SpawnElement> se = new List<SpawnElement>();
            se.Add(new SpawnElement { ElementId = "52dd5c5bd6a4850ea8b7539e", ElementType = 2 });

            List<Objective> objs = new List<Objective>();
            objs.Add(new Objective { Id = "", Value = "75", Unit ="%", Status = 5 });

            Actions act = new Actions
            {
                SourceId = "52cb205d1e601522209c44a8",
                Order = 1,
                Enabled = true,
                ElementState = 0,
                Next = string.Empty,
                Previous = string.Empty,
                SpawnElement = se,
                Id = "52dd5c5bd6a4850ea8b75399",
                ModuleId = "52dd5c5bd6a4850ea8b75398",
                Name = "Diabetic screening",
                Description = "Diabetic screening for initial assesment",
                CompletedBy = "Care Manager",
                Completed = true,
                Text = "text",
                Objectives = objs,
                Steps = new List<Step> { s1, s2 },
                Status = 1
            };

            return act;
        }
    }
}
