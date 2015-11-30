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

        [Test]
        public void TestEmailTransform()
        {
            string actualBody = string.Empty;
            string xmlBody = string.Empty;
            string xslBody = string.Empty;
            XmlDocument xml = new XmlDocument();

            #region Expected Body String
            string expectedBody = "&lt;table id=&quot;header_shadow&quot; style=&quot;text-align:center;margin-top:17px;width: 622px;&quot;&gt;&lt;tr&" +
                "gt;&lt;td&gt;&lt;table id=&quot;footer_shadow&quot; style=&quot;width:100%;margin-top:-5px;margin-bottom:-5px;&quot;&gt;&lt;tr&" +
                "gt;&lt;td&gt;&lt;img alt=&quot;&quot; style=&quot;display:block;&quot; src=&quot;" +
                "https://healthreminder.phytel.com/Email/Global/COMMON/EN/header-shadow.jpg&quot; /&gt;&lt;table id=&quot;wrapper&quot; class=&quot;wrapper&" +
                "quot; style=&quot;width: 100%;background-repeat:repeat-y;&quot; background=&quot;https://healthreminder.phytel.com/Email/Global/COMMON/EN/bg-repeat.jpg&" +
                "quot; background-repeat=&quot;repeat-y;&quot;&gt;&lt;tr&gt;&lt;td&gt;&lt;table id=&quot;container&quot; style=&quot;width:100%;" +
                "&quot;&gt;&lt;tr&gt;&lt;td&gt;&lt;table style=&quot;width:100%;&quot;&gt;&lt;tr&gt;&lt;td style=&quot;text-align:center;" +
                "&quot;&gt;&lt;table style=&quot;width:100%;text-align:center;&quot;&gt;&lt;tr&gt;&lt;td&gt;&lt;img alt=&quot;&" +
                "quot; style=&quot;width:auto;height:auto;&quot; src=&quot;logo.jpg&quot; /&gt;&lt;" +
                "/td&gt;&lt;/tr&gt;&lt;/table&gt;&lt;img alt=&quot;&quot; style=&quot;&quot; src=&quot;" +
                "https://healthreminder.phytel.com/Email/Global/COMMON/EN/divider_top.gif&quot; /&gt;&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;&lt;" +
                "table id=&quot;content&quot; style=&quot;border-collapse: collapse;width:100%;&quot;&gt;&lt;tr&gt;&lt;td style=&quot;text-align:left;" +
                "padding:0px 50px 0px 50px;&quot;&gt;&lt;table&gt;&lt;tr&gt;&lt;td style=&quot;margin: 0px 0px 5px 0px;&quot;&" +
                "gt;													Dear Test,&lt;br /&gt;&lt;/td&gt;&lt;/tr&gt;&lt;tr&gt;&lt;td style=&" +
                "quot;margin: 0px 0px 15px 0px;&quot;&gt;&lt;br /&gt;This is a courtesy message from narrative.&lt;br /&gt;&lt;" +
                "br /&gt;													  As our patient, we care about you and strive to give you the highest quality healthcare possible.  " +
                "We have some important information that is pertinent to maintaining your health.&lt;br /&gt;&lt;br /&gt;													  " +
                "Please call our office at (214) 555-1212 to retrieve your health information.  Please &lt;u&gt;do not&lt;/u&gt; reply to this email.  " +
                "To ensure your privacy, we would like to verify your identity over the phone prior to discussing any of your personal health information.&lt;br /&gt;&" +
                "lt;br /&gt;&lt;/td&gt;&lt;/tr&gt;&lt;tr&gt;&lt;td style=&quot;padding-right:50px;float:right&quot;&" +
                "gt;													  Sincerely,&lt;br /&gt;&lt;br /&gt;narrative " +
                "Staff											&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;&lt;br /&gt;&lt;/td&gt;&" +
                "lt;/tr&gt;&lt;/table&gt;&lt;table id=&quot;divider_bottom&quot; style=&quot;text-align:center;width:100%;height: 16px;" +
                "margin: 0px 0px 5px 0px;&quot;&gt;&lt;tr&gt;&lt;td style=&quot;text-align:center;&quot;&gt;&lt;img alt=&quot;&" +
                "quot; style=&quot;&quot; src=&quot;https://healthreminder.phytel.com/Email/Global/COMMON/EN/divider_bottom.gif&quot; /&gt;&lt;/td&gt;" +
                "&lt;/tr&gt;&lt;/table&gt;&lt;table style=&quot;border-collapse: collapse;width:100%;&quot;&gt;&lt;tr&gt;&lt;td id=&" +
                "quot;footer&quot; style=&quot;text-align:center;width:100%;vertical-align: middle;padding: 0 60px 0px 60px;height: 57px;font-size: 9px;line-height: 14px;&" +
                "quot;&gt;												CONFIDENTIALITY NOTICE: This electronic mail transmission is confidential, may be privileged and should be " +
                "read or retained only by the intended recipient. If you have received this transmission in error, would like to change your communication preferences, or would like to " +
                "unsubscribe from this service, please call our office																								  at " +
                "(214) 555-1212.											  &lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;&lt;/td&gt;&lt;/tr&gt;&lt;" +
                "/table&gt;&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;&lt;img alt=&quot;&quot; style=&quot;display:block;&quot; src=&quot;" +
                "https://healthreminder.phytel.com/Email/Global/COMMON/EN/footer-shadow.jpg&quot; /&gt;&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;&lt;/td&" +
                "gt;&lt;/tr&gt;&lt;/table&gt;";
            #endregion

            #region Setup XML/XSL
            xmlBody =   "<Email><SendID>3</SendID><ActivityID>10</ActivityID><ContractID>ABC001</ContractID>" +
                        "<ConfirmationURL IsCDATA=\"true\" Enable=\"true\"><![CDATA[yes.com?SendID=GTtixGtfqGkV1kuMvjwjXQ%3d%3d&ContractID=OkVhk6ATD34kP7Wp9Fibkw%3d%3d]]></ConfirmationURL>" +
                        "<OptOutURL IsCDATA=\"true\" Enable=\"true\" ><![CDATA[no.com?SendID=GTtixGtfqGkV1kuMvjwjXQ%3d%3d&ContractID=OkVhk6ATD34kP7Wp9Fibkw%3d%3d]]></OptOutURL>" +
                        "<ImagePath IsCDATA=\"true\" Enable=\"true\">https://healthreminder.phytel.com/Email/Global/COMMON/EN/</ImagePath>" +
                        "<Patient><PatientID>20</PatientID><FullName>What Is This</FullName><FirstName>Test</FirstName><LastName>Last</LastName></Patient>" +
                        "<Schedule><ScheduleID>30</ScheduleID><FullName>Test Schedule</FullName><FirstName /><LastName /></Schedule>" +
                        "<Facility><FacilityID>100</FacilityID><FacilityLogo Enable=\"true\" >logo.jpg</FacilityLogo><FacilityURL Enable=\"true\" >facility.com</FacilityURL>" +
                        "<Name>narrative</Name><Addr1>Facility Address</Addr1><Addr2>Facility Address 2</Addr2><City>Dallas</City><State>TX</State><Zip>77777</Zip><PhoneNumber>(214) 555-1212</PhoneNumber></Facility>" +
                        "<Message><Type>Health Reminder </Type><DayOfWeek>Friday</DayOfWeek><Month>November</Month><Date>20</Date><Year>2020</Year><Time>8:00 AM</Time><Duration>60</Duration><AppointmentSpecificMessage Enable=\"true\" />" +
                        "<FromEmailAddress>facility@test.com</FromEmailAddress><ReplyToEmailAddress>facilityreply@test.com</ReplyToEmailAddress><ToEmailAddress>test@test.com</ToEmailAddress>" +
                        "<DisplayName>facility</DisplayName><Subject>Health Reminder from </Subject><Body /></Message></Email>";

            xslBody =   "<xsl:stylesheet xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" version=\"1.0\">" +
                        "<xsl:output omit-xml-declaration=\"yes\" indent=\"yes\" />" +
                        "<xsl:template match=\"/\">" +
                        "<xsl:call-template name=\"Email\" />" +
                        "</xsl:template>" +
                        "<xsl:template name=\"Email\">" +
                        "<table id=\"header_shadow\" style=\"text-align:center;margin-top:17px;width: 622px;\">" +
                            "<tr>" +
                            "<td>" +
                                "<table id=\"footer_shadow\" style=\"width:100%;margin-top:-5px;margin-bottom:-5px;\">" +
                                "<tr>" +
                                    "<td>" +
                                    "<xsl:element name=\"img\">" +
                                        "<xsl:attribute name=\"alt\" />" +
                                        "<xsl:attribute name=\"style\">display:block;</xsl:attribute>" +
                                        "<xsl:attribute name=\"src\">" +
                                        "<xsl:value-of select=\"//Email/ImagePath\" />header-shadow.jpg</xsl:attribute>" +
                                    "</xsl:element>" +
                                    "<xsl:element name=\"table\">" +
                                        "<xsl:attribute name=\"id\">wrapper</xsl:attribute>" +
                                        "<xsl:attribute name=\"class\">wrapper</xsl:attribute>" +
                                        "<xsl:attribute name=\"style\">width: 100%;background-repeat:repeat-y;</xsl:attribute>" +
                                        "<xsl:attribute name=\"background\">" +
                                        "<xsl:value-of select=\"//Email/ImagePath\" />bg-repeat.jpg</xsl:attribute>" +
                                        "<xsl:attribute name=\"background-repeat\">repeat-y;</xsl:attribute>" +
                                        "<tr>" +
                                        "<td>" +
                                            "<table id=\"container\" style=\"width:100%;\">" +
                                            "<tr>" +
                                                "<td>" +
                                                "<table style=\"width:100%;\">" +
                                                    "<tr>" +
                                                    "<td style=\"text-align:center;\">" +
                                                        "<table style=\"width:100%;text-align:center;\">" +
                                                        "<tr>" +
                                                            "<td>" +
                                                            "<xsl:element name=\"img\">" +
                                                                "<xsl:attribute name=\"alt\">" +
                                                                "<xsl:value-of select=\"//Email/Facility/FacilityName\" />" +
                                                                "</xsl:attribute>" +
                                                                "<xsl:attribute name=\"style\">" +
														                        "width:auto;height:auto;" +
													                        "</xsl:attribute>" +
                                                                "<xsl:attribute name=\"src\">" +
                                                                "<xsl:value-of select=\"//Email/Facility/FacilityLogo\" />" +
                                                                "</xsl:attribute>" +
                                                            "</xsl:element>" +
                                                            "</td>" +
                                                        "</tr>" +
                                                        "</table>" +
                                                        "<xsl:element name=\"img\">" +
                                                        "<xsl:attribute name=\"alt\" />" +
                                                        "<xsl:attribute name=\"style\" />" +
                                                        "<xsl:attribute name=\"src\">" +
                                                            "<xsl:value-of select=\"//Email/ImagePath\" />divider_top.gif</xsl:attribute>" +
                                                        "</xsl:element>" +
                                                    "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                                "<table id=\"content\" style=\"border-collapse: collapse;width:100%;\">" +
                                                    "<tr>" +
                                                    "<td style=\"text-align:left;padding:0px 50px 0px 50px;\">" +
                                                        "<table>" +
                                                        "<tr>" +
                                                            "<td style=\"margin: 0px 0px 5px 0px;\">													Dear <xsl:value-of select=\"//Email/Patient/FirstName\" />,<br /></td>" +
                                                        "</tr>" +
                                                        "<tr>" +
                                                            "<td style=\"margin: 0px 0px 15px 0px;\">" +
                                                            "<br />This is a courtesy message from <xsl:value-of select=\"//Email/Facility/Name\" />.<br /><br />													  As our patient, we care about you and strive to give you the highest quality healthcare possible.  We have some important information that is pertinent to maintaining your health.<br /><br />													  Please call our office at <xsl:value-of select=\"//Email/Facility/PhoneNumber\" /> to retrieve your health information.  Please <u>do not</u> reply to this email.  To ensure your privacy, we would like to verify your identity over the phone prior to discussing any of your personal health information.<br /><br /></td>" +
                                                        "</tr>" +
                                                        "<tr>" +
                                                            "<td style=\"padding-right:50px;float:right\">													  Sincerely,<br /><br /><xsl:value-of select=\"//Email/Facility/Name\" /> Staff											</td>" +
                                                        "</tr>" +
                                                        "</table>" +
                                                        "<br />" +
                                                    "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                                "<table id=\"divider_bottom\" style=\"text-align:center;width:100%;height: 16px;margin: 0px 0px 5px 0px;\">" +
                                                    "<tr>" +
                                                    "<td style=\"text-align:center;\">" +
                                                        "<xsl:element name=\"img\">" +
                                                        "<xsl:attribute name=\"alt\" />" +
                                                        "<xsl:attribute name=\"style\" />" +
                                                        "<xsl:attribute name=\"src\">" +
                                                            "<xsl:value-of select=\"//Email/ImagePath\" />divider_bottom.gif</xsl:attribute>" +
                                                        "</xsl:element>" +
                                                    "</td>" +
                                                    "</tr>" +
                                                "</table>" +
                                                "<table style=\"border-collapse: collapse;width:100%;\">" +
                                                    "<tr>" +
                                                    "<td id=\"footer\" style=\"text-align:center;width:100%;vertical-align: middle;padding: 0 60px 0px 60px;height: 57px;font-size: 9px;line-height: 14px;\">												CONFIDENTIALITY NOTICE: This electronic mail transmission is confidential, may be privileged and should be read or retained only by the intended recipient. If you have received this transmission in error, would like to change your communication preferences, or would like to unsubscribe from this service, please call our office												<xsl:if test=\"string-length(//Email/Facility/PhoneNumber) &gt; 0\">												  at <xsl:value-of select=\"//Email/Facility/PhoneNumber\" /></xsl:if>.											  </td>" +
                                                    "</tr>" +
                                                "</table>" +
                                                "</td>" +
                                            "</tr>" +
                                            "</table>" +
                                        "</td>" +
                                        "</tr>" +
                                    "</xsl:element>" +
                                    "<xsl:element name=\"img\">" +
                                        "<xsl:attribute name=\"alt\" />" +
                                        "<xsl:attribute name=\"style\">display:block;</xsl:attribute>" +
                                        "<xsl:attribute name=\"src\">" +
                                        "<xsl:value-of select=\"//Email/ImagePath\" />footer-shadow.jpg</xsl:attribute>" +
                                    "</xsl:element>" +
                                    "</td>" +
                                "</tr>" +
                                "</table>" +
                            "</td>" +
                            "</tr>" +
                        "</table>" +
                        "</xsl:template>" +
                        "</xsl:stylesheet>";
            #endregion

            xml.LoadXml(xmlBody);

            TemplateDetail templateDetail = new TemplateDetail{
                TemplateXSLBody = xslBody
            };

            actualBody = _manager.Transform(xml, templateDetail);

            Assert.AreEqual(expectedBody, actualBody);
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
