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
            TemplateResults results = _manager.BuildPatient(ResetXml(), testViewModel.EmailActivityDetail,
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
