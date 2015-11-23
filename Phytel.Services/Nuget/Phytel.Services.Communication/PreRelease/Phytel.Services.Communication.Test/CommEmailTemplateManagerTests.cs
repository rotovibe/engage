using NUnit.Framework;
using Phytel.Services.Communication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Moq;

namespace Phytel.Services.Communication.Test
{
    [TestFixture]
    [Category("CommEmailTemplateManager")]
    public class CommEmailTemplateManagerTests
    {
        #region Private Variables

        private ITemplateUtilities _templateUtilities;
        private ICommEmailTemplateManager _manager;
        private EmailActivityDetail _emailDetail = new EmailActivityDetail();
        private List<ActivityMedia> _medias = new List<ActivityMedia>();
        private Hashtable _missingObjects = new Hashtable();
        private XmlDocument _xDoc = new XmlDocument();
        private List<ContractPermission> _contractPermissionList;

        #endregion

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
            XmlDocument xDoc = new XmlDocument();
            xDoc = ResetXml(_xDoc);
            Hashtable missingObjects = new Hashtable();
            string expectedEmailTemplateXML;
            XmlDocument xDocExpected = new XmlDocument();
            TemplateResults expectedResults = new TemplateResults();

            expectedEmailTemplateXML = "<Email><SendID>3</SendID><ActivityID>10</ActivityID><ContractID>ABC001</ContractID>" +
                    "<ConfirmationURL IsCDATA=\"true\" Enable=\"true\"><![CDATA[yes.com?SendID=GTtixGtfqGkV1kuMvjwjXQ%3d%3d&ContractID=OkVhk6ATD34kP7Wp9Fibkw%3d%3d]]></ConfirmationURL>" +
                    "<OptOutURL IsCDATA=\"true\" Enable=\"true\" ><![CDATA[no.com?SendID=GTtixGtfqGkV1kuMvjwjXQ%3d%3d&ContractID=OkVhk6ATD34kP7Wp9Fibkw%3d%3d]]></OptOutURL>" +
                    "<ImagePath IsCDATA=\"true\" Enable=\"true\">https://healthreminder.phytel.com/Email/Global/COMMON/EN/</ImagePath>" +
                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><FirstName /><LastName /></Schedule>" +
                    "<Facility><FacilityID /><FacilityLogo Enable=\"true\" /><FacilityURL Enable=\"true\" /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><Type /><DayOfWeek /><Month /><Date /><Year /><Time /><Duration /><AppointmentSpecificMessage Enable=\"true\" /><FromEmailAddress />" +
                    "<ReplyToEmailAddress /><ToEmailAddress /><DisplayName /><Subject>Health Reminder from FACILITY_NAME</Subject><Body /></Message></Email>";

            xDocExpected.LoadXml(expectedEmailTemplateXML);
            expectedResults.PopulatedTemplate = xDocExpected;
            expectedResults.MissingObjects = missingObjects;  
            
            TemplateResults results = _manager.BuildHeader(xDoc, _emailDetail, missingObjects, "yes.com", "no.com");

            Assert.AreEqual(expectedResults.PopulatedTemplate, results.PopulatedTemplate);
            Assert.AreEqual(expectedResults.PopulatedTemplate.InnerXml.ToString(), results.PopulatedTemplate.InnerXml.ToString());
            Assert.AreEqual(expectedResults.MissingObjects, results.MissingObjects);
        }

        [Test]
        public void TestBuildPatient()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc = ResetXml(_xDoc);
            Hashtable missingObjects = new Hashtable();
            string expectedEmailTemplateXML;
            XmlDocument xDocExpected = new XmlDocument();
            TemplateResults expectedResults = new TemplateResults();

            expectedEmailTemplateXML = "<Email><SendID /><ActivityID /><ContractID />" +
                    "<ConfirmationURL IsCDATA=\"true\" Enable=\"true\" /><OptOutURL IsCDATA=\"true\" Enable=\"true\" />" +
                    "<ImagePath IsCDATA=\"true\" Enable=\"true\">https://healthreminder.phytel.com/Email/Global/COMMON/EN/</ImagePath>" +
                    "<Patient><PatientID>20</PatientID><FullName>What Is This</FullName><FirstName>Test</FirstName><LastName>Last</LastName></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><FirstName /><LastName /></Schedule>" +
                    "<Facility><FacilityID /><FacilityLogo Enable=\"true\" /><FacilityURL Enable=\"true\" /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><Type /><DayOfWeek /><Month /><Date /><Year /><Time /><Duration /><AppointmentSpecificMessage Enable=\"true\" /><FromEmailAddress />" +
                    "<ReplyToEmailAddress /><ToEmailAddress /><DisplayName /><Subject>Health Reminder from FACILITY_NAME</Subject><Body /></Message></Email>";
            
            xDocExpected.LoadXml(expectedEmailTemplateXML);
            expectedResults.PopulatedTemplate = xDocExpected;
            expectedResults.MissingObjects = missingObjects;  
            
            TemplateResults results = _manager.BuildPatient(xDoc, _emailDetail, missingObjects);

            Assert.AreEqual(expectedResults.PopulatedTemplate, results.PopulatedTemplate);
            Assert.AreEqual(expectedResults.PopulatedTemplate.InnerXml.ToString(), results.PopulatedTemplate.InnerXml.ToString());
            Assert.AreEqual(expectedResults.MissingObjects, results.MissingObjects);
        }

        [Test]
        public void TestBuildSchedule()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc = ResetXml(_xDoc);
            Hashtable missingObjects = new Hashtable();
            string expectedEmailTemplateXML;
            XmlDocument xDocExpected = new XmlDocument();
            TemplateResults expectedResults = new TemplateResults();

            expectedEmailTemplateXML = "<Email><SendID /><ActivityID /><ContractID />" +
                    "<ConfirmationURL IsCDATA=\"true\" Enable=\"true\" /><OptOutURL IsCDATA=\"true\" Enable=\"true\" />" +
                    "<ImagePath IsCDATA=\"true\" Enable=\"true\">https://healthreminder.phytel.com/Email/Global/COMMON/EN/</ImagePath>" +
                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID>30</ScheduleID><FullName>Test Schedule</FullName><FirstName /><LastName /></Schedule>" +
                    "<Facility><FacilityID /><FacilityLogo Enable=\"true\" /><FacilityURL Enable=\"true\" /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><Type /><DayOfWeek /><Month /><Date /><Year /><Time /><Duration /><AppointmentSpecificMessage Enable=\"true\" /><FromEmailAddress />" +
                    "<ReplyToEmailAddress /><ToEmailAddress /><DisplayName /><Subject>Health Reminder from FACILITY_NAME</Subject><Body /></Message></Email>";

            xDocExpected.LoadXml(expectedEmailTemplateXML);
            expectedResults.PopulatedTemplate = xDocExpected;
            expectedResults.MissingObjects = missingObjects;  

            TemplateResults results = _manager.BuildSchedule(xDoc, _emailDetail, _medias, missingObjects);

            Assert.AreEqual(expectedResults.PopulatedTemplate, results.PopulatedTemplate);
            Assert.AreEqual(expectedResults.PopulatedTemplate.InnerXml.ToString(), results.PopulatedTemplate.InnerXml.ToString());
            Assert.AreEqual(expectedResults.MissingObjects, results.MissingObjects);
        }

        [Test]
        public void TestBuildFacility()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc = ResetXml(_xDoc);
            Hashtable missingObjects = new Hashtable();
            string expectedEmailTemplateXML;
            XmlDocument xDocExpected = new XmlDocument();
            TemplateResults expectedResults = new TemplateResults();

            expectedEmailTemplateXML = "<Email><SendID /><ActivityID /><ContractID />" +
                    "<ConfirmationURL IsCDATA=\"true\" Enable=\"true\" /><OptOutURL IsCDATA=\"true\" Enable=\"true\" />" +
                    "<ImagePath IsCDATA=\"true\" Enable=\"true\">https://healthreminder.phytel.com/Email/Global/COMMON/EN/</ImagePath>" +
                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><FirstName /><LastName /></Schedule>" +
                    "<Facility><FacilityID>100</FacilityID><FacilityLogo Enable=\"true\" >logo.jpg</FacilityLogo><FacilityURL Enable=\"true\" >facility.com</FacilityURL>" +
                    "<Name>narrative</Name><Addr1>Facility Address</Addr1><Addr2>Facility Address 2</Addr2><City>Dallas</City><State>TX</State><Zip>77777</Zip><PhoneNumber>(214) 555-1212</PhoneNumber></Facility>" +
                    "<Message><Type /><DayOfWeek /><Month /><Date /><Year /><Time /><Duration /><AppointmentSpecificMessage Enable=\"true\" /><FromEmailAddress />" +
                    "<ReplyToEmailAddress /><ToEmailAddress /><DisplayName /><Subject>Health Reminder from FACILITY_NAME</Subject><Body /></Message></Email>";

            xDocExpected.LoadXml(expectedEmailTemplateXML);
            expectedResults.PopulatedTemplate = xDocExpected;
            expectedResults.MissingObjects = missingObjects;

            TemplateResults results = _manager.BuildFacility(xDoc, _emailDetail, _medias, missingObjects, "ACMOT");

            Assert.AreEqual(expectedResults.PopulatedTemplate, results.PopulatedTemplate);
            Assert.AreEqual(expectedResults.PopulatedTemplate.InnerXml.ToString(), results.PopulatedTemplate.InnerXml.ToString());
            Assert.AreEqual(expectedResults.MissingObjects, results.MissingObjects);
        }

        [Test]
        public void TestBuildEmail()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc = ResetXml(_xDoc);
            Hashtable missingObjects = new Hashtable();
            string expectedEmailTemplateXML;
            XmlDocument xDocExpected = new XmlDocument();
            TemplateResults expectedResults = new TemplateResults();

            expectedEmailTemplateXML = "<Email><SendID /><ActivityID /><ContractID />" +
                    "<ConfirmationURL IsCDATA=\"true\" Enable=\"true\" /><OptOutURL IsCDATA=\"true\" Enable=\"true\" />" +
                    "<ImagePath IsCDATA=\"true\" Enable=\"true\">https://healthreminder.phytel.com/Email/Global/COMMON/EN/</ImagePath>" +
                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><FirstName /><LastName /></Schedule>" +
                    "<Facility><FacilityID /><FacilityLogo Enable=\"true\" /><FacilityURL Enable=\"true\" /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><Type>Health Reminder </Type><DayOfWeek /><Month /><Date /><Year /><Time /><Duration /><AppointmentSpecificMessage Enable=\"true\" /><FromEmailAddress>facility@test.com</FromEmailAddress>" +
                    "<ReplyToEmailAddress>facilityreply@test.com</ReplyToEmailAddress><ToEmailAddress>test@test.com</ToEmailAddress><DisplayName>facility</DisplayName>" +
                    "<Subject>Health Reminder from </Subject><Body /></Message></Email>";

            xDocExpected.LoadXml(expectedEmailTemplateXML);
            expectedResults.PopulatedTemplate = xDocExpected;
            expectedResults.MissingObjects = missingObjects;

            TemplateResults results = _manager.BuildEmailMessage(xDoc, _emailDetail, _medias, missingObjects, 2); 

            Assert.AreEqual(expectedResults.PopulatedTemplate, results.PopulatedTemplate);
            Assert.AreEqual(expectedResults.PopulatedTemplate.InnerXml.ToString(), results.PopulatedTemplate.InnerXml.ToString());
            Assert.AreEqual(expectedResults.MissingObjects, results.MissingObjects);
        }

        [Test]
        public void TestBuildApptDateTime()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc = ResetXml(_xDoc);
            Hashtable missingObjects = new Hashtable();
            string expectedEmailTemplateXML;
            XmlDocument xDocExpected = new XmlDocument();
            TemplateResults expectedResults = new TemplateResults();

            expectedEmailTemplateXML = "<Email><SendID /><ActivityID /><ContractID />" +
                    "<ConfirmationURL IsCDATA=\"true\" Enable=\"true\" /><OptOutURL IsCDATA=\"true\" Enable=\"true\" />" +
                    "<ImagePath IsCDATA=\"true\" Enable=\"true\">https://healthreminder.phytel.com/Email/Global/COMMON/EN/</ImagePath>" +
                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><FirstName /><LastName /></Schedule>" +
                    "<Facility><FacilityID /><FacilityLogo Enable=\"true\" /><FacilityURL Enable=\"true\" /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><Type /><DayOfWeek>Friday</DayOfWeek><Month>November</Month><Date>20</Date><Year>2020</Year><Time>8:00 AM</Time><Duration>60</Duration><AppointmentSpecificMessage Enable=\"true\" /><FromEmailAddress />" +
                    "<ReplyToEmailAddress /><ToEmailAddress /><DisplayName /><Subject>Health Reminder from FACILITY_NAME</Subject><Body /></Message></Email>";

            xDocExpected.LoadXml(expectedEmailTemplateXML);
            expectedResults.PopulatedTemplate = xDocExpected;
            expectedResults.MissingObjects = missingObjects;

            TemplateResults results = _manager.BuildApptDateTime(xDoc, _emailDetail, missingObjects);

            Assert.AreEqual(expectedResults.PopulatedTemplate, results.PopulatedTemplate);
            Assert.AreEqual(expectedResults.PopulatedTemplate.InnerXml.ToString(), results.PopulatedTemplate.InnerXml.ToString());
            Assert.AreEqual(expectedResults.MissingObjects, results.MissingObjects);
        }

        [Test]
        public void TestBuildApptSpecificMessage()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc = ResetXml(_xDoc);
            Hashtable missingObjects = new Hashtable();
            string expectedEmailTemplateXML;
            XmlDocument xDocExpected = new XmlDocument();
            TemplateResults expectedResults = new TemplateResults();

            expectedEmailTemplateXML = "<Email><SendID /><ActivityID /><ContractID />" +
                    "<ConfirmationURL IsCDATA=\"true\" Enable=\"true\" /><OptOutURL IsCDATA=\"true\" Enable=\"true\" />" +
                    "<ImagePath IsCDATA=\"true\" Enable=\"true\">https://healthreminder.phytel.com/Email/Global/COMMON/EN/</ImagePath>" +
                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><FirstName /><LastName /></Schedule>" +
                    "<Facility><FacilityID /><FacilityLogo Enable=\"true\" /><FacilityURL Enable=\"true\" /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><Type /><DayOfWeek /><Month /><Date /><Year /><Time /><Duration />" +
                    "<AppointmentSpecificMessage Enable=\"true\" >This Is An Appointment Specific Message.</AppointmentSpecificMessage><FromEmailAddress />" +
                    "<ReplyToEmailAddress /><ToEmailAddress /><DisplayName /><Subject>Health Reminder from FACILITY_NAME</Subject><Body /></Message></Email>";

            xDocExpected.LoadXml(expectedEmailTemplateXML);
            expectedResults.PopulatedTemplate = xDocExpected;
            expectedResults.MissingObjects = missingObjects;

            TemplateResults results = _manager.BuildAppointmentSpecificMessage(xDoc, _emailDetail, _medias, missingObjects, 0, _contractPermissionList);

            Assert.AreEqual(expectedResults.PopulatedTemplate, results.PopulatedTemplate);
            Assert.AreEqual(expectedResults.PopulatedTemplate.InnerXml.ToString(), results.PopulatedTemplate.InnerXml.ToString());
            Assert.AreEqual(expectedResults.MissingObjects, results.MissingObjects);
        }

        [Test]
        public void TestBuildIntroPatient()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc = ResetXml(_xDoc);
            Hashtable missingObjects = new Hashtable();
            string expectedEmailTemplateXML;
            XmlDocument xDocExpected = new XmlDocument();
            TemplateResults expectedResults = new TemplateResults();

            expectedEmailTemplateXML = "<Email><SendID /><ActivityID /><ContractID />" +
                    "<ConfirmationURL IsCDATA=\"true\" Enable=\"true\" /><OptOutURL IsCDATA=\"true\" Enable=\"true\" />" +
                    "<ImagePath IsCDATA=\"true\" Enable=\"true\">https://healthreminder.phytel.com/Email/Global/COMMON/EN/</ImagePath>" +
                    "<Patient><PatientID>20</PatientID><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><FirstName /><LastName /></Schedule>" +
                    "<Facility><FacilityID /><FacilityLogo Enable=\"true\" /><FacilityURL Enable=\"true\" /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><Type /><DayOfWeek /><Month /><Date /><Year /><Time /><Duration /><AppointmentSpecificMessage Enable=\"true\" /><FromEmailAddress />" +
                    "<ReplyToEmailAddress /><ToEmailAddress /><DisplayName /><Subject>Health Reminder from FACILITY_NAME</Subject><Body /></Message></Email>";

            xDocExpected.LoadXml(expectedEmailTemplateXML);
            expectedResults.PopulatedTemplate = xDocExpected;
            expectedResults.MissingObjects = missingObjects;

            TemplateResults results = _manager.BuildIntroPatient(xDoc, _emailDetail, missingObjects);

            Assert.AreEqual(expectedResults.PopulatedTemplate, results.PopulatedTemplate);
            Assert.AreEqual(expectedResults.PopulatedTemplate.InnerXml.ToString(), results.PopulatedTemplate.InnerXml.ToString());
            Assert.AreEqual(expectedResults.MissingObjects, results.MissingObjects);
        }

        [Test]
        public void TestBuildIntroFacility()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc = ResetXml(_xDoc);
            Hashtable missingObjects = new Hashtable();
            string expectedEmailTemplateXML;
            XmlDocument xDocExpected = new XmlDocument();
            TemplateResults expectedResults = new TemplateResults();

            expectedEmailTemplateXML = "<Email><SendID /><ActivityID /><ContractID />" +
                    "<ConfirmationURL IsCDATA=\"true\" Enable=\"true\" /><OptOutURL IsCDATA=\"true\" Enable=\"true\" />" +
                    "<ImagePath IsCDATA=\"true\" Enable=\"true\">https://healthreminder.phytel.com/Email/Global/COMMON/EN/</ImagePath>" +
                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><FirstName /><LastName /></Schedule>" +
                    "<Facility><FacilityID>100</FacilityID><FacilityLogo Enable=\"true\" /><FacilityURL Enable=\"true\" />" +
                    "<Name>narrative</Name><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><Type /><DayOfWeek /><Month /><Date /><Year /><Time /><Duration /><AppointmentSpecificMessage Enable=\"true\" /><FromEmailAddress />" +
                    "<ReplyToEmailAddress /><ToEmailAddress /><DisplayName /><Subject>Health Reminder from FACILITY_NAME</Subject><Body /></Message></Email>";

            xDocExpected.LoadXml(expectedEmailTemplateXML);
            expectedResults.PopulatedTemplate = xDocExpected;
            expectedResults.MissingObjects = missingObjects;

            TemplateResults results = _manager.BuildIntroFacility(xDoc, _emailDetail, _medias, missingObjects);

            Assert.AreEqual(expectedResults.PopulatedTemplate, results.PopulatedTemplate);
            Assert.AreEqual(expectedResults.PopulatedTemplate.InnerXml.ToString(), results.PopulatedTemplate.InnerXml.ToString());
            Assert.AreEqual(expectedResults.MissingObjects, results.MissingObjects);
        }

        [Test]
        public void TestBuildIntroEmail()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc = ResetXml(_xDoc);
            Hashtable missingObjects = new Hashtable();
            string expectedEmailTemplateXML;
            XmlDocument xDocExpected = new XmlDocument();
            TemplateResults expectedResults = new TemplateResults();

            expectedEmailTemplateXML = "<Email><SendID /><ActivityID /><ContractID />" +
                    "<ConfirmationURL IsCDATA=\"true\" Enable=\"true\" /><OptOutURL IsCDATA=\"true\" Enable=\"true\" />" +
                    "<ImagePath IsCDATA=\"true\" Enable=\"true\">https://healthreminder.phytel.com/Email/Global/COMMON/EN/</ImagePath>" +
                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><FirstName /><LastName /></Schedule>" +
                    "<Facility><FacilityID /><FacilityLogo Enable=\"true\" /><FacilityURL Enable=\"true\" /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><Type>IntroductoryEmail</Type><DayOfWeek /><Month /><Date /><Year /><Time /><Duration /><AppointmentSpecificMessage Enable=\"true\" /><FromEmailAddress>facility@test.com</FromEmailAddress>" +
                    "<ReplyToEmailAddress>facilityreply@test.com</ReplyToEmailAddress><ToEmailAddress>test@test.com</ToEmailAddress><DisplayName>facility</DisplayName>" +
                    "<Subject>IntroductoryEmail</Subject><Body /></Message></Email>";

            xDocExpected.LoadXml(expectedEmailTemplateXML);
            expectedResults.PopulatedTemplate = xDocExpected;
            expectedResults.MissingObjects = missingObjects;

            TemplateResults results = _manager.BuildIntroEmailMessage(xDoc, _emailDetail, _medias, missingObjects, 2);

            Assert.AreEqual(expectedResults.PopulatedTemplate, results.PopulatedTemplate);
            Assert.AreEqual(expectedResults.PopulatedTemplate.InnerXml.ToString(), results.PopulatedTemplate.InnerXml.ToString());
            Assert.AreEqual(expectedResults.MissingObjects, results.MissingObjects);
        }

        private void SpecificAppointmentMsgTestDelegate()
        {
            List<ContractPermission> permissionRows = null;
            _manager.IsAppointmentSpecificMsgEnabled(permissionRows, 1);

            permissionRows = new List<ContractPermission>();
            _manager.IsAppointmentSpecificMsgEnabled(permissionRows, 1);
        }

        private XmlDocument ResetXml(XmlDocument xDoc)
        {
            XmlDocument result = new XmlDocument();

            string originalEmailTemplateXML = "<Email><SendID /><ActivityID /><ContractID />" +
                    "<ConfirmationURL IsCDATA=\"true\" Enable=\"true\" /><OptOutURL IsCDATA=\"true\" Enable=\"true\" />" +
                    "<ImagePath IsCDATA=\"true\" Enable=\"true\">https://healthreminder.phytel.com/Email/Global/COMMON/EN/</ImagePath>" +
                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><FirstName /><LastName /></Schedule>" +
                    "<Facility><FacilityID /><FacilityLogo Enable=\"true\" /><FacilityURL Enable=\"true\" /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><Type /><DayOfWeek /><Month /><Date /><Year /><Time /><Duration /><AppointmentSpecificMessage Enable=\"true\" /><FromEmailAddress />" +
                    "<ReplyToEmailAddress /><ToEmailAddress /><DisplayName /><Subject>Health Reminder from FACILITY_NAME</Subject><Body /></Message></Email>";
            result.LoadXml(originalEmailTemplateXML);

            return result;
        }

        private void SetUpData()
        {
            #region Setup EmailActivityDetail

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
            string scheduleDateTime = "11/20/2020 08:00:00 AM";
            
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
            _emailDetail.ScheduleDateTime = scheduleDateTime;

            #endregion

            #region Setup ActivityMedias

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
                Filename = "logo.jpg"
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
                Narrative = "facility@test.com"
            };

            ActivityMedia facilityReplyAddress = new ActivityMedia
            {
                OwnerID = facilityId,
                CategoryCode = "REPLY",
                OwnerCode = "EMAIL",
                Narrative = "facilityreply@test.com"
            };

            ActivityMedia facilityDisplayName = new ActivityMedia
            {
                OwnerID = facilityId,
                CategoryCode = "NAME",
                OwnerCode = "EMAIL",
                Narrative = "facility"
            };

            ActivityMedia apptSpecificMsg = new ActivityMedia
            {
                OwnerID = patientId,
                CategoryCode = "TTC",
                OwnerCode = "EMAIL",
                LanguagePreferenceCode = "EN",
                FacilityID = facilityId,
                Narrative = "This is an appointment specific message."
            };

            _medias.Add(facilityName);
            _medias.Add(facilityDisplayName);
            _medias.Add(facilityEmailAddress);
            _medias.Add(facilityReplyAddress);
            _medias.Add(facilityLogo);
            _medias.Add(facilityUrl);
            _medias.Add(scheduleName);
            _medias.Add(apptSpecificMsg);

            #endregion

            _contractPermissionList = new List<ContractPermission>();
            ContractPermission contractpermission = new ContractPermission
            {
                ChildObjectID = (int)Prompts.AppointmentSpecificMessage,
                RoleID = 0
            };
            _contractPermissionList.Add(contractpermission);
        }

    }
}
