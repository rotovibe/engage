using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Contact.DTO;

namespace Phytel.API.DataDomain.Contact.MongoDB.Repository.Tests
{
    [TestClass()]
    public class PhoneVisitorTests
    {
        [TestMethod()]
        public void GetContactPhonesTest()
        {
            ContactData cData = new ContactData { Phones = new List<PhoneData>
                    {
                        new PhoneData
                        {
                            Id = "123456789012345678901111",
                            Number = 9724445555,
                            PhonePreferred = true,
                            IsText = true,
                            DataSource = "Engage",
                            TextPreferred = false,
                            OptOut = false,
                            TypeId = "111111111111111111111112"
                        }
                    }};
            PutContactDataRequest req = new PutContactDataRequest
            {
                ContactData = cData
            };
            var contact = new MEContact("123456789012345678901234", null);

            PhoneVisitor.GetContactPhones(cData.Phones, ref contact);

            Assert.IsNotNull(contact.Phones);
            Assert.AreEqual(contact.Phones[0].DataSource, "Engage");
        }
    }
}
