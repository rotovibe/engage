using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Cohort.DTO;

namespace Phytel.API.DataDomain.Cohort.Test
{
    [TestClass]
    public class CohortTest
    {
        [TestMethod]
        public void GetCohortByID()
        {
            ICohortRepository<GetCohortDataResponse> repo = CohortRepositoryFactory<GetCohortDataResponse>.GetCohortRepository("InHealth001", "NG", "");

            repo.Select(new Interface.APIExpression());
        }

        [TestMethod]
        public void GetCohortByID_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            GetCohortDataRequest request = new GetCohortDataRequest { CohortID = "528aa055d4332317acc50978", Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            GetCohortDataResponse response = DataCohortManager.GetCohortByID(request);

            // Assert
            Assert.IsTrue(response.Cohort.SName == "All(f)");

        }

        [TestMethod]
        public void GetAllCohorts_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            GetAllCohortsDataRequest request = new GetAllCohortsDataRequest { Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            GetAllCohortsDataResponse response = DataCohortManager.GetCohorts(request);

            // Assert
            Assert.AreNotEqual(0, response.Cohorts.Count);
        }
    }
}
