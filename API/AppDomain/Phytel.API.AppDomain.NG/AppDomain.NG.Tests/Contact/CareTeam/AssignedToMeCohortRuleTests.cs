using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.AppDomain.NG.Test.Contact
{
    [TestClass()]
    public class AssignedToMeCohortRuleTests
    {
        [TestInitialize]
        public void SetUp()
        {

        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AssignedToMeRule_Run_Null_Input_ShouldThrow()
        {

            var rule = new AssignedToMeRule(null,null,null);

            rule.Run(null, null);

        }

        [TestMethod]
        public void AssignedToMeRule_Run_User_Inactive_Success()
        {
            //Arrange
            var mockContactDataController = new Mock<IContactEndpointUtil>();
            var mockLogger = new Mock<ILogger>();
            var mockCohortRuleUtil = new Mock<ICohortRuleUtil>();

            var rule = new AssignedToMeRule(mockContactDataController.Object, mockLogger.Object, mockCohortRuleUtil.Object);
            var ruleResponse = rule.Run(new DTO.CareTeam { ContactId = "cid", Members = new List<Member> { new Member { ContactId = "mcId", RoleId = Constants.PCMRoleId, Core = false, StatusId =  0} } }, new CohortRuleCheckData { ContactId = "cid", ContractNumber = "inhealth001", UserId = "1234", UsersContactIds = new List<string>
            {
                "mcId",
                "m2"
            }});

            Assert.IsNotNull(ruleResponse);

            mockContactDataController.Verify(mcdc => mcdc.AssignContactsToCohortPatientView(It.IsAny<string>(), new List<string> { "mcId" }, It.IsAny<double>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            
        }

        [TestMethod]
        public void AssignedToMeRule_Run_User_Active_Success()
        {
            //Arrange
            var mockContactDataController = new Mock<IContactEndpointUtil>();
            var mockLogger = new Mock<ILogger>();
            var mockCohortRuleUtil = new Mock<ICohortRuleUtil>();

            var rule = new AssignedToMeRule(mockContactDataController.Object, mockLogger.Object, mockCohortRuleUtil.Object);
            var ruleResponse = rule.Run(new DTO.CareTeam { ContactId = "cid", Members = new List<Member> { new Member { ContactId = "mcId", RoleId = Constants.PCMRoleId, Core = false, StatusId = (int)DataDomain.Contact.DTO.CareTeamMemberStatus.Active } } }, new CohortRuleCheckData
            {
                ContactId = "cid",
                ContractNumber = "inhealth001",
                UserId = "1234",
                UsersContactIds = new List<string>
            {
                "mcId",
                "m2"
            }
            });

            Assert.IsNotNull(ruleResponse);

            mockContactDataController.Verify(mcdc => mcdc.AssignContactsToCohortPatientView(It.IsAny<string>(), new List<string> { "mcId" }, It.IsAny<double>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        }

        [TestMethod]
        public void AssignedToMeRule_Run_NonUser_Success()
        {
            //Arrange
            var mockContactDataController = new Mock<IContactEndpointUtil>();
            var mockLogger = new Mock<ILogger>();
            var mockCohortRuleUtil = new Mock<ICohortRuleUtil>();

            // mockCohortRuleUtil.Setup(mcru => mcru.GetCareTeamActiveCorePCM(It.IsAny<CareTeam>())).Returns(new Member { ContactId = "cc" });

            var rule = new AssignedToMeRule(mockContactDataController.Object, mockLogger.Object, mockCohortRuleUtil.Object);
            var ruleResponse = rule.Run(new DTO.CareTeam { ContactId = "cid", Members = new List<Member> { new Member { ContactId = "mcId", RoleId = Constants.PCMRoleId, Core = false } } }, new CohortRuleCheckData
            {
                ContactId = "cid",
                ContractNumber = "inhealth001",
                UserId = "1234",
                UsersContactIds = new List<string>
            {
                "mcId1",
                "m21"
            }
            });

            Assert.IsNotNull(ruleResponse);


            mockContactDataController.Verify(mcdc => mcdc.AssignContactsToCohortPatientView(It.IsAny<string>(), new List<string>(), It.IsAny<double>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        }
    }
}
