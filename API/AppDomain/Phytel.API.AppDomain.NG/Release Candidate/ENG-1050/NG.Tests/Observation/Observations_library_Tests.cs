using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.Service.Test
{
    [TestClass]
    public class Observations_library_Tests
    {
        [TestMethod]
        public void Get_Additional_Observation_Library_Test()
        {
            string contractNumber = "InHealth001";
            double version = 1.0;
            string token = "53458596d6a48504b492f1bf";
            string typeId = "53067453fe7a591a348e1b66";
            bool? standard = false;

            JsonServiceClient.HttpWebRequestFilter = x => x.Headers.Add(string.Format("Token: {0}", token));

            IRestClient client = new JsonServiceClient();
            //[Route("/{Version}/{ContractNumber}/Observation/Type/{TypeId}/MatchLibrary/{Standard}", "GET")]
            GetObservationsResponse response = client.Get<GetObservationsResponse>(
                string.Format(@"http://localhost:888/Nightingale/{0}/{1}/Observation/Type/{2}/MatchLibrary/{3}",
                version,
                contractNumber,
                typeId,
                standard));
        }
    }
}
