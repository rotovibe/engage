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
            GetProblemDataRequest request = new GetProblemDataRequest { ProblemID = "528a66d6d4332317acc5095d", Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            GetProblemDataResponse response = LookUpDataManager.GetProblem(request);

            // Assert
            Assert.IsTrue(response.Problem.Name == "High Cholesterol");
            
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
            GetAllProblemsDataResponse response = LookUpDataManager.GetAllProblems(request);

            // Assert
            Assert.AreNotEqual(0, response.Problems.Count);
        }

        [TestMethod]
        public void SearchProblem_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            SearchProblemsDataRequest request = new SearchProblemsDataRequest { Active = true, Type = "Chronic", Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            SearchProblemsDataResponse response = LookUpDataManager.SearchProblem(request);

            // Assert
            Assert.AreNotEqual(0, response.Problems.Count);
        }


        #region Objective
        [TestMethod]
        public void GetObjectiveByID_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            GetObjectiveDataRequest request = new GetObjectiveDataRequest { ObjectiveID = "52a0beb9d43323141c9eb26c", Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            GetObjectiveDataResponse response = LookUpDataManager.GetObjectiveByID(request);

            // Assert
            Assert.IsTrue(response.Objective.Name == "Reduce Risk Factors");

        }

        #endregion

        #region Category
        [TestMethod]
        public void GetCategoryByID_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            GetCategoryDataRequest request = new GetCategoryDataRequest { CategoryID = "52a0bc3fd4332322b4aed5b5", Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            GetCategoryDataResponse response = LookUpDataManager.GetCategoryByID(request);

            // Assert
            Assert.IsTrue(response.Category.Text == "Cost");

        } 
        #endregion

    }
}
