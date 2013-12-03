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
        public void GetModuleByID()
        {
            string controlValue = "Tony";
            string sampleValue;
            string ModuleID = "52781cd8fe7a5925fcee5bf3";
            string contractNumber = "InHealth001";
            string context ="NG";
            IRestClient client = new JsonServiceClient();

            //JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("APIKey:{0}", "12345"));

            GetModuleResponse response = client.Post<GetModuleResponse>("http://localhost:8888/NG/data/Module",
                new GetModuleRequest { ModuleID = ModuleID, ContractNumber = contractNumber, Context = context } as object);

            sampleValue = string.Empty;

            Assert.AreEqual(controlValue, sampleValue);
        }
    }
}
