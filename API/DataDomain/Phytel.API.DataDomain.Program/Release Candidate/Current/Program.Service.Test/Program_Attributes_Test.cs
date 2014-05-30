using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Program_Attributes_Test
    {
        [TestMethod]
        public void Select_Attribute_Test()
        {
            string url = "http://localhost:8888/Program";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string planElementId = "52ede291fe7a590728e1a382";

            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetProgramAttributeResponse resp =
                                client.Get<GetProgramAttributeResponse>(
                                string.Format("{0}/{1}/{2}/{3}/Program/Attributes/?PlanElementId={4}",
                                url,
                                context,
                                version,
                                contractNumber,
                                planElementId));
        }

        [TestMethod]
        public void Update_Attribute_Test()
        {
            string url = "http://localhost:8888/Program";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string planElementId = "52ede291fe7a590728e1a382";

            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            PutUpdateProgramAttributesResponse resp =
                                client.Put<PutUpdateProgramAttributesResponse>(
                                string.Format("{0}/{1}/{2}/{3}/Program/Attributes/Update/",
                                url,
                                context,
                                version,
                                contractNumber,
                                planElementId), new PutUpdateProgramAttributesRequest
                                {
                                    ProgramAttributes = new ProgramAttributeData { 
                                        PlanElementId = planElementId ,
                                     //AssignedBy = "test",
                                      //EligibilityEndDate = System.DateTime.UtcNow,
                                       RemovedReason = "This is a removed reason!"
                                    }
                                } as object);
        }

        [TestMethod]
        public void Insert_Attribute_Test()
        {
            string url = "http://localhost:8888/Program";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            string planElementId = "52ede291fe7a590728e1a382";

            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            PutProgramAttributesResponse resp =
                                client.Put<PutProgramAttributesResponse>(
                                string.Format("{0}/{1}/{2}/{3}/Program/Attributes/Insert/",
                                url,
                                context,
                                version,
                                contractNumber,
                                planElementId), new PutProgramAttributesRequest
                                {
                                    ProgramAttributes = new ProgramAttributeData
                                    {
                                        PlanElementId = planElementId,
                                        //AssignedBy = "test",
                                        //EligibilityEndDate = System.DateTime.UtcNow,
                                        RemovedReason = "This is a removed reason!"
                                    }
                                } as object);
        }
    }
}
