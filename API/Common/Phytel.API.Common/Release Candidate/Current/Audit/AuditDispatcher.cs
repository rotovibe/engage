using Phytel.Framework.ASE.Process;
//using Phytel.API.AppDomain.Audit.DTO;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;

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

        public static void WriteAudit(AuditData auditLogToProcess)
        {
            AuditData auditLog = auditLogToProcess;
            auditLog.EventDateTime = DateTime.Now;

            QueueMessage newMessage = null;

            //where should I get this?
            string dbName = ConfigurationManager.AppSettings.Get("PhytelServicesConnName");
            string configqueue =  "AUDIT_QUEUE";

            DataSet ds = Phytel.Services.SQLDataService.Instance.ExecuteSQL(dbName, false, string.Format("Select [Value] From ApplicationSetting Where [Key] = '{0}'",configqueue));

            if (ds.Tables[0].Rows.Count > 0)
            {
                string messageQueue = ds.Tables[0].Rows[0]["Value"].ToString();
                //string messageQueue = "fake";

                string xmlBody = ToXML(auditLog);

                newMessage = new QueueMessage(Phytel.Framework.ASE.Data.Common.ASEMessageType.Process, messageQueue);
                newMessage.Body = xmlBody;

#if DEBUG
                Debug.WriteLine(string.Format("***** Message Body ***** {0}", newMessage.Body));
#endif

                string title = string.Empty;
                if (auditLog.Type.ToString() == "PageView")
                    title = auditLog.Type.ToString() + " - " + auditLog.SourcePage;
                else
                    title = auditLog.Type.ToString();

                MessageQueueHelper.SendMessage(@messageQueue, newMessage, title);
            }
            
        }

        public static void WriteAudit(DataAudit.DataAudit auditLogToProcess)
        {
            DataAudit.DataAudit auditLog = auditLogToProcess;
            auditLog.TimeStamp = DateTime.Now;

            QueueMessage newMessage = null;

            //where should I get this?
            string dbName = ConfigurationManager.AppSettings.Get("PhytelServicesConnName");
            string configqueue =  "DATA_AUDIT_QUEUE";

            DataSet ds = Phytel.Services.SQLDataService.Instance.ExecuteSQL(dbName, false, string.Format("Select [Value] From ApplicationSetting Where [Key] = '{0}'", configqueue));

            if (ds.Tables[0].Rows.Count > 0)
            {
                string messageQueue = ds.Tables[0].Rows[0]["Value"].ToString();
                //string messageQueue = "fake";

                string xmlBody = ToXML(auditLog);

                newMessage = new QueueMessage(Phytel.Framework.ASE.Data.Common.ASEMessageType.Process, messageQueue);
                newMessage.Body = xmlBody;

                string title = string.Empty;
                if (auditLog.Type.ToString() == "PageView")
                    title = auditLog.Type.ToString();
                else
                    title = auditLog.Type.ToString();

                MessageQueueHelper.SendMessage(@messageQueue, newMessage, title);
            }

        }

        public static void WriteAudit(DataAudit.DataAudit auditLogToProcess, string title)
        {
            DataAudit.DataAudit auditLog = auditLogToProcess;
            auditLog.TimeStamp = DateTime.Now;

            QueueMessage newMessage = null;

            //where should I get this?
            string dbName = ConfigurationManager.AppSettings.Get("PhytelServicesConnName");
            string configqueue = "DATA_AUDIT_QUEUE";

            DataSet ds = Phytel.Services.SQLDataService.Instance.ExecuteSQL(dbName, false, string.Format("Select [Value] From ApplicationSetting Where [Key] = '{0}'", configqueue));

            if (ds.Tables[0].Rows.Count > 0)
            {
                string messageQueue = ds.Tables[0].Rows[0]["Value"].ToString();
                //string messageQueue = "fake";

                string xmlBody = ToXML(auditLog);

                newMessage = new QueueMessage(Phytel.Framework.ASE.Data.Common.ASEMessageType.Process, messageQueue);
                newMessage.Body = xmlBody;

                MessageQueueHelper.SendMessage(@messageQueue, newMessage, title);
            }

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
