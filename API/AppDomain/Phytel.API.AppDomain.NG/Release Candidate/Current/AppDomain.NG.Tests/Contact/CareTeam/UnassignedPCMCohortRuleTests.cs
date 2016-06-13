using System;
using System.Collections.Generic;
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

            rule.Run(null, null);

        }

       [TestMethod]
        public void UnassignedPCMCohortRule_Run_Does_Not_Have_ActiveCorePCM_Should_Call_Add_PCM()
        {
            //Arrange
            var mockContactDataController  = new Mock<IContactEndpointUtil>();
            var mockLogger = new Mock<ILogger>();
            var mockCohortRuleUtil = new Mock<ICohortRuleUtil>();

            mockCohortRuleUtil.Setup(mcru => mcru.CheckIfCareTeamHasActiveCorePCM(It.IsAny<CareTeam>())).Returns(false);

            var rule = new UnAssignedPCMRule(mockContactDataController.Object, mockLogger.Object, mockCohortRuleUtil.Object);
            var ruleResponse = rule.Run(new CareTeam { ContactId = "cid", Members = new List<Member> { new Member { ContactId = "mcId", RoleId = Constants.PCMRoleId, Core = false }}}, new CohortRuleCheckData { ContactId = "cid", ContractNumber = "inhealth001", UserId = "1234"});

            Assert.IsNotNull(ruleResponse);

            mockCohortRuleUtil.Verify(c => c.CheckIfCareTeamHasActiveCorePCM(It.IsAny<CareTeam>()),Times.Once);
            mockContactDataController.Verify(mcdc => mcdc.RemovePCMCohortPatientView(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);



        }

       [TestMethod]
       public void UnassignedPCMCohortRule_Run_Has_ActiveCorePCM_Should_NOT_Call_Add_PCM()
       {
           //Arrange
           var mockContactDataController = new Mock<IContactEndpointUtil>();
           var mockLogger = new Mock<ILogger>();
           var mockCohortRuleUtil = new Mock<ICohortRuleUtil>();

           mockCohortRuleUtil.Setup(mcru => mcru.CheckIfCareTeamHasActiveCorePCM(It.IsAny<CareTeam>())).Returns(true);

           var rule = new UnAssignedPCMRule(mockContactDataController.Object, mockLogger.Object, mockCohortRuleUtil.Object);
           var ruleResponse = rule.Run(new CareTeam { ContactId = "cid", Members = new List<Member> { new Member { ContactId = "mcId", RoleId = "OtherroleId", Core = false } } }, new CohortRuleCheckData { ContactId = "cid", ContractNumber = "inhealth001", UserId = "1234" });

           Assert.IsNotNull(ruleResponse);

           mockCohortRuleUtil.Verify(c => c.CheckIfCareTeamHasActiveCorePCM(It.IsAny<CareTeam>()), Times.Once);
           mockContactDataController.Verify(mcdc => mcdc.RemovePCMCohortPatientView(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);



       }

    }
}
