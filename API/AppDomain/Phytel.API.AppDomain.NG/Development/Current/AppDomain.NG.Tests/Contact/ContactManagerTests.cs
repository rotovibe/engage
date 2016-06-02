using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.Service.Mappers;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.AppDomain.NG.Test.Contact
{
    [TestClass()]
    public class ContactManagerTests
    {
        [TestInitialize]
        public void SetUp()
        {
            ContactMapper.Build();
        }

        #region SaveCareTeam Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ContactManager_SaveCareTeam_Null_Request_Should_Throw()
        {
            var contactManager = new ContactManager();
            contactManager.SaveCareTeam(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ContactManager_SaveCareTeam_Null_CareTeam_Request_Should_Throw()
        {
            var contactManager = new ContactManager();
            contactManager.SaveCareTeam(new SaveCareTeamRequest { CareTeam = null });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ContactManager_SaveCareTeam_InCompleteMember_EmptyContactId_Should_Throw()
        {
            var contactManager = new ContactManager();
            var members = new List<Member>
            {
                new Member {  StatusId = 1  }
            };

            var stubRequest = new SaveCareTeamRequest {ContactId = string.Empty, CareTeam = new CareTeam() {Members = members}};
            contactManager.SaveCareTeam(stubRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ContactManager_SaveCareTeam_InCompleteMember_EmptyStatusId_Should_Throw()
        {
            var contactManager = new ContactManager();
            var members = new List<Member>
            {
                new Member {  ContactId  = "cid"}
            };

            var stubRequest = new SaveCareTeamRequest { ContactId = "cid", CareTeam = new CareTeam() { Members = members } };
            contactManager.SaveCareTeam(stubRequest);
        }


        [TestMethod]
        public void ContactManager_SaveCareTeam_Contact_Is_Not_IsPatient_Should_Throw()
        {
            //Arrange
            var contactManager = new ContactManager();
            var members = new List<Member>
            {
                new Member {  ContactId  = "cid", StatusId = 1, RoleId = "rid"}
            };

            var stubRequest = new SaveCareTeamRequest { ContactId = "cid", CareTeam = new CareTeam() { Members = members } };
            var mockContactEndPointUtil = new Mock<IContactEndpointUtil>();
            mockContactEndPointUtil.Setup(mceu => mceu.SaveCareTeam(It.IsAny<SaveCareTeamRequest>()))
                .Returns(new SaveCareTeamDataResponse());

            mockContactEndPointUtil.Setup(mceu => mceu.GetContactByContactId(It.IsAny<AppDomain.NG.DTO.GetContactByContactIdRequest>()))
                .Returns(new ContactData { PatientId = "" });

            var mockCohortRuleUtil = new Mock<ICohortRuleUtil>();
            mockCohortRuleUtil.Setup(mcru => mcru.HasMultipleActiveCorePCM(It.IsAny<AppDomain.NG.DTO.CareTeam>()))
                .Returns(false);
            mockCohortRuleUtil.Setup(mcru => mcru.HasMultipleActiveCorePCP(It.IsAny<AppDomain.NG.DTO.CareTeam>()))
                .Returns(false);

            var mockCohortRuleProcessor = new Mock<ICohortRulesProcessor>();

            contactManager.EndpointUtil = mockContactEndPointUtil.Object;
            contactManager.CohortRuleUtil = mockCohortRuleUtil.Object;
            contactManager.CohortRules = mockCohortRuleProcessor.Object;


            //Act
            try
            {
                var response = contactManager.SaveCareTeam(stubRequest);
            }
            catch (Exception ex){
                Assert.AreEqual(string.Format("Contact with id: {0} is not a patient.", "cid"), ex.Message);
            }
            

            //Assert.
            mockContactEndPointUtil.Verify(mr => mr.SaveCareTeam(It.IsAny<SaveCareTeamRequest>()), Times.Never);
        }

        [TestMethod]
        public void ContactManager_SaveCareTeam_Contact_Is_Null_Should_Throw()
        {
            //Arrange
            var contactManager = new ContactManager();
            var members = new List<Member>
            {
                new Member {  ContactId  = "cid", StatusId = 1, RoleId = "rid"}
            };

            var stubRequest = new SaveCareTeamRequest { ContactId = "cid", CareTeam = new CareTeam() { Members = members } };
            var mockContactEndPointUtil = new Mock<IContactEndpointUtil>();
            mockContactEndPointUtil.Setup(mceu => mceu.SaveCareTeam(It.IsAny<SaveCareTeamRequest>()))
                .Returns(new SaveCareTeamDataResponse());

            mockContactEndPointUtil.Setup(mceu => mceu.GetContactByContactId(It.IsAny<AppDomain.NG.DTO.GetContactByContactIdRequest>()))
                .Returns((ContactData)null);

            var mockCohortRuleUtil = new Mock<ICohortRuleUtil>();
            mockCohortRuleUtil.Setup(mcru => mcru.HasMultipleActiveCorePCM(It.IsAny<AppDomain.NG.DTO.CareTeam>()))
                .Returns(false);
            mockCohortRuleUtil.Setup(mcru => mcru.HasMultipleActiveCorePCP(It.IsAny<AppDomain.NG.DTO.CareTeam>()))
                .Returns(false);

            var mockCohortRuleProcessor = new Mock<ICohortRulesProcessor>();

            contactManager.EndpointUtil = mockContactEndPointUtil.Object;
            contactManager.CohortRuleUtil = mockCohortRuleUtil.Object;
            contactManager.CohortRules = mockCohortRuleProcessor.Object;


            //Act
            try
            {
                var response = contactManager.SaveCareTeam(stubRequest);
            }
            catch (Exception ex)
            {

                Assert.AreEqual(string.Format("Contact with id: {0} does not exist.", "cid"), ex.Message);
            }


            //Assert.
            mockContactEndPointUtil.Verify(mr => mr.SaveCareTeam(It.IsAny<SaveCareTeamRequest>()), Times.Never);
        }

        [TestMethod]
        public void ContactManager_SaveCareTeam_Success()
        {
            //Arrange
            var contactManager = new ContactManager();
            var members = new List<Member>
            {
                new Member {  ContactId  = "cid", StatusId = 1, RoleId = "rid"}
            };

            var stubRequest = new SaveCareTeamRequest { ContactId = "cid", CareTeam = new CareTeam() { Members = members } };
            var mockContactEndPointUtil = new Mock<IContactEndpointUtil>();
            mockContactEndPointUtil.Setup(mceu => mceu.SaveCareTeam(It.IsAny<SaveCareTeamRequest>()))
                .Returns(new SaveCareTeamDataResponse());

            mockContactEndPointUtil.Setup(mceu => mceu.GetContactByContactId(It.IsAny<AppDomain.NG.DTO.GetContactByContactIdRequest>()))
                .Returns(new ContactData { PatientId = "pid"});

            mockContactEndPointUtil.Setup(mceu => mceu.GetContactsByContactIds(It.IsAny<List<string>>(), It.IsAny<double>(), It.IsAny<string>(),It.IsAny<string>()))
                .Returns(new List<ContactData>());

            var mockCohortRuleUtil = new Mock<ICohortRuleUtil>();
            mockCohortRuleUtil.Setup(mcru => mcru.HasMultipleActiveCorePCM(It.IsAny<AppDomain.NG.DTO.CareTeam>()))
                .Returns(false);
            mockCohortRuleUtil.Setup(mcru => mcru.HasMultipleActiveCorePCP(It.IsAny<AppDomain.NG.DTO.CareTeam>()))
                .Returns(false);
            
            var mockCohortRuleProcessor = new Mock<ICohortRulesProcessor>();

            contactManager.EndpointUtil = mockContactEndPointUtil.Object;
            contactManager.CohortRuleUtil = mockCohortRuleUtil.Object;
            contactManager.CohortRules = mockCohortRuleProcessor.Object;


            //Act
            var response = contactManager.SaveCareTeam(stubRequest);

            //Assert.
            Assert.IsNotNull(response);
            mockContactEndPointUtil.Verify(mr => mr.SaveCareTeam(It.IsAny<SaveCareTeamRequest>()), Times.Once);
        }

        #endregion

        #region Update Care Team Member 

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ContactManager_UpdateCareTeamMember_Null_Request_Should_Throw()
        {
            var contactManager = new ContactManager();
            contactManager.UpdateCareTeamMember(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ContactManager_UpdateCareTeamMember_Null_CareTeamMember_Request_Should_Throw()
        {
            var contactManager = new ContactManager();
            contactManager.UpdateCareTeamMember(new UpdateCareTeamMemberRequest() { CareTeamMember = null });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ContactManager_UpdateCareTeamMember_EmptyOrNullContactId_Should_Throw()
        {
            var contactManager = new ContactManager();


            var stubRequest = new UpdateCareTeamMemberRequest() { CareTeamId = "careteamId", Id = "memberId", CareTeamMember = new Member() { StatusId = 1 } };
            contactManager.UpdateCareTeamMember(stubRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ContactManager_UpdateCareTeamMember_InCompleteMember_EmptyMemberContactId_Should_Throw()
        {
            var contactManager = new ContactManager();
            

            var stubRequest = new UpdateCareTeamMemberRequest() { ContactId = "cid", CareTeamId = "careteamId", Id = "memberId", CareTeamMember = new Member() { StatusId = 1 } };
            contactManager.UpdateCareTeamMember(stubRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ContactManager_UpdateCareTeamMember_EmptyOrNullCareTeamId_Should_Throw()
        {
            var contactManager = new ContactManager();


            var stubRequest = new UpdateCareTeamMemberRequest() { ContactId = "cid", Id="memberId", CareTeamMember = new Member() {Id="memberId", StatusId = 1 } };
            contactManager.UpdateCareTeamMember(stubRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ContactManager_UpdateCareTeamMember_EmptyOrNullMemberId_Should_Throw()
        {
            var contactManager = new ContactManager();


            var stubRequest = new UpdateCareTeamMemberRequest() { ContactId = "cid",CareTeamId = "careteamId",CareTeamMember = new Member() { Id = "memberId", StatusId = 1 } };
            contactManager.UpdateCareTeamMember(stubRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ContactManager_UpdateCareTeamMember_UnmatchedMemberId_Should_Throw()
        {
            var contactManager = new ContactManager();


            var stubRequest = new UpdateCareTeamMemberRequest() { ContactId = "cid", CareTeamId = "careteamId", Id = "memberId", CareTeamMember = new Member() { Id = "memberId1", StatusId = 1 } };
            contactManager.UpdateCareTeamMember(stubRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void ContactManager_UpdateCareTeamMember_ContactIdDoesNotExist_Should_Throw()
        {
            //Arrange           
           
            var contactManager = new ContactManager();

            var stubRequest = new UpdateCareTeamMemberRequest() { ContactId = "cid", CareTeamId = "careteamId", Id = "memberId", CareTeamMember = new Member() { ContactId = "memberCid", Id = "memberId", StatusId = 1, RoleId = "roleId" } };
            var mockContactEndPointUtil = new Mock<IContactEndpointUtil>();
            mockContactEndPointUtil.Setup(mceu => mceu.UpdateCareTeamMember(It.IsAny<UpdateCareTeamMemberRequest>()))
                .Returns(new UpdateCareTeamMemberResponse());
            if (mockContactEndPointUtil.Setup(mcue => mcue.GetContactByContactId(It.IsAny<GetContactByContactIdRequest>())) == null){}

            contactManager.EndpointUtil = mockContactEndPointUtil.Object;


            var mockCohortRulesProcessor = new Mock<ICohortRulesProcessor>();
            contactManager.CohortRules = mockCohortRulesProcessor.Object;

            //Act
            var response = contactManager.UpdateCareTeamMember(stubRequest);

            //Assert.
            Assert.IsNotNull(response);
            mockContactEndPointUtil.Verify(mr => mr.UpdateCareTeamMember(It.IsAny<UpdateCareTeamMemberRequest>()), Times.Once);
        }

        [TestMethod]
        public void ContactManager_UpdateCareTeamMember_Success()
        {
            //Arrange           
            var contactData = new ContactData()
            {
                Id = "cid"
            };
            var contactManager = new ContactManager();
            
            var stubRequest = new UpdateCareTeamMemberRequest() { ContactId = "cid", CareTeamId = "careteamId", Id = "memberId", CareTeamMember = new Member() { ContactId = "memberCid", Id = "memberId",StatusId = 1,RoleId = "roleId"} };
            var mockContactEndPointUtil = new Mock<IContactEndpointUtil>();
            mockContactEndPointUtil.Setup(mceu => mceu.UpdateCareTeamMember(It.IsAny<UpdateCareTeamMemberRequest>()))
                .Returns(new UpdateCareTeamMemberResponse());
            mockContactEndPointUtil.Setup(mcue => mcue.GetContactByContactId(It.IsAny<GetContactByContactIdRequest>()))
                .Returns(contactData);

            contactManager.EndpointUtil = mockContactEndPointUtil.Object;
            

            var mockCohortRulesProcessor = new Mock<ICohortRulesProcessor>();
            contactManager.CohortRules = mockCohortRulesProcessor.Object;

            //Act
            var response = contactManager.UpdateCareTeamMember(stubRequest);

            //Assert.
            Assert.IsNotNull(response);
            mockContactEndPointUtil.Verify(mr => mr.UpdateCareTeamMember(It.IsAny<UpdateCareTeamMemberRequest>()), Times.Once);
        }
        #endregion

        #region GetCareTeam Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ContactManager_GetCareTeam_Null_Request_Should_Throw()
        {
            var contactManager = new ContactManager();
            contactManager.GetCareTeam(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ContactManager_GetCareTeam_Null_ContactId_Request_Should_Throw()
        {
            var contactManager = new ContactManager();
            contactManager.GetCareTeam(new GetCareTeamRequest { ContactId = string.Empty });
        }
        #endregion
    }
}
