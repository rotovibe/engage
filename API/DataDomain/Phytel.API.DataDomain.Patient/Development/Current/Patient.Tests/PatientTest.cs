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
            DataPatientRequest request = new DataPatientRequest{ PatientID = "5"};

            DataPatientResponse response = PatientDataManager.GetPatientByID(request);

            Assert.IsTrue(response.FirstName == "Tony");
        }
    }
}
