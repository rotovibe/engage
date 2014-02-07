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
            GetPatientDataRequest request = new GetPatientDataRequest { PatientID = "52e26f5b072ef7191c11e0b6" };

            GetPatientDataResponse response = PatientDataManager.GetPatientByID(request);

            Assert.IsTrue(response.Patient.FirstName == "Phyliss");
        }
    }
}
