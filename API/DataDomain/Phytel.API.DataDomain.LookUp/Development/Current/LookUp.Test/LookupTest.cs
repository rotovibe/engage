using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.LookUp.DTO;

namespace Phytel.API.DataDomain.LookUp.Test
{
    [TestClass]
    public class LookUpTest
    {
        #region Problem
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
            GetAllProblemsDataRequest request = new GetAllProblemsDataRequest { Context = context, ContractNumber = contractNumber, Version = version };

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
        #endregion

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
            Assert.IsTrue(response.Category.Name == "Cost");

        } 
        #endregion

        #region Contact Related LookUps
        [TestMethod]
        public void GetAllCommModes_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            GetAllCommModesDataRequest request = new GetAllCommModesDataRequest { Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            GetAllCommModesDataResponse response = LookUpDataManager.GetAllCommModes(request);

            // Assert
            Assert.AreNotEqual(0, response.CommModes.Count);
        }

        [TestMethod]
        public void GetAllStates_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            GetAllStatesDataRequest request = new GetAllStatesDataRequest { Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            GetAllStatesDataResponse response = LookUpDataManager.GetAllStates(request);

            // Assert
            Assert.AreNotEqual(0, response.States.Count);
        }

        [TestMethod]
        public void GetAllTimesOfDays_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            GetAllTimesOfDaysDataRequest request = new GetAllTimesOfDaysDataRequest { Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            GetAllTimesOfDaysDataResponse response = LookUpDataManager.GetAllTimesOfDays(request);

            // Assert
            Assert.AreNotEqual(0, response.TimesOfDays.Count);
        }

        [TestMethod]
        public void GetAllTimeZones_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            GetAllTimeZonesDataRequest request = new GetAllTimeZonesDataRequest { Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            GetAllTimeZonesDataResponse response = LookUpDataManager.GetAllTimeZones(request);

            // Assert
            Assert.AreNotEqual(0, response.TimeZones.Count);
        }

        [TestMethod]
        public void GetAllCommTypes_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            GetAllCommTypesDataRequest request = new GetAllCommTypesDataRequest { Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            GetAllCommTypesDataResponse response = LookUpDataManager.GetAllCommTypes(request);

            // Assert
            Assert.AreNotEqual(0, response.CommTypes.Count);
        }

        [TestMethod]
        public void GetAllLanguages_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            GetAllLanguagesDataRequest request = new GetAllLanguagesDataRequest { Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            GetAllLanguagesDataResponse response = LookUpDataManager.GetAllLanguages(request);

            // Assert
            Assert.AreNotEqual(0, response.Languages.Count);
        }

        [TestMethod]
        public void GetDefaultTimeZone_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            GetTimeZoneDataRequest request = new GetTimeZoneDataRequest { Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            GetTimeZoneDataResponse response = LookUpDataManager.GetDefaultTimeZone(request);

            // Assert
            Assert.AreEqual("Central (UTC-06:00)", response.TimeZone.Name);
        }
        #endregion

        [TestMethod]
        public void GetLookUpsByType_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            GetLookUpsDataRequest request = new GetLookUpsDataRequest { Context = context, ContractNumber = contractNumber, Version = version, Type = "BarrierCategory" };

            // Act
            GetLookUpsDataResponse response = LookUpDataManager.GetLookUpsByType(request);

            // Assert
            Assert.IsTrue(response.LookUpsData.Count > 0);
        }

    }
}
