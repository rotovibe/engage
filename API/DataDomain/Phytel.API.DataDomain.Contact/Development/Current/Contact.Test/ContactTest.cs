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
            GetContactDataRequest request = new GetContactDataRequest { PatientId = "52e26f79072ef7191c122328" };

            ContactData response = ContactDataManager.GetContactByPatientId(request);

            Assert.IsTrue(response.ContactId == "52e7568bd43323149870c221");
        }

        [TestMethod]
        public void GetContactByPatientId_Minimal_Test()
        {
            GetContactDataRequest request = new GetContactDataRequest { PatientId = "52e26f53072ef7191c11d5e2" };

            ContactData response = ContactDataManager.GetContactByPatientId(request);

            Assert.IsTrue(response.ContactId == "52e749e3d43323149870c214");
        }

        [TestMethod]
        public void GetContactByPatientId_Empty_Test()
        {
            GetContactDataRequest request = new GetContactDataRequest { PatientId = "52e26f4b072ef7191c11b026" };

            ContactData response = ContactDataManager.GetContactByPatientId(request);

            Assert.IsNotNull(response.ContactId);
        }
    }
}
