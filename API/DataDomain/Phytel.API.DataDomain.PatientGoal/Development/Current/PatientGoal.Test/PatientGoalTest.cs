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
            GetPatientGoalDataRequest request = new GetPatientGoalDataRequest { Id = "53011e8ed4332316c093952a" };

            GetPatientGoalDataResponse response = PatientGoalDataManager.GetPatientGoal(request);

            Assert.IsNotNull(response.GoalData);
        }

        [TestMethod]
        public void GetAllPatientGoals()
        {
            GetAllPatientGoalsDataRequest request = new GetAllPatientGoalsDataRequest { PatientId = "52f55874072ef709f84e68c5" };

            GetAllPatientGoalsDataResponse response = PatientGoalDataManager.GetPatientGoalList(request);

            Assert.IsNotNull(response.PatientGoalsData);
        }

        [TestMethod]
        public void InitializePatientGoal()
        {
            PutInitializeGoalDataRequest request = new PutInitializeGoalDataRequest { PatientId = "52f55874072ef709f84e68c5" };

            PutInitializeGoalDataResponse response = PatientGoalDataManager.InitializeGoal(request);

            Assert.IsNotNull(response.Id);
        }


        [TestMethod]
        public void InitializePatientBarrier()
        {
            PutInitializeBarrierDataRequest request = new PutInitializeBarrierDataRequest { PatientGoalId = "52fc609fd43323258c5c8c71" };

            PutInitializeBarrierDataResponse response = PatientGoalDataManager.InitializeBarrier(request);

            Assert.IsNotNull(response.Id);
        }
    }
}
