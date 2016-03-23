using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Phytel.API.DataDomain.Contact.ContactTypeLookUp;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Contact.MongoDB.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact.Test
{
    [TestClass]
    public class ContactTypeLookUpManagerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ContactTypeLookUpManager_Null_Factory_Should_Throw()
        {

            var dataManager = new ContactTypeLookUpManager(null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ContactTypeLookUpManager_Null_Repository_Should_Throw()
        {
            var mockFactory = new Mock<IContactTypeLookUpRepositoryFactory>();
            

            mockFactory.Setup(
                f => f.GetContactTypeLookUpRepository(It.IsAny<IDataDomainRequest>(), It.IsAny<RepositoryType>()))
                .Returns((IContactTypeLookUpRepository)null);
           
            var dataManager = new ContactTypeLookUpManager(mockFactory.Object);
            var data = dataManager.GetContactTypeLookUps(It.IsAny<GetContactTypeLookUpDataRequest>());

        }

        [TestMethod]
        public void ContactTypeLookUpManager_Empty_Data_Should_Return_Zero()
        {
            var mockFactory = new Mock<IContactTypeLookUpRepositoryFactory>();
            var mockRepository = new Mock<IContactTypeLookUpRepository>();

            mockRepository.Setup(r => r.GetContactTypeLookUps(It.IsAny<GroupType>()))
                .Returns(new List<MEContactTypeLookup>() {});

            var stubRequest = new Mock<GetContactTypeLookUpDataRequest>();

            mockFactory.Setup(
                f => f.GetContactTypeLookUpRepository(It.IsAny<IDataDomainRequest>(), It.IsAny<RepositoryType>()))
                .Returns(mockRepository.Object);

            var dataManager = new ContactTypeLookUpManager(mockFactory.Object);
            var data = dataManager.GetContactTypeLookUps(stubRequest.Object);

            Assert.AreEqual(data.ContactTypeLookUps.Count,0);
            Assert.IsNotNull(data);

        }
    }
}
 