using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Action.DTO;

namespace Phytel.API.DataDomain.Action.Test
{
    [TestClass]
    public class ActionTest
    {
        [TestMethod]
        public void GetActionByID_Test()
        {
            // Arrange
            double version = 1.0;
            string contractNumber = "InHealth001";
            string context = "NG";
            GetActionDataRequest request = new GetActionDataRequest { ActionID = "52a0f33bd43323141c9eb274", Context = context, ContractNumber = contractNumber, Version = version };

            // Act
            GetActionDataResponse response = ActionDataManager.GetActionByID(request);

            // Assert
            Assert.IsTrue(response != null); //.Action.Name == "Verify P4H Eligibility");

        }
    }
}
