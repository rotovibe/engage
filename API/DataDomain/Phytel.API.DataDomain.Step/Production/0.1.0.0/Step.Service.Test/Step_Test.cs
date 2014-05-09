using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Step.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Step.Services.Test
{
    [TestClass]
    public class User_Services_Test
    {
        //[TestMethod]
        //public void GetStepByID()
        //{
        //    string controlValue = "Tony";
        //    string sampleValue;
        //    string StepID = "52781cd8fe7a5925fcee5bf3";
        //    string contractNumber = "InHealth001";
        //    string context ="NG";
        //    IRestClient client = new JsonServiceClient();

        //    //JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("APIKey:{0}", "12345"));

        //    GetStepResponse response = client.Post<GetStepResponse>("http://localhost:8888/NG/data/Step",
        //        new GetStepRequest { StepID = StepID, ContractNumber = contractNumber, Context = context } as object);

        //    sampleValue = string.Empty;

        //    Assert.AreEqual(controlValue, sampleValue);
        //}
    }
}
