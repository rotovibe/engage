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
            // Arrange

            string type = "Lab";

            GetAllowedStatesDataRequest request = new GetAllowedStatesDataRequest { TypeName = type, Context = context, ContractNumber = contractNumber, UserId = userId, Version = 1 };
            IPatientObservationRepository repo = new PatientObservationRepositoryFactory().GetRepository(request, RepositoryType.PatientObservation);
            GetAllowedStatesDataResponse response = new PatientObservationDataManager().GetAllowedStates(request);

            Assert.IsTrue(response.StatesData.Count > 0);
        }

        [TestMethod]
        public void GetAllowedObservationStates_IncorrectType_NullResult_Test()
        {
            // Arrange
            string userId = string.Empty;
            string contractNumber = "InHealth001";
            string context = "NG";
            string type = "Labs";

            GetAllowedStatesDataRequest request = new GetAllowedStatesDataRequest { TypeName = type, Context = context, ContractNumber = contractNumber, UserId = userId, Version = 1 };
            IPatientObservationRepository repo = new PatientObservationRepositoryFactory().GetRepository(request, RepositoryType.PatientObservation);
            GetAllowedStatesDataResponse response = new PatientObservationDataManager().GetAllowedStates(request);

            Assert.IsNull(response.StatesData);
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
    }
}
