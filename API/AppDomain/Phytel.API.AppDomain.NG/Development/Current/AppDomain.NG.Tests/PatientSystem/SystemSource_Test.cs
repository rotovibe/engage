using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Test.PatientSystem
{
    //[TestClass]
    public class SystemSource_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        string url = "http://localhost:888/Nightingale";
        IRestClient client = new JsonServiceClient();
        string token = "559d757a84ac072a88fb1245";

        [TestMethod]
        public void GetActiveSystemSources_Test()
        {
            GetActiveSystemSourcesRequest request = new GetActiveSystemSourcesRequest
            {
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                Token = token
            };

            JsonServiceClient.HttpWebRequestFilter = x =>
                x.Headers.Add(string.Format("{0}: {1}", "Token", token));

            //[Route("/{Version}/{ContractNumber}/SystemSource", "GET")]
            GetActiveSystemSourcesResponse response = client.Get<GetActiveSystemSourcesResponse>(string.Format("{0}/{1}/{2}/SystemSource", url, version, contractNumber));

            Assert.IsNotNull(response);
        }
    }
}
