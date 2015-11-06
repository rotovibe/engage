using NUnit.Framework;
using Phytel.Services.Communication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Service.Communication.Test
{
    [TestFixture]
    [Category("CommEmailTemplateManager")]
    public class CommEmailTemplateManagerTests
    {
        private TemplateUtilities _templateUtilities;
        private CommEmailTemplateManager _manager;

        public CommEmailTemplateManagerTests()
        {
            _templateUtilities = new TemplateUtilities();
            _manager = new CommEmailTemplateManager(_templateUtilities);
        }
        
        [Test]
        public void TestSpecificAppointmentMsg()
        {
            Assert.DoesNotThrow(new TestDelegate(SpecificAppointmentMsgTestDelegate));

            ContractPermission desired = new ContractPermission(){ChildObjectID = (int)Prompts.AppointmentSpecificMessage,RoleID = 1};
            ContractPermission undesired1 = new ContractPermission() { ChildObjectID = 1, RoleID = 1 };
            ContractPermission undesired2 = new ContractPermission() { ChildObjectID = 2, RoleID = 2 };

            List<ContractPermission> permissions = new List<ContractPermission>();
            permissions.Add(desired);
            permissions.Add(undesired1);
            permissions.Add(undesired2);

            Assert.IsTrue(_manager.IsAppointmentSpecificMsgEnabled(permissions, 1), "Expected true");
            Assert.IsFalse(_manager.IsAppointmentSpecificMsgEnabled(permissions, 2), "Expected false");

        }

        private void SpecificAppointmentMsgTestDelegate()
        {
            List<ContractPermission> permissionRows = null;
            _manager.IsAppointmentSpecificMsgEnabled(permissionRows, 1);

            permissionRows = new List<ContractPermission>();
            _manager.IsAppointmentSpecificMsgEnabled(permissionRows, 1);
        }

    }
}
