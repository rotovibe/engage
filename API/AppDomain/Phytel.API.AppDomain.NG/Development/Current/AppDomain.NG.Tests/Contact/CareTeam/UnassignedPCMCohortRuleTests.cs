using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.Test.Contact

{
    [TestClass]
    public class UnassignedPCMCohortRuleTests
    {
        [TestInitialize]
        public void SetUp()
        {
            
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnassignedPCMCohortRule_Run_Null_Input_ShouldThrow()
        {
            
            var rule = new UnAssignedPCMRule(null, null, null);

            rule.Run(null);

        }

       // [TestMethod]
        public void UnassignedPCMCohortRule_Run_Does_Not_Have_ActiveCorePCM_Should_Call_Add_PCM()
        {
            //Arrange
            var mockContactDataController  = new Mock<IContactEndpointUtil>();
            var mockLogger = new Mock<ILogger>();
            var mockCohortRuleUtil = new Mock<ICohortRuleUtil>();

            mockCohortRuleUtil.Setup(mcru => mcru.CheckIfCareTeamHasActiveCorePCM(It.IsAny<CareTeam>())).Returns(false);

            var rule = new UnAssignedPCMRule(mockContactDataController.Object, mockLogger.Object, mockCohortRuleUtil.Object);


           

        }

    }
}
