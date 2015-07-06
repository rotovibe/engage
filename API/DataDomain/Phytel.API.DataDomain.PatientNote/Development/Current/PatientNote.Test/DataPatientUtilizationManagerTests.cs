using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientNote.Test;
using Phytel.API.DataDomain.PatientNote.Test.Stub;

namespace Phytel.API.DataDomain.PatientNote.Tests
{
    [TestClass()]
    public class DataPatientUtilizationManagerTests
    {
        [TestMethod()]
        public void InsertPatientUtilizationTest()
        {
            IDataPatientUtilizationManager mgr = new StubDataPatientUtilizationManager();
            var result = mgr.InsertPatientUtilization(TestInit.PatientUtilizationData);
            Assert.IsNotNull(result);
        }
    }
}
