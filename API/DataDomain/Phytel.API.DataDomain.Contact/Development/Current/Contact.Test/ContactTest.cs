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
        public void SearchContacts_Test()
        {
            SearchContactsDataRequest request = new SearchContactsDataRequest();

            request.ContractNumber = "InHealth001";
            request.UserId = "DD_TestHarness";
            request.Version = "v1";
            List<string> ids = new List<string>();
            ids.Add("52f012acd433231e1cfbc974");
            ids.Add("52f012b7d433231e1cfbc975");
           // request.ContactIds = ids;

            SearchContactsDataResponse response = ContactDataManager.SearchContacts(request);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void UpdateContact_Test()
        {
            //List<int> weekDays = new List<int>();
            //List<string> timesOfday = new List<string>();
            //List<LanguageData> language = new List<LanguageData>();
            //List<CommModeData> modes = new List<CommModeData>();
            //List<PhoneData> phones = new List<PhoneData>();
            //List<EmailData> emails = new List<EmailData>();
            //List<AddressData> addresses = new List<AddressData>();

        //    phones.Add(new PhoneData { Id = "52ebf4b6d433230b0cf8780e", IsText = false, Number = "1111111111", OptOut = false, PhonePreferred = true, TextPreferred = false, TypeId = "52e18c2ed433232028e9e3a6" });
          //  phones.Add(new PhoneData { Id = "-1", IsText = false, Number = "222222222", OptOut = false, PhonePreferred = true, TextPreferred = false, TypeId = "52e18c32d433232028e9e3a7" });

         //   emails.Add(new EmailData { Id = "52ebf4bad433230b0cf87810", Text = "snehal@phytel.com", TypeId = "52e18c2ed433232028e9e3a6", OptOut = false, Preferred = true });
            //emails.Add(new EmailData { Id = "-1", Text = "ingavale@phytel.com", TypeId = "52e18c32d433232028e9e3a7", OptOut = false, Preferred = true });

          //  addresses.Add(new AddressData { Id = "-52ebf4c0d433230b0cf87812", Line1 = "line1", Line2 = "line2", Line3 = "", City = "dallas", PostalCode = "", StateId = "52e195b8d433232028e9e3e4", TypeId = "52e18c2ed433232028e9e3a6", OptOut = false, Preferred = true });
            //addresses.Add(new AddressData { Id = "-2", Line1 = "some lane", Line2 = "some block", Line3 = "some", City = "austin", PostalCode = "", StateId = "52e195b8d433232028e9e3e4", TypeId = "52e18c32d433232028e9e3a7", OptOut = false, Preferred = true });
            
            //weekDays = new List<int>();
            //weekDays.Add(6);

            //modes.Add(new CommModeData { ModeId = "52e17cc2d433232028e9e38f", OptOut = false, Preferred = false });
            //modes.Add(new CommModeData { ModeId = "52e17ce6d433232028e9e390", OptOut = false, Preferred = false });

            //timesOfday = new List<string>();
            //timesOfday.Add("52e17de8d433232028e9e394");
            //timesOfday.Add("52e17dedd433232028e9e395");

            PutUpdateContactDataRequest request = new PutUpdateContactDataRequest();

            request.ContractNumber = "InHealth001";
            request.UserId = "DD_TestHarness";
            request.Version = "v1";
            
            //List<CommModeData> modes = new List<CommModeData>();
            //modes.Add(new CommModeData { ModeId = "52e17cc2d433232028e9e38f", OptOut = false, Preferred = false });
            //modes.Add(new CommModeData { ModeId = "52e17ce6d433232028e9e390", OptOut = true, Preferred = false });
            //modes.Add(new CommModeData { ModeId = "52e17d08d433232028e9e391", OptOut = false, Preferred = true });
            //modes.Add(new CommModeData { ModeId = "52e17d10d433232028e9e392", OptOut = false, Preferred = false });
            //request.Modes = modes;

            //List<AddressData> addresses = new List<AddressData>();
            //addresses.Add(new AddressData { Id = "52e75858d43323149870c22a", Line1 = "phytel", Line2 = "11511 luna road", Line3 = "suite 600", City = "Dallas", PostalCode = "75234", StateId = "52e195b8d433232028e9e3e4", Preferred = false, OptOut = false, TypeId = "52e18c45d433232028e9e3ab" });
            //request.Addresses = addresses;

            List<PhoneData> phones = new List<PhoneData>();
            //phones.Add(new PhoneData { Id = "52e7583dd43323149870c225", IsText = false, Number = "2142142147", OptOut = false, PhonePreferred = true, TextPreferred = false, TypeId = "52e18c2ed433232028e9e3a6" });
            //phones.Add(new PhoneData { Id = "52e75847d43323149870c226", IsText = true, Number = "8178178179", OptOut = false, PhonePreferred = false, TextPreferred = true, TypeId = "52e18c38d433232028e9e3a8" });
            phones.Add(new PhoneData { Id = "52ec1f53d433232330c8bb7c", IsText = false, Number = 8179035768, OptOut = false, PhonePreferred = false, TextPreferred = false, TypeId = "52e18c32d433232028e9e3a7" });
            request.Phones = phones;

            //List<EmailData> emails = new List<EmailData>();
            //emails.Add(new EmailData { Id = "52e75852d43323149870c228", OptOut = false, Preferred = true, TypeId = "52e18c32d433232028e9e3a7", Text = "test1@gmail.com" });
            //emails.Add(new EmailData { Id = "52e75855d43323149870c229", OptOut = false, Preferred = false, TypeId = "52e18c41d433232028e9e3aa", Text = "test2@gmail.com" });
            //request.Emails = emails;

            //List<LanguageData> languages = new List<LanguageData>();
            //languages.Add(new LanguageData { LookUpLanguageId = "52e199dfd433232028e9e3f3", Preferred = true });
            //languages.Add(new LanguageData { LookUpLanguageId = "52e199d5d433232028e9e3f2", Preferred = false });
            //languages.Add(new LanguageData { LookUpLanguageId = "52e199d1d433232028e9e3f1", Preferred = false });
            //languages.Add(new LanguageData { LookUpLanguageId = "52e199cdd433232028e9e3f0", Preferred = false });
            //request.Languages = languages;


            //List<string> times = new List<string>();
            //times.Add("52e17de8d433232028e9e394");
            //times.Add("52e17dedd433232028e9e395");
            //request.TimesOfDaysId = times;

            List<int> days = new List<int>();
            days.Add(1);
            days.Add(2);
            days.Add(3);
            days.Add(4);
            days.Add(0);
            days.Add(5);
            request.WeekDays = days;

            //request.pat = "52e26f5b072ef7191c11e0b6";
            request.ContactId = "52ec1b77d6a4850b78581986";
            request.TimeZoneId = "52e1817ad433232028e9e39d";

            PutUpdateContactDataResponse response = ContactDataManager.UpdateContact(request);

            Assert.IsNotNull(response);
        }
    }
}
