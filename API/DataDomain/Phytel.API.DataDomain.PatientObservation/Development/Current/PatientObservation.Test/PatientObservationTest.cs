using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientObservation.DTO;

namespace Phytel.API.DataDomain.PatientObservation.Test
{
    [TestClass]
    public class PatientObservationTest
    {
        [TestMethod]
        public void GetPatientObservationByID()
        {
            GetPatientObservationRequest request = new GetPatientObservationRequest{ PatientObservationID = "5"};

            GetPatientObservationResponse response = PatientObservationDataManager.GetPatientObservationByID(request);

            Assert.IsTrue(response.PatientObservation.PatientObservationID == "Tony");
        }


        [TestMethod]
        public void GetAllowedObservationStates_CorrectType_Test()
        {
            // Arrange
            string userId = string.Empty;
            string contractNumber = "InHealth001";
            string context = "NG";
            string type = "Lab";

            IPatientObservationRepository<object> repo =
                PatientObservationRepositoryFactory<object>.GetPatientObservationRepository(contractNumber, context, userId);

            List<int> types = repo.GetAllowedObservationStates(type);
            
            Assert.IsTrue(types.Count > 0);
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

            List<int> types = repo.GetAllowedObservationStates(type);

            Assert.IsNull(types);
        }
    }
}
