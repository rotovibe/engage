using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;
using System.IO;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Action_Processing_response_spawn_Tests
    {
        [TestMethod]
        public void Action_process_check_for_valid_response_spawning_Tests()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string priority = "3";
            string version = "v1";
            string token = "52e1065cd6a4850cf0d4b023";
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
            Actions act = null;
            using (TextReader reader = File.OpenText(@"C:\Projects\tfs2013\PhytelCode\Phytel.Net\Services\API\AppDomain\Phytel.API.AppDomain.NG\Development\Current\NG.Tests\Action_Sample.txt"))
            {
                act = (Actions)ServiceStack.Text.JsonSerializer.DeserializeFromReader(reader, typeof(Actions));
            }
            return act;
        }
    }
}
