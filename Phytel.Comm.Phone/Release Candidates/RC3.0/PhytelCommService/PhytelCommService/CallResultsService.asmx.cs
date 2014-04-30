using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Text;
using System.Configuration;
using System.IO;
using System.Xml;


namespace Phytel.CommService
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class CallResultsService : System.Web.Services.WebService
    {
        [WebMethod]
        public respObject_Array sendOCQEResults(reqObject_Array parameters)
        {
            string xmlBody = string.Empty;
            respObject_Array returnResponse = new respObject_Array();
            try
            {
                returnResponse.gfResponse = new respObject[parameters.gfRequest.Length];
                int i = 0;

                xmlBody = "<CallSet>";
                foreach (reqObject rObject in parameters.gfRequest)
                {
                    respObject resp = new respObject();
                    resp.CallID = rObject.CallID;
                    resp.returnStatus = "ACK";
                    resp.returnString = null;

                    //add object fields to new node in 1 Dom
                    xmlBody += BuildResultsXml(rObject);

                    returnResponse.gfResponse[i] = resp;
                    i++;
                }
                xmlBody += "</CallSet>";
                SaveResultsAndSendMessage(xmlBody);

            }
            catch (SoapException soapEx)
            {
                LogHelper.LogError(soapEx);
                throw soapEx;
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                throw ex;
            }

            return returnResponse;
        }

        private string BuildResultsXml(reqObject request)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<Call>");
            sb.Append(string.Format("<SendID>{0}</SendID>", request.SendID.ToString()));
            sb.Append(string.Format("<ContractID>{0}</ContractID>", request.ContractID.ToString()));
            sb.Append(string.Format("<FacilityID>{0}</FacilityID>", request.FacilityID.ToString()));
            sb.Append(string.Format("<ProviderID>{0}</ProviderID>", request.ProviderID.ToString()));
            sb.Append(string.Format("<ActivityID>{0}</ActivityID>", request.ActivityID.ToString()));
            sb.Append(string.Format("<ScheduleID>{0}</ScheduleID>", request.ScheduleID.ToString()));
            sb.Append(string.Format("<CommEventID>{0}</CommEventID>", request.CommEventID.ToString()));
            sb.Append(string.Format("<CallID>{0}</CallID>", request.CallID.ToString()));
            sb.Append(string.Format("<CallDistributorTimeOfCall>{0}</CallDistributorTimeOfCall>", request.CallDistributorTimeOfCall.ToString()));
            sb.Append(string.Format("<CallDuration>{0}</CallDuration>", request.CallDuration.ToString()));
            sb.Append(string.Format("<ResultCode>{0}</ResultCode>", request.ResultCode));
            sb.Append(string.Format("<ResultStatus>{0}</ResultStatus>", request.ResultStatus));
            sb.Append(string.Format("<HangupLocale>{0}</HangupLocale>", request.HangupLocale));
            sb.Append(string.Format("<Recording>{0}</Recording>", request.Recording.ToString()));
            sb.Append(string.Format("<FileLocation>{0}</FileLocation>", request.FileLocation));
            sb.Append(string.Format("<FileName>{0}</FileName>", request.FileName));
            sb.Append(string.Format("<LanguageResultsCode>{0}</LanguageResultsCode>", request.LanguageResultsCode));
            sb.Append(string.Format("<TransferPrimaryStatusCode>{0}</TransferPrimaryStatusCode>", request.TransferPrimaryStatusCode));
            sb.Append(string.Format("<TransferSecondaryStatusCode>{0}</TransferSecondaryStatusCode>", request.TransferSecondaryStatusCode));
            sb.Append(string.Format("<TransferDestination>{0}</TransferDestination>", request.TransferDestination));
            sb.Append(string.Format("<CommDialerIP>{0}</CommDialerIP>", request.CommDialerIP));
            sb.Append(string.Format("<CommRecordingServerIP>{0}</CommRecordingServerIP>", request.CommRecordingServerIP));
            sb.Append(string.Format("<CommApplicationURL>{0}</CommApplicationURL>", request.CommApplicationURL));
            sb.Append(string.Format("<CommMediaServerIP>{0}</CommMediaServerIP>", request.CommMediaServerIP));
            sb.Append(string.Format("<CommVXMLBrowser>{0}</CommVXMLBrowser>", request.CommVXMLBrowser));
            sb.Append(string.Format("<CommChannel>{0}</CommChannel>", request.CommChannel.ToString()));

            sb.Append("</Call>");

            return sb.ToString();
        }

        /// <summary>
        /// Method to save results to local file server
        /// and post message on commResults queue
        /// </summary>
        /// <param name="xmlBody"></param>
        private void SaveResultsAndSendMessage(string xmlBody)
        {
            DateTime currentDate = System.DateTime.Now;
            string filePath = ConfigurationManager.AppSettings.Get("ResultsPath");
            string tempFileName = "CommResults-" + currentDate.ToString("MMddyy'-'HHmmss'-'ffffff") + System.Guid.NewGuid() + ".txt";
            string fileName = filePath + "\\" + tempFileName;
            try
            {
                if (ConfigurationManager.AppSettings.Get("PostToQueue") == "True")
                {
                    Phytel.Framework.ASE.Process.QueueMessage msg = new Phytel.Framework.ASE.Process.QueueMessage();
                    msg.Body = xmlBody;
                    msg.Routes.Add(new Phytel.Framework.ASE.Process.QueueMessageRoute(ConfigurationManager.AppSettings.Get("routeQueue")));
                    msg.Header.Type = Phytel.Framework.ASE.Common.MessageType.Process;

                    Phytel.Framework.ASE.Process.MessageQueueHelper.SendMessage(ConfigurationManager.AppSettings.Get("routeQueue"),
                        msg, "NewFound OCQE Result");

                }
            }
            catch (Exception ex)
            {
                //Log 
                LogHelper.LogError(ex);
                SaveToExceptionFolder(tempFileName, xmlBody);
            }
            try
            {
                if (!String.IsNullOrEmpty(filePath))
                {
                    FileStream fs = File.Create(fileName.ToString());
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(xmlBody);
                    sw.Close();
                    fs.Close();
                }
            }
            catch (Exception e)
            {
                //Log 
                LogHelper.LogError(e);
                SaveToLocalFolder(tempFileName, xmlBody);
            }
        }

        /// <summary>
        /// Method to save results file to exception folder
        /// which will later be processed
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="xmlBody"></param>
        private void SaveToExceptionFolder(string fileName, string xmlBody)
        {
            try
            {
                if (!String.IsNullOrEmpty(fileName) && !String.IsNullOrEmpty(fileName.Trim()))
                {
                    string exceptionFilePath = ConfigurationManager.AppSettings.Get("LocalExceptionFilePath");
                    string exceptionFileName = exceptionFilePath + "\\" + fileName;
                    if (!String.IsNullOrEmpty(exceptionFileName) && !String.IsNullOrEmpty(exceptionFileName))
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(xmlBody);
                        xmlDoc.Save(exceptionFileName);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// Method to save results file to local folder
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="xmlBody"></param>
        private void SaveToLocalFolder(string fileName, string xmlBody)
        {
            try
            {
                if (!String.IsNullOrEmpty(fileName) && !String.IsNullOrEmpty(fileName.Trim()))
                {
                    string localFilePath = ConfigurationManager.AppSettings.Get("LocalFilePath");
                    using (StreamWriter sw = new StreamWriter(Server.MapPath(localFilePath + "\\" + fileName.ToString())))
                    {
                        sw.Write(xmlBody);
                    }
                }
            }
            catch (Exception e)
            {
                //Log and not throwing exception as 
                //queue message already sent 
                LogHelper.LogError(e);

            }
        }




    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace = "http://tempuri.org/")]
    public partial class reqObject_Array
    {

        private reqObject[] gfRequestField;

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public reqObject[] gfRequest
        {
            get
            {
                return this.gfRequestField;
            }
            set
            {
                this.gfRequestField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace = "http://tempuri.org/")]
    public partial class reqObject
    {

        private int sendIDField;

        private string contractIDField;

        private int facilityIDField;

        private System.Nullable<int> providerIDField;

        private int activityIDField;

        private System.Nullable<int> scheduleIDField;

        private int commEventIDField;

        private string callIDField;

        private System.DateTime callDistributorTimeOfCallField;

        private long callDurationField;

        private string resultCodeField;

        private string resultStatusField;

        private string hangupLocaleField;

        private bool recordingField;

        private string fileLocationField;

        private string fileNameField;

        private string patientLanguageResultsCodeField;

        private string transferPrimaryStatusCodeField;

        private string transferSecondaryStatusCodeField;

        private string transferDestinationField;

        private string commDialerIPField;

        private string commRecordingServerIPField;

        private string commApplicationURLField;

        private string commMediaServerIPField;

        private string commVXMLBrowserField;

        private System.Nullable<int> commChannelField;
       
       /// <remarks/>
        public int SendID
        {
            get
            {
                return this.sendIDField;
            }
            set
            {
                this.sendIDField = value;
            }
        }

        /// <remarks/>
        public string ContractID
        {
            get
            {
                return this.contractIDField;
            }
            set
            {
                this.contractIDField = value;
            }
        }

        /// <remarks/>
        public int FacilityID
        {
            get
            {
                return this.facilityIDField;
            }
            set
            {
                this.facilityIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public System.Nullable<int> ProviderID
        {
            get
            {
                return this.providerIDField;
            }
            set
            {
                this.providerIDField = value;
            }
        }

        /// <remarks/>
        public int ActivityID
        {
            get
            {
                return this.activityIDField;
            }
            set
            {
                this.activityIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public System.Nullable<int> ScheduleID
        {
            get
            {
                return this.scheduleIDField;
            }
            set
            {
                this.scheduleIDField = value;
            }
        }

        /// <remarks/>
        public int CommEventID
        {
            get
            {
                return this.commEventIDField;
            }
            set
            {
                this.commEventIDField = value;
            }
        }

        /// <remarks/>
        public string CallID
        {
            get
            {
                return this.callIDField;
            }
            set
            {
                this.callIDField = value;
            }
        }

        /// <remarks/>
        public System.DateTime CallDistributorTimeOfCall
        {
            get
            {
                return this.callDistributorTimeOfCallField;
            }
            set
            {
                this.callDistributorTimeOfCallField = value;
            }
        }

        /// <remarks/>
        public long CallDuration
        {
            get
            {
                return this.callDurationField;
            }
            set
            {
                this.callDurationField = value;
            }
        }

        /// <remarks/>
        public string ResultCode
        {
            get
            {
                return this.resultCodeField;
            }
            set
            {
                this.resultCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public string ResultStatus
        {
            get
            {
                return this.resultStatusField;
            }
            set
            {
                this.resultStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public string HangupLocale
        {
            get
            {
                return this.hangupLocaleField;
            }
            set
            {
                this.hangupLocaleField = value;
            }
        }

        /// <remarks/>
        public bool Recording
        {
            get
            {
                return this.recordingField;
            }
            set
            {
                this.recordingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public string FileLocation
        {
            get
            {
                return this.fileLocationField;
            }
            set
            {
                this.fileLocationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public string FileName
        {
            get
            {
                return this.fileNameField;
            }
            set
            {
                this.fileNameField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public string LanguageResultsCode
        {
            get
            {
                return this.patientLanguageResultsCodeField;
            }
            set
            {
                this.patientLanguageResultsCodeField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public string TransferPrimaryStatusCode
        {
            get
            {
                return this.transferPrimaryStatusCodeField;
            }
            set
            {
                this.transferPrimaryStatusCodeField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public string TransferSecondaryStatusCode
        {
            get
            {
                return this.transferSecondaryStatusCodeField;
            }
            set
            {
                this.transferSecondaryStatusCodeField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public string TransferDestination
        {
            get
            {
                return this.transferDestinationField;
            }
            set
            {
                this.transferDestinationField = value;
            }
        }


        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public string CommDialerIP
        {
            get
            {
                return this.commDialerIPField;
            }
            set
            {
                this.commDialerIPField = value;
            }
        }


        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public string CommRecordingServerIP
        {
            get
            {
                return this.commRecordingServerIPField;
            }
            set
            {
                this.commRecordingServerIPField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public string CommApplicationURL
        {
            get
            {
                return this.commApplicationURLField;
            }
            set
            {
                this.commApplicationURLField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public string CommMediaServerIP
        {
            get
            {
                return this.commMediaServerIPField;
            }
            set
            {
                this.commMediaServerIPField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public string CommVXMLBrowser
        {
            get
            {
                return this.commVXMLBrowserField;
            }
            set
            {
                this.commVXMLBrowserField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public System.Nullable<int> CommChannel
        {
            get
            {
                return this.commChannelField;
            }
            set
            {
                this.commChannelField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace = "http://tempuri.org/")]
    public partial class respObject
    {

        private string callIDField;

        private string returnStatusField;

        private string returnStringField;

        /// <remarks/>
        public string CallID
        {
            get
            {
                return this.callIDField;
            }
            set
            {
                this.callIDField = value;
            }
        }

        /// <remarks/>
        public string returnStatus
        {
            get
            {
                return this.returnStatusField;
            }
            set
            {
                this.returnStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public string returnString
        {
            get
            {
                return this.returnStringField;
            }
            set
            {
                this.returnStringField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace = "http://tempuri.org/")]
    public partial class respObject_Array
    {

        private respObject[] gfResponseField;

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public respObject[] gfResponse
        {
            get
            {
                return this.gfResponseField;
            }
            set
            {
                this.gfResponseField = value;
            }
        }
    }
}

