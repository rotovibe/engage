using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Patient.DTO;

namespace Phytel.API.DataDomain.Patient.Test
{
    [TestClass]
    public class PatientTest
    {
        [TestMethod]
        public void GetPatientByID()
        {
            GetPatientDataRequest request = new GetPatientDataRequest { PatientID = "528e170c072ef71700e5e73b" };

            GetPatientDataResponse response = PatientDataManager.GetPatientByID(request);

            Assert.IsTrue(response.Patient.FirstName == "Phyliss");
        }
    }
}
