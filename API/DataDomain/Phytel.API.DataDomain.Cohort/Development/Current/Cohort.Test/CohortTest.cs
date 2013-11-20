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
            ICohortRepository<GetCohortResponse> repo = CohortRepositoryFactory<GetCohortResponse>.GetCohortRepository("InHealth001", "NG");

            repo.Select(new Interface.APIExpression());
        }

        [TestMethod]
        public void GetCohortByID_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            GetCohortRequest request = new GetCohortRequest { CohortID = "528aa03ad4332317acc50976", Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            GetCohortResponse response = DataCohortManager.GetCohortByID(request);

            // Assert
            Assert.IsTrue(response.Cohort.SName == "All(f)");

        }

        [TestMethod]
        public void GetAllCohorts_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string context = "NG";
            GetAllCohortsRequest request = new GetAllCohortsRequest { Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            GetAllCohortsResponse response = DataCohortManager.GetCohorts(request);

            // Assert
            Assert.AreNotEqual(0, response.Cohorts.Count);
        }
    }
}
