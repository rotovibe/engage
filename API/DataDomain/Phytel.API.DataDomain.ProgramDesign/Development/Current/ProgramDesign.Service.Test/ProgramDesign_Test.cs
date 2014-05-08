using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.ProgramDesign.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using MongoDB.Bson;

namespace Phytel.API.DataDomain.ProgramDesign.Services.Test
{
    [TestClass]
    public class User_ProgramDesign_Test
    {
        [TestMethod]
        public void Post_ProgramDesignByID()
        {
            //string url = "http://localhost:8888/ProgramDesign";
            //string ProgramDesignID = "??";
            //string contractNumber = "InHealth001";
            //string context = "NG";
            //string version = "1.0";

            ////Set the value of this call to a valid ContactId (Contact Collection _id field) from the contract database
            //IRestClient client = Common.Helper.GetJsonServiceClient("531f2dfa072ef727c4d2a3f2");

            //GetProgramDesignResponse response = client.Post<GetProgramDesignResponse>(
            //    string.Format("{0}/{1}/{2}/{3}/ProgramDesign/{4}", url, context, version, contractNumber, ProgramDesignID),
            //    new GetProgramDesignResponse() as object);

            //Assert.AreEqual(string.Empty, string.Empty);
        }

        [TestMethod]
        public void GetProgramData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetProgramResponse response = client.Get<GetProgramResponse>(string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Program/{2}",
                context,
                contractNumber,
                "53694ae85a4d13013059354d"));

            Assert.IsTrue(response.Status.Message == null);
        }

        [TestMethod]
        public void GetModuleData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetModuleResponse response = client.Get<GetModuleResponse>(string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Module/{2}",
                context,
                contractNumber,
                "53694c135a4d13119cdf0893"));

            Assert.IsTrue(response.Status.Message == null);
        }

        [TestMethod]
        public void GetActionData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetActionDataResponse response = client.Get<GetActionDataResponse>(string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Action/{2}",
                context,
                contractNumber,
                "536956315a4d130f58903225"));

            Assert.IsTrue(response.Status.Message == null);
        }

        [TestMethod]
        public void GetTextStepData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetTextStepDataResponse response = client.Get<GetTextStepDataResponse>(string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/TextStep/{2}",
                context,
                contractNumber,
                "53695a295a4d13054cc381f2"));

            Assert.IsTrue(response.Status.Message == null);
        }

        [TestMethod]
        public void GetYesNoStepData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetYesNoStepDataResponse response = client.Get<GetYesNoStepDataResponse>(string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/YesNoStep/{2}",
                context,
                contractNumber,
                "536b97415a4d1310f4c45990"));

            Assert.IsTrue(response.Status.Message == null);
        }

        [TestMethod]
        public void PutProgramData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            PutProgramDataResponse response = client.Put<PutProgramDataResponse>("http://localhost:8888/ProgramDesign/NG/1/inhealth001/ProgramDesign/Program/Insert",
                new PutProgramDataRequest { Name = "programdesign.service.test", ContractNumber = contractNumber, Context = context } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutModuleData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            PutModuleDataResponse response = client.Put<PutModuleDataResponse>("http://localhost:8888/ProgramDesign/NG/1/inhealth001/ProgramDesign/Module/Insert",
                new PutModuleDataRequest { Name = "programdesign.service.test", ContractNumber = contractNumber, Context = context } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutActionData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            PutActionDataResponse response = client.Put<PutActionDataResponse>("http://localhost:8888/ProgramDesign/NG/1/inhealth001/ProgramDesign/Action/Insert",
                new PutActionDataRequest { Name = "programdesign.service.test", ContractNumber = contractNumber, Context = context } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutTextStepData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            PutTextStepDataResponse response = client.Put<PutTextStepDataResponse>("http://localhost:8888/ProgramDesign/NG/1/inhealth001/ProgramDesign/TextStep/Insert",
                new PutTextStepDataRequest { Title = "programdesign.service.test", ContractNumber = contractNumber, Context = context } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutYesNoStepData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            PutYesNoStepDataResponse response = client.Put<PutYesNoStepDataResponse>("http://localhost:8888/ProgramDesign/NG/1/inhealth001/ProgramDesign/YesNoStep/Insert",
                new PutYesNoStepDataRequest { Question = "programdesign.service.test", ContractNumber = contractNumber, Context = context } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutUpdateProgramData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string url = string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Program/{2}/Update",
                context,
                contractNumber,
                "53694ae85a4d13013059354d");

            PutUpdateProgramDataResponse response = client.Put<PutUpdateProgramDataResponse>(url,
                new PutUpdateProgramDataRequest { Name = "programdesign.service.test", ContractNumber = contractNumber, Context = context } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutUpdateModuleData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string url = string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Module/{2}/Update",
                context,
                contractNumber,
                "53694c135a4d13119cdf0893");

            PutUpdateModuleDataResponse response = client.Put<PutUpdateModuleDataResponse>(url,
                new PutUpdateModuleDataRequest { Name = "programdesign.service.test", ContractNumber = contractNumber, Context = context } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutUpdateActionData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string url = string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Action/{2}/Update",
                context,
                contractNumber,
                "536956315a4d130f58903225");

            PutUpdateActionDataResponse response = client.Put<PutUpdateActionDataResponse>(url,
                new PutUpdateActionDataRequest { Name = "programdesign.service.test", ContractNumber = contractNumber, Context = context } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutUpdateTextStepData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string url = string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/TextStep/{2}/Update",
                context,
                contractNumber,
                "53695a295a4d13054cc381f2");

            PutUpdateTextStepDataResponse response = client.Put<PutUpdateTextStepDataResponse>(url,
                new PutUpdateTextStepDataRequest { Title = "programdesign.service.test", ContractNumber = contractNumber, Context = context } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutUpdateYesNoStepData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string url = string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/YesNoStep/{2}/Update",
                context,
                contractNumber,
                "536b97415a4d1310f4c45990");

            PutUpdateYesNoStepDataResponse response = client.Put<PutUpdateYesNoStepDataResponse>(url,
                new PutUpdateYesNoStepDataRequest { Question = "programdesign.service.test", ContractNumber = contractNumber, Context = context } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void DeleteProgramData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            DeleteProgramDataResponse response = client.Delete<DeleteProgramDataResponse>(string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Program/{2}/Delete",
                                                        context,
                                                        contractNumber,
                                                        "53694ae85a4d13013059354d"));

            Assert.IsTrue(response.Deleted);
        }

        [TestMethod]
        public void DeleteModuleData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            DeleteModuleDataResponse response = client.Delete<DeleteModuleDataResponse>(string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Module/{2}/Delete",
                                                        context,
                                                        contractNumber,
                                                        "53694c135a4d13119cdf0893"));

            Assert.IsTrue(response.Deleted);
        }

        [TestMethod]
        public void DeleteActionData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));


            DeleteActionDataResponse response = 
                client.Delete<DeleteActionDataResponse>(string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Action/{2}/Delete",
                                                        context,
                                                        contractNumber,
                                                        "536956315a4d130f58903225"));

            Assert.IsTrue(response.Deleted);
        }

        [TestMethod]
        public void DeleteTextStepData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            DeleteTextStepDataResponse response =
                client.Delete<DeleteTextStepDataResponse>(string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/TextStep/{2}/Delete",
                                                        context,
                                                        contractNumber,
                                                        "53695a295a4d13054cc381f2"));
            
            Assert.IsTrue(response.Deleted);
        }

        [TestMethod]
        public void DeleteYesNoStepData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            DeleteYesNoStepDataResponse response =
                client.Delete<DeleteYesNoStepDataResponse>(string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/YesNoStep/{2}/Delete",
                                                        context,
                                                        contractNumber,
                                                        "536b97415a4d1310f4c45990"));

            Assert.IsTrue(response.Deleted);
        }

        [TestMethod]
        public void PutModuleInProgram()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string url = string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Program/{2}/Module/{3}",
                context,
                contractNumber,
                "53694ae85a4d13013059354d",
                "53694c135a4d13119cdf0893");

            PutModuleDataResponse response = client.Put<PutModuleDataResponse>(url,
                new PutModuleDataRequest {  } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutActionInModule()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string url = string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Module/{2}/Action/{3}",
                context,
                contractNumber,
                "53694c135a4d13119cdf0893",
                "536956315a4d130f58903225");

            PutActionDataResponse response = client.Put<PutActionDataResponse>(url,
                new PutActionDataRequest {  } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutTextStepInAction()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string url = string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Action/{2}/Text/{3}",
                context,
                contractNumber,
                "536956315a4d130f58903225",
                "53695a295a4d13054cc381f2");

            PutTextStepDataResponse response = client.Put<PutTextStepDataResponse>(url,
                new PutTextStepDataRequest {  } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutYesNoStepInAction()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string url = string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Action/{2}/YesNo/{3}",
                context,
                contractNumber,
                "536956315a4d130f58903225",
                "536b97415a4d1310f4c45990");

            PutYesNoStepDataResponse response = client.Put<PutYesNoStepDataResponse>(url,
                new PutYesNoStepDataRequest { } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }
    }
}
