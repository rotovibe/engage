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
    [TestClass]
    public class StubAllergy_Test
    {
        string context = "NG";
        string contractNumber = "InHealth001";
        string userId = "000000000000000000000000";
        double version = 1.0;
        IAllergyDataManager cm = new StubAllergyDataManager();

        [TestMethod]
        public void GetAllergies_Test()
        {
            GetAllAllergysRequest request = new GetAllAllergysRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
            };
            List<AllergyData> response = cm.GetAllergyList(request);
            Assert.IsTrue(response.Count == 2);
        } 
        
        
        [TestMethod]
        public void InitializeAllergy_Test()
        {
            PutInitializeAllergyDataRequest request = new PutInitializeAllergyDataRequest
            {
                Context = context,
                ContractNumber = contractNumber,
                UserId = userId,
                Version = version,
                AllergyName = "allergyName"
            };
            
            AllergyData response = cm.InitializeAllergy(request);
            Assert.IsTrue(request.AllergyName == response.Name);
        }


        [TestMethod]
        public void UpdateAllergy_Test()
        {

            AllergyData data = new AllergyData
            {
                DeleteFlag = false,
                Id = "5453cea0d433232a387d51b9",
                Name = "allergyName",
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

            AllergyData aData = cm.UpdateAllergy(request);
            Assert.IsTrue(aData.Name == data.Name);
        }
    }
}
