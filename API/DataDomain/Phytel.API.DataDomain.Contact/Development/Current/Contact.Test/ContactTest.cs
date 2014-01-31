using System.Collections.Generic;
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
            GetContactDataRequest request = new GetContactDataRequest { PatientId = "52e26f53072ef7191c11d5e2" };

            ContactData response = ContactDataManager.GetContactByPatientId(request);

            Assert.IsTrue(response.ContactId == "52ebc816d433232150813e49");
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

        [TestMethod]
        public void UpdateContact_Test()
        {
            List<int> weekDays = new List<int>();
            List<string> timesOfday = new List<string>();
            List<LanguageData> language = new List<LanguageData>();
            List<CommModeData> modes = new List<CommModeData>();
            List<PhoneData> phones = new List<PhoneData>();
            List<EmailData> emails = new List<EmailData>();
            List<AddressData> addresses = new List<AddressData>();

            //phones.Add(new PhoneData { Id = "-1", IsText = false, Number = "1111111111", OptOut = false, PhonePreferred = true, TextPreferred = false, TypeId = "52e18c2ed433232028e9e3a6" });

            //emails.Add(new EmailData {  Id = "-1", Text = "snehal@phytel.com", TypeId = "52e18c2ed433232028e9e3a6", OptOut = false, Preferred = true});

            //addresses.Add(new AddressData { Id = "-2", Line1 = "line1", Line2 = "line2", Line3 ="", City ="dallas", PostalCode ="", StateId ="52e195b8d433232028e9e3e4", TypeId ="52e18c2ed433232028e9e3a6", OptOut = false,   Preferred =  true});
            
            //weekDays = new List<int>();
            //weekDays.Add(6);

            //modes.Add(new CommModeData { ModeId = "52e17cc2d433232028e9e38f", OptOut = false, Preferred = false });
            //modes.Add(new CommModeData { ModeId = "52e17ce6d433232028e9e390", OptOut = false, Preferred = false });

            //timesOfday = new List<string>();
            //timesOfday.Add("52e17de8d433232028e9e394");
            //timesOfday.Add("52e17dedd433232028e9e395");
            
            
            PutUpdateContactDataRequest request = new PutUpdateContactDataRequest {
                ContactId = "52ebde65d6a4850b78c87a48",
                UserId = "testmethod",
                TimeZoneId = "52e1815dd433232028e9e399",
                //Languages = language,
                //WeekDays = weekDays,
                //TimesOfDaysId = timesOfday,
                //Modes = modes
                Phones = phones,
                Emails = emails,
                Addresses = addresses
            };

            PutUpdateContactDataResponse response = ContactDataManager.UpdateContact(request);

            Assert.IsNotNull(response);
        }
    }
}
