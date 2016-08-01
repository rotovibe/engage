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
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            GetProblemDataRequest request = new GetProblemDataRequest { ProblemID = "528a6709d4332317acc50962", Context = context, ContractNumber = contractNumber, Version = version };

            LookUpDataManager lm = new LookUpDataManager { Factory = new LookUpRepositoryFactory() };
            // Act
            GetProblemDataResponse response = lm.GetProblem(request);

            // Assert
            Assert.IsTrue(response.Problem.Name == "CKD");

        }

        [TestMethod]
        public void GetProblems_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            GetAllProblemsDataRequest request = new GetAllProblemsDataRequest { Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            LookUpDataManager lm = new LookUpDataManager { Factory = new LookUpRepositoryFactory() };
            GetAllProblemsDataResponse response = lm.GetAllProblems(request);

            // Assert
            Assert.AreNotEqual(0, response.Problems.Count);
        }

        [TestMethod]
        public void SearchProblem_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            SearchProblemsDataRequest request = new SearchProblemsDataRequest { Active = true, Type = "Chronic", Context = context, ContractNumber = contractNumber, Version = version };
            // Act
            LookUpDataManager lm = new LookUpDataManager { Factory = new LookUpRepositoryFactory() };
            SearchProblemsDataResponse response = lm.SearchProblem(request);

            // Assert
            Assert.AreNotEqual(0, response.Problems.Count);
        } 
        #endregion

        #region Objective
        [TestMethod]
        public void GetObjectiveByID_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            GetObjectiveDataRequest request = new GetObjectiveDataRequest { ObjectiveID = "52a0beb9d43323141c9eb26c", Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            LookUpDataManager lm = new LookUpDataManager { Factory = new LookUpRepositoryFactory() };
            GetObjectiveDataResponse response = lm.GetObjectiveByID(request);

            // Assert
            Assert.IsTrue(response.Objective.Name == "Reduce Risk Factors");

        }

        #endregion

        #region Category
        [TestMethod]
        public void GetCategoryByID_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            GetCategoryDataRequest request = new GetCategoryDataRequest { CategoryID = "52a0bc5fd4332322b4aed5b7", Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            LookUpDataManager lm = new LookUpDataManager { Factory = new LookUpRepositoryFactory() };
            GetCategoryDataResponse response = lm.GetCategoryByID(request);

            // Assert
            Assert.IsTrue(response.Category.Name == "Process");

        } 
        #endregion

        #region Contact Related LookUps
        [TestMethod]
        public void GetAllCommModes_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            GetAllCommModesDataRequest request = new GetAllCommModesDataRequest { Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            LookUpDataManager lm = new LookUpDataManager { Factory = new LookUpRepositoryFactory() };
            GetAllCommModesDataResponse response = lm.GetAllCommModes(request);

            // Assert
            Assert.AreNotEqual(0, response.CommModes.Count);
        }

        [TestMethod]
        public void GetAllStates_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            GetAllStatesDataRequest request = new GetAllStatesDataRequest { Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            LookUpDataManager lm = new LookUpDataManager { Factory = new LookUpRepositoryFactory() };
            GetAllStatesDataResponse response = lm.GetAllStates(request);

            // Assert
            Assert.AreNotEqual(0, response.States.Count);
        }

        [TestMethod]
        public void GetAllTimesOfDays_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            GetAllTimesOfDaysDataRequest request = new GetAllTimesOfDaysDataRequest { Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            LookUpDataManager lm = new LookUpDataManager { Factory = new LookUpRepositoryFactory() };
            GetAllTimesOfDaysDataResponse response = lm.GetAllTimesOfDays(request);

            // Assert
            Assert.AreNotEqual(0, response.TimesOfDays.Count);
        }

        [TestMethod]
        public void GetAllTimeZones_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            GetAllTimeZonesDataRequest request = new GetAllTimeZonesDataRequest { Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            LookUpDataManager lm = new LookUpDataManager { Factory = new LookUpRepositoryFactory() };
            GetAllTimeZonesDataResponse response = lm.GetAllTimeZones(request);

            // Assert
            Assert.AreNotEqual(0, response.TimeZones.Count);
        }

        [TestMethod]
        public void GetAllCommTypes_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            GetAllCommTypesDataRequest request = new GetAllCommTypesDataRequest { Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            LookUpDataManager lm = new LookUpDataManager { Factory = new LookUpRepositoryFactory() };
            GetAllCommTypesDataResponse response = lm.GetAllCommTypes(request);

            // Assert
            Assert.AreNotEqual(0, response.CommTypes.Count);
        }

        [TestMethod]
        public void GetAllLanguages_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            GetAllLanguagesDataRequest request = new GetAllLanguagesDataRequest { Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            LookUpDataManager lm = new LookUpDataManager { Factory = new LookUpRepositoryFactory() };
            GetAllLanguagesDataResponse response = lm.GetAllLanguages(request);

            // Assert
            Assert.AreNotEqual(0, response.Languages.Count);
        }

        [TestMethod]
        public void GetDefaultTimeZone_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            GetTimeZoneDataRequest request = new GetTimeZoneDataRequest { Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            LookUpDataManager lm = new LookUpDataManager { Factory = new LookUpRepositoryFactory() };
            GetTimeZoneDataResponse response = lm.GetDefaultTimeZone(request);

            // Assert
            Assert.AreEqual("Central", response.TimeZone.Name);
        }
        #endregion

        [TestMethod]
        public void GetLookUpsByType_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            GetLookUpsDataRequest request = new GetLookUpsDataRequest { Context = context, ContractNumber = contractNumber, Version = version, Name = "reaction" };

            // Act
            LookUpDataManager lm = new LookUpDataManager { Factory = new LookUpRepositoryFactory() };
            GetLookUpsDataResponse response = lm.GetLookUpsByType(request);

            // Assert
            Assert.IsTrue(response.LookUpsData.Count > 0);
        }

        [TestMethod]
        public void GetLookUpDetails_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            GetLookUpDetailsDataRequest request = new GetLookUpDetailsDataRequest { Context = context, ContractNumber = contractNumber, Version = version, Name = "NoteWho" };

            // Act
            LookUpDataManager lm = new LookUpDataManager { Factory = new LookUpRepositoryFactory() };
            GetLookUpDetailsDataResponse response = lm.GetLookUpDetails(request);

            // Assert
            Assert.IsTrue(response.LookUpDetailsData.Count > 0);
        }

        [TestMethod]
        public void GetAllObjectives_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            GetAllObjectivesDataRequest request = new GetAllObjectivesDataRequest { Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            LookUpDataManager lm = new LookUpDataManager { Factory = new LookUpRepositoryFactory() };
            GetAllObjectivesDataResponse response = lm.GetAllObjectives(request);

            // Assert
            Assert.AreNotEqual(0, response.ObjectivesData.Count);
        }

        [TestMethod]
        public void GetAllSettings_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            GetAllSettingsDataRequest request = new GetAllSettingsDataRequest { Context = context, ContractNumber = contractNumber, Version = version};

            // Act
            LookUpDataManager lm = new LookUpDataManager { Factory = new LookUpRepositoryFactory() };
            GetAllSettingsDataResponse response = lm.GetAllSettings(request);

            // Assert
            Assert.IsTrue(response.SettingsData.Count > 0);
        }

    }
}
