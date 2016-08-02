using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Phytel.API.DataDomain.PatientSystem.Tests
{
    [TestClass()]
    public class EngageIdTests
    {
        [TestMethod()]
        public void Create_New_ID()
        {
            var id = EngageId.New();
            Assert.IsNotNull(id);
        }
    }
}
