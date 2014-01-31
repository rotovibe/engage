using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.Test
{
    [TestClass]
    public class NGManager_Tests
    {
        [TestMethod]
        public void GetPatientByID_Test()
        {
            //PatientResponse response = NGManager.GetPatientByID("1", "NG", "inHealth001");
            //Assert.IsTrue(response.LastName == "DiGiorgio");
        }

        #region PatientProblem
        [TestMethod]
        public void GetAllPatientProblems_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string token = "1234";
            NGManager ngManager = new NGManager();
            GetAllPatientProblemsRequest request = new GetAllPatientProblemsRequest
            {
                ContractNumber = contractNumber,
                Token = token,
                Version = version,
                PatientID = "528bdccc072ef7071c2e22ae"
            };
            // Act
            List<PatientProblem> response = ngManager.GetPatientProblems(request);

            //Assert
            Assert.IsTrue(response.Count > 0);
        }

        #endregion

        [TestMethod]
        public void GetAllCohorts_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string token = "1234";
            NGManager ngManager = new NGManager();
            GetAllCohortsRequest request = new GetAllCohortsRequest
            {
                ContractNumber = contractNumber,
                Token = token,
                Version = version
            };
            // Act
            List<Cohort> response = ngManager.GetCohorts(request);

            //Assert
            Assert.IsTrue(response.Count > 0);
        }

        [TestMethod]
        public void GetAllSettings_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string token = "52936c88d6a48509e8d30632";
            NGManager ngManager = new NGManager();
            GetAllSettingsRequest request = new GetAllSettingsRequest
            {
                ContractNumber = contractNumber,
                Token = token,
                Version = version
            };
            // Act
            GetAllSettingsResponse response = ngManager.GetAllSettings(request);

            //Assert
            Assert.IsTrue(response.Settings.Count > 0);
        }

        #region Contact

        [TestMethod]
        public void GetContactByPatientId_Test()
        {
            // Arrange
            string version = "v1";
            string contractNumber = "InHealth001";
            string token = "1234";
            NGManager ngManager = new NGManager();
            GetContactRequest request = new GetContactRequest
            {
                ContractNumber = contractNumber,
                Token = token,
                Version = version,
                PatientID = "528bdccc072ef7071c2e22ae"
            };
            // Act
            Contact response = ngManager.GetContactByPatientId(request);

            //Assert
            Assert.IsTrue(response != null);
        }

         [TestMethod]
        public void UpdatePatient_Test()
        {
            PutUpdateContactRequest request = new PutUpdateContactRequest();
            request.ContractNumber = "InHealth001";
            request.UserId = "AD_TestHarness"; 
            request.Version = "v1";
            Contact contact = new Contact();
            contact.Modes = new List<CommMode>();
            
            List<CommMode> modes = new List<CommMode>();
            modes.Add(new CommMode { LookUpModeId = "52e17cc2d433232028e9e38f", OptOut = false, Preferred = false } );
            modes.Add(new CommMode { LookUpModeId = "52e17ce6d433232028e9e390", OptOut = true, Preferred = false } );
            modes.Add(new CommMode { LookUpModeId = "52e17d08d433232028e9e391", OptOut = false, Preferred = true } );
            modes.Add(new CommMode { LookUpModeId = "52e17d10d433232028e9e392", OptOut = false, Preferred = false });
            contact.Modes = modes;

            List<Address> addresses = new List<Address>();
            addresses.Add(new Address { Id = "52e75858d43323149870c22a", Line1 = "phytel", Line2 = "11511 luna road", Line3 = "suite 600", City = "Dallas", PostalCode = "75234", StateId = "52e195b8d433232028e9e3e4", Preferred = false ,  OptOut = false, TypeId =  "52e18c45d433232028e9e3ab"});
            contact.Addresses = addresses;

            List<Phone> phones = new List<Phone>();
            phones.Add(new Phone { Id = "52e7583dd43323149870c225", IsText = false, Number = "2142142147", OptOut = false, PhonePreferred = true, TextPreferred = false, TypeId = "52e18c2ed433232028e9e3a6" });
            phones.Add(new Phone { Id = "52e75847d43323149870c226", IsText = true, Number = "8178178179", OptOut = false, PhonePreferred = false, TextPreferred = true, TypeId = "52e18c38d433232028e9e3a8" });
            phones.Add(new Phone { Id = "52e7584bd43323149870c227", IsText = false, Number = "9729729723", OptOut = false, PhonePreferred = false, TextPreferred = false, TypeId = "52e18c32d433232028e9e3a7" });
            contact.Phones = phones;

            List<Email> emails = new List<Email>();
            emails.Add(new Email { Id = "52e75852d43323149870c228", OptOut = false, Preferred = true, TypeId = "52e18c32d433232028e9e3a7", Text = "test1@gmail.com" });
            emails.Add(new Email { Id = "52e75855d43323149870c229", OptOut = false, Preferred = false, TypeId = "52e18c41d433232028e9e3aa", Text = "test2@gmail.com" });
            contact.Emails = emails;

            List<Language> languages = new List<Language>();
            languages.Add(new Language { LookUpLanguageId = "52e199dfd433232028e9e3f3", Preferred = true });
            languages.Add(new Language { LookUpLanguageId = "52e199d5d433232028e9e3f2", Preferred = false });
            languages.Add(new Language { LookUpLanguageId = "52e199d1d433232028e9e3f1", Preferred = false });
            languages.Add(new Language { LookUpLanguageId = "52e199cdd433232028e9e3f0", Preferred = false });
            contact.Languages = languages;

            List<string> times = new List<string>();
            times.Add("52e17de8d433232028e9e394");
            times.Add("52e17dedd433232028e9e395");
            contact.TimesOfDaysId = times;

            List<int> days = new List<int>();
            days.Add(1);
            days.Add(2);
            days.Add(3);
            days.Add(4);
            contact.WeekDays = days;

            contact.PatientId = "52e26f5b072ef7191c11e0b6";
            contact.Id = "52ebc816d433232150813e49";
            contact.TimeZoneId = "52e1817ad433232028e9e39d";
            request.Token = "52ebf7d3d6a4850b78619119";


            request.Contact = contact;
            NGManager ngManager = new NGManager();
            PutUpdateContactResponse response =  ngManager.PutUpdateContact(request);

            Assert.IsNotNull(response);
        }
        #endregion
    }
}
//{"Contact":{"Modes":[{"LookUpModeId":"52e17cc2d433232028e9e38f","OptOut":false,"Preferred":false},{"LookUpModeId":"52e17ce6d433232028e9e390","OptOut":true,"Preferred":false},{"LookUpModeId":"52e17d08d433232028e9e391","OptOut":false,"Preferred":true},{"LookUpModeId":"52e17d10d433232028e9e392","OptOut":false,"Preferred":false}],

//"Addresses":[{"Id":"52e75858d43323149870c22a","OptOut":false,"Preferred":true,"TypeId":"52e18c45d433232028e9e3ab","City":"Dallas","StateId":"52e195b8d433232028e9e3e4","PostalCode":"75234","Line1":"phytel","Line2":"11511 luna road","Line3":"suite 600"}],

//"Phones":[{"Id":"52e7583dd43323149870c225","IsText":false,"Number":"2142142147","OptOut":false,"PhonePreferred":true,"TextPreferred":false,"TypeId":"52e18c2ed433232028e9e3a6"},{"Id":"52e75847d43323149870c226","IsText":true,"Number":"8178178179","OptOut":false,"PhonePreferred":false,"TextPreferred":true,"TypeId":"52e18c38d433232028e9e3a8"},{"Id":"52e7584bd43323149870c227","IsText":false,"Number":"9729729723","OptOut":false,"PhonePreferred":false,"TextPreferred":false,"TypeId":"52e18c32d433232028e9e3a7"}],

//"Emails":[{"Id":"52e75852d43323149870c228","OptOut":false,"Preferred":true,"TypeId":"52e18c32d433232028e9e3a7","Text":"test1@gmail.com"},{"Id":"52e75855d43323149870c229","OptOut":false,"Preferred":false,"TypeId":"52e18c41d433232028e9e3aa","Text":"test2@gmail.com"}],

//"Languages":[{"LookUpLanguageId":"52e199dfd433232028e9e3f3","Preferred":true},{"LookUpLanguageId":"52e199d5d433232028e9e3f2","Preferred":false},{"LookUpLanguageId":"52e199d1d433232028e9e3f1","Preferred":false},{"LookUpLanguageId":"52e199cdd433232028e9e3f0","Preferred":false}],

//"TimesOfDaysId":["52e17de8d433232028e9e394","52e17dedd433232028e9e395"],
//"WeekDays":["1","2","3","4"],
//"UserId":1,
//"PatientId":"52e26f5b072ef7191c11e0b6",
//"Id":"52ebc816d433232150813e49",
//"TimeZoneId":"52e1817ad433232028e9e39d"},
//"Token":"52ebf7d3d6a4850b78619119"}