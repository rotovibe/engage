using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Cohort.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Cohort.Services.Test
{
    [TestClass]
    public class User_Services_Test
    {
        [TestMethod]
        public void GetCohortByID()
        {
            string controlValue = "Tony";
            string sampleValue;
            string CohortID = "52781cd8fe7a5925fcee5bf3";
            string contractNumber = "InHealth001";
            string context ="NG";
            IRestClient client = new JsonServiceClient();

            //JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("APIKey:{0}", "12345"));

            CohortResponse response = client.Post<CohortResponse>("http://localhost:8888/NG/data/Cohort",
                new CohortRequest { CohortID = CohortID, ContractNumber = contractNumber, Context = context } as object);

            sampleValue = string.Empty;

            Assert.AreEqual(controlValue, sampleValue);
        }
    }
}
