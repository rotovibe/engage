using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Contact;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Contact.Test.Stubs;
using Phytel.API.DataDomain.Contact.DTO;
using Moq;
using Phytel.API.DataDomain.Contact.CareTeam;
using Phytel.API.DataDomain.Contact.ContactTypeLookUp;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;
using Phytel.API.Interface;
using ServiceStack.Common;

namespace Phytel.API.DataDomain.Contact.Test
{
    [TestClass]
    public class CareTeamDataManagerTests
    {
        #region Update Care Team Member
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CareTeamDataManager_UpdateCareTeamMember_Null_Request_Should_Throw()
        {
            var mockFactory = new Mock<ICareTeamRepositoryFactory>();


            mockFactory.Setup(
                f => f.GetCareTeamRepository(It.IsAny<IDataDomainRequest>(), It.IsAny<RepositoryType>()))
                .Returns((ICareTeamRepository)null);

            var careTeamDataManager = new CareTeamDataManager(mockFactory.Object);

            var data = careTeamDataManager.UpdateCareTeamMember(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CareTeamDataManager_UpdateCareTeamMember_Null_CareTeamMember_Request_Should_Throw()
        {
            var mockFactory = new Mock<ICareTeamRepositoryFactory>();
            var stubRequest = new UpdateCareTeamMemberDataRequest() { CareTeamId = "careteamId", Id = "memberId", CareTeamMemberData = null };

            mockFactory.Setup(
                f => f.GetCareTeamRepository(It.IsAny<IDataDomainRequest>(), It.IsAny<RepositoryType>()))
                .Returns((ICareTeamRepository)null);

            var careTeamDataManager = new CareTeamDataManager(mockFactory.Object);

            var data = careTeamDataManager.UpdateCareTeamMember(stubRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CareTeamDataManager_UpdateCareTeamMember_EmptyOrNullContactId_Request_Should_Throw()
        {
            var mockFactory = new Mock<ICareTeamRepositoryFactory>();
            var stubRequest = new UpdateCareTeamMemberDataRequest() { CareTeamId = "careteamId", Id = "memberId", CareTeamMemberData = new CareTeamMemberData() { Id = "memberId1", ContactId = "memberCid", StatusId = 1 } };

            mockFactory.Setup(
                f => f.GetCareTeamRepository(It.IsAny<IDataDomainRequest>(), It.IsAny<RepositoryType>()))
                .Returns((ICareTeamRepository)null);

            var careTeamDataManager = new CareTeamDataManager(mockFactory.Object);

            var data = careTeamDataManager.UpdateCareTeamMember(stubRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CareTeamDataManager_UpdateCareTeamMember_InCompleteMember_EmptyMemberContactId_Should_Throw()
        {
            var mockFactory = new Mock<ICareTeamRepositoryFactory>();
            var stubRequest = new UpdateCareTeamMemberDataRequest() { ContactId = "cid", CareTeamId = "careteamId", Id = "memberId", CareTeamMemberData = new CareTeamMemberData() { Id = "memberId1", StatusId = 1 } };

            mockFactory.Setup(
                f => f.GetCareTeamRepository(It.IsAny<IDataDomainRequest>(), It.IsAny<RepositoryType>()))
                .Returns((ICareTeamRepository)null);

            var careTeamDataManager = new CareTeamDataManager(mockFactory.Object);

            var data = careTeamDataManager.UpdateCareTeamMember(stubRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CareTeamDataManager_UpdateCareTeamMember_EmptyOrNullCareTeamId_Should_Throw()
        {
            var mockFactory = new Mock<ICareTeamRepositoryFactory>();
            var stubRequest = new UpdateCareTeamMemberDataRequest() { ContactId = "cid", Id = "memberId", CareTeamMemberData = new CareTeamMemberData() { Id = "memberId", StatusId = 1 } };

            mockFactory.Setup(
                f => f.GetCareTeamRepository(It.IsAny<IDataDomainRequest>(), It.IsAny<RepositoryType>()))
                .Returns((ICareTeamRepository)null);

            var careTeamDataManager = new CareTeamDataManager(mockFactory.Object);

            var data = careTeamDataManager.UpdateCareTeamMember(stubRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CareTeamDataManager_UpdateCareTeamMember_EmptyOrNullMemberId_Should_Throw()
        {
            var mockFactory = new Mock<ICareTeamRepositoryFactory>();
            var stubRequest = new UpdateCareTeamMemberDataRequest() { ContactId = "cid", CareTeamMemberData = new CareTeamMemberData() { Id = "memberId1", StatusId = 1 } };

            mockFactory.Setup(
                f => f.GetCareTeamRepository(It.IsAny<IDataDomainRequest>(), It.IsAny<RepositoryType>()))
                .Returns((ICareTeamRepository)null);

            var careTeamDataManager = new CareTeamDataManager(mockFactory.Object);

            var data = careTeamDataManager.UpdateCareTeamMember(stubRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CareTeamDataManager_UpdateCareTeamMember_UnmatchedMemberId_Should_Throw()
        {
            var mockFactory = new Mock<ICareTeamRepositoryFactory>();
            var stubRequest = new UpdateCareTeamMemberDataRequest() { ContactId = "cid", CareTeamId = "careteamId", Id = "memberId", CareTeamMemberData = new CareTeamMemberData() { Id = "memberId1", StatusId = 1 } };

            mockFactory.Setup(
                f => f.GetCareTeamRepository(It.IsAny<IDataDomainRequest>(), It.IsAny<RepositoryType>()))
                .Returns((ICareTeamRepository)null);

            var careTeamDataManager = new CareTeamDataManager(mockFactory.Object);

            var data = careTeamDataManager.UpdateCareTeamMember(stubRequest);
        }
        #endregion

        #region Get Care Team 
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CareTeamDataManager_GetCareTeam_Null_Request_Should_Throw()
        {
            var mockFactory = new Mock<ICareTeamRepositoryFactory>();
            mockFactory.Setup(
                f => f.GetCareTeamRepository(It.IsAny<IDataDomainRequest>(), It.IsAny<RepositoryType>()))
                .Returns((ICareTeamRepository)null);

            var manager = new CareTeamDataManager(mockFactory.Object);
            var data = manager.GetCareTeam(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CareTeamDataManager_GetCareTeam_Null_ContactId_Should_Throw()
        {
            var mockFactory = new Mock<ICareTeamRepositoryFactory>();
            var stubRequest = new GetCareTeamDataRequest { ContactId = null  };

            mockFactory.Setup(
                f => f.GetCareTeamRepository(It.IsAny<IDataDomainRequest>(), It.IsAny<RepositoryType>()))
                .Returns((ICareTeamRepository)null);

            var manager = new CareTeamDataManager(mockFactory.Object);
            var data = manager.GetCareTeam(stubRequest);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CareTeamDataManager_GetCareTeam_Null_Repository_Should_Throw()
        {
            var mockFactory = new Mock<ICareTeamRepositoryFactory>();
            var stubRequest = new GetCareTeamDataRequest { ContactId = "12345" };
            mockFactory.Setup(
                f => f.GetCareTeamRepository(It.IsAny<IDataDomainRequest>(), It.IsAny<RepositoryType>()))
                .Returns((ICareTeamRepository)null);

            var manager = new CareTeamDataManager(mockFactory.Object);
            var data = manager.GetCareTeam(stubRequest);
        }
        #endregion
    }
}
