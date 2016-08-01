using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientGoal;
using Phytel.API.DataDomain.PatientGoal.DTO;

namespace Phytel.API.DataDomain.PatientGoal.Test
{
    [TestClass]
    public class PatientGoalTest
    {
        IPatientGoalDataManager m = new PatientGoalDataManager { Factory = new PatientGoalRepositoryFactory() };
        
        [TestMethod]
        public void GetPatientGoalByID()
        {
            GetPatientGoalDataRequest request = new GetPatientGoalDataRequest { Id = "5304d677d6a4850f14e4cc93" };

            GetPatientGoalDataResponse response = m.GetPatientGoal(request);

            Assert.IsNotNull(response.GoalData);
        }

        [TestMethod]
        public void GetAllPatientGoals()
        {
            GetAllPatientGoalsDataRequest request = new GetAllPatientGoalsDataRequest { PatientId = "52f55860072ef709f84e60f7" };

            GetAllPatientGoalsDataResponse response = m.GetPatientGoalList(request);

            Assert.IsNotNull(response.PatientGoalsData);
        }

        [TestMethod]
        public void InitializePatientGoal()
        {
            PutInitializeGoalDataRequest request = new PutInitializeGoalDataRequest { PatientId = "531f2dcd072ef727c4d29fb0" };

            PutInitializeGoalDataResponse response = m.InitializeGoal(request);

            Assert.IsNotNull(response.Goal);
        }


        [TestMethod]
        public void InitializePatientBarrier()
        {
            PutInitializeBarrierDataRequest request = new PutInitializeBarrierDataRequest { PatientGoalId = "52fc609fd43323258c5c8c71" };

            PutInitializeBarrierDataResponse response = m.InitializeBarrier(request);

            Assert.IsNotNull(response.Id);
        }

        [TestMethod]
        public void GeCustomAttributesByType()
        {
            GetCustomAttributesDataRequest request = new GetCustomAttributesDataRequest { TypeId = 2 };

            GetCustomAttributesDataResponse response = m.GetCustomAttributesByType(request);

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
                Barrier = new PatientBarrierData {  Id = "53050042d6a4850f149fb504", Name = "name changed", StatusId = 2, Details="this barrier details 123"},
                BarrierIdsList = barrierIds
            };

            PutUpdateBarrierResponse response = m.UpdatePatientBarrier(req);
            Assert.IsNotNull(response.BarrierData);
        }
    }
}
