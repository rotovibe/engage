using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Contact.DTO;

namespace Phytel.API.DataDomain.Contact.Test
{
    [TestClass]
    public class ContactTest
    {
        [TestMethod]
        public void GetContactByPatientId_Test()
        {
            GetContactDataRequest request = new GetContactDataRequest { PatientId = "52e26f5b072ef7191c11e0b6" };

            GetContactDataResponse response = ContactDataManager.GetContactByPatientId(request);

            Assert.IsTrue(response.Contact.ContactId == "52e74731d43323149870c213");
        }

        [TestMethod]
        public void GetContactByPatientId_Empty_Test()
        {
            GetContactDataRequest request = new GetContactDataRequest { PatientId = "52e26f53072ef7191c11d5e2" };

            GetContactDataResponse response = ContactDataManager.GetContactByPatientId(request);

            Assert.IsTrue(response.Contact.ContactId == "52e749e3d43323149870c214");
        }
    }
}
