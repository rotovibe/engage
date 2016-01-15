using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Services.Communication
{
    public static class TaskTypeCategory
    {
        public const string AppointmentReminder = "ACAPT";
        public const string AppointmentResponse = "ACRSP";
        public const string IntroductoryEmail = "ACINT";
        public const string IntroductoryText = "ACINT";
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
        public const string ResponseQueued = "Response Queued";
        public const string ResponseSent = "Response Sent";
        public const string ResponseDelivered = "Response Delivered";
        public const string ResponseFailed = "ResponseFailed";

        public const string TextDeliveryFailure = "Unable to deliver text";
        public const string TextFailed = "Text Failed";
        public const string TextQueued = "Text Queued";
        public const string TextSent = "Text Sent";
        public const string TextHelp = "Text Help";
        public const string TextStop = "Text Stop";
        public const string TextDelivered = "Text Delivered";
        public const string TextRecieved = "Text Inbound Recieved";
        public const string TextUnDelivered = "Text Undelivered";
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
        public const string ImagePathSetting = "ImagePath";
        public const string Enable = "Enable";
        public const string CommRequestId = "CommRequestId";
        public const string CommRequestSource = "CommRequestSource";

        public const string EmailAlertNode = BaseNode + "EmailAlert";
        public const string EmailAlertSender = "EmailAlertSender";
        public const string EmailAlertRecipient = "EmailAlertRecipient";

        #region ModeSpecificTags
        public const string ModePlaceHolder = "{mode}";

        //EmailHeader
        public const string ModeSendID = "{mode}/SendID";
        public const string ModeActivityID = "{mode}/ActivityID";
        public const string ModeEmailContractID = "{mode}/ContractID";

        public const string ModeConfirmationURL = "{mode}/ConfirmationURL[@Enable='{0}']";
        public const string ModeOptOutURL = "{mode}/OptOutURL[@Enable='{0}']";
        public const string ModeRescheduleURL = "{mode}/RescheduleURL[@Enable='{0}']";
        public const string ModeCancelURL = "{mode}/CancelURL[@Enable='{0}']";

        public const string ModeImagePath = "{mode}/ImagePath[@Enable='{0}']";
        //Patient
        public const string ModePatientID = "{mode}/Patient/PatientID";
        public const string ModePatientFullName = "{mode}/Patient/FullName";
        public const string ModePatientFirstName = "{mode}/Patient/FirstName";
        public const string ModePatientLastName = "{mode}/Patient/LastName";
        //Schedule
        public const string ModeScheduleID = "{mode}/Schedule/ScheduleID";
        public const string ModeScheduleFullName = "{mode}/Schedule/FullName";
        public const string ModeScheduleDisplayName = "{mode}/Schedule/DisplayName";

        //Facility
        public const string ModeFacilityID = "{mode}/Facility/FacilityID";
        public const string ModeFacilityName = "{mode}/Facility/Name";
        public const string ModeFacilityDisplayName = "{mode}/Facility/DisplayName";
        public const string ModeFacilityAddr1 = "{mode}/Facility/Addr1";
        public const string ModeFacilityAddr2 = "{mode}/Facility/Addr2";
        public const string ModeFacilityCity = "{mode}/Facility/City";
        public const string ModeFacilityState = "{mode}/Facility/State";
        public const string ModeFacilityZip = "{mode}/Facility/Zip";
        public const string ModeFacilityPhoneNumber = "{mode}/Facility/PhoneNumber";
        public const string ModeFacilityLogo = "{mode}/Facility/FacilityLogo";
        public const string ModeFacilityURL = "{mode}/Facility/FacilityURL";
        //Message
        public const string ModeMessageType = "{mode}/Message/Type";
        public const string ModeApptDayOfWeek = "{mode}/Message/DayOfWeek";
        public const string ModeApptMonth = "{mode}/Message/Month";
        public const string ModeApptDate = "{mode}/Message/Date";
        public const string ModeApptYear = "{mode}/Message/Year";
        public const string ModeApptTime = "{mode}/Message/Time";
        public const string ModeApptDuration = "{mode}/Message/Duration";
        public const string ModeAppointmentSpecificMessage = "{mode}/Message/AppointmentSpecificMessage";
        public const string ModeFromEmailAddress = "{mode}/Message/FromEmailAddress";
        public const string ModeReplyToEmailAddress = "{mode}/Message/ReplyToEmailAddress";
        public const string ModeToEmailAddress = "{mode}/Message/ToEmailAddress";
        public const string ModeDisplayName = "{mode}/Message/DisplayName";
        public const string ModeMessageSubject = "{mode}/Message/Subject";
        public const string ModeMessageBody = "{mode}/Message/Body";
        public const string ModeFromPhoneNumber = "{mode}/Message/TextFromNumber";
        public const string ModeToPhoneNumber = "{mode}/Message/TextToNumber";
        public const string ModeHelpPhoneNumber = "{mode}/Message/TextHelpNumber";
        public const string ModeProviderName = "{mode}/Message/ProviderName";
        

        public const string ModeContractID = "{mode}/ContractID";

        #endregion

        public const string EmailDebugNode = BaseNode + "EmailDebug";
        public const string DebugEmail = "IsEmailDebug";
        public const string DebugEmailRecipients = "DebugEmailRecipients";
        public const string TransactionGroupIdNode = BaseNode + "TransactionGroupIds";
        public const string ACTransactionGroupID = "ACTransactionGroupID";
        public const string ORTransactionGroupID = "ORTransactionGroupID";
        public const string ConfirmOptOutURLSettingNode = BaseNode + "ConfirmationURL";
        public const string ConfirmURLSetting = "ConfirmURL";
        public const string OptOutURLSetting = "OptOutURL";

        public const string CommURIsNode = BaseNode + "BaseCommURIs";
        public const string CommURI = "CommAppUrl";
        public const string CommunicationURI = "CommDataUrl";
        public const string ModeNode = BaseNode + "Mode";
        public const string ContractURI = "ContractUrl";
        
    }

    public static class EmailAlert
    {
        public const string MissingEmailConfiguration = "Communication Platform - Missing email configuration";
    }

    public static class TextAlert
    {
        public const string MissingEmailConfiguration = "Communication Platform - Missing text configuration";
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
