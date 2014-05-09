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
            string userid = "531f2df9072ef727c4d2a3df";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetProgramResponse response = client.Get<GetProgramResponse>(string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Program/{2}?userid={3}",
                context,
                contractNumber,
                "53694ae85a4d13013059354d",
                userid));

            Assert.IsTrue(response.Status.Message == null);
        }

        [TestMethod]
        public void GetModuleData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "531f2df9072ef727c4d2a3df";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetModuleResponse response = client.Get<GetModuleResponse>(string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Module/{2}?userid={3}",
                context,
                contractNumber,
                "53694c135a4d13119cdf0893",
                userid));

            Assert.IsTrue(response.Status.Message == null);
        }

        [TestMethod]
        public void GetActionData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "531f2df9072ef727c4d2a3df";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetActionDataResponse response = client.Get<GetActionDataResponse>(string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Action/{2}?userid={3}",
                context,
                contractNumber,
                "536956315a4d130f58903225",
                userid));

            Assert.IsTrue(response.Status.Message == null);
        }

        [TestMethod]
        public void GetStepData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "531f2df9072ef727c4d2a3df";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetStepDataResponse response = client.Get<GetStepDataResponse>(string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Step/{2}?userid={3}",
                context,
                contractNumber,
                "53695a295a4d13054cc381f2",
                userid));

            Assert.IsTrue(response.Status.Message == null);
        }

        [TestMethod]
        public void PutProgramData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "531f2df9072ef727c4d2a3df";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "UserId", "5325c821072ef705080d3488"));

            string url = string.Format("http://localhost:8888/ProgramDesign/NG/1/inhealth001/ProgramDesign/Program/Insert?userid={0}",
                userid);

            PutProgramDataResponse response = client.Put<PutProgramDataResponse>(url,
                new PutProgramDataRequest { Name = "programdesign.service.test", ContractNumber = contractNumber, Context = context } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutModuleData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "5325c821072ef705080d3488";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string url = string.Format("http://localhost:8888/ProgramDesign/NG/1/inhealth001/ProgramDesign/Module/Insert?userid={0}",
                userid);

            PutModuleDataResponse response = client.Put<PutModuleDataResponse>(url,
                new PutModuleDataRequest { Name = "programdesign.service.test", ContractNumber = contractNumber, Context = context } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutActionData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "5325c821072ef705080d3488";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string url = string.Format("http://localhost:8888/ProgramDesign/NG/1/inhealth001/ProgramDesign/Action/Insert?userid={0}",
                userid);

            PutActionDataResponse response = client.Put<PutActionDataResponse>(url,
                new PutActionDataRequest { Name = "programdesign.service.test", ContractNumber = contractNumber, Context = context } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutStepData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "5325c821072ef705080d3488";
            IRestClient client = new JsonServiceClient();

            string url = string.Format("http://localhost:8888/ProgramDesign/NG/1/inhealth001/ProgramDesign/Step/Insert?userid={0}",
                userid);

            PutStepDataResponse response = client.Put<PutStepDataResponse>(url,
                new PutStepDataRequest { Title = "programdesign.service.test step", ContractNumber = contractNumber, Context = context } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutTextStepData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "5325c821072ef705080d3488";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string url = string.Format("http://localhost:8888/ProgramDesign/NG/1/inhealth001/ProgramDesign/TextStep/Insert?userid={0}",
                userid);

            PutTextStepDataResponse response = client.Put<PutTextStepDataResponse>(url,
                new PutTextStepDataRequest { Title = "programdesign.service.test", ContractNumber = contractNumber, Context = context } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutYesNoStepData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "5325c821072ef705080d3488";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string url = string.Format("http://localhost:8888/ProgramDesign/NG/1/inhealth001/ProgramDesign/YesNoStep/Insert?userid={0}",
                userid);

            PutYesNoStepDataResponse response = client.Put<PutYesNoStepDataResponse>(url,
                new PutYesNoStepDataRequest { Question = "programdesign.service.test", ContractNumber = contractNumber, Context = context } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutUpdateProgramData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "5325c821072ef705080d3488";
            string programId = "536cfb7f5a4d1313f8f41db1";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string url = string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Program/{2}/Update?userid={3}",
                context,
                contractNumber,
                programId,
                userid);

            PutUpdateProgramDataResponse response = client.Put<PutUpdateProgramDataResponse>(url,
                new PutUpdateProgramDataRequest { Description = "programdesign.service.test", ContractNumber = contractNumber, Context = context } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutUpdateModuleData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "5325c821072ef705080d3488";
            string moduleId = "536cfdbd5a4d1313f8f41db8";
            IRestClient client = new JsonServiceClient();
            //JsonServiceClient.HttpWebRequestFilter = x =>
            //                x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string url = string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Module/{2}?userid={3}",
                context,
                contractNumber,
                moduleId,
                userid);

            PutUpdateModuleDataResponse response = client.Put<PutUpdateModuleDataResponse>(url,
                new PutUpdateModuleDataRequest { Description = "programdesign.service.test", ContractNumber = contractNumber, Context = context } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutUpdateActionData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "5325c821072ef705080d3488";
            string actionId = "536cfe005a4d1313f8f41dbf";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string url = string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Action/{2}/Update?userid={3}",
                context,
                contractNumber,
                actionId,
                userid);

            PutUpdateActionDataResponse response = client.Put<PutUpdateActionDataResponse>(url,
                new PutUpdateActionDataRequest { Description = "programdesign.service.test", ContractNumber = contractNumber, Context = context } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutUpdateStepData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "5325c821072ef705080d3488";
            string textId = "536d32aa5a4d130998577212";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string url = string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Step/{2}/Update?userid={3}",
                context,
                contractNumber,
                textId,
                userid);

            PutUpdateStepDataResponse response = client.Put<PutUpdateStepDataResponse>(url,
                new PutUpdateStepDataRequest { Description = "programdesign.service.test", ContractNumber = contractNumber, Context = context } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutUpdateTextStepData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "5325c821072ef705080d3488";
            string textId = "536d001e5a4d1313f8c1757b";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string url = string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/TextStep/{2}/Update?userid={3}",
                context,
                contractNumber,
                textId,
                userid);

            PutUpdateTextStepDataResponse response = client.Put<PutUpdateTextStepDataResponse>(url,
                new PutUpdateTextStepDataRequest { Description = "programdesign.service.test", ContractNumber = contractNumber, Context = context } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutUpdateYesNoStepData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "5325c821072ef705080d3488";
            string yesNoId = "536d009e5a4d1313f8c17582";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string url = string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/YesNoStep/{2}/Update?userid={3}",
                context,
                contractNumber,
                yesNoId,
                userid);

            PutUpdateYesNoStepDataResponse response = client.Put<PutUpdateYesNoStepDataResponse>(url,
                new PutUpdateYesNoStepDataRequest { Notes = "programdesign.service.test", ContractNumber = contractNumber, Context = context } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void DeleteProgramData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "5325c821072ef705080d3488";
            string programId = "53694ae85a4d13013059354d";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            DeleteProgramDataResponse response = client.Delete<DeleteProgramDataResponse>(string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Program/{2}/Delete?userid={3}",
                                                        context,
                                                        contractNumber,
                                                        programId,
                                                        userid));

            Assert.IsTrue(response.Deleted);
        }

        [TestMethod]
        public void DeleteModuleData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "5325c821072ef705080d3488";
            string moduleId = "53694c135a4d13119cdf0893";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            DeleteModuleDataResponse response = client.Delete<DeleteModuleDataResponse>(string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Module/{2}/Delete?userid={3}",
                                                        context,
                                                        contractNumber,
                                                        moduleId,
                                                        userid));

            Assert.IsTrue(response.Deleted);
        }

        [TestMethod]
        public void DeleteActionData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "5325c821072ef705080d3488";
            string actionId = "536956315a4d130f58903225";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));


            DeleteActionDataResponse response =
                client.Delete<DeleteActionDataResponse>(string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Action/{2}/Delete?userid={3}",
                                                        context,
                                                        contractNumber,
                                                        actionId,
                                                        userid));

            Assert.IsTrue(response.Deleted);
        }

        [TestMethod]
        public void DeleteStepData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "5325c821072ef705080d3488";
            string textId = "53695a295a4d13054cc381f2";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            DeleteStepDataResponse response =
                client.Delete<DeleteStepDataResponse>(string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/TextStep/{2}/Delete?userid={3}",
                                                        context,
                                                        contractNumber,
                                                        textId,
                                                        userid));

            Assert.IsTrue(response.Deleted);
        }

        [TestMethod]
        public void DeleteTextStepData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "5325c821072ef705080d3488";
            string textId = "53695a295a4d13054cc381f2";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            DeleteTextStepDataResponse response =
                client.Delete<DeleteTextStepDataResponse>(string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/TextStep/{2}/Delete?userid={3}",
                                                        context,
                                                        contractNumber,
                                                        textId,
                                                        userid));
            
            Assert.IsTrue(response.Deleted);
        }

        [TestMethod]
        public void DeleteYesNoStepData()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "5325c821072ef705080d3488";
            string yesNoId = "536b97415a4d1310f4c45990";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            DeleteYesNoStepDataResponse response =
                client.Delete<DeleteYesNoStepDataResponse>(string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Step/YesNo/{2}/Delete?userid={3}",
                                                        context,
                                                        contractNumber,
                                                        yesNoId,
                                                        userid));

            Assert.IsTrue(response.Deleted);
        }

        [TestMethod]
        public void PutModuleInProgram()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "5325c821072ef705080d3488";
            string programId = "536cfb7f5a4d1313f8f41db1";
            string moduleId = "536cfdbd5a4d1313f8f41db8";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string url = string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Program/{2}/Module/{3}?userid={3}",
                context,
                contractNumber,
                programId,
                moduleId,
                userid);

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
            string userid = "5325c821072ef705080d3488";
            string moduleId = "536cfdbd5a4d1313f8f41db8";
            string actionId = "536cfe005a4d1313f8f41dbf";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string url = string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Module/{2}/Action/{3}?userid={3}",
                context,
                contractNumber,
                moduleId,
                actionId,
                userid);

            PutActionDataResponse response = client.Put<PutActionDataResponse>(url,
                new PutActionDataRequest {  } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutStepInAction()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "5325c821072ef705080d3488";
            string actionId = "536cfe005a4d1313f8f41dbf";
            string textId = "536d001e5a4d1313f8c1757b";
            IRestClient client = new JsonServiceClient();

            string url = string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Action/{2}/Step/{3}?userid={3}",
                context,
                contractNumber,
                actionId,
                textId,
                userid);

            PutStepDataResponse response = client.Put<PutStepDataResponse>(url,
                new PutStepDataRequest { } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }

        [TestMethod]
        public void PutTextStepInAction()
        {
            string contractNumber = "InHealth001";
            string context = "NG";
            string userid = "5325c821072ef705080d3488";
            string actionId = "536cfe005a4d1313f8f41dbf";
            string textId = "536d001e5a4d1313f8c1757b";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string url = string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Action/{2}/Text/{3}?userid={3}",
                context,
                contractNumber,
                actionId,
                textId,
                userid);

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
            string userid = "5325c821072ef705080d3488";
            string actionId = "536cfe005a4d1313f8f41dbf";
            string yesNoId = "536d009e5a4d1313f8c17582";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            string url = string.Format("http://localhost:8888/ProgramDesign/{0}/1/{1}/ProgramDesign/Action/{2}/YesNo/{3}?userid={3}",
                context,
                contractNumber,
                actionId,
                yesNoId,
                userid);

            PutYesNoStepInActionResponse response = client.Put<PutYesNoStepInActionResponse>(url,
                new PutYesNoStepInActionRequest { Context = context, ContractNumber = contractNumber } as object);


            ObjectId id;

            Assert.IsTrue(ObjectId.TryParse(response.Id, out id));
        }
    }
}
