using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientSystem.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.PatientSystem.Test
{
    //[TestClass]
    public class System_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        string url = "http://localhost:8888/PatientSystem";
        IRestClient client = new JsonServiceClient();

        [TestMethod]
        public void GetSystemSources_Test()
        {
            GetSystemsDataRequest request = new GetSystemsDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version
            };
            //[Route("/{Context}/{Version}/{ContractNumber}/System", "GET")]
            GetSystemsDataResponse response = client.Get<GetSystemsDataResponse>(string.Format("{0}/{1}/{2}/{3}/System?UserId={4}", url, context, version, contractNumber, request.UserId));
            Assert.IsNotNull(response);
        }
    }
}
