using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Phytel.API.DataDomain.PatientObservation.Tests
{
    [TestClass()]
    public class MongoPatientObservationRepository_Tests
    {
        [TestClass()]
        public class FindByObservationIDTest
        {
            [TestMethod()]
            public void FindByObservationID_Exists()
            {
                MongoPatientObservationRepository repo = new MongoPatientObservationRepository("InHealth001");
                var obId = "533ed16ed4332307bc592bbb";
                var patientId = "5325db0cd6a4850adcbba81a";
                var result = repo.FindByObservationID(obId, patientId);
                Assert.IsNotNull(result);
            }
        }
    }
}
