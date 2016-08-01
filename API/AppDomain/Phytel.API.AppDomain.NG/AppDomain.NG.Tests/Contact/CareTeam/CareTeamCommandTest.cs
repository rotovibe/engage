using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.Test.Contact
{
    [TestClass]
    public class CareTeamCommandTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CareTeamCommand_Null_Request_Should_Throw()
        {
            var command = new CareTeamCommand(null, new ContactEndpointUtil(), "cid");
            command.Execute();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CareTeamCommand_Null_ContactId_Should_Throw()
        {
            var command = new CareTeamCommand(new PostDeletePatientRequest(), new ContactEndpointUtil(),null);
            command.Execute();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CareTeamCommand_Null_ContactEndpointUtil_Should_Throw()
        {
            var command = new CareTeamCommand(new PostDeletePatientRequest(), null, "cid");
            command.Execute();
        }
    }
}
