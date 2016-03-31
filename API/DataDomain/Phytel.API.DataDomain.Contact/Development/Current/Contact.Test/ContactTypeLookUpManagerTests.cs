using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
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
        public void ContactTypeLookUpManager_GetContactTypeLookUps_Null_Factory_Should_Throw()
        {

            var dataManager = new ContactTypeLookUpManager(null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ContactTypeLookUpManager_GetContactTypeLookUps_Null_Repository_Should_Throw()
        {
            var mockFactory = new Mock<IContactTypeLookUpRepositoryFactory>();
            

            mockFactory.Setup(
                f => f.GetContactTypeLookUpRepository(It.IsAny<IDataDomainRequest>(), It.IsAny<RepositoryType>()))
                .Returns((IContactTypeLookUpRepository)null);
           
            var dataManager = new ContactTypeLookUpManager(mockFactory.Object);
            var data = dataManager.GetContactTypeLookUps(It.IsAny<GetContactTypeLookUpDataRequest>());

        }

        [TestMethod]
        public void ContactTypeLookUpManager_GetContactTypeLookUps_Empty_Data_Should_Return_Zero()
        {
            var mockFactory = new Mock<IContactTypeLookUpRepositoryFactory>();
            var mockRepository = new Mock<IContactTypeLookUpRepository>();

            mockRepository.Setup(r => r.GetContactTypeLookUps(It.IsAny<ContactLookUpGroupType>()))
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

        [TestMethod]
        public void ContactTypeLookUpManager_GetContactTypeLookUps_NoChildren_Success()
        {
            var mockFactory = new Mock<IContactTypeLookUpRepositoryFactory>();
            var mockRepository = new Mock<IContactTypeLookUpRepository>();

            var lookUpData = new List<MEContactTypeLookup>
            {
                new MEContactTypeLookup(ObjectId.Empty.ToString(),DateTime.UtcNow)
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "Parent",
                    Role = "Parent",
                    ParentId = ObjectId.Empty
                },
                new MEContactTypeLookup(ObjectId.Empty.ToString(),DateTime.UtcNow)
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "Parent1",
                    Role = "Parent1",
                    ParentId = ObjectId.Empty
                }

            };

            mockRepository.Setup(r => r.GetContactTypeLookUps(It.IsAny<ContactLookUpGroupType>()))
                .Returns(lookUpData);

            var stubRequest = new Mock<GetContactTypeLookUpDataRequest>();

            mockFactory.Setup(
                f => f.GetContactTypeLookUpRepository(It.IsAny<IDataDomainRequest>(), It.IsAny<RepositoryType>()))
                .Returns(mockRepository.Object);

            var dataManager = new ContactTypeLookUpManager(mockFactory.Object);
            var data = dataManager.GetContactTypeLookUps(stubRequest.Object);

            Assert.AreEqual(data.ContactTypeLookUps.Count, 2);
            Assert.IsNotNull(data);

        }

        [TestMethod]
        public void ContactTypeLookUpManager_GetContactTypeLookUps_HasChildren_Success()
        {
            var mockFactory = new Mock<IContactTypeLookUpRepositoryFactory>();
            var mockRepository = new Mock<IContactTypeLookUpRepository>();

            var lookUpData = new List<MEContactTypeLookup>
            {
                new MEContactTypeLookup(ObjectId.Empty.ToString(),DateTime.UtcNow)
                {
                    Id = ObjectId.Parse("56f16991078e10eb86038512"),
                    Name = "Parent",
                    Role = "Parent",
                    ParentId = ObjectId.Empty
                },
                new MEContactTypeLookup(ObjectId.Empty.ToString(),DateTime.UtcNow)
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "Parent1",
                    Role = "Parent1",
                    ParentId = ObjectId.Empty
                },
                new MEContactTypeLookup(ObjectId.Empty.ToString(),DateTime.UtcNow)
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "Child",
                    Role = "Child",
                    ParentId = ObjectId.Parse("56f16991078e10eb86038512")
                }

            };

            mockRepository.Setup(r => r.GetContactTypeLookUps(It.IsAny<ContactLookUpGroupType>()))
                .Returns(lookUpData);

            var stubRequest = new Mock<GetContactTypeLookUpDataRequest>();

            mockFactory.Setup(
                f => f.GetContactTypeLookUpRepository(It.IsAny<IDataDomainRequest>(), It.IsAny<RepositoryType>()))
                .Returns(mockRepository.Object);

            var dataManager = new ContactTypeLookUpManager(mockFactory.Object);
            var data = dataManager.GetContactTypeLookUps(stubRequest.Object);

            Assert.AreEqual(data.ContactTypeLookUps.Count, 2);
            Assert.AreEqual(data.ContactTypeLookUps.FirstOrDefault(c => c.Id == "56f16991078e10eb86038512").Children.Count, 1);
            Assert.IsNotNull(data);

        }

        [TestMethod]
        public void ContactTypeLookUpManager_GetContactTypeLookUps__Flatten_HasChildren_Success()
        {
            var mockFactory = new Mock<IContactTypeLookUpRepositoryFactory>();
            var mockRepository = new Mock<IContactTypeLookUpRepository>();

            var lookUpData = new List<MEContactTypeLookup>
            {
                new MEContactTypeLookup(ObjectId.Empty.ToString(),DateTime.UtcNow)
                {
                    Id = ObjectId.Parse("56f16991078e10eb86038512"),
                    Name = "Parent",
                    Role = "Parent",
                    ParentId = ObjectId.Empty
                },
                new MEContactTypeLookup(ObjectId.Empty.ToString(),DateTime.UtcNow)
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "Parent1",
                    Role = "Parent1",
                    ParentId = ObjectId.Empty
                },
                new MEContactTypeLookup(ObjectId.Empty.ToString(),DateTime.UtcNow)
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = "Child",
                    Role = "Child",
                    ParentId = ObjectId.Parse("56f16991078e10eb86038512")
                }

            };
            mockRepository.Setup(r => r.GetContactTypeLookUps(It.IsAny<ContactLookUpGroupType>()))
                .Returns(lookUpData);

            var stubRequest = new GetContactTypeLookUpDataRequest
            {
                FlattenData = true
            };
            
            mockFactory.Setup(
                f => f.GetContactTypeLookUpRepository(It.IsAny<IDataDomainRequest>(), It.IsAny<RepositoryType>()))
                .Returns(mockRepository.Object);

            var dataManager = new ContactTypeLookUpManager(mockFactory.Object);
            var data = dataManager.GetContactTypeLookUps(stubRequest);

            Assert.AreEqual(data.ContactTypeLookUps.Count, 3);
            Assert.AreEqual(data.ContactTypeLookUps.FirstOrDefault(c => c.Id == "56f16991078e10eb86038512").Children.Count, 0);
            Assert.IsNotNull(data);

        }
    }
}
 