using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Test
{
    [TestClass]
    public class Data_MedicationMap_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        string url = "http://localhost:888/Nightingale";
        IRestClient client = new JsonServiceClient();
        string token = "55d4a5c484ac07221847231c";

        [TestMethod]
        public void InitializeMedicationMap_Test()
        {
            PostInitializeMedicationMapRequest request = new PostInitializeMedicationMapRequest
            {
                
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                MedicationMap = new MedicationMap { FullName = "ibup_sneh1" }
            };

            JsonServiceClient.HttpWebRequestFilter = x =>
                x.Headers.Add(string.Format("{0}: {1}", "Token", token));

            // [Route("/{Version}/{ContractNumber}/MedicationMap/Initialize", "POST")]
            PostInitializeMedicationMapResponse response = client.Post<PostInitializeMedicationMapResponse>(string.Format("{0}/{1}/{2}/MedicationMap/Initialize", url, version, contractNumber), request);

            Assert.IsNotNull(response);
        }


        [TestMethod]
        public void DeleteMedicationMap_Test()
        {
            JsonServiceClient.HttpWebRequestFilter = x =>
                x.Headers.Add(string.Format("{0}: {1}", "Token", token));

            //[Route("/{Version}/{ContractNumber}/MedicationMap/{Ids}", "DELETE")]
            DeleteMedicationMapsResponse response = client.Delete<DeleteMedicationMapsResponse>(string.Format("{0}/{1}/{2}/MedicationMap/{3}", url, version, contractNumber,"  55dc968984ac0728842f118b , 55dc96e484ac0728842f1386 "));

            Assert.IsNotNull(response);
        }
    }
}
