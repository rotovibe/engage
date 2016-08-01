using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.Test.Contact
{
    [TestClass()]
    public class AssignedToMePCMCohortRuleTests
    {
        [TestInitialize]
        public void SetUp()
        {

        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AssignedToMePCMRule_Run_Null_Input_ShouldThrow()
        {

            var rule = new AssignedToMePCMRule(null, null, null);

            rule.Run(null, null);

        }

        [TestMethod]
        public void AssignedToMePCMRule_Run_Does_Not_Have_ActiveCorePCM_Should_NOT_Call_Add_PCM()
        {
            //Arrange
            var mockContactDataController = new Mock<IContactEndpointUtil>();
            var mockLogger = new Mock<ILogger>();
            var mockCohortRuleUtil = new Mock<ICohortRuleUtil>();

            mockCohortRuleUtil.Setup(mcru => mcru.GetCareTeamActiveCorePCM(It.IsAny<CareTeam>())).Returns(new Member { ContactId = "cc"});

            var rule = new AssignedToMePCMRule(mockContactDataController.Object, mockLogger.Object, mockCohortRuleUtil.Object);
            var ruleResponse = rule.Run(new DTO.CareTeam { ContactId = "cid", Members = new List<Member> { new Member { ContactId = "mcId", RoleId = Constants.PCMRoleId, Core = false } } }, new CohortRuleCheckData { ContactId = "cid", ContractNumber = "inhealth001", UserId = "1234" });

            Assert.IsNotNull(ruleResponse);

            mockCohortRuleUtil.Verify(c => c.GetCareTeamActiveCorePCM(It.IsAny<DTO.CareTeam>()), Times.Once);
            mockContactDataController.Verify(mcdc => mcdc.AddPCMToCohortPatientView(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<string>(), It.IsAny<string>(),true), Times.Never);



        }

        [TestMethod]
        public void AssignedToMePCMRule_Run_Has_ActiveCorePCM_User_Should_Call_Add_PCM()
        {
            //Arrange
            var mockContactDataController = new Mock<IContactEndpointUtil>();
            var mockLogger = new Mock<ILogger>();
            var mockCohortRuleUtil = new Mock<ICohortRuleUtil>();

            mockCohortRuleUtil.Setup(mcru => mcru.GetCareTeamActiveCorePCM(It.IsAny<CareTeam>())).Returns(new Member { ContactId = "mcid" });
            var rule = new AssignedToMePCMRule(mockContactDataController.Object, mockLogger.Object, mockCohortRuleUtil.Object);
            var ruleResponse = rule.Run(new DTO.CareTeam { ContactId = "cid", Members = new List<Member> { new Member { ContactId = "mcid", RoleId = "OtherroleId", Core = false } } }, new CohortRuleCheckData { ContactId = "cid", ContractNumber = "inhealth001", UserId = "1234", UsersContactIds = new List<string> { "cid","mcid"}});

            Assert.IsNotNull(ruleResponse);

            mockCohortRuleUtil.Verify(c => c.GetCareTeamActiveCorePCM(It.IsAny<DTO.CareTeam>()), Times.Once);
            mockContactDataController.Verify(mcdc => mcdc.AddPCMToCohortPatientView(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<string>(), It.IsAny<string>(),true), Times.Once);
            
        }

        [TestMethod]
        public void AssignedToMePCMRule_Run_Has_ActiveCorePCM_NotUser_Should_Call_Add_PCM()
        {
            //Arrange
            var mockContactDataController = new Mock<IContactEndpointUtil>();
            var mockLogger = new Mock<ILogger>();
            var mockCohortRuleUtil = new Mock<ICohortRuleUtil>();

            mockCohortRuleUtil.Setup(mcru => mcru.GetCareTeamActiveCorePCM(It.IsAny<CareTeam>())).Returns(new Member { ContactId = "mcid" });
            var rule = new AssignedToMePCMRule(mockContactDataController.Object, mockLogger.Object, mockCohortRuleUtil.Object);
            var ruleResponse = rule.Run(new DTO.CareTeam { ContactId = "cid", Members = new List<Member> { new Member { ContactId = "mcid", RoleId = "OtherroleId", Core = false } } }, new CohortRuleCheckData { ContactId = "cid", ContractNumber = "inhealth001", UserId = "1234", UsersContactIds = new List<string> { "user1", "user2" } });

            Assert.IsNotNull(ruleResponse);

            mockCohortRuleUtil.Verify(c => c.GetCareTeamActiveCorePCM(It.IsAny<DTO.CareTeam>()), Times.Once);
            mockContactDataController.Verify(mcdc => mcdc.AddPCMToCohortPatientView(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<string>(), It.IsAny<string>(),true), Times.Never);

        }
    }
}
