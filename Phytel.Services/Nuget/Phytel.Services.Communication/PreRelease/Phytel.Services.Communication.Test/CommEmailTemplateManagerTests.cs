using NUnit.Framework;
using Phytel.Services.Communication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NUnit.Framework;
using Moq;

namespace Phytel.Services.Communication.Test
{
    [TestFixture]
    [Category("CommEmailTemplateManager")]
    public class CommEmailTemplateManagerTests
    {
        private ITemplateUtilities _templateUtilities;
        private ICommEmailTemplateManager _manager;
        private EmailActivityDetail _emailDetail = new EmailActivityDetail();
        private List<ActivityMedia> _medias = new List<ActivityMedia>();
        private Hashtable _missingObjects = new Hashtable();
        private XmlDocument _xDoc = new XmlDocument();
        private string _expectedEmailTemplateXML;
        private XmlDocument _xDocExpected = new XmlDocument();
        private TemplateResults expectedResults = new TemplateResults();


        [SetUp]
        public void SetUp()
        {
            _templateUtilities = new TemplateUtilities();
            SetUpData();
            _manager = new CommEmailTemplateManager(_templateUtilities);
        }
        
        [Test]
        public void TestSpecificAppointmentMsg()
        {
            Assert.DoesNotThrow(new TestDelegate(SpecificAppointmentMsgTestDelegate));

            ContractPermission desired = new ContractPermission(){ChildObjectID = (int)Prompts.AppointmentSpecificMessage,RoleID = 1};
            ContractPermission undesired1 = new ContractPermission() { ChildObjectID = 1, RoleID = 1 };
            ContractPermission undesired2 = new ContractPermission() { ChildObjectID = 2, RoleID = 2 };

            List<ContractPermission> permissions = new List<ContractPermission>();
            permissions.Add(desired);
            permissions.Add(undesired1);
            permissions.Add(undesired2);

            Assert.IsTrue(_manager.IsAppointmentSpecificMsgEnabled(permissions, 1), "Expected true");
            Assert.IsFalse(_manager.IsAppointmentSpecificMsgEnabled(permissions, 2), "Expected false");

        }

        [Test]
        public void TestBuildHeader()
        {
            _expectedEmailTemplateXML = "<Email><SendID>3</SendID><ActivityID>10</ActivityID><ContractID>200</ContractID>" +
                    "<ConfirmationURL IsCDATA=\"true\" Enable=\"true\">yes.com</ConfirmationURL><OptOutURL IsCDATA=\"true\" Enable=\"true\" >no.com</OptOutURL>" +
                    "<ImagePath IsCDATA=\"true\" Enable=\"true\">https://healthreminder.phytel.com/Email/Global/COMMON/EN/</ImagePath>" +
                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><FirstName /><LastName /></Schedule>" +
                    "<Facility><FacilityID /><FacilityLogo Enable=\"true\" /><FacilityURL Enable=\"true\" /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><Type /><DayOfWeek /><Month /><Date /><Year /><Time /><Duration /><AppointmentSpecificMessage Enable=\"true\" /><FromEmailAddress />" +
                    "<ReplyToEmailAddress /><ToEmailAddress /><DisplayName /><Subject /><Body /></Message></Email>";

            _xDocExpected.LoadXml(_expectedEmailTemplateXML);
            expectedResults.PopulatedTemplate = _xDocExpected;
            
            TemplateResults results = _manager.BuildHeader(_xDoc, _emailDetail, _missingObjects, "", "");

            Assert.AreEqual(expectedResults.PopulatedTemplate, results.PopulatedTemplate);
            Assert.AreEqual(expectedResults.MissingObjects, results.MissingObjects);
        }

        [Test]
        public void TestBuildPatient()
        {
            _expectedEmailTemplateXML = "<Email><SendID /><ActivityID /><ContractID />" +
                    "<ConfirmationURL IsCDATA=\"true\" Enable=\"true\" /><OptOutURL IsCDATA=\"true\" Enable=\"true\" />" +
                    "<ImagePath IsCDATA=\"true\" Enable=\"true\">https://healthreminder.phytel.com/Email/Global/COMMON/EN/</ImagePath>" +
                    "<Patient><PatientID>20</PatientID><FullName>what is this</FullName><FirstName>test</FirstName><LastName>last</LastName></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><FirstName /><LastName /></Schedule>" +
                    "<Facility><FacilityID /><FacilityLogo Enable=\"true\" /><FacilityURL Enable=\"true\" /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><Type /><DayOfWeek /><Month /><Date /><Year /><Time /><Duration /><AppointmentSpecificMessage Enable=\"true\" /><FromEmailAddress />" +
                    "<ReplyToEmailAddress /><ToEmailAddress /><DisplayName /><Subject /><Body /></Message></Email>";
            
            _xDocExpected.LoadXml(_expectedEmailTemplateXML);
            expectedResults.PopulatedTemplate = _xDocExpected;
            
            TemplateResults results = _manager.BuildPatient(_xDoc, _emailDetail, _missingObjects);

            Assert.AreEqual(expectedResults.PopulatedTemplate, results.PopulatedTemplate);
            Assert.AreEqual(expectedResults.MissingObjects, results.MissingObjects);
        }

        [Test]
        public void TestBuildSchedule()
        {
            _expectedEmailTemplateXML = "<Email><SendID /><ActivityID /><ContractID />" +
                    "<ConfirmationURL IsCDATA=\"true\" Enable=\"true\" /><OptOutURL IsCDATA=\"true\" Enable=\"true\" />" +
                    "<ImagePath IsCDATA=\"true\" Enable=\"true\">https://healthreminder.phytel.com/Email/Global/COMMON/EN/</ImagePath>" +
                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID>30</ScheduleID><FullName>Test Schedule</FullName><FirstName /><LastName /></Schedule>" +
                    "<Facility><FacilityID /><FacilityLogo Enable=\"true\" /><FacilityURL Enable=\"true\" /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><Type /><DayOfWeek /><Month /><Date /><Year /><Time /><Duration /><AppointmentSpecificMessage Enable=\"true\" /><FromEmailAddress />" +
                    "<ReplyToEmailAddress /><ToEmailAddress /><DisplayName /><Subject /><Body /></Message></Email>";

            _xDocExpected.LoadXml(_expectedEmailTemplateXML);
            expectedResults.PopulatedTemplate = _xDocExpected;

            TemplateResults results = _manager.BuildSchedule(_xDoc, _emailDetail, _medias, _missingObjects);

            Assert.AreEqual(expectedResults.PopulatedTemplate, results.PopulatedTemplate);
            Assert.AreEqual(expectedResults.MissingObjects, results.MissingObjects);
        }

        [Test]
        public void TestBuildFacility()
        {
            _expectedEmailTemplateXML = "<Email><SendID /><ActivityID /><ContractID />" +
                    "<ConfirmationURL IsCDATA=\"true\" Enable=\"true\" /><OptOutURL IsCDATA=\"true\" Enable=\"true\" />" +
                    "<ImagePath IsCDATA=\"true\" Enable=\"true\">https://healthreminder.phytel.com/Email/Global/COMMON/EN/</ImagePath>" +
                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><FirstName /><LastName /></Schedule>" +
                    "<Facility><FacilityID>100</FacilityID><FacilityLogo Enable=\"true\" >logo.jpg</FacilityLogo><FacilityURL Enable=\"true\" >facility.com</FacilityURL>" +
                    "<Name>narrative</Name><Addr1>facility address</Addr1><Addr2>facility address 2</Addr2><City>dallas</City><State>TX</State><Zip>77777</Zip><PhoneNumber>(214) 555-1212</PhoneNumber></Facility>" +
                    "<Message><Type /><DayOfWeek /><Month /><Date /><Year /><Time /><Duration /><AppointmentSpecificMessage Enable=\"true\" /><FromEmailAddress />" +
                    "<ReplyToEmailAddress /><ToEmailAddress /><DisplayName /><Subject /><Body /></Message></Email>";

            _xDocExpected.LoadXml(_expectedEmailTemplateXML);
            expectedResults.PopulatedTemplate = _xDocExpected;

            TemplateResults results = _manager.BuildFacility(_xDoc, _emailDetail, _medias, _missingObjects, "ACMOT");

            Assert.AreEqual(expectedResults.PopulatedTemplate, results.PopulatedTemplate);
            Assert.AreEqual(expectedResults.MissingObjects, results.MissingObjects);
        }

        private void SpecificAppointmentMsgTestDelegate()
        {
            List<ContractPermission> permissionRows = null;
            _manager.IsAppointmentSpecificMsgEnabled(permissionRows, 1);

            permissionRows = new List<ContractPermission>();
            _manager.IsAppointmentSpecificMsgEnabled(permissionRows, 1);
        }

        private void SetUpData()
        {

            # region XML Setup

            string originalEmailTemplateXML = "<Email><SendID /><ActivityID /><ContractID />" +
                    "<ConfirmationURL IsCDATA=\"true\" Enable=\"true\" /><OptOutURL IsCDATA=\"true\" Enable=\"true\" />" +
                    "<ImagePath IsCDATA=\"true\" Enable=\"true\">https://healthreminder.phytel.com/Email/Global/COMMON/EN/</ImagePath>" +
                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><FirstName /><LastName /></Schedule>" +
                    "<Facility><FacilityID /><FacilityLogo Enable=\"true\" /><FacilityURL Enable=\"true\" /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><Type /><DayOfWeek /><Month /><Date /><Year /><Time /><Duration /><AppointmentSpecificMessage Enable=\"true\" /><FromEmailAddress />" +
                    "<ReplyToEmailAddress /><ToEmailAddress /><DisplayName /><Subject /><Body /></Message></Email>";
            _xDoc.LoadXml(originalEmailTemplateXML);

            # endregion

            int facilityId = 100;
            int templateId = 1;
            int campaignId = 2;
            int sendId = 3;
            string contractNumber = "ABC001";
            int contractID = 200;
            int activityId = 10;
            int patientId = 20;
            string patientFirstName = "test";
            string patientLastName = "last";
            string patientNameLF = "what is this";
            string facilityAddr1 = "facility address";
            string facilityAddr2 = "facility address 2";
            string facilityCity = "dallas";
            string facilityState = "TX";
            string facilityZip = "77777";
            string facilityPhoneNumber = "2145551212";
            string appointmentDateTime = "1/1/2015";
            string toEmailAddress = "test@test.com";
            int scheduleID = 30;
            int appointmentDuration = 60;
            
            _emailDetail.TaskTypeCategory = TaskTypeCategory.OutreachRecall;
            _emailDetail.TemplateID = templateId;
            _emailDetail.CampaignID = campaignId; // If either are = 0, throws exception
            _emailDetail.FacilityID = facilityId;
            _emailDetail.SendID = sendId;
            _emailDetail.ContractNumber = contractNumber;
            _emailDetail.ContractID = contractID;
            _emailDetail.ActivityID = activityId;
            _emailDetail.PatientID = patientId;
            _emailDetail.PatientFirstName = patientFirstName;
            _emailDetail.PatientLastName = patientLastName;
            _emailDetail.PatientNameLF = patientNameLF;
            _emailDetail.FacilityID = facilityId;
            _emailDetail.FacilityAddrLine1 = facilityAddr1;
            _emailDetail.FacilityAddrLine2 = facilityAddr2;
            _emailDetail.FacilityCity = facilityCity;
            _emailDetail.FacilityState = facilityState;
            _emailDetail.FacilityZipCode = facilityZip;
            _emailDetail.ProviderACDNumber = facilityPhoneNumber;
            _emailDetail.ScheduleDateTime = appointmentDateTime;
            _emailDetail.ToEmailAddress = toEmailAddress;
            _emailDetail.RecipientSchedID = scheduleID;
            _emailDetail.ScheduleDuration = appointmentDuration;

            ActivityMedia facilityName = new ActivityMedia
            {
                OwnerID = facilityId,
                CategoryCode = "SNOVR",
                OwnerCode = "EMAIL",
                LanguagePreferenceCode = "EN",
                Narrative = "narrative"
            };

            ActivityMedia facilityLogo = new ActivityMedia
            {
                OwnerID = facilityId,
                CategoryCode = "LOGO",
                OwnerCode = "EMAIL",
                Narrative = "logo.jpg"
            };

            ActivityMedia facilityUrl = new ActivityMedia
            {
                OwnerID = facilityId,
                CategoryCode = "URL",
                OwnerCode = "EMAIL",
                Narrative = "facility.com"
            };

            ActivityMedia scheduleName = new ActivityMedia
            {
                OwnerID = scheduleID,
                CategoryCode = "SCOVR",
                OwnerCode = "EMAIL",
                LanguagePreferenceCode = "EN",
                Narrative = "Test Schedule"
            };

            ActivityMedia facilityEmailAddress = new ActivityMedia
            {
                OwnerID = facilityId,
                CategoryCode = "EMAIL",
                OwnerCode = "EMAIL",
                LanguagePreferenceCode = "EN",
                Narrative = "facility@test.com"
            };

            ActivityMedia facilityReplyAddress = new ActivityMedia
            {
                OwnerID = facilityId,
                CategoryCode = "REPLY",
                OwnerCode = "EMAIL",
                LanguagePreferenceCode = "EN",
                Narrative = "facilityreply@test.com"
            };

            _medias.Add(facilityName);
            _medias.Add(facilityEmailAddress);
            _medias.Add(facilityReplyAddress);
            _medias.Add(facilityLogo);
            _medias.Add(facilityUrl);
            _medias.Add(scheduleName);

            Hashtable missingObjectsExpected = new Hashtable();            
            expectedResults.MissingObjects = missingObjectsExpected;            
        }

    }
}
