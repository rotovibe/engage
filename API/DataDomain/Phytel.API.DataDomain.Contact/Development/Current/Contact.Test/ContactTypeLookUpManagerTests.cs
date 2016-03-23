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

        
    }
}
 