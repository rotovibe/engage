using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Net.Mail;
using System.Xml;
using System.Configuration;


namespace JangoEmailResultsService
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class EmailEventTrackingService
    {
        [WebGet(UriTemplate = "/TrackSentEvent?TransEmailID={transEmailID}&UserID={userID}&ToEmailAddress={toEmailAddress}&Subject={subject}&TransGroupID={transGroupID}&SentTime={sentTime}&OpenTrackEnabled={openTrackEnabled}&ClickTrackEnabled={clickTrackEnabled}&FromEmailAddress={fromEmailAddress}&MessageSize={messageSize}&CustomID={customID}", BodyStyle=WebMessageBodyStyle.Wrapped), OperationContract]
        public bool TrackSentEvent(string transEmailID, string userID, string toEmailAddress, string subject, string transGroupID, string sentTime, string openTrackEnabled, string clickTrackEnabled, string fromEmailAddress, string messageSize, string customID)
        {
            try{
                //this will update the InProcess column on the SendQueue Table to 50
                //update the NotifySender column on the Activity table to 171 'Email Sent'
                //and create a new record on the CommunicationEventHistory table

                CommAPI.Data.SaveEmailResultsRequest req = new CommAPI.Data.SaveEmailResultsRequest();
                req.EventType = "EmailSent";
                req.ErrorMessage = "Email Sent";
                req.ActivityNotifySender = 171;
                if (!string.IsNullOrEmpty(toEmailAddress))
                {
                    req.ToEmailAddress = toEmailAddress;
                    req.CommunicationSource = toEmailAddress;
                }
                if (!string.IsNullOrEmpty(sentTime))
                    req.EventDateTime = Convert.ToDateTime(sentTime);
                if (!string.IsNullOrEmpty(fromEmailAddress))
                    req.FromEmailAddress = fromEmailAddress;
                if (!string.IsNullOrEmpty(customID))
                {
                    string[] customIDParts = customID.Split('^');
                    if (customIDParts[0] != null)
                        req.ContractNumber = customIDParts[0];
                    if (customIDParts[1] != null)
                        req.SendID = Convert.ToInt32(customIDParts[1]);
                }
                req.SaveFlag = 1;
                req.Inprocess = 50;
                req.CommCallDistributorTimeOfCall = DateTime.Now;
                req.SaveActivityAndSendQueueFlag = 1;
                req.Status = "UNCNF";
                req.ActivityStatus = "UNCNF";
                req.ActivityStatusDate = DateTime.Now;
                req.CommCallID = transEmailID;

                CommAPI.Services.Email.ResultsService resultsSvc = new CommAPI.Services.Email.ResultsService();
                resultsSvc.SaveResults(req);

                
            }
            catch (Exception ex)
            {
                string processIDString = ConfigurationManager.AppSettings.Get("ProcessID");
                int processID = 0;
                int.TryParse(processIDString, out processID);
                Phytel.Framework.ASE.Process.Log.LogError(processID, string.Format("JangoEmailResultsService - TrackSentEvent : Invalid communication results for CustomID : " + customID), Phytel.Framework.ASE.Common.LogErrorCode.Error, Phytel.Framework.ASE.Common.LogErrorSeverity.High, ex.Message);
                return false;
            }
            return true;
        }

        [WebGet(UriTemplate = "/TrackOpenEvent?UserID={userID}&TransEmailID={transEmailID}&TransGroupID={transGroupID}&ToEmailAddress={toEmailAddress}&OpenTime={openTime}&IPAddress={ipAddress}&Browser={browser}&CustomID={customID}", BodyStyle = WebMessageBodyStyle.Wrapped), OperationContract]
        public bool TrackOpenEvent(string userID, string transEmailID, string transGroupID, string toEmailAddress, string openTime, string ipAddress, string browser, string customID)
        {
            try{
                //this will update the InProcess column on the SendQueue Table to 50
                //(this should already be 50 because of the Sent Event but in case it is not this will take care of it)
                //update the NotifySender column on the Activity table to 172 'Email Opened'
                //and create a new record on the CommunicationEventHistory table

                CommAPI.Data.SaveEmailResultsRequest req = new CommAPI.Data.SaveEmailResultsRequest();
                req.EventType = "EmailOpened";
                req.ErrorMessage = "Email Opened";
                req.ActivityNotifySender = 172;
                if (!string.IsNullOrEmpty(toEmailAddress))
                    req.ToEmailAddress = toEmailAddress;
                if (!string.IsNullOrEmpty(openTime))
                    req.EventDateTime = Convert.ToDateTime(openTime);
                if (!string.IsNullOrEmpty(ipAddress))
                    req.CommDialerIP = ipAddress;
                if (!string.IsNullOrEmpty(browser))
                    req.CommVXMLBrowser = browser;
                if (!string.IsNullOrEmpty(customID))
                {
                    string[] customIDParts = customID.Split('^');
                    if (customIDParts[0] != null)
                        req.ContractNumber = customIDParts[0];
                    if (customIDParts[1] != null)
                        req.SendID = Convert.ToInt32(customIDParts[1]);
                }
                req.SaveFlag = 1;
                req.Inprocess = 50;
                req.CommCallDistributorTimeOfCall = DateTime.Now;
                req.SaveActivityAndSendQueueFlag = 0;
                req.CommCallID = transEmailID;

                CommAPI.Services.Email.ResultsService resultsSvc = new CommAPI.Services.Email.ResultsService();
                resultsSvc.SaveResults(req);

             
            }
            catch (Exception ex)
            {
                string processIDString = ConfigurationManager.AppSettings.Get("ProcessID");
                int processID = 0;
                int.TryParse(processIDString, out processID);
                Phytel.Framework.ASE.Process.Log.LogError(processID, string.Format("JangoEmailResultsService - TrackOpenEvent : Invalid communication results for CustomID : " + customID), Phytel.Framework.ASE.Common.LogErrorCode.Error, Phytel.Framework.ASE.Common.LogErrorSeverity.High, ex.Message);
                return false;
            }
            return true;
        }

        [WebGet(UriTemplate = "/TrackClickEvent?UserID={userID}&TransEmailID={transEmailID}&TransGroupID={transGroupID}&ToEmailAddress={toEmailAddress}&URL={url}&Position={position}&HTML={html}&ClickTime={clickTime}&IPAddress={ipAddress}&Browser={browser}&CustomID={customID}", BodyStyle = WebMessageBodyStyle.Wrapped), OperationContract]
        public bool TrackClickEvent(string userID, string transEmailID, string transGroupID, string toEmailAddress, string url, string position, string html, string clickTime, string ipAddress, string browser, string customID)
        {
            try{
                //this event will handle the click event tracking as well as the unsubscribe tracking if the link clicked was an unsubscribe link

                //this will create a new record on the CommunicationEventHistory table with Notify Sender 175 'Email Clicked'
            
                CommAPI.Data.SaveEmailResultsRequest req = new CommAPI.Data.SaveEmailResultsRequest();
                req.EventType = "EmailClicked";
                req.ErrorMessage = "Email Clicked";
                req.ActivityNotifySender = 175;
                if (!string.IsNullOrEmpty(toEmailAddress))
                    req.ToEmailAddress = toEmailAddress;
                if (!string.IsNullOrEmpty(clickTime))
                    req.EventDateTime = Convert.ToDateTime(clickTime);
                if (!string.IsNullOrEmpty(ipAddress))
                    req.CommDialerIP = ipAddress;
                if (!string.IsNullOrEmpty(browser))
                    req.CommVXMLBrowser = browser;
                if (!string.IsNullOrEmpty(url))
                    req.CommApplicationURL = url;
                if (!string.IsNullOrEmpty(position))
                    req.ErrorMessage += " - Position: " + position;
                if (!string.IsNullOrEmpty(position))
                    req.ErrorMessage += " - IsHTML: " + html;
                if (!string.IsNullOrEmpty(customID))
                {
                    string[] customIDParts = customID.Split('^');
                    if (customIDParts[0] != null)
                        req.ContractNumber = customIDParts[0];
                    if (customIDParts[1] != null)
                        req.SendID = Convert.ToInt32(customIDParts[1]);
                }
                req.SaveFlag = 1;
                req.CommCallDistributorTimeOfCall = DateTime.Now;
                req.SaveActivityAndSendQueueFlag = 0;
                req.CommCallID = transEmailID;


                CommAPI.Services.Email.ResultsService resultsSvc = new CommAPI.Services.Email.ResultsService();
                resultsSvc.SaveResults(req);

                
            }
            catch (Exception ex)
            {
                string processIDString = ConfigurationManager.AppSettings.Get("ProcessID");
                int processID = 0;
                int.TryParse(processIDString, out processID);
                Phytel.Framework.ASE.Process.Log.LogError(processID, string.Format("JangoEmailResultsService - TrackClickEvent : Invalid communication results for CustomID : " + customID), Phytel.Framework.ASE.Common.LogErrorCode.Error, Phytel.Framework.ASE.Common.LogErrorSeverity.High, ex.Message);
                return false;
            }
            return true;
        }

        [WebGet(UriTemplate = "/TrackBounceEvent?UserID={userID}&ToEmailAddress={toEmailAddress}&BounceTime={bounceTime}&DiagnosticCode={diagnosticCode}&Definitive={definitive}&TransGroupID={transGroupID}&TransEmailID={transEmailID}&CustomID={customID}", BodyStyle = WebMessageBodyStyle.Wrapped), OperationContract]
        public bool TrackBounceEvent(string userID, string toEmailAddress, string bounceTime, string diagnosticCode, string definitive, string transGroupID, string transEmailID, string customID)
        {
            try{
                
                //this will update the NotifySender column on the Activity table to 176 'Email Bounced'
                //and create a new record on the CommunicationEventHistory table

                CommAPI.Data.SaveEmailResultsRequest req = new CommAPI.Data.SaveEmailResultsRequest();
                req.EventType = "EmailBounced";
                req.ErrorMessage = "Email Bounced";
                req.ActivityNotifySender = 176;
                if (!string.IsNullOrEmpty(toEmailAddress))
                    req.ToEmailAddress = toEmailAddress;
                if (!string.IsNullOrEmpty(bounceTime))
                    req.EventDateTime = Convert.ToDateTime(bounceTime);
                if (!string.IsNullOrEmpty(diagnosticCode))
                    req.ErrorMessage += " - Code: " + diagnosticCode;
                if (!string.IsNullOrEmpty(definitive))
                    req.ErrorMessage += " - IsDefinitive: " + definitive;
                if (!string.IsNullOrEmpty(customID))
                {
                    string[] customIDParts = customID.Split('^');
                    if (customIDParts[0] != null)
                        req.ContractNumber = customIDParts[0];
                    if (customIDParts[1] != null)
                        req.SendID = Convert.ToInt32(customIDParts[1]);
                }
                req.SaveFlag = 1;
                req.CommCallDistributorTimeOfCall = DateTime.Now;
                req.Inprocess = 50;
                req.SaveActivityAndSendQueueFlag = 1;
                req.Status = "UNCNF";
                req.ActivityStatus = "UNCNF";
                req.ActivityStatusDate = DateTime.Now;
                req.CommCallID = transEmailID;


                CommAPI.Services.Email.ResultsService resultsSvc = new CommAPI.Services.Email.ResultsService();
                resultsSvc.SaveResults(req);

            }
            catch (Exception ex)
            {
                string processIDString = ConfigurationManager.AppSettings.Get("ProcessID");
                int processID = 0;
                int.TryParse(processIDString, out processID);
                Phytel.Framework.ASE.Process.Log.LogError(processID, string.Format("JangoEmailResultsService - TrackBounceEvent : Invalid communication results for CustomID : " + customID), Phytel.Framework.ASE.Common.LogErrorCode.Error, Phytel.Framework.ASE.Common.LogErrorSeverity.High, ex.Message);
                return false;
            }

            return true;
        }

        [WebGet(UriTemplate = "/TrackUnsubscribeEvent?UserID={userID}&ToEmailAddress={toEmailAddress}&UnsubscribeType={unsubscribeType}&UnsubscribeTime={unsubscribeTime}&InsertedOn={insertedOn}&IPAddress={ipAddress}&TransGroupID={transGroupID}&TransEmailID={transEmailID}&CustomID={customID}", BodyStyle = WebMessageBodyStyle.Wrapped), OperationContract]
        public bool TrackUnsubscribeEvent(string userID, string toEmailAddress, string unsubscribeType, string unsubscribeTime, string insertedOn, string ipAddress, string transGroupID, string transEmailID, string customID)
        {
            try{
                //this will update the NotifySender column on the Activity table to 177 'Email Opted Out'
                //and create a new record on the CommunicationEventHistory table

                CommAPI.Data.SaveEmailResultsRequest req = new CommAPI.Data.SaveEmailResultsRequest();
                req.EventType = "EmailOptedOut";
                req.ErrorMessage = "Email Opted Out";
                req.ActivityNotifySender = 177;
                if (!string.IsNullOrEmpty(toEmailAddress))
                    req.ToEmailAddress = toEmailAddress;
                if (!string.IsNullOrEmpty(unsubscribeType))
                    req.ErrorMessage += " - Type: " + unsubscribeType;
                if (!string.IsNullOrEmpty(unsubscribeTime))
                    req.EventDateTime = Convert.ToDateTime(unsubscribeTime);
                if (!string.IsNullOrEmpty(insertedOn))
                    req.ErrorMessage += " - InsertedOn: " + insertedOn;
                if (!string.IsNullOrEmpty(ipAddress))
                    req.CommDialerIP = ipAddress;
                if (!string.IsNullOrEmpty(customID))
                {
                    string[] customIDParts = customID.Split('^');
                    if (customIDParts[0] != null)
                        req.ContractNumber = customIDParts[0];
                    if (customIDParts[1] != null)
                        req.SendID = Convert.ToInt32(customIDParts[1]);
                }
                req.SaveFlag = 1;
                req.CommCallDistributorTimeOfCall = DateTime.Now;
                req.SaveActivityAndSendQueueFlag = 0;
                req.CommCallID = transEmailID;


                CommAPI.Services.Email.ResultsService resultsSvc = new CommAPI.Services.Email.ResultsService();
                resultsSvc.SaveResults(req);

             
            }
            catch (Exception ex)
            {
                string processIDString = ConfigurationManager.AppSettings.Get("ProcessID");
                int processID = 0;
                int.TryParse(processIDString, out processID);
                Phytel.Framework.ASE.Process.Log.LogError(processID, string.Format("JangoEmailResultsService - TrackUnsubscribeEvent : Invalid communication results for CustomID : " + customID), Phytel.Framework.ASE.Common.LogErrorCode.Error, Phytel.Framework.ASE.Common.LogErrorSeverity.High, ex.Message);
                return false;
            }
            return true;
        }

        [WebGet(UriTemplate = "/TrackComplaintEvent?UserID={userID}&ToEmailAddress={toEmailAddress}&ComplaintTime={complaintTime}&AbuseID={abuseID}&TransGroupID={transGroupID}&TransEmailID={transEmailID}&CustomID={customID}", BodyStyle = WebMessageBodyStyle.Wrapped), OperationContract]
        public bool TrackComplaintEvent(string userID, string toEmailAddress, string complaintTime, string abuseID, string transGroupID, string transEmailID, string customID)
        {
            try{
                //this will update the NotifySender column on the Activity table to 178 'Email Complaint'
                //and create a new record on the CommunicationEventHistory table

                CommAPI.Data.SaveEmailResultsRequest req = new CommAPI.Data.SaveEmailResultsRequest();
                req.EventType = "EmailComplaint";
                req.ErrorMessage = "Email Complaint";
                req.ActivityNotifySender = 178;
                if (!string.IsNullOrEmpty(toEmailAddress))
                    req.ToEmailAddress = toEmailAddress;
                if (!string.IsNullOrEmpty(complaintTime))
                    req.EventDateTime = Convert.ToDateTime(complaintTime);
                if (!string.IsNullOrEmpty(abuseID))
                    req.ErrorMessage += " - AbuseID: " + abuseID;
                if (!string.IsNullOrEmpty(customID))
                {
                    string[] customIDParts = customID.Split('^');
                    if (customIDParts[0] != null)
                        req.ContractNumber = customIDParts[0];
                    if (customIDParts[1] != null)
                        req.SendID = Convert.ToInt32(customIDParts[1]);
                }
                req.SaveFlag = 1;
                req.CommCallDistributorTimeOfCall = DateTime.Now;
                req.SaveActivityAndSendQueueFlag = 0;
                req.CommCallID = transEmailID;


                CommAPI.Services.Email.ResultsService resultsSvc = new CommAPI.Services.Email.ResultsService();
                resultsSvc.SaveResults(req);
             
            }
            catch (Exception ex)
            {
                string processIDString = ConfigurationManager.AppSettings.Get("ProcessID");
                int processID = 0;
                int.TryParse(processIDString, out processID);
                Phytel.Framework.ASE.Process.Log.LogError(processID, string.Format("JangoEmailResultsService - TrackComplaintEvent : Invalid communication results for CustomID : " + customID), Phytel.Framework.ASE.Common.LogErrorCode.Error, Phytel.Framework.ASE.Common.LogErrorSeverity.High, ex.Message);
                return false;
            }
            
            return true;
        }
        

        [WebGet(UriTemplate = ""), OperationContract]
        public bool GetEvent()
        {
            return true;
        }


    }
}
