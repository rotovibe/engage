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

namespace Phytel.API.DataDomain.Contact.Tests
{
    [TestClass()]
    public class ContactDataManagerTests
    {
        [TestClass()]
        public class AddRecentPatient
        {
            [TestMethod()]
            [TestCategory("NIGHT-911")]
            [TestProperty("TFS", "10409")]
            [TestProperty("Layer", "DD.ContactDataManager")]
            public void Add_One_Patient_To_Contact_Success()
            {
                ContactDataManager cm = new ContactDataManager { Factory = new StubContactRepositoryFactory() };
                PutRecentPatientRequest request = new PutRecentPatientRequest
                {
                    PatientId = "111156789012345678901111",
                    ContactId = "123456789012345678901234",
                    UserId = "666656789012345678906666",
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    Version = 1.0
                };

                PutRecentPatientResponse response = cm.AddRecentPatient(request);
                bool result = response.SuccessData;
                Assert.IsTrue(result);
            }

            [TestMethod()]
            [TestCategory("NIGHT-911")]
            [TestProperty("TFS", "10409")]
            [TestProperty("Layer", "DD.ContactDataManager")]
            public void Add_One_Patient_To_Contact_DEV_Success()
            {
                ContactDataManager cm = new ContactDataManager { Factory = new StubContactRepositoryFactory() };
                PutRecentPatientRequest request = new PutRecentPatientRequest
                {
                    PatientId = "5325d9e7d6a4850adcbba4ad",
                    ContactId = "5325c81f072ef705080d347e",
                    UserId = "5325c81f072ef705080d347e",
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    Version = 1.0
                };

                PutRecentPatientResponse response = cm.AddRecentPatient(request);
                
                Assert.IsNotNull(response);
            }


            [TestMethod()]
            [TestProperty("Layer", "DD.ContactDataManager")]
            public void DeletePatient()
            {
                ContactDataManager cm = new ContactDataManager { Factory = new ContactRepositoryFactory() };
                DeleteContactByPatientIdDataRequest request = new DeleteContactByPatientIdDataRequest
                {
                    PatientId = "5325db70d6a4850adcbba946",
                    UserId = "5325c81f072ef705080d347e",
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    Version = 1.0
                };

                DeleteContactByPatientIdDataResponse response = cm.DeleteContactByPatientId(request);

                Assert.IsNotNull(response);
            }

            [TestMethod()]            
            public void UpdateCareTeamMember_Test()
            {
                CareTeamMemberData ctm = new CareTeamMemberData()
                {
                    ContactId = "5325c821072ef705080d3488",
                    Id = "570e957fee4785557c0bd569",
                    Notes = "Test123",
                    StatusId = 1,
                    Core = true,
                    RoleId = "56f169f8078e10eb86038514"

                };
                var request = new UpdateCareTeamMemberDataRequest()
                {
                    CareTeamMemberData = ctm,
                    ContactId = "5325db7bd6a4850adc047053",
                    UserId = "5325c81f072ef705080d347e",
                    CareTeamId = "570e957fee4785557c0bd56a",
                    Context = "NG",
                    ContractNumber = "InHealth001",
                    Version = 1.0
                };
                var fact = new CareTeamRepositoryFactory();
                CareTeamDataManager cm = new CareTeamDataManager (fact);

                var response = cm.UpdateCareTeamMember(request);

                Assert.IsTrue(response.Status.ErrorCode.IsEmpty());
            }

        }
    }

    [TestClass]
    public class ContactDataManagerUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataContactManager_GetContactByContactId_Null_Request_Should_Throw()
        {
            var mockFactory = new Mock<IContactRepositoryFactory>();


            mockFactory.Setup(
                f => f.GetRepository(It.IsAny<IDataDomainRequest>(), It.IsAny<RepositoryType>()))
                .Returns((IContactRepository)null);

            var dataManager = new ContactDataManager();
            dataManager.Factory = mockFactory.Object;
            var data = dataManager.GetContactByContactId(null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void DataContactManager_GetContactByContactId_Null_Repository_Should_Throw()
        {
            var mockFactory = new Mock<IContactRepositoryFactory>();

            var stubRequest = new Mock<GetContactByContactIdDataRequest>();
            mockFactory.Setup(
                f => f.GetRepository(It.IsAny<IDataDomainRequest>(), It.IsAny<RepositoryType>()))
                .Returns((IContactRepository)null);

            var dataManager = new ContactDataManager {Factory = mockFactory.Object};
            var data = dataManager.GetContactByContactId(stubRequest.Object);
        }

        [TestMethod]
        public void DataContactManager_GetContactByContactId_Success()
        {
            var mockFactory = new Mock<IContactRepositoryFactory>();
            var mockRepository = new Mock<IContactRepository>();
            var stubContactData = new Mock<ContactData>();

            mockRepository.Setup(mr => mr.FindByID(It.IsAny<string>())).Returns((object) stubContactData.Object);

            var stubRequest = new Mock<GetContactByContactIdDataRequest>();
            mockFactory.Setup(
                f => f.GetRepository(It.IsAny<IDataDomainRequest>(), It.IsAny<RepositoryType>()))
                .Returns(mockRepository.Object);

            var dataManager = new ContactDataManager { Factory = mockFactory.Object };
            var data = dataManager.GetContactByContactId(stubRequest.Object);
            
            Assert.IsNotNull(data);
            mockRepository.Verify(mr => mr.FindByID(It.IsAny<string>()), Times.Once);

        }
    }

    [TestClass]
    public class CareTeamDataManagerUnitTests
    {
        
    }
}
