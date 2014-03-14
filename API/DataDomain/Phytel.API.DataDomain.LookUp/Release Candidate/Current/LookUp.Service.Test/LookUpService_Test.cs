using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.LookUp.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.DataDomain.LookUp.Services.Test
{
    [TestClass]
    public class LookUpService_Test
    {
        #region Problem
        [TestMethod]
        public void GetAllProblem_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            // Act
            GetAllProblemsDataResponse response = client.Get<GetAllProblemsDataResponse>
                (string.Format("{0}/{1}/{2}/{3}/problems",
                  "http://localhost:8888/LookUp/", context, version, contractNumber));

            // Assert
            Assert.AreNotEqual(0, response.Problems.Count);
        }

        [TestMethod]
        public void GetProblemByID_Test()
        {
            // Arrange
            string expectedValue = "Arthritis";
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            string problemID = "528a66fdd4332317acc50960";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            // Act
            GetProblemDataResponse response = client.Get<GetProblemDataResponse>
                (string.Format("{0}/{1}/{2}/{3}/problem/{4}",
                  "http://localhost:8888/LookUp/", context, version, contractNumber, problemID));

            string actualValue = response.Problem.Name;
            // Assert
            Assert.AreEqual(expectedValue, actualValue, true);
        }

        [TestMethod]
        public void SearchProblem_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";

            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            // Act
            GetAllProblemsDataResponse response = client.Post<GetAllProblemsDataResponse>
                (string.Format("{0}/{1}/{2}/{3}/problems",
                  "http://localhost:8888/LookUp/", context, version, contractNumber),
                      new SearchProblemsDataRequest
                      {
                          Active = true,
                          Version = version,
                          ContractNumber = contractNumber,
                          Context = context
                      }
                  );

            // Assert
            Assert.AreNotEqual(0, response.Problems.Count);
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
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            // Act
            GetAllCommModesDataResponse response = client.Get<GetAllCommModesDataResponse>
                (string.Format("{0}/{1}/{2}/{3}/CommModes",
                  "http://localhost:8888/LookUp/", context, version, contractNumber));

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
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            // Act
            GetAllStatesDataResponse response = client.Get<GetAllStatesDataResponse>
                (string.Format("{0}/{1}/{2}/{3}/States",
                  "http://localhost:8888/LookUp/", context, version, contractNumber));

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
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            // Act
            GetAllTimesOfDaysDataResponse response = client.Get<GetAllTimesOfDaysDataResponse>
                (string.Format("{0}/{1}/{2}/{3}/TimesOfDays",
                  "http://localhost:8888/LookUp/", context, version, contractNumber));

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
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            // Act
            GetAllTimeZonesDataResponse response = client.Get<GetAllTimeZonesDataResponse>
                (string.Format("{0}/{1}/{2}/{3}/TimeZones",
                  "http://localhost:8888/LookUp/", context, version, contractNumber));

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
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            // Act
            GetAllCommTypesDataResponse response = client.Get<GetAllCommTypesDataResponse>
                (string.Format("{0}/{1}/{2}/{3}/CommTypes",
                  "http://localhost:8888/LookUp/", context, version, contractNumber));

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
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            // Act
            GetAllLanguagesDataResponse response = client.Get<GetAllLanguagesDataResponse>
                (string.Format("{0}/{1}/{2}/{3}/Languages",
                  "http://localhost:8888/LookUp/", context, version, contractNumber));

            // Assert
            Assert.AreNotEqual(0, response.Languages.Count);
        } 
        #endregion



        [TestMethod]
        public void GetLookUpByType_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();
            JsonServiceClient.HttpWebRequestFilter = x =>
                            x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "531f2df9072ef727c4d2a3df"));

            // Act
            GetLookUpsDataResponse response = client.Get<GetLookUpsDataResponse>
                (string.Format("{0}/{1}/{2}/{3}/{4}",
                  "http://localhost:8888/LookUp/", context, version, contractNumber, "FocusArea"));

            // Assert
            Assert.AreNotEqual(0, response.LookUpsData.Count);
        }
    }
}
