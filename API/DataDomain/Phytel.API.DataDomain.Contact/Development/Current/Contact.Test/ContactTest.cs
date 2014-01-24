using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Contact.DTO;

namespace Phytel.API.DataDomain.Contact.Test
{
    [TestClass]
    public class ContactTest
    {
        [TestMethod]
        public void GetContactByID()
        {
            GetContactRequest request = new GetContactRequest{ ContactID = "5"};

            GetContactResponse response = ContactDataManager.GetContactByID(request);

            Assert.IsTrue(response.Contact.ContactID == "Tony");
        }
    }
}
