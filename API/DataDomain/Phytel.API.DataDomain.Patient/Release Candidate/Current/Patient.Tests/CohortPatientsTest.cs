using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Patient.DTO;

namespace Phytel.API.DataDomain.Patient.Test
{
    [TestClass]
    public class CohortPatientsTest
    {
        [TestMethod]
        public void GetCohortPatientsByID_WithNoFilter()
        {
            GetCohortPatientsDataRequest request = new GetCohortPatientsDataRequest
            {
                CohortID = "528ed9b3072ef70e10099687",
                Version = "v1",
                Context = "NG",
                SearchFilter = "",
                ContractNumber = "InHealth001",
                Skip = 0,
                Take = 100
            };

            GetCohortPatientsDataResponse response = PatientDataManager.GetCohortPatients(request);

            Assert.IsTrue(response.CohortPatients.Count > 0);
        }

        [TestMethod]
        public void GetCohortPatientsByID_WithSingleFilter()
        {
            GetCohortPatientsDataRequest request = new GetCohortPatientsDataRequest
            {
                CohortID = "528ed9b3072ef70e10099687",
                Version = "v1",
                Context = "NG",
                SearchFilter = "Jonell",
                ContractNumber = "InHealth001",
                Skip = 0,
                Take = 100
            };

            GetCohortPatientsDataResponse response = PatientDataManager.GetCohortPatients(request);

            Assert.IsTrue(response.CohortPatients.Count > 0);
        }

        [TestMethod]
        public void GetCohortPatientsByID_WithDoubleFilterSpace()
        {
            GetCohortPatientsDataRequest request = new GetCohortPatientsDataRequest
            {
                CohortID = "528ed9b3072ef70e10099687",
                Version = "v1",
                Context = "NG",
                SearchFilter = "Jonell Tigue",
                ContractNumber = "InHealth001",
                Skip = 0,
                Take = 100
            };

            GetCohortPatientsDataResponse response = PatientDataManager.GetCohortPatients(request);

            Assert.IsTrue(response.CohortPatients.Count > 0);
        }

        [TestMethod]
        public void GetCohortPatientsByID_WithDoubleFilterComma()
        {
            GetCohortPatientsDataRequest request = new GetCohortPatientsDataRequest
            {
                CohortID = "528ed9b3072ef70e10099687",
                Version = "v1",
                Context = "NG",
                SearchFilter = "Tigue, Jonell",
                ContractNumber = "InHealth001",
                Skip = 0,
                Take = 100
            };

            GetCohortPatientsDataResponse response = PatientDataManager.GetCohortPatients(request);

            Assert.IsTrue(response.CohortPatients.Count > 0);
        }

        [TestMethod]
        public void GetCohortPatientsByID_WithStartingComma()
        {
            GetCohortPatientsDataRequest request = new GetCohortPatientsDataRequest
            {
                CohortID = "528ed9b3072ef70e10099687",
                Version = "v1",
                Context = "NG",
                SearchFilter = ", Jonell",
                ContractNumber = "InHealth001",
                Skip = 0,
                Take = 100
            };

            GetCohortPatientsDataResponse response = PatientDataManager.GetCohortPatients(request);

            Assert.IsTrue(response.CohortPatients.Count > 0);
        }
    }
}
