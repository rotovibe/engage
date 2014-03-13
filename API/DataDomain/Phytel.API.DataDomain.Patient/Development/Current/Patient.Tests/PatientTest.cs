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
            GetPatientDataRequest request = new GetPatientDataRequest { PatientID = "531f2dcc072ef727c4d29e1a" };

            GetPatientDataResponse response = PatientDataManager.GetPatientByID(request);

            Assert.IsTrue(response.Patient.FirstName == "Phyliss");
        }

        [TestMethod]
        public void GetPatientSSN()
        {
            GetPatientSSNDataRequest request = new GetPatientSSNDataRequest { PatientId = "531f2dce072ef727c4d2a065", UserId = "531f2df6072ef727c4d2a3c0" };

            GetPatientSSNDataResponse response = PatientDataManager.GetPatientSSN(request);

            Assert.IsNotNull(response.SSN);
        }

        [TestMethod]
        public void UpdatePatientBackground_Test()
        {
            PutPatientBackgroundDataRequest request = new PutPatientBackgroundDataRequest {  PatientId = "52f55899072ef709f84e7637", UserId = "bb241c64-a0ff-4e01-ba5f-4246ef50780e" };

            PutPatientBackgroundDataResponse response = PatientDataManager.UpdatePatientBackground(request);

            Assert.IsTrue(response.Success);
        }

        [TestMethod]
        public void UpdatePatient_Test()
        {
            PutUpdatePatientDataRequest request = new PutUpdatePatientDataRequest
            {
                Id = "531f2dce072ef727c4d2a065",
                FullSSN = "888-88-8888",
                UserId = "531f2df5072ef727c4d2a3bc",
                Priority = 1,
                PreferredName = "\"\"",
                LastName = "Aarsvold"
            };

            PutUpdatePatientDataResponse response = PatientDataManager.UpdatePatient(request);

            Assert.IsNotNull(response);
        }
    }
}
