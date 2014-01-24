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
            GetContactDataRequest request = new GetContactDataRequest { PatientId = "52aa4a1fd433231384af23a8" };

            GetContactDataResponse response = ContactDataManager.GetContactByPatientId(request);

            Assert.IsTrue(response.Contact.ContactId == "52e2d2dbd4332304583ca35b");
        }
    }
}
