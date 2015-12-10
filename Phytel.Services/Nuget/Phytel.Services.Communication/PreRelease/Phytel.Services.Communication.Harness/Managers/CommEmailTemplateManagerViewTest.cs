using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Phytel.Services.Communication.Harness.Model;

namespace Phytel.Services.Communication.Harness.Managers
{
    public class CommEmailTemplateManagerViewTest
    {
        private readonly TestViewModel _testViewModel;
        private readonly ICommEmailTemplateManager _manager;        
        //private List<ContractPermission> _contractPermissionList;

        public CommEmailTemplateManagerViewTest()
        {
            _testViewModel = new TestViewModel();
            ITemplateUtilities templateUtilities = new TemplateUtilities();
            _manager = new CommEmailTemplateManager(templateUtilities);
        }

        // Test methods
        public TestViewModel BuildApptDateTime_Test(TestViewModel testViewModel)
        {
            var missingObjects = new Hashtable();
            TemplateResults results = _manager.BuildApptDateTime(ResetXml(), testViewModel.EmailActivityDetail,
                missingObjects);
            _testViewModel.XmlDocString = FormatXml(results.PopulatedTemplate);
            _testViewModel.MissingObjects = results.MissingObjects;
            return _testViewModel;
        }

        public TestViewModel BuildEmail_Test(TestViewModel testViewModel)
        {
            var missingObjects = new Hashtable();
            TemplateResults results = _manager.BuildEmailMessage(ResetXml(), testViewModel.EmailActivityDetail, testViewModel.EmailActivityMedia, missingObjects, 2);
            _testViewModel.XmlDocString = FormatXml(results.PopulatedTemplate);
            _testViewModel.MissingObjects = results.MissingObjects;            
            return _testViewModel;
        }

        public TestViewModel BuildFacility_Test(TestViewModel testViewModel)
        {
            var missingObjects = new Hashtable();
            TemplateResults results = _manager.BuildFacility(ResetXml(), testViewModel.EmailActivityDetail,
                testViewModel.EmailActivityMedia, missingObjects, "");
            _testViewModel.XmlDocString = FormatXml(results.PopulatedTemplate);
            _testViewModel.MissingObjects = results.MissingObjects;
            return _testViewModel;
        }

        public TestViewModel BuildHeader_Test(TestViewModel testViewModel)
        {
            var missingObjects = new Hashtable();
            TemplateResults results = _manager.BuildHeader(ResetXml(), testViewModel.EmailActivityDetail,
                missingObjects, "yes.com", "no.com");
            _testViewModel.XmlDocString = FormatXml(results.PopulatedTemplate);
            _testViewModel.MissingObjects = results.MissingObjects;
            return _testViewModel;
        }

        public TestViewModel BuildIntroEmailMessage_Test(TestViewModel testViewModel)
        {
            var missingObjects = new Hashtable();
            TemplateResults results = _manager.BuildIntroEmailMessage(ResetXml(), testViewModel.EmailActivityDetail,
                testViewModel.EmailActivityMedia, missingObjects, 2);
            _testViewModel.XmlDocString = FormatXml(results.PopulatedTemplate);
            _testViewModel.MissingObjects = results.MissingObjects;
            return _testViewModel;
        }

        public TestViewModel BuildIntroFacility_Test(TestViewModel testViewModel)
        {
            var missingObjects = new Hashtable();
            TemplateResults results = _manager.BuildIntroFacility(ResetXml(), testViewModel.EmailActivityDetail,
                testViewModel.EmailActivityMedia, missingObjects);
            _testViewModel.XmlDocString = FormatXml(results.PopulatedTemplate);
            _testViewModel.MissingObjects = results.MissingObjects;
            return _testViewModel;
        }

        public TestViewModel BuildIntroPatient_Test(TestViewModel testViewModel)
        {
            var missingObjects = new Hashtable();
            TemplateResults results = _manager.BuildIntroPatient(ResetXml(), testViewModel.EmailActivityDetail,
                missingObjects);
            _testViewModel.XmlDocString = FormatXml(results.PopulatedTemplate);
            _testViewModel.MissingObjects = results.MissingObjects;
            return _testViewModel;
        }

        public TestViewModel BuildPatient_Test(TestViewModel testViewModel)
        {
            var missingObjects = new Hashtable();
            TemplateResults results = _manager.BuildPatient(ResetXml(), testViewModel.EmailActivityDetail,
                missingObjects);
            _testViewModel.XmlDocString = FormatXml(results.PopulatedTemplate);
            _testViewModel.MissingObjects = results.MissingObjects;
            return _testViewModel;
        }

        public TestViewModel BuildSchedule_Test(TestViewModel testViewModel)
        {
            var missingObjects = new Hashtable();
            TemplateResults results = _manager.BuildSchedule(ResetXml(), testViewModel.EmailActivityDetail,
                testViewModel.EmailActivityMedia, missingObjects);
            _testViewModel.XmlDocString = FormatXml(results.PopulatedTemplate);
            _testViewModel.MissingObjects = results.MissingObjects;
            return _testViewModel;
        }

        public TestViewModel EmailTransform_Test(TestViewModel testViewModel)
        {
            string xmlBody;
            if (string.IsNullOrEmpty(testViewModel.TemplateDetail.TemplateXMLBody))
            {
                xmlBody = "<Email><SendID>3</SendID><ActivityID>10</ActivityID><ContractID>ABC001</ContractID>" +
                                 "<ConfirmationURL IsCDATA=\"true\" Enable=\"true\"><![CDATA[yes.com?SendID=GTtixGtfqGkV1kuMvjwjXQ%3d%3d&ContractID=OkVhk6ATD34kP7Wp9Fibkw%3d%3d]]></ConfirmationURL>" +
                                 "<OptOutURL IsCDATA=\"true\" Enable=\"true\" ><![CDATA[no.com?SendID=GTtixGtfqGkV1kuMvjwjXQ%3d%3d&ContractID=OkVhk6ATD34kP7Wp9Fibkw%3d%3d]]></OptOutURL>" +
                                 "<ImagePath IsCDATA=\"true\" Enable=\"true\">https://healthreminder.phytel.com/Email/Global/COMMON/EN/</ImagePath>" +
                                 "<Patient><PatientID>20</PatientID><FullName>What Is This</FullName><FirstName>Test</FirstName><LastName>Last</LastName></Patient>" +
                                 "<Schedule><ScheduleID>30</ScheduleID><FullName>Test Schedule</FullName><FirstName /><LastName /></Schedule>" +
                                 "<Facility><FacilityID>100</FacilityID><FacilityLogo Enable=\"true\" >logo.jpg</FacilityLogo><FacilityURL Enable=\"true\" >facility.com</FacilityURL>" +
                                 "<Name>Facility Name</Name><Addr1>Facility Address</Addr1><Addr2>Facility Address 2</Addr2><City>Dallas</City><State>TX</State><Zip>77777</Zip><PhoneNumber>(214) 555-1212</PhoneNumber></Facility>" +
                                 "<Message><Type>Health Reminder </Type><DayOfWeek>Friday</DayOfWeek><Month>November</Month><Date>20</Date><Year>2020</Year><Time>8:00 AM</Time><Duration>60</Duration><AppointmentSpecificMessage Enable=\"true\" />" +
                                 "<FromEmailAddress>facility@test.com</FromEmailAddress><ReplyToEmailAddress>facilityreply@test.com</ReplyToEmailAddress><ToEmailAddress>test@test.com</ToEmailAddress>" +
                                 "<DisplayName>facility</DisplayName><Subject>Health Reminder from </Subject><Body /></Message></Email>";
            }
            else
            {
                xmlBody = testViewModel.TemplateDetail.TemplateXMLBody;
            }

            string xslBody = "<xsl:stylesheet xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" version=\"1.0\">" +
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
                        
            var xml = new XmlDocument();
            xml.LoadXml(xmlBody);
            testViewModel.TemplateDetail = new TemplateDetail
            {
                TemplateXSLBody = xslBody,
                TemplateXMLBody = FormatXml(xml)
            };

            testViewModel.TransformResult = _manager.Transform(xml, testViewModel.TemplateDetail);

            return testViewModel;            
        }

        // Utility methods
        private static XmlDocument ResetXml()
        {
            var result = new XmlDocument();

            const string emptyEmailTemplate = "<Email><SendID /><ActivityID /><ContractID />" +
                                                    "<ConfirmationURL IsCDATA=\"true\" Enable=\"true\" /><OptOutURL IsCDATA=\"true\" Enable=\"true\" />" +
                                                    "<ImagePath IsCDATA=\"true\" Enable=\"true\">https://healthreminder.phytel.com/Email/Global/COMMON/EN/</ImagePath>" +
                                                    "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                                                    "<Schedule><ScheduleID /><FullName /><FirstName /><LastName /></Schedule>" +
                                                    "<Facility><FacilityID /><FacilityLogo Enable=\"true\" /><FacilityURL Enable=\"true\" /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                                                    "<Message><Type /><DayOfWeek /><Month /><Date /><Year /><Time /><Duration /><AppointmentSpecificMessage Enable=\"true\" /><FromEmailAddress />" +
                                                    "<ReplyToEmailAddress /><ToEmailAddress /><DisplayName /><Subject>Health Reminder from FACILITY_NAME</Subject><Body /></Message></Email>";
            result.LoadXml(emptyEmailTemplate);

            return result;
        }

        private static string FormatXml(XmlDocument doc)
        {
            var sb = new StringBuilder();
            TextWriter tr = new StringWriter(sb);
            var wr = new XmlTextWriter(tr) {Formatting = Formatting.Indented};
            doc.Save(wr);
            wr.Close();
            return sb.ToString();
        }
    }
}
