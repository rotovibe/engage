using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;
using System.IO;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Action_Processing_response_saving_Tests
    {
        private static string actionPath = @"C:\Projects\tfs2013\PhytelCode\Phytel.Net\Services\API\AppDomain\Phytel.API.AppDomain.NG\Development\Current\NG.Tests\";

        [TestMethod]
        public void Action_process_saving_response_Tests()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string priority = "3";
            string version = "v1";
            string token = "52ee8f85d6a4850b30e8634c";
            string programId = "52eda010d6a4850b30bebb37";
            string patientId = "52e26f43072ef7191c119974";
            string actionID = "52eda010d6a4850b30bebb3c";
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
            Actions act = null;
            using (TextReader reader = File.OpenText(actionPath + "Action_Response_saving_Sample.txt"))
            {
                act = (Actions)ServiceStack.Text.JsonSerializer.DeserializeFromReader(reader, typeof(Actions));
            }
            return act;
        }
    }
}
