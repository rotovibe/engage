using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Test
{
    [TestClass]
    public class DesignerService_Test
    {
        string context = "engage";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        string url = "http://localhost:888/Nightingale";
        IRestClient client = new JsonServiceClient();
        string token = "558866803fe31e13542e28da";

        [TestMethod]
        public void Get_Test()
        {
            GetTestRequest request = new GetTestRequest
            {
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version
            };

            JsonServiceClient.HttpWebRequestFilter = x =>
                x.Headers.Add(string.Format("{0}: {1}", "token", token));

            //[Route("/{Version}/{ContractNumber}/Test", "GET")]
            GetTestResponse response = client.Get<GetTestResponse>(string.Format("{0}/{1}/{2}/Test/{3}", url, version, contractNumber));

            Assert.IsNotNull(response);
        }
    }
}