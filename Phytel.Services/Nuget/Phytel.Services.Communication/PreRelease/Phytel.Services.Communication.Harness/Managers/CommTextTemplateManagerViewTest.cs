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
    public class CommTextTemplateManagerViewTest
    {
        private readonly TestViewModel _testViewModel;
        private readonly ICommTextTemplateManager _manager;

        public CommTextTemplateManagerViewTest()
        {
            _testViewModel = new TestViewModel();
            ITemplateUtilities templateUtilities = new TemplateUtilities();
            _manager = new CommTextTemplateManager(templateUtilities);
        }

        // Text methods
        public TestViewModel BuildApptDateTime_Test(TestViewModel testViewModel)
        {
            var missingObjects = new Hashtable();
            TemplateResults results = _manager.BuildApptDateTime(ResetXml(), testViewModel.TextActivityDetail,
                missingObjects);
            _testViewModel.XmlDocString = FormatXml(results.PopulatedTemplate);
            _testViewModel.MissingObjects = results.MissingObjects;
            return _testViewModel;
        }

        public TestViewModel BuildFacility_Test(TestViewModel testViewModel)
        {
            var missingObjects = new Hashtable();
            TemplateResults results = _manager.BuildFacility(ResetXml(), testViewModel.TextActivityDetail, testViewModel.TextActivityMedia, missingObjects);
            _testViewModel.XmlDocString = FormatXml(results.PopulatedTemplate);
            _testViewModel.MissingObjects = results.MissingObjects;
            return _testViewModel;
        }

        public TestViewModel BuildHeader_Test(TestViewModel testViewModel)
        {
            var missingObjects = new Hashtable();
            TemplateResults results = _manager.BuildHeader(ResetXml(), testViewModel.TextActivityDetail, missingObjects);
            _testViewModel.XmlDocString = FormatXml(results.PopulatedTemplate);
            _testViewModel.MissingObjects = results.MissingObjects;
            return _testViewModel;
        }

        public TestViewModel BuildIntroFacility_Test(TestViewModel testViewModel)
        {
            var missingObjects = new Hashtable();
            TemplateResults results = _manager.BuildIntroFacility(ResetXml(), testViewModel.TextActivityDetail, testViewModel.TextActivityMedia, missingObjects);
            _testViewModel.XmlDocString = FormatXml(results.PopulatedTemplate);
            _testViewModel.MissingObjects = results.MissingObjects;
            return _testViewModel;
        }

        public TestViewModel BuildIntroHeader_Test(TestViewModel testViewModel)
        {
            var missingObjects = new Hashtable();
            TemplateResults results = _manager.BuildIntroHeader(ResetXml(), testViewModel.TextActivityDetail, missingObjects);
            _testViewModel.XmlDocString = FormatXml(results.PopulatedTemplate);
            _testViewModel.MissingObjects = results.MissingObjects;
            return _testViewModel;
        }

        public TestViewModel BuildIntroPatient_Test(TestViewModel testViewModel)
        {
            var missingObjects = new Hashtable();
            TemplateResults results = _manager.BuildIntroPatient(ResetXml(), testViewModel.TextActivityDetail, missingObjects);
            _testViewModel.XmlDocString = FormatXml(results.PopulatedTemplate);
            _testViewModel.MissingObjects = results.MissingObjects;
            return _testViewModel;
        }

        public TestViewModel BuildIntroTextMessage_Test(TestViewModel testViewModel)
        {
            var missingObjects = new Hashtable();
            TemplateResults results = _manager.BuildIntroTextMessage(ResetXml(), testViewModel.TextActivityDetail, missingObjects);
            _testViewModel.XmlDocString = FormatXml(results.PopulatedTemplate);
            _testViewModel.MissingObjects = results.MissingObjects;
            return _testViewModel;
        }

        public TestViewModel BuildIntroSchedule_Test(TestViewModel testViewModel)
        {
            var missingObjects = new Hashtable();
            TemplateResults results = _manager.BuildIntroSchedule(ResetXml(), testViewModel.TextActivityDetail, testViewModel.TextActivityMedia, missingObjects);
            _testViewModel.XmlDocString = FormatXml(results.PopulatedTemplate);
            _testViewModel.MissingObjects = results.MissingObjects;
            return _testViewModel;
        }

        public TestViewModel BuildPatient_Test(TestViewModel testViewModel)
        {
            var missingObjects = new Hashtable();
            TemplateResults results = _manager.BuildIntroPatient(ResetXml(), testViewModel.TextActivityDetail, missingObjects);
            _testViewModel.XmlDocString = FormatXml(results.PopulatedTemplate);
            _testViewModel.MissingObjects = results.MissingObjects;
            return _testViewModel;
        }

        public TestViewModel BuildSchedule_Test(TestViewModel testViewModel)
        {
            var missingObjects = new Hashtable();
            TemplateResults results = _manager.BuildSchedule(ResetXml(), testViewModel.TextActivityDetail, testViewModel.TextActivityMedia, missingObjects);
            _testViewModel.XmlDocString = FormatXml(results.PopulatedTemplate);
            _testViewModel.MissingObjects = results.MissingObjects;
            return _testViewModel;
        }

        public TestViewModel BuildTextMessage_Test(TestViewModel testViewModel)
        {
            var missingObjects = new Hashtable();
            TemplateResults results = _manager.BuildTextMessage(ResetXml(), testViewModel.TextActivityDetail, missingObjects);
            _testViewModel.XmlDocString = FormatXml(results.PopulatedTemplate);
            _testViewModel.MissingObjects = results.MissingObjects;
            return _testViewModel;
        }

        public TestViewModel TextTransform_Test(TestViewModel testViewModel)
        {
            string xmlBody;
            if (string.IsNullOrEmpty(testViewModel.TemplateDetail.TemplateXMLBody))
            {
                xmlBody = "<TEXT><SendID>3</SendID><ActivityID>10</ActivityID><ContractID>ABC001</ContractID>" +
                              "<Patient><PatientID>20</PatientID><FullName>what is this</FullName><FirstName>test</FirstName><LastName>last</LastName></Patient>" +
                              "<Schedule><ScheduleID>30</ScheduleID><FullName>Test Schedule 1</FullName><DisplayName>Test Schedule</DisplayName></Schedule>" +
                              "<Facility><FacilityID>100</FacilityID><DisplayName /><Name>Test Facility</Name><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber>(214)555-1212</PhoneNumber></Facility>" +
                              "<Message><DayOfWeek>Friday</DayOfWeek><Month>Nov.</Month><Date>20</Date><Year>2020</Year><Time>8:00 AM</Time><DateTime />" +
                              "<TextFromNumber>8175551212</TextFromNumber><TextToNumber>4695551212</TextToNumber><TextHelpNumber>2145551212</TextHelpNumber><Body /></Message></TEXT>";
            }
            else
            {
                xmlBody = testViewModel.TemplateDetail.TemplateXMLBody;
            }

            string xslBody = "<xsl:stylesheet xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" version=\"1.0\">" +
                                "<xsl:output omit-xml-declaration=\"yes\" indent=\"yes\" />" +
                                "<xsl:template match=\"/\">" +
                                "<xsl:call-template name=\"TEXT\" />" +
                                "</xsl:template>" +
                                "<xsl:template name=\"TEXT\">" +
                                "You have an appt with <xsl:value-of select=\"substring(//TEXT/Schedule/DisplayName, 1, 13)\" /> on <xsl:value-of select=\"concat(//TEXT/Message/Month, ' ', //TEXT/Message/Date, ' ', //TEXT/Message/Year, ' at ', //TEXT/Message/Time)\" />. Txt YES to confirm or NO to cancel. Txt STOP to opt-out. HELP 4 HELP. MSG data rates may apply." +
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
            XmlDocument result = new XmlDocument();

            const string emptyTextTemplate = "<TEXT><SendID /><ActivityID /><ContractID />" +
                                                   "<Patient><PatientID /><FullName /><FirstName /><LastName /></Patient>" +
                                                   "<Schedule><ScheduleID /><FullName /><DisplayName /></Schedule>" +
                                                   "<Facility><FacilityID /><DisplayName /><Name /><Addr1 /><Addr2 /><City /><State /><Zip /><PhoneNumber /></Facility>" +
                                                   "<Message><DayOfWeek /><Month /><Date /><Year /><Time /><DateTime /><TextFromNumber /><TextToNumber /><TextHelpNumber /><Body /></Message></TEXT>";
            result.LoadXml(emptyTextTemplate);

            return result;
        }

        private static string FormatXml(XmlDocument doc)
        {
            var sb = new StringBuilder();
            TextWriter tr = new StringWriter(sb);
            var wr = new XmlTextWriter(tr) { Formatting = Formatting.Indented };
            doc.Save(wr);
            wr.Close();
            return sb.ToString();
        }
    }
}
