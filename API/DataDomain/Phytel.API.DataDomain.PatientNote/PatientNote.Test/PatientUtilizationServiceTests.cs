using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.DataDomain.PatientNote.Test;
using Phytel.API.DataDomain.PatientNote.Test.Stub;

namespace Phytel.API.DataDomain.PatientNote.Service.Tests
{
    [TestClass()]
    public class PatientUtilizationServiceTests
    {
        private static PatientUtilizationData _uData;

        [ClassInitialize()]
        public static void Init(TestContext context)
        {
        }

        [TestMethod()]
        public void InsertUtilization()
        {
            var serv = new PatientUtilizationService { Manager = new StubDataPatientUtilizationManager() };

            var result = serv.Manager.InsertPatientUtilization(TestInit.PatientUtilizationData);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void GetAllUtilizationsByPatient()
        {
            var serv = new PatientUtilizationService { Manager = new StubDataPatientUtilizationManager() };
            var patientId = "111111111111111111111111";

            var result = serv.Manager.GetPatientUtilizations(patientId);
            Assert.AreEqual(result[0], TestInit.PatientUtilizationData);
        }

        [TestMethod()]
        public void GetUtilizationById()
        {
            var serv = new PatientUtilizationService { Manager = new StubDataPatientUtilizationManager() };
            var UtilId = "5591b232fe7a5c20bcb03882";

            var result = serv.Manager.GetPatientUtilization(UtilId);
            Assert.AreEqual(result, TestInit.PatientUtilizationData);
        }

        [TestMethod()]
        public void UpdatePatientUtilization()
        {
            var serv = new PatientUtilizationService { Manager = new StubDataPatientUtilizationManager() };

            var result = serv.Manager.UpdatePatientUtilization(TestInit.NewPatientUtilizationData);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void DeletePatientUtilization()
        {
            var serv = new PatientUtilizationService { Manager = new StubDataPatientUtilizationManager() };

            var result = serv.Manager.DeletePatientUtilization(TestInit.PatientUtilizationData.Id);
            Assert.IsTrue(result);
        }
    }
}
