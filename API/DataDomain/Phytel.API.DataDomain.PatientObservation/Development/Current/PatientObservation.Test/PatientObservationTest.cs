using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientObservation.DTO;

namespace Phytel.API.DataDomain.PatientObservation.Test
{
    [TestClass]
    public class PatientObservationTest
    {
        static string userId = "000000000000000000000000";
        static string contractNumber = "InHealth001";
        static string context = "NG";
        static int version = 1;
        
        [TestMethod]
        public void InitializePatientProblem_Test()
        {
            GetInitializeProblemDataRequest request = new GetInitializeProblemDataRequest { Context = context, ContractNumber = contractNumber, ObservationId = "533ed16ed4332307bc592bb9", PatientId = "5325db00d6a4850adcbba802", UserId = userId, Version = version };

            GetInitializeProblemDataResponse response = new PatientObservationDataManager().GetInitializeProblem(request);

            Assert.IsNotNull(response.PatientObservation);
        }

        [TestMethod]
        public void UpdatePatientProblem_Test()
        {
            PutUpdateObservationDataRequest request = new PutUpdateObservationDataRequest { Context = context, ContractNumber = contractNumber, PatientId = "5325da48d6a4850adcbba5c2", PatientObservationData = GetProblem(), UserId = userId, Version = version };

            bool response = (bool) new PatientObservationDataManager().PutUpdateOfPatientObservationRecord(request);

            Assert.IsTrue(response);
        }

        private PatientObservationRecordData GetProblem()
        {
            PatientObservationRecordData data = new PatientObservationRecordData
            {
                Id = "5347051dd6a48504b497a186",
                DeleteFlag = true
            };
            return data;
        }
    }
}
