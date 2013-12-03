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
            GetActionRequest request = new GetActionRequest{ ActionID = "5"};

            GetActionResponse response = ActionDataManager.GetActionByID(request);

            Assert.IsTrue(response.Action.ActionID == "Tony");
        }
    }
}
