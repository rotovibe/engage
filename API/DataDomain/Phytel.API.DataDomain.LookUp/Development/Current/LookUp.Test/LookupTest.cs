using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.LookUp.DTO;

namespace Phytel.API.DataDomain.LookUp.Test
{
    [TestClass]
    public class LookUpTest
    {
        [TestMethod]
        public void GetConditionByID_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            GetConditionRequest request = new GetConditionRequest { ConditionID = "527c1b1ad4332324ac199142", Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            ConditionResponse response = LookUpDataManager.GetConditionByID(request);

            // Assert
            Assert.IsTrue(response.Condition.Name == "Arthritis");
            
        }

        [TestMethod]
        public void FindConditions_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            FindConditionsRequest request = new FindConditionsRequest {  Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            ConditionsResponse response = LookUpDataManager.FindConditions(request);

            // Assert
            Assert.AreNotEqual(0, response.Conditions.Count);
        }

    }
}
