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
        string token = "54b81d0b84ac050580839a18";

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
        public void DeleteCustomMedications_Test()
        {
            List<MedicationMap> medList = new List<MedicationMap>();
            medList.Add(new MedicationMap { FullName = "BLUEFISH", Route = "", Strength = "56 ml", Form = "" });
            medList.Add(new MedicationMap { FullName = "CATFISH", Route = "", Strength = "", Form = "TABLET" });
            
            PutDeleteMedMapRequest request = new PutDeleteMedMapRequest
            {
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                MedicationMaps = medList
            };

            JsonServiceClient.HttpWebRequestFilter = x =>
                x.Headers.Add(string.Format("{0}: {1}", "Token", token));

            //[Route("/{Version}/{ContractNumber}/MedicationMap/Delete", "PUT")]
            PutDeleteMedMapResponse response = client.Put<PutDeleteMedMapResponse>(string.Format("{0}/{1}/{2}/MedicationMap/Delete", url, version, contractNumber), request);

            Assert.IsNotNull(response);
        }
    }
}
