using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientGoal.DTO;

namespace Phytel.API.DataDomain.PatientGoal.Test
{
    [TestClass]
    public class PatientGoalTest
    {
        [TestMethod]
        public void GetPatientGoalByID()
        {
            GetPatientGoalRequest request = new GetPatientGoalRequest{ PatientGoalID = "5"};

            GetPatientGoalResponse response = PatientGoalDataManager.GetPatientGoalByID(request);

            Assert.IsTrue(response.PatientGoal.PatientGoalID == "Tony");
        }
    }
}
