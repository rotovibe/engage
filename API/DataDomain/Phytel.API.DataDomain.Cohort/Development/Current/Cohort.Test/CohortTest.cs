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
            ICohortRepository<CohortResponse> repo = CohortRepositoryFactory<CohortResponse>.GetCohortRepository("InHealth001", "NG");

            repo.Select(new Interface.APIExpression());
        }
    }
}
