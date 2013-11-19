using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.LookUp.DTO;

namespace Phytel.API.DataDomain.LookUp.Test
{
    [TestClass]
    public class LookUpTest
    {
        [TestMethod]
        public void GetProblemByID_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            GetProblemRequest request = new GetProblemRequest { ProblemID = "527c1b1ad4332324ac199142", Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            GetProblemResponse response = LookUpDataManager.GetProblemByID(request);

            // Assert
            Assert.IsTrue(response.Problem.Name == "Arthritis");
            
        }

        [TestMethod]
        public void FindProblems_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            GetAllProblemRequest request = new GetAllProblemRequest {  Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            GetAllProblemResponse response = LookUpDataManager.FindProblems(request);

            // Assert
            Assert.AreNotEqual(0, response.Problems.Count);
        }

    }
}
