using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using Phytel.Services.Communication.Harness.Model;
using Phytel.Services.Communication.Harness.Managers;

namespace Phytel.Services.Communication.Harness
{

    public partial class MainWindow
    {
        // Data
        private readonly TestViewModel _testViewModel = new TestViewModel();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeEmailTestData();
            InitializeTextTestData();
        }

        public void InitializeEmailTestData()
        {
            XmlDocTb.Text = "";
            MissingObjectsLb.Items.Clear();
            TestBrowser.NavigateToString(" ");
            _testViewModel.TemplateDetail = new TemplateDetail();
            var media = new List<ActivityMedia>();
            _testViewModel.EmailActivityDetail = new EmailActivityDetail
            {
                FacilityID = 100,
                TemplateID = 1,
                CampaignID = 2,
                SendID = 3,
                ContractNumber = "ABC001",
                ContractID = 200,
                ActivityID = 10,
                PatientID = 20,
                PatientFirstName = "First",
                PatientLastName = "Last",
                PatientNameLF = "What is this",
                FacilityAddrLine1 = "Facility Address 1",
                FacilityAddrLine2 = "Facility Address 2",
                FacilityCity = "Dallas",
                FacilityState = "TX",
                FacilityZipCode = "77777",
                ProviderACDNumber = "2145551212",
                ToEmailAddress = "test@test.com",
                RecipientSchedID = 30,
                ScheduleDuration = 60,
                ScheduleDateTime = "11/20/2020 08:00:00 AM",
                TaskTypeCategory = TaskTypeCategory.OutreachRecall
            };

            _testViewModel.FacilityName = new ActivityMedia
            {
                OwnerID = _testViewModel.EmailActivityDetail.FacilityID,
                CategoryCode = "SNOVR",
                OwnerCode = "EMAIL",
                LanguagePreferenceCode = "EN",
                Narrative = "Name of Facility"
            };

            _testViewModel.FacilityLogo = new ActivityMedia
            {
                OwnerID = _testViewModel.EmailActivityDetail.FacilityID,
                CategoryCode = "LOGO",
                OwnerCode = "EMAIL",
                Filename = "logo.jpg"
            };

            _testViewModel.FacilityUrl = new ActivityMedia
            {
                OwnerID = _testViewModel.EmailActivityDetail.FacilityID,
                CategoryCode = "URL",
                OwnerCode = "EMAIL",
                Narrative = "facility.com"
            };

            _testViewModel.ScheduleName = new ActivityMedia
            {
                OwnerID = _testViewModel.EmailActivityDetail.RecipientSchedID,
                CategoryCode = "SCOVR",
                OwnerCode = "EMAIL",
                LanguagePreferenceCode = "EN",
                Narrative = "Test Schedule"
            };

            _testViewModel.FacilityEmailAddress = new ActivityMedia
            {
                OwnerID = _testViewModel.EmailActivityDetail.FacilityID,
                CategoryCode = "EMAIL",
                OwnerCode = "EMAIL",
                Narrative = "facility@test.com"
            };

            _testViewModel.FacilityReplyAddress = new ActivityMedia
            {
                OwnerID = _testViewModel.EmailActivityDetail.FacilityID,
                CategoryCode = "REPLY",
                OwnerCode = "EMAIL",
                Narrative = "facilityreply@test.com"
            };

            _testViewModel.FacilityDisplayName = new ActivityMedia
            {
                OwnerID = _testViewModel.EmailActivityDetail.FacilityID,
                CategoryCode = "NAME",
                OwnerCode = "EMAIL",
                Narrative = "Facility Display Name"
            };

            _testViewModel.AppointmentSpecificMessage = new ActivityMedia
            {
                OwnerID = _testViewModel.EmailActivityDetail.PatientID,
                CategoryCode = "TTC",
                OwnerCode = "EMAIL",
                LanguagePreferenceCode = "EN",
                FacilityID = _testViewModel.EmailActivityDetail.FacilityID,
                Narrative = "This is an appointment specific message."
            };

            media.Add(_testViewModel.FacilityName);
            media.Add(_testViewModel.FacilityLogo);
            media.Add(_testViewModel.FacilityUrl);
            media.Add(_testViewModel.ScheduleName);
            media.Add(_testViewModel.FacilityEmailAddress);
            media.Add(_testViewModel.FacilityReplyAddress);
            media.Add(_testViewModel.FacilityDisplayName);
            media.Add(_testViewModel.AppointmentSpecificMessage);

            _testViewModel.EmailActivityMedia = media;

            _testViewModel.ContractPermissions = new List<ContractPermission>();
            ContractPermission contractpermission = new ContractPermission
            {
                ChildObjectID = (int)Prompts.AppointmentSpecificMessage,
                RoleID = 0
            };
            _testViewModel.ContractPermissions.Add(contractpermission);

            DataContext = _testViewModel;
        }

        public void InitializeTextTestData()
        {
            XmlDocTb.Text = "";
            MissingObjectsLb.Items.Clear();
            TestBrowser.NavigateToString(" ");
            var media = new List<ActivityMedia>();
            _testViewModel.TextActivityDetail = new TextActivityDetail
            {
                FacilityID = 100,
                TemplateID = 1,
                CampaignID = 2,
                SendID = 3,
                ContractNumber = "ABC001",
                ContractID = 200,
                ActivityID = 10,
                PatientID = 20,
                PatientFirstName = "First",
                PatientLastName = "Last",
                PatientNameLF = "What is this",
                ProviderACDNumber = "2145551212",
                TextFromNumber = "8175551212",
                PhoneNumber = "2145551212",
                ScheduleDateTime = "11/20/2020 08:00:00 AM",
                ScheduleName = "Schedule Name",
                RecipientSchedID = 30,
                ScheduleDuration = 60,
                FacilityName = "Name",
                TaskTypeCategory = TaskTypeCategory.OutreachRecall,
            };

            _testViewModel.TextFacilityName = new ActivityMedia
            {
                OwnerID = _testViewModel.TextActivityDetail.FacilityID,
                CategoryCode = "SNOVR",
                OwnerCode = "TEXT",
                LanguagePreferenceCode = "EN",
                Narrative = "Facility Name"
            };

            _testViewModel.TextScheduleName = new ActivityMedia
            {
                OwnerID = _testViewModel.TextActivityDetail.RecipientSchedID,
                CategoryCode = "SCOVR",
                OwnerCode = "TEXT",
                LanguagePreferenceCode = "EN",
                Narrative = "Test Schedule"
            };

            media.Add(_testViewModel.TextFacilityName);
            media.Add(_testViewModel.TextScheduleName);

            _testViewModel.TextActivityMedia = media;

            DataContext = _testViewModel;
        }

        private void Reset_Email_Data_Click(object sender, RoutedEventArgs e)
        {
            InitializeEmailTestData();
        }

        private void Reset_Text_Data_Click(object sender, RoutedEventArgs e)
        {
            InitializeTextTestData();
        }

        private void Call_Email_Test_Method_Click(object sender, RoutedEventArgs e)
        {            
            MissingObjectsLb.Items.Clear();
            string selectedMethod = TestEmailMethodCb.Text;
            if (selectedMethod == "EmailTransform_Test")
            {
                _testViewModel.TemplateDetail.TemplateXMLBody = XmlDocTb.Text;
            }
            object[] parameters = { _testViewModel };
            TestViewModel results = Invoke<CommEmailTemplateManagerViewTest>(selectedMethod, parameters);
            if (selectedMethod == "EmailTransform_Test")
            {
                XmlDocTb.Text = results.TemplateDetail.TemplateXMLBody;
                MissingObjectsLb.Visibility = Visibility.Collapsed;
                TestBrowser.Visibility = Visibility.Visible;             
                TestBrowser.NavigateToString(System.Net.WebUtility.HtmlDecode(results.TransformResult));
            }
            else
            {
                MissingObjectsLb.Visibility = Visibility.Visible;
                TestBrowser.Visibility = Visibility.Collapsed;
                XmlDocTb.Text = results.XmlDocString;
                foreach (var mo in results.MissingObjects.Values)
                {
                    MissingObjectsLb.Items.Add(mo);
                }
            }
        }

        private void Call_Text_Test_Method_Click(object sender, RoutedEventArgs e)
        {
            MissingObjectsLb.Items.Clear();
            string selectedMethod = TestTextMethodCb.Text;
            if (selectedMethod == "TextTransform_Test")
            {
                _testViewModel.TemplateDetail.TemplateXMLBody = XmlDocTb.Text;
            }
            object[] parameters = { _testViewModel };
            TestViewModel results = Invoke<CommTextTemplateManagerViewTest>(selectedMethod, parameters);
            if (selectedMethod == "TextTransform_Test")
            {
                XmlDocTb.Text = results.TemplateDetail.TemplateXMLBody;
                MissingObjectsLb.Visibility = Visibility.Collapsed;
                TestBrowser.Visibility = Visibility.Visible;
                TestBrowser.NavigateToString(System.Net.WebUtility.HtmlDecode(results.TransformResult));
            }
            else
            {
                MissingObjectsLb.Visibility = Visibility.Visible;
                TestBrowser.Visibility = Visibility.Collapsed;
                XmlDocTb.Text = results.XmlDocString;
                foreach (var mo in results.MissingObjects.Values)
                {
                    MissingObjectsLb.Items.Add(mo);
                }
            }
        }

        // Invoke selected test method
        public TestViewModel Invoke<T>(string methodName, object[] parameters) where T : new()
        {
            var instance = new T();
            MethodInfo method = typeof (T).GetMethod(methodName);
            object results = method.Invoke(instance, parameters);
            if (results.GetType() == typeof (TestViewModel)) { return (TestViewModel)results; }
            return null;      
        }

        private void TestEmailMethodCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)TestEmailMethodCb.SelectedItem;
            if(typeItem.Content.ToString() == "EmailTransform_Test") { InitializeEmailTestData(); }
            TestEmailBtn.IsEnabled = !string.IsNullOrEmpty(typeItem.Content.ToString());
        }

        private void TestTextMethodCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)TestTextMethodCb.SelectedItem;
            TestTextBtn.IsEnabled = !string.IsNullOrEmpty(typeItem.Content.ToString());
        }
    }
}