using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Program.Services.Test
{
    [TestClass]
    public class User_Services_Test
    {
        [TestMethod]
        public void GetProgramByID()
        {
            string controlValue = "Tony";
            string sampleValue;
            string ProgramID = "52781cd8fe7a5925fcee5bf3";
            string contractNumber = "InHealth001";
            string context ="NG";
            IRestClient client = new JsonServiceClient();

            //JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("APIKey:{0}", "12345"));

            ProgramResponse response = client.Post<ProgramResponse>("http://localhost:8888/NG/data/Program",
                new ProgramRequest { ProgramID = ProgramID, ContractNumber = contractNumber, Context = context } as object);

            sampleValue = string.Empty;

            Assert.AreEqual(controlValue, sampleValue);
        }
    }
}
