using System;
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

        [TestMethod]
        public void GetCurrentPatientObservations_Test()
        {
            // Arrange
            string userId = string.Empty;
            string contractNumber = "InHealth001";
            string context = "NG";
            string patientId = "5325d9f3d6a4850adcbba4ce";

            GetCurrentPatientObservationsDataRequest request = new GetCurrentPatientObservationsDataRequest { PatientId = patientId, Context = context, ContractNumber = contractNumber, UserId = userId, Version = 1 };

            PatientObservationDataManager cm = new PatientObservationDataManager { Factory = new PatientObservationRepositoryFactory() };
            GetCurrentPatientObservationsDataResponse response = cm.GetCurrentPatientObservations(request);

            Assert.IsNotNull(response);
        }


        [TestMethod]
        public void GetHistoricalPatientObservations_Test()
        {
            // Arrange
            string userId = string.Empty;
            string contractNumber = "InHealth001";
            string context = "NG";
            string patientId = "5325db20d6a4850adcbba84e";

            GetHistoricalPatientObservationsDataRequest request = new GetHistoricalPatientObservationsDataRequest { PatientId = patientId, Context = context, ContractNumber = contractNumber, UserId = userId, Version = 1, ObservationId = "530c270afe7a592f64473e38" };

            PatientObservationDataManager cm = new PatientObservationDataManager { Factory = new PatientObservationRepositoryFactory() };
            List<PatientObservationData> response = cm.GetHistoricalPatientObservations(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void SavePatientObservations_Test()
        {
            // Arrange
            string userId = "5325c821072ef705080d3488";
            string contractNumber = "InHealth001";
            string context = "NG";
            string patientId = "5325d9f3d6a4850adcbba4ce";
            List<PatientObservationRecordData> recordData = new List<PatientObservationRecordData>();

            PatientObservationRecordData data1 = new PatientObservationRecordData
            {
                DisplayId = 1,
                EndDate = DateTime.Now.AddDays(10),
                DeleteFlag = false,
                Id = "53e3ccb8d6a485134024f1c3",
                NonNumericValue = "22",
                Source = "CM1",
                StartDate = DateTime.Now,
                StateId = 1,
                Units = "%",
                Value = 33.0
            };

            PatientObservationRecordData data2 = new PatientObservationRecordData
            {
                DisplayId = 1,
                EndDate = DateTime.Now.AddDays(15),
                DeleteFlag = false,
                Id = "53e3ccb8d6a485134024f1ce",
                NonNumericValue = "44",
                Source = "CM2",
                StartDate = DateTime.Now.AddDays(1),
                StateId = 2,
                Units = "ml",
                Value = 44.0
            };
            recordData.Add(data1);
            recordData.Add(data2);
            PutUpdatePatientObservationsDataRequest request = new PutUpdatePatientObservationsDataRequest { PatientId = patientId, Context = context, ContractNumber = contractNumber, UserId = userId, Version = 1, PatientObservationsRecordData = recordData };

            PatientObservationDataManager cm = new PatientObservationDataManager { Factory = new PatientObservationRepositoryFactory() };
            PutUpdatePatientObservationsDataResponse response = cm.UpdatePatientObservations(request);

            Assert.IsNotNull(response);
        }
    }
}
