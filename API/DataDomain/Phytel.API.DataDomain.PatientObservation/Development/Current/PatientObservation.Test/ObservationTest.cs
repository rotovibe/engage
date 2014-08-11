using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.DataDomain.PatientObservation.MongoDB.DTO;

namespace Phytel.API.DataDomain.PatientObservation.Test
{
    /// <summary>
    /// Summary description for ObservationTest
    /// </summary>
    [TestClass]
    public class ObservationTest
    {
        static string userId = string.Empty;
        static string contractNumber = "InHealth001";
        static string context = "NG";
        static int version = 1;

        [TestMethod]
        public void GetAllowedObservationStates_CorrectType_Test()
        {
            GetAllowedStatesDataRequest request = new GetAllowedStatesDataRequest { Context = context, ContractNumber = contractNumber, UserId = userId, Version = 1 };
            PatientObservationDataManager cm = new PatientObservationDataManager { Factory = new PatientObservationRepositoryFactory() };
            GetAllowedStatesDataResponse response = cm.GetAllowedStates(request);

            Assert.IsTrue(response.StatesData.Count > 0);
        }

        [TestMethod]
        public void GetObservations_Test()
        {
            // Arrange
            string userId = string.Empty;
            string contractNumber = "InHealth001";
            string context = "NG";


            GetObservationsDataRequest request = new GetObservationsDataRequest { Context = context, ContractNumber = contractNumber, UserId = userId, Version = 1 };
            IPatientObservationRepository repo = new PatientObservationRepositoryFactory().GetRepository(request, RepositoryType.PatientObservation);            
            GetObservationsDataResponse response = new PatientObservationDataManager().GetObservationsData(request);

            Assert.IsNotNull(response.ObservationsData);
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
        public void SavePatientObservations_Test()
        {
            // Arrange
            string userId = "5325c821072ef705080d3488";
            string contractNumber = "InHealth001";
            string context = "NG";
            string patientId = "5325d9f3d6a4850adcbba4ce";
            List<PatientObservationRecordData> recordData = new List<PatientObservationRecordData>();

            PatientObservationRecordData data1 = new PatientObservationRecordData { 
                DisplayId = 1,
                EndDate  = DateTime.Now.AddDays(10),
                DeleteFlag = false,
                Id = "53e3ccb8d6a485134024f1c3",
                NonNumericValue = "22",
                Source = "CM1",
                StartDate = DateTime.Now,
                StateId = 1,
                Units = "%",
                Value = 33.0
            };

            PatientObservationRecordData data2= new PatientObservationRecordData
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
