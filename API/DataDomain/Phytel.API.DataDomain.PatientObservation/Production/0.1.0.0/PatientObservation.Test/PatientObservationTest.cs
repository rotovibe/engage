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
    }
}
