using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Module.DTO;

namespace Phytel.API.DataDomain.Module.Test
{
    [TestClass]
    public class ModuleTest
    {
        [TestMethod]
        public void GetModuleByID()
        {
            ModuleRequest request = new ModuleRequest{ ModuleID = "5"};

            ModuleResponse response = ModuleDataManager.GetModuleByID(request);

            Assert.IsTrue(response.ModuleID == "Tony");
        }
    }
}
