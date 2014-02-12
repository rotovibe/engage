using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientGoal;
using Phytel.API.DataDomain.PatientGoal.DTO;

namespace Phytel.API.DataDomain.PatientGoal.Test
{
    [TestClass]
    public class PatientGoalTest
    {
        [TestMethod]
        public void GetPatientGoalByID()
        {
            GetPatientGoalDataRequest request = new GetPatientGoalDataRequest { PatientGoalId = "5" };

            GetPatientGoalDataResponse response = PatientGoalDataManager.GetPatientGoalByID(request);

            Assert.IsTrue(response.PatientGoal.Name == "Tony");
        }
    }
}
