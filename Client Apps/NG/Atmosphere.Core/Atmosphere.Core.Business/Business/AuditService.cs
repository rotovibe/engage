using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using C3.Data;
using Phytel.API.DataDomain.ASE.Common.Enums;
using Phytel.API.DataDomain.ASE.DTO.Message;
using Phytel.ASE.Core;

namespace C3.Business
{
    public class AuditService
    {
        #region Private
        
        private static volatile AuditService _svc = null;
        private static object syncRoot = new Object();

        private AuditService() { }

        #endregion

        #region Public
        
        public static AuditService Instance
        {
            get
            {
                if (_svc == null)
                {
                    lock (syncRoot)
                    {
                        if (_svc == null)
                            _svc = new AuditService();
                    }
                }

                return _svc;
            }
        }

        public AuditData LogEvent(AuditData auditLogToProcess)
        {
            AuditData auditLog = auditLogToProcess;
            auditLog.EventDateTime = DateTime.Now;

            QueueMessage newMessage = null;

            string messageQueue = ApplicationSettingService.Instance.GetSetting("AUDIT_QUEUE").Value;

            string xmlBody = ToXML(auditLog);

            newMessage = new QueueMessage(ASEMessageType.Process, messageQueue);
            newMessage.Body = xmlBody;

            string title = string.Empty;
            if (auditLog.Type.ToString() == "PageView")
                title = auditLog.Type.ToString() + " - " + auditLog.SourcePage;
            else
                title = auditLog.Type.ToString();

            MessageQueueHelper.SendMessage(@messageQueue, newMessage, title);

            return auditLog;
        }
        

        public string ToXML(Object oObject)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(oObject.GetType());
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, oObject);
                xmlStream.Position = 0;
                xmlDoc.Load(xmlStream);
                return xmlDoc.InnerXml;
            }
        }

        public Message PopulateMessage(object ex)
        {
            Message message = new Message();
            message.Id = string.Empty;
            message.Source = string.Empty;
            message.StackTrace = string.Empty;

            if (ex is System.Exception)
            {
                if (ex != null)
                {
                    message.Text = ((Exception)ex).Message;
                    message.Source = ((Exception)ex).Source;
                    message.StackTrace = ((Exception)ex).StackTrace;
                }
                else
                {
                    message.Text = "Unknown Server Error -- Exception was empty";
                }
            }
            else if (ex is ApplicationMessage)
            {
                message.Id = ((ApplicationMessage)ex).Code;
                message.Text = ((ApplicationMessage)ex).Text;
            }
            else if (ex is string)
                message.Text = ex.ToString(); 

            return message;
        }

        #endregion
    }
}
