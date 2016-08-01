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

        
    }
}
