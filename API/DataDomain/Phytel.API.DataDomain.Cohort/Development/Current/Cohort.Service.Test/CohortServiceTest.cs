using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Cohort.DTO;
using ServiceStack;

namespace Phytel.API.DataDomain.Cohort.Service.Test
{
    [TestClass]
    public class CohortServiceTest
    {
        [TestMethod]
        public void GetAllCohorts_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            IRestClient client = new JsonServiceClient();

            // Act
            GetAllCohortsDataResponse response = client.Get <GetAllCohortsDataResponse>
                (string.Format("{0}/{1}/{2}/{3}/cohorts",
                  "http://localhost:8888/Cohort/", context, version, contractNumber));

            // Assert
            Assert.AreNotEqual(0, response.Cohorts.Count);
        }

        [TestMethod]
        public void GetCohortByID_Test()
        {
            // Arrange
            string expectedValue = "All(f)";
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            string cohortID = "528aa055d4332317acc50978";
            IRestClient client = new JsonServiceClient();

            // Act
            GetCohortDataResponse response = client.Get<GetCohortDataResponse>
                (string.Format("{0}/{1}/{2}/{3}/cohort/{4}",
                  "http://localhost:8888/Cohort/", context, version, contractNumber, cohortID));

            string actualValue = response.Cohort.SName;
            // Assert
            Assert.AreEqual(expectedValue, actualValue, true);
        }
    }
}
