using System.Collections.Generic;
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
            GetPatientGoalDataRequest request = new GetPatientGoalDataRequest { Id = "5304d677d6a4850f14e4cc93" };

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

            Assert.IsNotNull(response.Goal);
        }


        [TestMethod]
        public void InitializePatientBarrier()
        {
            PutInitializeBarrierDataRequest request = new PutInitializeBarrierDataRequest { PatientGoalId = "52fc609fd43323258c5c8c71" };

            PutInitializeBarrierDataResponse response = PatientGoalDataManager.InitializeBarrier(request);

            Assert.IsNotNull(response.Id);
        }

        [TestMethod]
        public void GeCustomAttributesByType()
        {
            GetCustomAttributesDataRequest request = new GetCustomAttributesDataRequest { TypeId = 2 };

            GetCustomAttributesDataResponse response = PatientGoalDataManager.GetCustomAttributesByType(request);

            Assert.IsNotNull(response.CustomAttributes);
        }

        [TestMethod]
        public void UpdateBarriers_Test()
        {
            List<string> barrierIds = new List<string>();
            barrierIds.Add("53050058d6a4850f149fb508");
            PutUpdateBarrierRequest req = new PutUpdateBarrierRequest
            {
                Context = "NG",
                ContractNumber = "InHealth001",
                PatientGoalId = "5304fffcd6a4850f149fb4fb",
                Barrier = new PatientBarrierData {  Id = "53050042d6a4850f149fb504", Name = "name changed", StatusId = 2},
                BarrierIdsList = barrierIds
            };

            PutUpdateBarrierResponse response = PatientGoalDataManager.UpdatePatientBarrier(req);
            Assert.IsTrue(response.Updated);
        }
    }
}
