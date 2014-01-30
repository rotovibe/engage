using Phytel.Framework.ASE.Process;
//using Phytel.API.AppDomain.Audit.DTO;
using System;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Phytel.API.Common.Audit
{
    public static class AuditDispatcher
    {
        static readonly string AuditSystemUrl = ConfigurationManager.AppSettings["AuditSystemUrl"];

        public static void SendDispatchAsynch(DispatchEventArgs args)
        {

#if Debug
            // synchronous for testing
            FireDispatch(args);
#else
            //asynch call
            Task.Factory.StartNew(() => FireDispatch(args));
#endif
        }

        private static void FireDispatch(DispatchEventArgs args)
        {
            //IRestClient client = new JsonServiceClient();
            //string className = args.payload.GetType().Name;
            //string pathName = className.Substring(3, className.Length - 10).ToLower();

            ////{Version}/{ContractNumber}/auditerror/{ObjectID} ??
            //PutAuditResponse par = client.Post<PutAuditResponse>(string.Format("{0}/{1}/{2}/{3}",
            //        AuditSystemUrl,
            //        ((IAppDomainRequest)args.payload).Version,
            //        ((IAppDomainRequest)args.payload).ContractNumber,
            //        pathName)
            //        , args.payload);
        }

        public static void LogAuditAsynch(AuditData data)
        { 

#if DEBUG
            // synchronous for testing
            WriteAudit(data);
#else
          //hand this to a new thread so the original process can continue
            new Thread(() =>
            {
                // handle work here
                //test the response to here, even though I don't send it back to the client
                WriteAudit(AuditData data);

            }).Start()  
#endif
            
        }

        private static void WriteAudit(AuditData auditLogToProcess)
        {
            AuditData auditLog = auditLogToProcess;
            auditLog.EventDateTime = DateTime.Now;

            QueueMessage newMessage = null;

            //where should I get this?
            string messageQueue = @".\private$\c3audit"; // ApplicationSettingService.Instance.GetSetting("AUDIT_QUEUE").Value;
            //string messageQueue = "fake";

            string xmlBody = ToXML(auditLog);

            newMessage = new QueueMessage(Phytel.Framework.ASE.Data.Common.ASEMessageType.Process, messageQueue);
            newMessage.Body = xmlBody;

            string title = string.Empty;
            if (auditLog.Type.ToString() == "PageView")
                title = auditLog.Type.ToString() + " - " + auditLog.SourcePage;
            else
                title = auditLog.Type.ToString();

            MessageQueueHelper.SendMessage(@messageQueue, newMessage, title);

            //return auditLog;
        }

        private static string ToXML(Object oObject)
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
    }
}
