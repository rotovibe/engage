using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Outreach.CommMgr
{
    public static class TaskTypeCategory
    {
        public const string AppointmentReminder = "ACAPT";
        public const string IntroductoryEmail = "ACINT";
        public const string OutreachRecall = "ACMOT";
    }
    public static class SendQueueErrorMessage
    {
        //public const string InvalidEmailAddress = "Invalid EmailAddress";
        public const string InvalidActivityConfiguration = "Invalid email activity configuration";
        //public const string EmptyPhoneNumber = "No Notification Phone Number";
        //public const string ActivityAlreadyAccessed = "Activity Already Accessed";
        public const string ActivityAlreadyComplete = "Activity Already Completed Or Problem With Media";
        public const string NotificationAlreadyDelivered = "Notification Already Delivered";
        public const string PendingNotificationCanceled = "Notification Cancelled, Activity Status = PEND";
        //public const string NewNotificationCanceled = "Notification Cancelled, Activity Status = NEW";
        public const string ActivityNotFound = "Activity Not Found";
        //public const string CallStilllNeededFailed = "Unable to determine if CallStillNeeded";
        public const string AfterNormalHours = "After Normal Delivery Hr of ";
        public const string BeforeNormalHours = "Before Normal Delivery Hr of ";
        //public const string UnsupportedLanguage = "Unsupported communication language";
        public const string EmailDeliveryFailure = "Unable to deliver email";
        public const string EmailBounced = "Email Bounced";
    }

    public static class SendQueueStatus
    {
        public const string Failed = "FAILD";
        public const string Cancel = "CANCL";
        public const string Confirm = "CNF";
        public const string Pending = "PEND";
        public const string Reschedule = "RESCH";
        public const string Unconfirmed = "UNCNF";
    }

    public static class ActivityStatus
    {
        public const string Cancel = "CANCL";
        public const string Confirm = "CNF";
        public const string Failed = "FAILD";
        public const string Pending = "PEND";
        public const string Reschedule = "RESCH";
        public const string Unconfirmed = "UNCNF";
        public const string Successful = "ACCES";

    }

    public static class XMLFields
    {
        public const string BaseNode = "/Phytel.ASE.Process/ProcessConfiguration/";
        public const string ScheduleNode = BaseNode + "Schedules/Schedule";
        public const string AlertSettingsNode = BaseNode + "AlertSettings";
        public const string TemplateSettingsNode = BaseNode + "TemplateSettings";
        public const string SettingsNode = BaseNode + "Settings";
        public const string RouteNode = BaseNode + "Routes/Route";
        public const string RoundRobinNode = BaseNode + "RoundRobinQueues";
        public const string RoundRobinQueueNode = BaseNode + "RoundRobinQueues/Queue";
        public const string MediaSettingsNode = BaseNode + "MediaSettings[@ServerID='{0}']";
        public const string MediaServer = "MediaServer";
        public const string QueueRoute = "Routes/Route";
        public const string IsNewCommPlatform = "IsNewCommPlatform";
        public const string MessagesPerQueue = "MessagesPerQueue";
        public const string ContractID = "contractId";
        public const string ContractNumber = "contractNumber";
        public const string ScheduleID = "scheduleId";
        public const string BatchID = "batchId";
        public const string CommunicationTypeID = "communicationTypeId";
        public const string ActivitiesPerBatch = "ActivitiesPerBatch";
        public const string ACTemplateType = "AppointmentReminder";
        public const string OutreachTemplateType = "Outreach";
        public const string EmailAlertNode = BaseNode + "EmailAlert";
        public const string EmailAlertSender = "EmailAlertSender";
        public const string EmailAlertRecipient = "EmailAlertRecipient";
        
        public const string ImagePathSetting = "ImagePath";
        //EmailHeader
        public const string SendID = "Email/SendID";
        public const string ActivityID = "Email/ActivityID";
        public const string EmailContractID = "Email/ContractID";

        public const string ConfirmationURL = "Email/ConfirmationURL[@Enable='{0}']";
        public const string OptOutURL = "Email/OptOutURL[@Enable='{0}']";
        public const string RescheduleURL = "Email/RescheduleURL[@Enable='{0}']";
        public const string CancelURL = "Email/CancelURL[@Enable='{0}']";

        public const string ImagePath = "Email/ImagePath[@Enable='{0}']";
        //Patient
        public const string PatientID = "Email/Patient/PatientID";
        public const string PatientFullName = "Email/Patient/FullName";
        public const string PatientFirstName = "Email/Patient/FirstName";
        public const string PatientLastName = "Email/Patient/LastName";
        //Schedule
        public const string EmailScheduleID = "Email/Schedule/ScheduleID";
        public const string ScheduleFullName = "Email/Schedule/FullName";

        //Facility
        public const string FacilityID = "Email/Facility/FacilityID";
        public const string FacilityName = "Email/Facility/Name";
        public const string FacilityAddr1 = "Email/Facility/Addr1";
        public const string FacilityAddr2 = "Email/Facility/Addr2";
        public const string FacilityCity = "Email/Facility/City";
        public const string FacilityState = "Email/Facility/State";
        public const string FacilityZip = "Email/Facility/Zip";
        public const string FacilityPhoneNumber = "Email/Facility/PhoneNumber";
        public const string FacilityLogo = "Email/Facility/FacilityLogo";
        public const string FacilityURL = "Email/Facility/FacilityURL";
        //Message
        public const string MessageType = "Email/Message/Type";
        public const string ApptDayOfWeek = "Email/Message/DayOfWeek";
        public const string ApptMonth = "Email/Message/Month";
        public const string ApptDate = "Email/Message/Date";
        public const string ApptYear = "Email/Message/Year";
        public const string ApptTime = "Email/Message/Time";
        public const string ApptDuration = "Email/Message/Duration";
        public const string AppointmentSpecificMessage = "Email/Message/AppointmentSpecificMessage";
        public const string FromEmailAddress = "Email/Message/FromEmailAddress";
        public const string ReplyToEmailAddress = "Email/Message/ReplyToEmailAddress";
        public const string ToEmailAddress = "Email/Message/ToEmailAddress";
        public const string DisplayName = "Email/Message/DisplayName";
        public const string MessageSubject = "Email/Message/Subject";
        public const string MessageBody = "Email/Message/Body";

        public const string Enable = "Enable";

        public const string EmailFacilityID = "Email/Facility/FacilityID";

        public const string EmailDebugNode = BaseNode + "EmailDebug";
        public const string DebugEmail = "IsEmailDebug";
        public const string DebugEmailRecipients = "DebugEmailRecipients";
        public const string TransactionGroupIdNode = BaseNode + "TransactionGroupIds";
        public const string ACTransactionGroupID = "ACTransactionGroupID";
        public const string ORTransactionGroupID = "ORTransactionGroupID";
        public const string ConfirmOptOutURLSettingNode = BaseNode + "ConfirmationURL";
        public const string ConfirmURLSetting = "ConfirmURL";
        public const string OptOutURLSetting = "OptOutURL";
        public const string BaseCommURIsNode = BaseNode + "BaseCommURIs";
        public const string BaseCommURI = "CommAppUrl";
        public const string BaseCommunicationURI = "CommDataUrl";
    }

    public static class EmailAlert
    {
        public const string MissingEmailConfiguration = "Communication Platform - Missing email configuration";
    }

    public static class RegExPatterns
    {
        public const string PhoneNumberLengthPattern = "^[1-9][0-9]{9}$";
        public const string AreaCodePhoneNumberPattern = "(\\d)(?!\\1+$)\\d{9}";
        public const string PhoneNumberPattern = "[1-9][0-9][0-9](\\d)(?!\\1+$)\\d{6}";
        public const string EmailAddressPatern = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                                    @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                                    @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";


    }

    public static class MessageTypes
    {
        public const string AppointmentReminder = "Appointment Reminder ";
        public const string IntroductoryEmail = "IntroductoryEmail";
        public const string OutreachRecall = "Health Reminder ";

    }
}
