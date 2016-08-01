using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Module.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Module.Services.Test
{
    [TestClass]
    public class User_Services_Test
    {
        [TestMethod]
        public void Get_ModlueByID_with_3_objectives()
        {
            string url = "http://localhost:8888/Module";
            string moduleID = "52a0a8c2fe7a5915485bdfd4";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetModuleResponse response = client.Get<GetModuleResponse>(
                string.Format("{0}/{1}/{2}/{3}/Module/{4}", url, context, version, contractNumber, moduleID));

            Assert.AreEqual(moduleID, response.Module.Id);
            Assert.IsTrue(response.Module.Objectives.Count == 3);
        }

        [TestMethod]
        public void Get_ModlueByID_with_2_objectives()
        {
            string url = "http://localhost:8888/Module";
            string moduleID = "52a0a775fe7a5915485bdfd1";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetModuleResponse response = client.Get<GetModuleResponse>(
                string.Format("{0}/{1}/{2}/{3}/Module/{4}", url, context, version, contractNumber, moduleID));

            Assert.AreEqual(moduleID, response.Module.Id);
            Assert.IsTrue(response.Module.Objectives.Count == 2);
        }

        [TestMethod]
        public void Get_ModlueByID_with_1_objective()
        {
            string url = "http://localhost:8888/Module";
            string moduleID = "52a0a7fffe7a5915485bdfd2";
            string contractNumber = "InHealth001";
            string context = "NG";
            double version = 1.0;
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            GetModuleResponse response = client.Get<GetModuleResponse>(
                string.Format("{0}/{1}/{2}/{3}/Module/{4}", url, context, version, contractNumber, moduleID));

            Assert.AreEqual(moduleID, response.Module.Id);
            Assert.IsTrue(response.Module.Objectives.Count == 1);
        }

        //[TestMethod]
        //public void Get_ModlueByID_with_3_objectives()
        //{
        //    string url = "http://localhost:8888/Module";
        //    string moduleID = "52a0a8c2fe7a5915485bdfd4";
        //    string contractNumber = "InHealth001";
        //    string context = "NG";
        //    double version = 1.0;
        //    IRestClient client = new JsonServiceClient();
        //    JsonServiceClient.HttpWebRequestFilter = x =>
        //                    x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

        //    GetModuleResponse response = client.Get<GetModuleResponse>(
        //        string.Format("{0}/{1}/{2}/{3}/Module/{4}", url, context, version, contractNumber, moduleID));

        //    Assert.AreEqual(moduleID, response.Module.Id);
        //    Assert.IsTrue(response.Module.Objectives.Count.Equals(3));
        //}
    }
}
