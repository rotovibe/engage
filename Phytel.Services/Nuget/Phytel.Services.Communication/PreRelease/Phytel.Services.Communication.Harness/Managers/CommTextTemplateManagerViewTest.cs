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
            TemplateResults results = _manager.BuildPatient(ResetXml(), testViewModel.TextActivityDetail,
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

        public TestViewModel BuildIntroTextMesssage_Test(TestViewModel testViewModel)
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
