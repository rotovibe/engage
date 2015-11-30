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
    [Category("CommTextTemplateManager")]
    public class CommTextTemplateManagerTests
    {
        #region Private Variables

        private ITemplateUtilities _templateUtilities;
        private ICommTextTemplateManager _manager;
        private TextActivityDetail _textDetail = new TextActivityDetail();
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
            _manager = new CommTextTemplateManager(_templateUtilities);            
        }

        [Test]
        public void TestBuildHeader()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc = ResetXml(_xDoc);
            Hashtable missingObjects = new Hashtable();
            string expectedTextTemplateXML;
            XmlDocument xDocExpected = new XmlDocument();
            TemplateResults expectedResults = new TemplateResults();

            expectedTextTemplateXML = "<TEXT><SendID>3</SendID><ActivityID>10</ActivityID><ContractID>ABC001</ContractID>" +
                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><DisplayName /></Schedule>" +
                    "<Facility><FacilityID /><DisplayName /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><DayOfWeek /><Month /><Date /><Year /><Time /><DateTime /><TextFromNumber /><TextToNumber /><TextHelpNumber /><Body /></Message></TEXT>";

            xDocExpected.LoadXml(expectedTextTemplateXML);
            expectedResults.PopulatedTemplate = xDocExpected;
            expectedResults.MissingObjects = missingObjects;  
            
            TemplateResults results = _manager.BuildHeader(xDoc, _textDetail, missingObjects);

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
            string expectedTextTemplateXML;
            XmlDocument xDocExpected = new XmlDocument();
            TemplateResults expectedResults = new TemplateResults();

            expectedTextTemplateXML = "<TEXT><SendID /><ActivityID /><ContractID />" +
                    "<Patient><PatientID>20</PatientID><FullName>What Is This</FullName><FirstName>Test</FirstName><LastName>Last</LastName></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><DisplayName /></Schedule>" +
                    "<Facility><FacilityID /><DisplayName /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><DayOfWeek /><Month /><Date /><Year /><Time /><DateTime /><TextFromNumber /><TextToNumber /><TextHelpNumber /><Body /></Message></TEXT>";

            xDocExpected.LoadXml(expectedTextTemplateXML);
            expectedResults.PopulatedTemplate = xDocExpected;
            expectedResults.MissingObjects = missingObjects;  
            
            TemplateResults results = _manager.BuildPatient(xDoc, _textDetail, missingObjects);

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
            string expectedTextTemplateXML;
            XmlDocument xDocExpected = new XmlDocument();
            TemplateResults expectedResults = new TemplateResults();

            expectedTextTemplateXML = "<TEXT><SendID /><ActivityID /><ContractID />" +
                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID>30</ScheduleID><FullName>Test Schedule 1</FullName><DisplayName>Test Schedule</DisplayName></Schedule>" +
                    "<Facility><FacilityID /><DisplayName /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><DayOfWeek /><Month /><Date /><Year /><Time /><DateTime /><TextFromNumber /><TextToNumber /><TextHelpNumber /><Body /></Message></TEXT>";

            xDocExpected.LoadXml(expectedTextTemplateXML);
            expectedResults.PopulatedTemplate = xDocExpected;
            expectedResults.MissingObjects = missingObjects;  

            TemplateResults results = _manager.BuildSchedule(xDoc, _textDetail, _medias, missingObjects);

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
            string expectedTextTemplateXML;
            XmlDocument xDocExpected = new XmlDocument();
            TemplateResults expectedResults = new TemplateResults();

            expectedTextTemplateXML = "<TEXT><SendID /><ActivityID /><ContractID />" +
                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><DisplayName /></Schedule>" +
                    "<Facility><FacilityID>100</FacilityID><DisplayName>narrative</DisplayName><Name>Test Facility</Name><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber>(214)555-1212</PhoneNumber></Facility>" +
                    "<Message><DayOfWeek /><Month /><Date /><Year /><Time /><DateTime /><TextFromNumber /><TextToNumber /><TextHelpNumber /><Body /></Message></TEXT>";

            xDocExpected.LoadXml(expectedTextTemplateXML);
            expectedResults.PopulatedTemplate = xDocExpected;
            expectedResults.MissingObjects = missingObjects;

            TemplateResults results = _manager.BuildFacility(xDoc, _textDetail, _medias, missingObjects);

            Assert.AreEqual(expectedResults.PopulatedTemplate, results.PopulatedTemplate);
            Assert.AreEqual(expectedResults.PopulatedTemplate.InnerXml.ToString(), results.PopulatedTemplate.InnerXml.ToString());
            Assert.AreEqual(expectedResults.MissingObjects, results.MissingObjects);
        }

        [Test]
        public void TestBuildText()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc = ResetXml(_xDoc);
            Hashtable missingObjects = new Hashtable();
            string expectedEmailTemplateXML;
            XmlDocument xDocExpected = new XmlDocument();
            TemplateResults expectedResults = new TemplateResults();

            expectedEmailTemplateXML = "<TEXT><SendID /><ActivityID /><ContractID />" +
                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><DisplayName /></Schedule>" +
                    "<Facility><FacilityID /><DisplayName /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><DayOfWeek /><Month /><Date /><Year /><Time /><DateTime />" +
                    "<TextFromNumber>8175551212</TextFromNumber><TextToNumber>4695551212</TextToNumber><TextHelpNumber>2145551212</TextHelpNumber><Body /></Message></TEXT>";

            xDocExpected.LoadXml(expectedEmailTemplateXML);
            expectedResults.PopulatedTemplate = xDocExpected;
            expectedResults.MissingObjects = missingObjects;

            TemplateResults results = _manager.BuildTextMessage(xDoc, _textDetail, missingObjects); 

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

            expectedEmailTemplateXML = "<TEXT><SendID /><ActivityID /><ContractID />" +
                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><DisplayName /></Schedule>" +
                    "<Facility><FacilityID /><DisplayName /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><DayOfWeek>Friday</DayOfWeek><Month>Nov.</Month><Date>20</Date><Year>2020</Year><Time>8:00 AM</Time><DateTime /><TextFromNumber /><TextToNumber /><TextHelpNumber /><Body /></Message></TEXT>";

            xDocExpected.LoadXml(expectedEmailTemplateXML);
            expectedResults.PopulatedTemplate = xDocExpected;
            expectedResults.MissingObjects = missingObjects;

            TemplateResults results = _manager.BuildApptDateTime(xDoc, _textDetail, missingObjects);

            Assert.AreEqual(expectedResults.PopulatedTemplate, results.PopulatedTemplate);
            Assert.AreEqual(expectedResults.PopulatedTemplate.InnerXml.ToString(), results.PopulatedTemplate.InnerXml.ToString());
            Assert.AreEqual(expectedResults.MissingObjects, results.MissingObjects);
        }

        [Test]
        public void TestBuildIntroHeader()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc = ResetXml(_xDoc);
            Hashtable missingObjects = new Hashtable();
            string expectedEmailTemplateXML;
            XmlDocument xDocExpected = new XmlDocument();
            TemplateResults expectedResults = new TemplateResults();

            expectedEmailTemplateXML = "<TEXT><SendID>3</SendID><ActivityID>10</ActivityID><ContractID>ABC001</ContractID>" +
                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><DisplayName /></Schedule>" +
                    "<Facility><FacilityID /><DisplayName /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><DayOfWeek /><Month /><Date /><Year /><Time /><DateTime /><TextFromNumber /><TextToNumber /><TextHelpNumber /><Body /></Message></TEXT>";

            xDocExpected.LoadXml(expectedEmailTemplateXML);
            expectedResults.PopulatedTemplate = xDocExpected;
            expectedResults.MissingObjects = missingObjects;

            TemplateResults results = _manager.BuildIntroHeader(xDoc, _textDetail, missingObjects);

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

            expectedEmailTemplateXML = "<TEXT><SendID /><ActivityID /><ContractID />" +
                    "<Patient><PatientID>20</PatientID><FullName>What Is This</FullName><FirstName>Test</FirstName><LastName>Last</LastName></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><DisplayName /></Schedule>" +
                    "<Facility><FacilityID /><DisplayName /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><DayOfWeek /><Month /><Date /><Year /><Time /><DateTime /><TextFromNumber /><TextToNumber /><TextHelpNumber /><Body /></Message></TEXT>";

            xDocExpected.LoadXml(expectedEmailTemplateXML);
            expectedResults.PopulatedTemplate = xDocExpected;
            expectedResults.MissingObjects = missingObjects;

            TemplateResults results = _manager.BuildIntroPatient(xDoc, _textDetail, missingObjects);

            Assert.AreEqual(expectedResults.PopulatedTemplate, results.PopulatedTemplate);
            Assert.AreEqual(expectedResults.PopulatedTemplate.InnerXml.ToString(), results.PopulatedTemplate.InnerXml.ToString());
            Assert.AreEqual(expectedResults.MissingObjects, results.MissingObjects);
        }

        [Test]
        public void TestBuildIntroSchedule()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc = ResetXml(_xDoc);
            Hashtable missingObjects = new Hashtable();
            string expectedEmailTemplateXML;
            XmlDocument xDocExpected = new XmlDocument();
            TemplateResults expectedResults = new TemplateResults();

            expectedEmailTemplateXML = "<TEXT><SendID /><ActivityID /><ContractID />" +
                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID>30</ScheduleID><FullName>Test Schedule 1</FullName><DisplayName>Test Schedule</DisplayName></Schedule>" +
                    "<Facility><FacilityID /><DisplayName /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><DayOfWeek /><Month /><Date /><Year /><Time /><DateTime /><TextFromNumber /><TextToNumber /><TextHelpNumber /><Body /></Message></TEXT>";

            xDocExpected.LoadXml(expectedEmailTemplateXML);
            expectedResults.PopulatedTemplate = xDocExpected;
            expectedResults.MissingObjects = missingObjects;

            TemplateResults results = _manager.BuildIntroSchedule(xDoc, _textDetail, _medias, missingObjects);

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

            expectedEmailTemplateXML = "<TEXT><SendID /><ActivityID /><ContractID />" +
                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><DisplayName /></Schedule>" +
                    "<Facility><FacilityID>100</FacilityID><DisplayName>narrative</DisplayName><Name>Test Facility</Name><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber>(214)555-1212</PhoneNumber></Facility>" +
                    "<Message><DayOfWeek /><Month /><Date /><Year /><Time /><DateTime /><TextFromNumber /><TextToNumber /><TextHelpNumber /><Body /></Message></TEXT>";

            xDocExpected.LoadXml(expectedEmailTemplateXML);
            expectedResults.PopulatedTemplate = xDocExpected;
            expectedResults.MissingObjects = missingObjects;

            TemplateResults results = _manager.BuildIntroFacility(xDoc, _textDetail, _medias, missingObjects);

            Assert.AreEqual(expectedResults.PopulatedTemplate, results.PopulatedTemplate);
            Assert.AreEqual(expectedResults.PopulatedTemplate.InnerXml.ToString(), results.PopulatedTemplate.InnerXml.ToString());
            Assert.AreEqual(expectedResults.MissingObjects, results.MissingObjects);
        }

        [Test]
        public void TestBuildIntroText()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc = ResetXml(_xDoc);
            Hashtable missingObjects = new Hashtable();
            string expectedEmailTemplateXML;
            XmlDocument xDocExpected = new XmlDocument();
            TemplateResults expectedResults = new TemplateResults();

            expectedEmailTemplateXML = "<TEXT><SendID /><ActivityID /><ContractID />" +
                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><DisplayName /></Schedule>" +
                    "<Facility><FacilityID /><DisplayName /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><DayOfWeek /><Month /><Date /><Year /><Time /><DateTime />" +
                    "<TextFromNumber>8175551212</TextFromNumber><TextToNumber>4695551212</TextToNumber><TextHelpNumber>2145551212</TextHelpNumber><Body /></Message></TEXT>";

            xDocExpected.LoadXml(expectedEmailTemplateXML);
            expectedResults.PopulatedTemplate = xDocExpected;
            expectedResults.MissingObjects = missingObjects;

            TemplateResults results = _manager.BuildIntroTextMessage(xDoc, _textDetail, missingObjects);

            Assert.AreEqual(expectedResults.PopulatedTemplate, results.PopulatedTemplate);
            Assert.AreEqual(expectedResults.PopulatedTemplate.InnerXml.ToString(), results.PopulatedTemplate.InnerXml.ToString());
            Assert.AreEqual(expectedResults.MissingObjects, results.MissingObjects);
        }

        [Test]
        public void TestTextTransform()
        {
            string actualBody = string.Empty;
            string expectedBody = "You have an appt with Test Schedule on Nov. 20 2020 at 8:00 AM. Txt YES to confirm or NO to cancel. Txt STOP to opt-out. HELP 4 HELP. MSG data rates may apply.";

            string xmlBody = "<TEXT><SendID>3</SendID><ActivityID>10</ActivityID><ContractID>ABC001</ContractID>" +
                              "<Patient><PatientID>20</PatientID><FullName>what is this</FullName><FirstName>test</FirstName><LastName>last</LastName></Patient>" +
                              "<Schedule><ScheduleID>30</ScheduleID><FullName>Test Schedule 1</FullName><DisplayName>Test Schedule</DisplayName></Schedule>" +
                              "<Facility><FacilityID>100</FacilityID><DisplayName /><Name>Test Facility</Name><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber>(214)555-1212</PhoneNumber></Facility>" +
                              "<Message><DayOfWeek>Friday</DayOfWeek><Month>Nov.</Month><Date>20</Date><Year>2020</Year><Time>8:00 AM</Time><DateTime />" +
                              "<TextFromNumber>8175551212</TextFromNumber><TextToNumber>4695551212</TextToNumber><TextHelpNumber>2145551212</TextHelpNumber><Body /></Message></TEXT>";
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlBody);
            string xslBody = "<xsl:stylesheet xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" version=\"1.0\">" +
                                "<xsl:output omit-xml-declaration=\"yes\" indent=\"yes\" />" +
                                "<xsl:template match=\"/\">" +
                                "<xsl:call-template name=\"TEXT\" />" +
                                "</xsl:template>" +
                                "<xsl:template name=\"TEXT\">" +
                                "You have an appt with <xsl:value-of select=\"substring(//TEXT/Schedule/DisplayName, 1, 13)\" /> on <xsl:value-of select=\"concat(//TEXT/Message/Month, ' ', //TEXT/Message/Date, ' ', //TEXT/Message/Year, ' at ', //TEXT/Message/Time)\" />. Txt YES to confirm or NO to cancel. Txt STOP to opt-out. HELP 4 HELP. MSG data rates may apply." +
                                "</xsl:template>" +
                                "</xsl:stylesheet>";
            TemplateDetail templateDetail = new TemplateDetail
            {
                TemplateXSLBody = xslBody
            };

            actualBody = _manager.Transform(xml, templateDetail);

            Assert.AreEqual(expectedBody, actualBody);
        }

        private XmlDocument ResetXml(XmlDocument xDoc)
        {
            XmlDocument result = new XmlDocument();

            string originalTextTemplateXML = "<TEXT><SendID /><ActivityID /><ContractID />" +
                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                    "<Schedule><ScheduleID /><FullName /><DisplayName /></Schedule>" +
                    "<Facility><FacilityID /><DisplayName /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                    "<Message><DayOfWeek /><Month /><Date /><Year /><Time /><DateTime /><TextFromNumber /><TextToNumber /><TextHelpNumber /><Body /></Message></TEXT>";
            result.LoadXml(originalTextTemplateXML);

            return result;
        }

        private void SetUpData()
        {
            #region Setup TextActivityDetail

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
            string facilityPhoneNumber = "2145551212";
            string toPhoneNumber = "4695551212";
            string fromPhoneNumber = "8175551212";
            string appointmentDateTime = "1/1/2015";
            int scheduleID = 30;
            int appointmentDuration = 60;
            string scheduleDateTime = "11/20/2020 08:00:00 AM";
            string scheduleName = "Test Schedule 1";
            string facilityName = "Test Facility";
            
            _textDetail.TaskTypeCategory = TaskTypeCategory.OutreachRecall;
            _textDetail.TemplateID = templateId;
            _textDetail.CampaignID = campaignId; // If either are = 0, throws exception
            _textDetail.FacilityID = facilityId;
            _textDetail.SendID = sendId;
            _textDetail.ContractNumber = contractNumber;
            _textDetail.ContractID = contractID;
            _textDetail.ActivityID = activityId;
            _textDetail.PatientID = patientId;
            _textDetail.PatientFirstName = patientFirstName;
            _textDetail.PatientLastName = patientLastName;
            _textDetail.PatientNameLF = patientNameLF;
            _textDetail.FacilityID = facilityId;
            _textDetail.ProviderACDNumber = facilityPhoneNumber;
            _textDetail.ScheduleDateTime = appointmentDateTime;
            _textDetail.RecipientSchedID = scheduleID;
            _textDetail.ScheduleDuration = appointmentDuration;
            _textDetail.ScheduleDateTime = scheduleDateTime;
            _textDetail.ScheduleName = scheduleName;
            _textDetail.FacilityName = facilityName;
            _textDetail.PhoneNumber = toPhoneNumber;
            _textDetail.TextFromNumber = fromPhoneNumber;

            #endregion

            #region Setup ActivityMedias

            ActivityMedia facilityDisplayName = new ActivityMedia
            {
                OwnerID = facilityId,
                CategoryCode = "SNOVR",
                OwnerCode = "TEXT",
                LanguagePreferenceCode = "EN",
                Narrative = "narrative"
            };

            ActivityMedia scheduleDisplayName = new ActivityMedia
            {
                OwnerID = scheduleID,
                CategoryCode = "SCOVR",
                OwnerCode = "TEXT",
                LanguagePreferenceCode = "EN",
                Narrative = "Test Schedule"
            };

            _medias.Add(facilityDisplayName);
            _medias.Add(scheduleDisplayName);

            #endregion

        }

    }
}
