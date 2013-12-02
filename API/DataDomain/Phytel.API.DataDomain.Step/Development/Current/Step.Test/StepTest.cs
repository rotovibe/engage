using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Step.DTO;

namespace Phytel.API.DataDomain.Step.Test
{
    [TestClass]
    public class StepTest
    {
        [TestMethod]
        public void GetStepByID()
        {
            StepRequest request = new StepRequest{ StepID = "5"};

            StepResponse response = StepDataManager.GetStepByID(request);

            Assert.IsTrue(response.StepID == "Tony");
        }
    }
}
