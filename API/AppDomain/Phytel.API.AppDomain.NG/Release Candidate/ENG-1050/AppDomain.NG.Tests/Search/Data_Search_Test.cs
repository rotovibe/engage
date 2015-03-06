using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.Allergy;
using Phytel.API.AppDomain.NG.DTO.Search;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Test
{
    //[TestClass]
    public class Data_Search_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        string url = "http://localhost:888/Nightingale";
        IRestClient client = new JsonServiceClient();
        string token = "54b81d0b84ac050580839a18";

        [TestMethod]
        public void GetPatientMedSupps_Test()
        {
            GetMedFieldsRequest request = new GetMedFieldsRequest
            {
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version
            };

            JsonServiceClient.HttpWebRequestFilter = x =>
                x.Headers.Add(string.Format("{0}: {1}", "Token", token));

            //[Route("/{Version}/{ContractNumber}/Search/Meds/Fields", "GET")]
            GetMedFieldsResponse response = client.Get<GetMedFieldsResponse>(string.Format("{0}/{1}/{2}/Search/Meds/Fields?Name={3}", url, version, contractNumber, "JAIPUR"));

            Assert.IsNotNull(response);
        }
    }
}