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
            GetPatientDataRequest request = new GetPatientDataRequest { PatientID = "531f2dcd072ef727c4d29fb0"};

            GetPatientDataResponse response = PatientDataManager.GetPatientByID(request);

            Assert.IsTrue(response.Patient.FirstName == "Phyliss");
        }

        [TestMethod]
        public void UpdatePatientBackground_Test()
        {
            PutPatientBackgroundDataRequest request = new PutPatientBackgroundDataRequest {  PatientId = "52f55899072ef709f84e7637", UserId = "bb241c64-a0ff-4e01-ba5f-4246ef50780e" };

            PutPatientBackgroundDataResponse response = PatientDataManager.UpdatePatientBackground(request);

            Assert.IsTrue(response.Success);
        }
    }
}
