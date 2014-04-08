using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientObservation.DTO;

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

            IPatientObservationRepository<object> repo =
                PatientObservationRepositoryFactory<object>.GetPatientObservationRepository(contractNumber, context, userId);
            GetAllowedStatesDataRequest request = new GetAllowedStatesDataRequest { TypeName = type, Context = context, ContractNumber = contractNumber, UserId = userId, Version = 1 };
            GetAllowedStatesDataResponse response = PatientObservationDataManager.GetAllowedStates(request);

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

            IPatientObservationRepository<object> repo =
                PatientObservationRepositoryFactory<object>.GetPatientObservationRepository(contractNumber, context, userId);
            GetAllowedStatesDataRequest request = new GetAllowedStatesDataRequest { TypeName = type, Context = context, ContractNumber = contractNumber, UserId = userId, Version = 1 };
            GetAllowedStatesDataResponse response = PatientObservationDataManager.GetAllowedStates(request);

            Assert.IsNull(response.StatesData);
        }
    }
}
