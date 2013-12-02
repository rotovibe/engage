using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Action.DTO;

namespace Phytel.API.DataDomain.Action.Test
{
    [TestClass]
    public class ActionTest
    {
        [TestMethod]
        public void GetActionByID()
        {
            ActionRequest request = new ActionRequest{ ActionID = "5"};

            ActionResponse response = ActionDataManager.GetActionByID(request);

            Assert.IsTrue(response.ActionID == "Tony");
        }
    }
}
