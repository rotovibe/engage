using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Action_Processing_Bug_572_Tests
    {
        [TestMethod]
        public void Get_Program_Details_summary_for_display_Tests()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string priority = "3";
            double version = 1.0;
            string token = "52dd7de7d6a4850ea84c2894";
            string programId = "52dd8c44d6a4850ea8a56c6f";
            string patientId = "528f6c4d072ef708ecd48f9a";
            string actionID = "52dd8c44d6a4850ea8a56cbe";
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
            # region
            //Step s0 = new Step()
            //{
            //    Id = "52dd8c44d6a4850ea8a56cbf",
            //    ActionId = "52dd8c44d6a4850ea8a56cbe",
            //    SourceId = "52a64270d433231824878c93",
            //    Order = 1,
            //    Enabled = true,
            //    ElementState = 0,
            //    Completed = false,
            //    StepTypeId = 2,
            //    ControlType = 0,
            //    SelectType = 0,
            //    IncludeTime = false,
            //    Header = string.Empty,
            //    Description = "P4H Enrollment detail script and eligibility details",
            //    Question = "Is patient a Diabetic?",
            //    Notes = "Example notes",
            //    Title = "P4H Program Details",
            //    Text = "Partner for Health is an ABC co wellness initiative designed to support employees and their spouses in their efforts to improve overall health through the implementation of a  combination of advanced laboratory tesing and individual lifestyle coaching.",
            //    Status = 1,
            //    SelectedResponseId = "52dd5c5bd6a4850ea8b7539b",
            //    Responses = new List<Response> { 
            //        new Response { Id = "52dd8c44d6a4850ea8a56cc0", 
            //            Nominal = true, 
            //            Text="", 
            //            Required=false, 
            //            Value=string.Empty, 
            //            Order = 1, 
            //            NextStepId = "52dd8c44d6a4850ea8a56cc1", 
            //            StepId = "52dd8c44d6a4850ea8a56cbf" }
            //    }
            //};

            //// complete step action
            //Step s1 = new Step()
            //{
            //    SourceId = "52cf3ade1e6015397c1bcc2c",
            //    Order = 2,
            //    Enabled = true,
            //    ElementState = 0,
            //    Completed = false,
            //    Id = "52dd5c5bd6a4850ea8b7539d",
            //    ActionId = "52dd5c5bd6a4850ea8b75399",
            //    StepTypeId = 7,
            //    Header = string.Empty,
            //    ControlType = 0,
            //    SelectType = 0,
            //    IncludeTime = false,
            //    Question = string.Empty,
            //    Description = "complete - final step in action",
            //     Text = "complete",
            //    Notes = "Example notes",
            //    Status = 1
            //};

            //List<SpawnElement> se = new List<SpawnElement>();
            //se.Add(new SpawnElement { ElementId = "52dd5c5bd6a4850ea8b7539e", ElementType = 2 });

            //List<Objective> objs = new List<Objective>();
            //objs.Add(new Objective { Id = "", Value = "75", Unit ="%", Status = 5 });

            //Actions act = new Actions
            //{
            //    SourceId = "52cb205d1e601522209c44a8",
            //    Order = 1,
            //    Enabled = true,
            //    ElementState = 0,
            //    Next = string.Empty,
            //    Previous = string.Empty,
            //    SpawnElement = se,
            //    Id = "52dd5c5bd6a4850ea8b75399",
            //    ModuleId = "52dd5c5bd6a4850ea8b75398",
            //    Name = "Diabetic screening",
            //    Description = "Diabetic screening for initial assesment",
            //    CompletedBy = "Care Manager",
            //    Completed = true,
            //    Text = "text",
            //    Objectives = objs,
            //    Steps = new List<Step> { s0, s1 },
            //    Status = 1
            //};
            #endregion

            string json = GetString();

            Actions act = (Actions)ServiceStack.Text.JsonSerializer.DeserializeFromString(json, typeof(Actions));
            return act;
        }

        private static string GetString()
        {
            string json = "{\"SourceId\":\"52cb202e1e601522209c44a4\",\"Order\":1,\"Enabled\":true,\"ElementState\":0,\"Completed\":true,\"Next\":\"\",\"Id\":\"52dd8c44d6a4850ea8a56cbe\",\"ModuleId\":\"52dd8c44d6a4850ea8a56cbd\",\"Name\":\"Program Eligibility\",\"Description\":\"check Program Eligibility for P4H program\",\"CompletedBy\":\"Care Manager\",\"Objectives\":[{\"Id\":\"52a0bef0d43323141c9eb26d\",\"Value\":\"75\",\"Unit\":\"%\",\"Status\":5}],\"Steps\":[{\"SourceId\":\"52a64270d433231824878c93\",\"Order\":1,\"Enabled\":true,\"ElementState\":0,\"Completed\":false,\"Id\":\"52dd8c44d6a4850ea8a56cbf\",\"ActionId\":\"52dd8c44d6a4850ea8a56cbe\",\"StepTypeId\":2,\"Header\":\"\",\"SelectedResponseId\":\"52dd8c44d6a4850ea8a56cc0\",\"ControlType\":0,\"SelectType\":0,\"IncludeTime\":false,\"Question\":null,\"Title\":\"P4H Program Details\",\"Description\":\"P4H Enrollment detail script and eligibility details\",\"Text\":\"Partner for Health is an ABC co wellness initiative designed to support employees and their spouses in their efforts to improve overall health through the implementation of a  combination of advanced laboratory tesing and individual lifestyle coaching.\",\"Ex\":null,\"Status\":1,\"Responses\":[{\"Id\":\"52dd8c44d6a4850ea8a56cc0\",\"Order\":1,\"Text\":\"\",\"stepid\":\"52dd8c44d6a4850ea8a56cbf\",\"Value\":\"\",\"Nominal\":true,\"Required\":false,\"NextStepId\":\"52dd8c44d6a4850ea8a56cc1\"}]},{\"SourceId\":\"52a641e8d433231824878c8f\",\"Order\":2,\"Enabled\":true,\"ElementState\":0,\"Completed\":false,\"Id\":\"52dd8c44d6a4850ea8a56cc1\",\"ActionId\":\"52dd8c44d6a4850ea8a56cbe\",\"StepTypeId\":1,\"Header\":null,\"SelectedResponseId\":\"\",\"ControlType\":0,\"SelectType\":0,\"IncludeTime\":false,\"Question\":\"Are you an ABC Employee?\",\"Description\":null,\"Text\":null,\"Ex\":null,\"Status\":1,\"Responses\":[{\"Id\":\"52dd8c44d6a4850ea8a56cc2\",\"Order\":1,\"Text\":\"Yes\",\"stepid\":\"52dd8c44d6a4850ea8a56cc1\",\"Value\":\"\",\"Nominal\":true,\"Required\":false,\"NextStepId\":\"52dd8c44d6a4850ea8a56cc7\"},{\"Id\":\"52dd8c44d6a4850ea8a56cc3\",\"Order\":2,\"Text\":\"No\",\"stepid\":\"52dd8c44d6a4850ea8a56cc1\",\"Value\":\"\",\"Nominal\":false,\"Required\":false,\"NextStepId\":\"52dd8c44d6a4850ea8a56cc4\"}]},{\"SourceId\":\"52a641f1d433231824878c90\",\"Order\":3,\"Enabled\":false,\"ElementState\":0,\"Completed\":false,\"Id\":\"52dd8c44d6a4850ea8a56cc4\",\"ActionId\":\"52dd8c44d6a4850ea8a56cbe\",\"StepTypeId\":1,\"Header\":null,\"SelectedResponseId\":\"\",\"ControlType\":0,\"SelectType\":0,\"IncludeTime\":false,\"Question\":\"Are you a spouse of an ABC Employee?\",\"Description\":null,\"Text\":null,\"Ex\":null,\"Status\":1,\"Responses\":[{\"Id\":\"52dd8c44d6a4850ea8a56cc5\",\"Order\":1,\"Text\":\"Yes\",\"stepid\":\"52dd8c44d6a4850ea8a56cc4\",\"Value\":\"\",\"Nominal\":true,\"Required\":false,\"NextStepId\":\"52dd8c44d6a4850ea8a56cc7\"},{\"Id\":\"52dd8c44d6a4850ea8a56cc6\",\"Order\":2,\"Text\":\"No\",\"stepid\":\"52dd8c44d6a4850ea8a56cc4\",\"Value\":\"\",\"Nominal\":false,\"Required\":false,\"NextStepId\":\"52dd8c44d6a4850ea8a56ccc\"}]},{\"SourceId\":\"52cb15401e601522209c4496\",\"Order\":4,\"Enabled\":false,\"ElementState\":0,\"Completed\":false,\"Id\":\"52dd8c44d6a4850ea8a56cc7\",\"ActionId\":\"52dd8c44d6a4850ea8a56cbe\",\"StepTypeId\":1,\"Header\":null,\"SelectedResponseId\":\"\",\"ControlType\":0,\"SelectType\":0,\"IncludeTime\":false,\"Question\":\"Is the HRA complete?\",\"Description\":null,\"Text\":null,\"Ex\":null,\"Status\":1,\"Responses\":[{\"Id\":\"52dd8c44d6a4850ea8a56cc8\",\"Order\":1,\"Text\":\"Yes\",\"stepid\":\"52dd8c44d6a4850ea8a56cc7\",\"Value\":\"\",\"Nominal\":false,\"Required\":false,\"NextStepId\":\"52dd8c44d6a4850ea8a56cca\"},{\"Id\":\"52dd8c44d6a4850ea8a56cc9\",\"Order\":2,\"Text\":\"No\",\"stepid\":\"52dd8c44d6a4850ea8a56cc7\",\"Value\":\"\",\"Nominal\":false,\"Required\":false,\"NextStepId\":\"52dd8c44d6a4850ea8a56ccc\"}]},{\"SourceId\":\"52d42fe01e601521285e97ff\",\"Order\":5,\"Enabled\":false,\"ElementState\":0,\"Completed\":false,\"Id\":\"52dd8c44d6a4850ea8a56cca\",\"ActionId\":\"52dd8c44d6a4850ea8a56cbe\",\"StepTypeId\":2,\"Header\":null,\"SelectedResponseId\":\"\",\"ControlType\":0,\"SelectType\":0,\"IncludeTime\":false,\"Question\":null,\"Title\":\"P4H Program Details\",\"Description\":\"P4H program eligibility details\",\"Text\":\"You are eligible for the program \",\"Ex\":null,\"Status\":1,\"Responses\":[{\"Id\":\"52dd8c44d6a4850ea8a56ccb\",\"Order\":1,\"Text\":\"\",\"stepid\":\"52dd8c44d6a4850ea8a56cca\",\"Value\":\"\",\"Nominal\":true,\"Required\":false,\"NextStepId\":\"52dd8c44d6a4850ea8a56cce\"}]},{\"SourceId\":\"52d42ffc1e601521285e9800\",\"Order\":6,\"Enabled\":false,\"ElementState\":0,\"Completed\":false,\"Id\":\"52dd8c44d6a4850ea8a56ccc\",\"ActionId\":\"52dd8c44d6a4850ea8a56cbe\",\"StepTypeId\":2,\"Header\":null,\"SelectedResponseId\":\"\",\"ControlType\":0,\"SelectType\":0,\"IncludeTime\":false,\"Question\":null,\"Title\":\"P4H Program Details\",\"Description\":\"P4H program in-eligibility details\",\"Notes\":\"NOTE:\",\"Text\":\"You do not qualify for the program since the screening was not Completed\",\"Ex\":null,\"Status\":1,\"Responses\":[{\"Id\":\"52dd8c44d6a4850ea8a56ccd\",\"Order\":1,\"Text\":\"\",\"stepid\":\"52dd8c44d6a4850ea8a56ccc\",\"Value\":\"\",\"Nominal\":true,\"Required\":false,\"NextStepId\":\"52dd8c44d6a4850ea8a56cce\"}]},{\"SourceId\":\"52cf3ade1e6015397c1bcc2c\",\"Order\":7,\"Enabled\":true,\"ElementState\":0,\"Completed\":false,\"Id\":\"52dd8c44d6a4850ea8a56cce\",\"ActionId\":\"52dd8c44d6a4850ea8a56cbe\",\"StepTypeId\":7,\"Header\":null,\"SelectedResponseId\":\"\",\"ControlType\":0,\"SelectType\":0,\"IncludeTime\":false,\"Question\":null,\"Description\":\"complete - final step in action\",\"Text\":\"complete\",\"Ex\":null,\"Status\":1,\"Responses\":null}],\"Status\":1}";

            return json;
        }
    }
}
