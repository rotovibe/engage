using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Phytel.API.AppDomain.NG.Command;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Contact.DTO;

namespace Phytel.API.AppDomain.NG.Test
{
     [TestClass]
    public class DereferencePatientFromContactCommandTests
    {
         [TestMethod]
         [ExpectedException(typeof(ArgumentNullException))]
         public void DereferencePatientInContactCommand_Null_Contact_Should_Throw()
         {
             var command = new DereferencePatientInContactCommand(null, new PostDeletePatientRequest(), new ContactEndpointUtil());
             command.Execute();
         }

         [TestMethod]
         [ExpectedException(typeof(ArgumentNullException))]
         public void DereferencePatientInContactCommand_Null_Request_Should_Throw()
         {
             var command = new DereferencePatientInContactCommand("someId", null, new ContactEndpointUtil());
             command.Execute();
         }

         [TestMethod]
         [ExpectedException(typeof(ArgumentNullException))]
         public void DereferencePatientInContactCommand_Null_ContactEndPointUtil_Should_Throw()
         {
             var command = new DereferencePatientInContactCommand("someId", new PostDeletePatientRequest(),null);
             command.Execute();
         }

         [TestMethod]
         public void DereferencePatientInContactCommand_Execute_Success()
         {
             var mockContactEndPointUtil = new Mock<IContactEndpointUtil>();
             mockContactEndPointUtil.Setup(
                 mcep =>
                     mcep.DereferencePatientInContact(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<string>(),
                         It.IsAny<string>())).Returns(new DereferencePatientDataResponse());


             var command = new DereferencePatientInContactCommand("someId", new PostDeletePatientRequest(), mockContactEndPointUtil.Object);
             command.Execute();

             mockContactEndPointUtil.Verify(c => c.DereferencePatientInContact(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<string>(),
                         It.IsAny<string>()),Times.Once);
         }
    }
}
