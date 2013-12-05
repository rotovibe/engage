using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Action.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Action.Services.Test
{
    [TestClass]
    public class User_Services_Test
    {
        //[TestMethod]
        //public void GetActionByID()
        //{
        //    string controlValue = "Tony";
        //    string sampleValue;
        //    string ActionID = "52781cd8fe7a5925fcee5bf3";
        //    string contractNumber = "InHealth001";
        //    string context ="NG";
        //    IRestClient client = new JsonServiceClient();

        //    //JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("APIKey:{0}", "12345"));

        //    GetActionResponse response = client.Post<GetActionResponse>("http://localhost:8888/NG/data/Action",
        //        new GetActionRequest { ActionID = ActionID, ContractNumber = contractNumber, Context = context } as object);

        //    sampleValue = string.Empty;

        //    Assert.AreEqual(controlValue, sampleValue);
        //}
    }
}
