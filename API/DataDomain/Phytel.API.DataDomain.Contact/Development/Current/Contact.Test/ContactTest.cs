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
            GetContactDataRequest request = new GetContactDataRequest { PatientId = "52e26f63072ef7191c11f7e6" };

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

        [TestMethod]
        public void UpdateContact_Test()
        {
            List<int> weekDays = new List<int>();
            List<string> timesOfday = new List<string>();
            List<LanguageData> language = new List<LanguageData>();
            List<CommModeData> modes = new List<CommModeData>();

            //weekDays = new List<int>();
            weekDays.Add(6);

            modes.Add(new CommModeData { ModeId = "52e17cc2d433232028e9e38f", OptOut = false, Preferred = false });
            modes.Add(new CommModeData { ModeId = "52e17ce6d433232028e9e390", OptOut = false, Preferred = false });

            //timesOfday = new List<string>();
            //timesOfday.Add("52e17de8d433232028e9e394");
            //timesOfday.Add("52e17dedd433232028e9e395");
            
            
            PutUpdateContactDataRequest request = new PutUpdateContactDataRequest {
                ContactId = "52e971c5d433231c304e8609",
                UserId = "testmethod",
                TimeZoneId = "52e1815dd433232028e9e399",
                Languages = language,
                WeekDays = weekDays,
                TimesOfDaysId = timesOfday,
                Modes = modes
            };

            PutUpdateContactDataResponse response = ContactDataManager.UpdateContact(request);

            Assert.IsNotNull(response);
        }
    }
}
