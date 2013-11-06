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
            PatientRequest request = new PatientRequest{ PatientID = "5"};

            PatientResponse response = PatientDataManager.GetPatientByID(request);

            Assert.IsTrue(response.FirstName == "Tony");
        }
    }
}
