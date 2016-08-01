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
            GetModuleRequest request = new GetModuleRequest{ ModuleID = "5"};

            GetModuleResponse response = ModuleDataManager.GetModuleByID(request);

            Assert.IsTrue(response.Module.Id == "Tony");
        }
    }
}
