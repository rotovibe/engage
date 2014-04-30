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
using Atmosphere.Communication.Business.Concrete;
using Atmosphere.Communication.Data.Interfaces;
using Atmosphere.Communication.Data;



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
            bool isSuccess = false;
            try{

                RestCommRepository commRepository = new RestCommRepository();
                CommResult req = new CommResult();
                req.EmailResultStatistics = new CommEmailResultStatistics();
                req.EmailResultStatistics.VendorTransID = transEmailID;
                req.EmailResultStatistics.TransactionGroupID = transGroupID;
                req.EmailResultStatistics.CustomID = customID;

                if (!string.IsNullOrEmpty(toEmailAddress))
                {
                    req.CommunicationSource = toEmailAddress;
                }
                if (!string.IsNullOrEmpty(sentTime))
                    req.TimeOfComm = Convert.ToDateTime(sentTime);
               
                isSuccess = commRepository.SaveSentEvent(req);

              
            }
            catch (Exception ex)
            {
                string processIDString = ConfigurationManager.AppSettings.Get("ASEProcessID");
                int processID = 0;
                int.TryParse(processIDString, out processID);
                Phytel.Framework.ASE.Process.Log.LogError(processID, string.Format("JangoEmailResultsService - TrackSentEvent : Invalid communication results for CustomID : " + customID), Phytel.Framework.ASE.Data.Common.LogErrorCode.Error, Phytel.Framework.ASE.Data.Common.LogErrorSeverity.High, ex.Message);
                isSuccess = false;
            }
            return isSuccess;
        }

        [WebGet(UriTemplate = "/TrackOpenEvent?UserID={userID}&TransEmailID={transEmailID}&TransGroupID={transGroupID}&ToEmailAddress={toEmailAddress}&OpenTime={openTime}&IPAddress={ipAddress}&Browser={browser}&CustomID={customID}", BodyStyle = WebMessageBodyStyle.Wrapped), OperationContract]
        public bool TrackOpenEvent(string userID, string transEmailID, string transGroupID, string toEmailAddress, string openTime, string ipAddress, string browser, string customID)
        {
            bool isSuccess = false;
            try{
                 RestCommRepository commRepository = new RestCommRepository();
                 CommResult req = new CommResult();
                 req.EmailResultStatistics = new CommEmailResultStatistics();
                 req.EmailResultStatistics.VendorTransID = transEmailID;
                 req.EmailResultStatistics.TransactionGroupID = transGroupID;
                 req.EmailResultStatistics.CustomID = customID;

                 if (!string.IsNullOrEmpty(toEmailAddress))
                 {
                     req.CommunicationSource = toEmailAddress;
                 }
                 if (!string.IsNullOrEmpty(openTime))
                     req.TimeOfComm = Convert.ToDateTime(openTime);
                 if (!string.IsNullOrEmpty(ipAddress))
                     req.EmailResultStatistics.IPAddress = ipAddress;
                 if (!string.IsNullOrEmpty(browser))
                     req.EmailResultStatistics.Browser = browser;
                 

                 isSuccess = commRepository.SaveOpenEvent(req);
            }
            catch (Exception ex)
            {
                string processIDString = ConfigurationManager.AppSettings.Get("ASEProcessID");
                int processID = 0;
                int.TryParse(processIDString, out processID);
                Phytel.Framework.ASE.Process.Log.LogError(processID, string.Format("JangoEmailResultsService - TrackOpenEvent : Invalid communication results for CustomID : " + customID), Phytel.Framework.ASE.Data.Common.LogErrorCode.Error, Phytel.Framework.ASE.Data.Common.LogErrorSeverity.High, ex.Message);
                isSuccess= false;
            }
            return isSuccess;
        }

        [WebGet(UriTemplate = "/TrackClickEvent?UserID={userID}&TransEmailID={transEmailID}&TransGroupID={transGroupID}&ToEmailAddress={toEmailAddress}&URL={url}&Position={position}&HTML={html}&ClickTime={clickTime}&IPAddress={ipAddress}&Browser={browser}&CustomID={customID}", BodyStyle = WebMessageBodyStyle.Wrapped), OperationContract]
        public bool TrackClickEvent(string userID, string transEmailID, string transGroupID, string toEmailAddress, string url, string position, string html, string clickTime, string ipAddress, string browser, string customID)
        {
            bool isSuccess = false;
            try{
                RestCommRepository commRepository = new RestCommRepository();

                CommResult req = new CommResult();
                req.EmailResultStatistics = new CommEmailResultStatistics();
                req.EmailResultStatistics.VendorTransID = transEmailID;
                req.EmailResultStatistics.TransactionGroupID = transGroupID;
                req.EmailResultStatistics.CustomID = customID;
                if (!string.IsNullOrEmpty(toEmailAddress))
                {
                    req.CommunicationSource = toEmailAddress;
                }
                if (!string.IsNullOrEmpty(clickTime))
                    req.TimeOfComm = Convert.ToDateTime(clickTime);
                if (!string.IsNullOrEmpty(ipAddress))
                {
                    req.EmailResultStatistics.IPAddress = ipAddress;
                    req.CommDialerIP = ipAddress;
                }
                if (!string.IsNullOrEmpty(browser))
                {
                    req.EmailResultStatistics.Browser = browser;
                    req.CommVXMLBrowser = browser;
                }

                if (!string.IsNullOrEmpty(url))
                {
                    req.EmailResultStatistics.URL = url;
                    req.CommApplicationURL = url;
                }
                req.ErrorMessage = "Email Clicked";
                if (!string.IsNullOrEmpty(position))
                    req.ErrorMessage += " - Position: " + position;
                if (!string.IsNullOrEmpty(position))
                    req.ErrorMessage += " - IsHTML: " + html;

                isSuccess = commRepository.SaveClickEvent(req);
               
            }
            catch (Exception ex)
            {
                string processIDString = ConfigurationManager.AppSettings.Get("ASEProcessID");
                int processID = 0;
                int.TryParse(processIDString, out processID);
                Phytel.Framework.ASE.Process.Log.LogError(processID, string.Format("JangoEmailResultsService - TrackClickEvent : Invalid communication results for CustomID : " + customID), Phytel.Framework.ASE.Data.Common.LogErrorCode.Error, Phytel.Framework.ASE.Data.Common.LogErrorSeverity.High, ex.Message);
                isSuccess =  false;
            }
            return isSuccess;
        }

        [WebGet(UriTemplate = "/TrackBounceEvent?UserID={userID}&ToEmailAddress={toEmailAddress}&BounceTime={bounceTime}&DiagnosticCode={diagnosticCode}&Definitive={definitive}&TransGroupID={transGroupID}&TransEmailID={transEmailID}&CustomID={customID}", BodyStyle = WebMessageBodyStyle.Wrapped), OperationContract]
        public bool TrackBounceEvent(string userID, string toEmailAddress, string bounceTime, string diagnosticCode, string definitive, string transGroupID, string transEmailID, string customID)
        {
            bool isSuccess = false;
            try{
                RestCommRepository commRepository = new RestCommRepository();

                CommResult req = new CommResult();
                req.EmailResultStatistics = new CommEmailResultStatistics();
                req.EmailResultStatistics.VendorTransID = transEmailID;
                req.EmailResultStatistics.TransactionGroupID = transGroupID;
                req.EmailResultStatistics.CustomID = customID;

                if (!string.IsNullOrEmpty(toEmailAddress))
                {
                    req.CommunicationSource = toEmailAddress;
                }
                if (!string.IsNullOrEmpty(bounceTime))
                    req.TimeOfComm = Convert.ToDateTime(bounceTime);
                req.ErrorMessage = "Email Bounced";
                if (!string.IsNullOrEmpty(diagnosticCode))
                        req.ErrorMessage += " - Code: " + diagnosticCode;
                if (!string.IsNullOrEmpty(definitive))
                        req.ErrorMessage += " - IsDefinitive: " + definitive;

                isSuccess = commRepository.SaveBounceEvent(req);
               
            }
            catch (Exception ex)
            {
                string processIDString = ConfigurationManager.AppSettings.Get("ASEProcessID");
                int processID = 0;
                int.TryParse(processIDString, out processID);
                Phytel.Framework.ASE.Process.Log.LogError(processID, string.Format("JangoEmailResultsService - TrackClickEvent : Invalid communication results for CustomID : " + customID), Phytel.Framework.ASE.Data.Common.LogErrorCode.Error, Phytel.Framework.ASE.Data.Common.LogErrorSeverity.High, ex.Message);
                isSuccess =  false;
            }
            return isSuccess;
        }

        [WebGet(UriTemplate = "/TrackUnsubscribeEvent?UserID={userID}&ToEmailAddress={toEmailAddress}&UnsubscribeType={unsubscribeType}&UnsubscribeTime={unsubscribeTime}&InsertedOn={insertedOn}&IPAddress={ipAddress}&TransGroupID={transGroupID}&TransEmailID={transEmailID}&CustomID={customID}", BodyStyle = WebMessageBodyStyle.Wrapped), OperationContract]
        public bool TrackUnsubscribeEvent(string userID, string toEmailAddress, string unsubscribeType, string unsubscribeTime, string insertedOn, string ipAddress, string transGroupID, string transEmailID, string customID)
        {
             
            try{
                RestCommRepository commRepository = new RestCommRepository();
             
            }
            catch (Exception ex)
            {
                string processIDString = ConfigurationManager.AppSettings.Get("ASEProcessID");
                int processID = 0;
                int.TryParse(processIDString, out processID);
                Phytel.Framework.ASE.Process.Log.LogError(processID, string.Format("JangoEmailResultsService - TrackUnsubscribeEvent : Invalid communication results for CustomID : " + customID), Phytel.Framework.ASE.Data.Common.LogErrorCode.Error, Phytel.Framework.ASE.Data.Common.LogErrorSeverity.High, ex.Message);
                return false;
            }
            return true;
        }

        [WebGet(UriTemplate = "/TrackComplaintEvent?UserID={userID}&ToEmailAddress={toEmailAddress}&ComplaintTime={complaintTime}&AbuseID={abuseID}&TransGroupID={transGroupID}&TransEmailID={transEmailID}&CustomID={customID}", BodyStyle = WebMessageBodyStyle.Wrapped), OperationContract]
        public bool TrackComplaintEvent(string userID, string toEmailAddress, string complaintTime, string abuseID, string transGroupID, string transEmailID, string customID)
        {
            bool isSuccess = false;
            try{
                RestCommRepository commRepository = new RestCommRepository();
                CommResult req = new CommResult();
                req.EmailResultStatistics = new CommEmailResultStatistics();
                req.EmailResultStatistics.VendorTransID = transEmailID;
                req.EmailResultStatistics.TransactionGroupID = transGroupID;
                req.EmailResultStatistics.CustomID = customID;

                if (!string.IsNullOrEmpty(toEmailAddress))
                {
                    req.CommunicationSource = toEmailAddress;
                }
                if (!string.IsNullOrEmpty(complaintTime))
                    req.TimeOfComm = Convert.ToDateTime(complaintTime);

                req.ErrorMessage = "Email Complaint";
                if (!string.IsNullOrEmpty(abuseID))
                    req.ErrorMessage += " - AbuseID: " + abuseID;

                isSuccess = commRepository.SaveComplaintEvent(req);
            }
            catch (Exception ex)
            {
                string processIDString = ConfigurationManager.AppSettings.Get("ASEProcessID");
                int processID = 0;
                int.TryParse(processIDString, out processID);
                Phytel.Framework.ASE.Process.Log.LogError(processID, string.Format("JangoEmailResultsService - TrackComplaintEvent : Invalid communication results for CustomID : " + customID), Phytel.Framework.ASE.Data.Common.LogErrorCode.Error, Phytel.Framework.ASE.Data.Common.LogErrorSeverity.High, ex.Message);
                isSuccess =  false;
            }

            return isSuccess;
        }
        

        [WebGet(UriTemplate = ""), OperationContract]
        public string GetEvent()
        {
            return "test";
        }


        private string SerializeAnObject(object AnObject)
        {
            System.Xml.Serialization.XmlSerializer Xml_Serializer = new System.Xml.Serialization.XmlSerializer(AnObject.GetType());
            System.IO.StringWriter Writer = new System.IO.StringWriter();
            Xml_Serializer.Serialize(Writer, AnObject);
            return Writer.ToString();
        }
    }
}
