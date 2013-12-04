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
        public void Get_ModlueByID()
        {
            string url = "http://localhost:8888/Module";
            string moduleID = "529fb367fe7a591f68ce4acd";
            string contractNumber = "InHealth001";
            string context = "NG";
            string version = "v1";
            IRestClient client = new JsonServiceClient();

            GetModuleResponse response = client.Get<GetModuleResponse>(
                string.Format("{0}/{1}/{2}/{3}/Module/{4}", url, context, version, contractNumber, moduleID));

            //Assert.AreEqual(ProgramID, response.Program.ProgramID);
        }
    }
}
