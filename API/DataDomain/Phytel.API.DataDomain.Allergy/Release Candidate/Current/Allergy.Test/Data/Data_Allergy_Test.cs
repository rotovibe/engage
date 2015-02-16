using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Allergy.DTO;
using Phytel.API.DataDomain.Allergy.Test;
using DataDomain.Allergy.Repo;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.Allergy.Test
{
    //[TestClass]
    public class Data_Allergy_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        string url = "http://localhost:8888/Allergy";
        IRestClient client = new JsonServiceClient();

        [TestMethod]
        public void InitializeAllergy_Test()
        {
            PutInitializeAllergyDataRequest request = new PutInitializeAllergyDataRequest {
                AllergyName = "testing name",
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version
            };
            //[Route("/{Context}/{Version}/{ContractNumber}/Allergy/Initialize", "PUT")]
            PutInitializeAllergyDataResponse response = client.Put<PutInitializeAllergyDataResponse>(
    string.Format("{0}/{1}/{2}/{3}/Allergy/Initialize", url, context, version, contractNumber), request);

            Assert.IsNotNull(response);
        }


        [TestMethod]
        public void UpdateAllergy_Test()
        {

            AllergyData data = new AllergyData
            {
                DeleteFlag = false,
                Id = "5453cea0d433232a387d51b9",
                Name = "testing",
                TypeIds = new List<string> { "5447d6ddfe7a59146485b512", "5446db5efe7a591e74013b6b", "5446db5efe7a591e74013b6c" },
                Version = 1.0
            };

            PutAllergyDataRequest request = new PutAllergyDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                AllergyData = data,
                UserId = userId,
                Version = version
            };

            //[Route("/{Context}/{Version}/{ContractNumber}/Allergy/Update", "PUT")]
            PutAllergyDataResponse response = client.Put<PutAllergyDataResponse>(
                string.Format("{0}/{1}/{2}/{3}/Allergy/Update", url, context, version, contractNumber), request);
            Assert.IsNotNull(response);
        }
    }
}
