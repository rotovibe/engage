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
            CohortRequest request = new CohortRequest{ CohortID = "5"};

            CohortResponse response = CohortDataManager.GetCohortByID(request);

            Assert.IsTrue(response.CohortID == "Tony");
        }
    }
}
