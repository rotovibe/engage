using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Responses_Tests
    {
        //[TestMethod]
        //public void Update_Response_Test()
        //{
        //    string url = "http://localhost:8888/Program";
        //    string contractNumber = "InHealth001";
        //    string context = "NG";
        //    double version = 1.0;
        //    string stepId = "52ed9eaffe7a5921b4880cd4";
        //    string userId = "bb241c64-a0ff-4e01-ba5f-4246ef50780e";

        //    IRestClient client = new JsonServiceClient();
        //    JsonServiceClient.HttpWebRequestFilter = x =>
        //                    x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

        //    PutUpdateResponseResponse resp =
        //                        client.Put<PutUpdateResponseResponse>(
        //                        string.Format("{0}/{1}/{2}/{3}/Program/Module/Action/Step/{4}/Responses/Update/",
        //                        url,
        //                        context,
        //                        version,
        //                        contractNumber,
        //                        stepId), new PutUpdateResponseRequest
        //                        {
        //                            ResponseDetail = new ResponseDetail
        //                            {
        //                                Id = "52ede291fe7a590728e1a382",
        //                                NextStepId = "000011111111111111110000",
        //                                Nominal = true,
        //                                Order = 1,
        //                                Required = false,
        //                                StepId = stepId,
        //                                Text = "blah Text",
        //                                Value = "blah value",
        //                                SpawnElement = null
        //                            },
        //                            UserId = userId,
        //                            Version = version
        //                        } as object);

        //}

        [TestMethod]
        public void Find_Response_Test()
        {
            string url = "http://localhost:8888/Program";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string stepId = "52ed9eaffe7a5921b4880cd4";
            string userId = "bb241c64-a0ff-4e01-ba5f-4246ef50780e";
            string responseId = "52ede291fe7a590728e1a382";

            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetStepResponseResponse resp =
                                client.Get<GetStepResponseResponse>(
                                string.Format("{0}/{1}/{2}/{3}/Program/Module/Action/Step/{4}/Response/?ResponseId={5}",
                                url,
                                context,
                                version,
                                contractNumber,
                                stepId,
                                responseId));

        }
    }
}
