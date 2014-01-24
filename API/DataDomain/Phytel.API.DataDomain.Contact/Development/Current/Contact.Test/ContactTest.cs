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
            GetContactDataRequest request = new GetContactDataRequest{ PatientId = "5"};

            GetContactDataResponse response = ContactDataManager.GetContactByPatientId(request);

            Assert.IsTrue(response.Contact.ContactId == "Tony");
        }
    }
}
