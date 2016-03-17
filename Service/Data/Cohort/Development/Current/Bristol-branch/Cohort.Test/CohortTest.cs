using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Cohort.DTO;
using Moq;
using Phytel.API.DataDomain.Cohort;

namespace Phytel.API.DataDomain.Cohort.Test
{
    [TestClass]

    public class CohortTest
    {
        private Mock<ICohortRepository<CohortData>> _mongoCohortRepository;

    [TestMethod]
        public void GetCohortByID()
        {
          //  ICohortRepository<GetCohortDataResponse> repo = CohortRepositoryFactory<GetCohortDataResponse>.GetCohortRepository("InHealth001", "NG", "nguser");

          //  repo.Select(new Interface.APIExpression());
        }

        [TestMethod]
        public void GetCohortByID_Test()
        {
            // Arrange
            CohortData cohortData = new CohortData() { ID = "AXY", Query = "GetByCohortId", QueryWithFilter = "db.InHealh.Find{cohortId: 234} ", SName = "SName", Sort = "ASC" };
            _mongoCohortRepository = new Mock<ICohortRepository<CohortData>>(MockBehavior.Default);
            _mongoCohortRepository.Setup(m => m.FindByID(It.IsAny<string>())).Returns(cohortData);
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            GetCohortDataRequest request = new GetCohortDataRequest {
                                                                                                CohortID = "530f9d9f072ef715f4b411d0",
                                                                                                Context = context, ContractNumber = contractNumber,
                                                                                                UserId = "nguser", Version = version
                                                                                            };

            // Act
            GetCohortDataResponse response = DataCohortManager.GetCohortByID(request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Cohort);
            Assert.IsNotNull(response.Cohort.ID);
            Assert.AreEqual(response.Cohort.ID, request.CohortID);
            Assert.AreEqual("Assigned to me", response.Cohort.SName);

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
