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
            GetProblemDataRequest request = new GetProblemDataRequest { ProblemID = "527c1b1ad4332324ac199142", Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            GetProblemDataResponse response = LookUpDataManager.GetProblem(request);

            // Assert
            Assert.IsTrue(response.Problem.Name == "Arthritis");
            
        }

        [TestMethod]
        public void GetProblems_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            GetAllProblemsDataRequest request = new GetAllProblemsDataRequest {  Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            GetAllProblemsDataResponse response = LookUpDataManager.GetAllProblem(request);

            // Assert
            Assert.AreNotEqual(0, response.Problems.Count);
        }

        [TestMethod]
        public void SeachProblem_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            SearchProblemsDataRequest request = new SearchProblemsDataRequest { Active = true, Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            SearchProblemsDataResponse response = LookUpDataManager.SearchProblem(request);

            // Assert
            Assert.AreNotEqual(0, response.Problems.Count);
        }

    }
}
