//using Phytel.API.AppDomain.Audit.DTO;
using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Phytel.API.DataDomain.ASE.Common.Enums;
using Phytel.API.DataDomain.ASE.DTO.Message;
using Phytel.ASE.Core;

namespace Phytel.API.Common.Audit
{
	public static class AuditDispatcher
	{
		static readonly string AuditSystemUrl = ConfigurationManager.AppSettings["AuditSystemUrl"];
		
		public static void WriteAudit(AuditData auditLogToProcess)
		{
			AuditData auditLog = auditLogToProcess;
			auditLog.EventDateTime = DateTime.UtcNow;

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

				newMessage = new QueueMessage(ASEMessageType.Process, messageQueue);
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
			auditLog.TimeStamp = DateTime.UtcNow;

			QueueMessage newMessage = null;

			//where should I get this?
			string dbName = ConfigurationManager.AppSettings.Get("PhytelServicesConnName");
			string configqueue =  "DATA_AUDIT_QUEUE";

			DataSet ds = Phytel.Services.SQLDataService.Instance.ExecuteSQL(dbName, false, string.Format("Select [Value] From ApplicationSetting Where [Key] = '{0}'", configqueue));

			if (ds.Tables[0].Rows.Count > 0)
			{
				string messageQueue = ds.Tables[0].Rows[0]["Value"].ToString();
				//string messageQueue = "fake";

				//string xmlBody = ToXML(auditLog);
				string xmlBody = MessageQueueHelper.SerializeXmlObject(auditLog);

				newMessage = new QueueMessage(ASEMessageType.Process, messageQueue);
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
			auditLog.TimeStamp = DateTime.UtcNow;

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

				newMessage = new QueueMessage(ASEMessageType.Process, messageQueue);
				newMessage.Body = xmlBody;

				MessageQueueHelper.SendMessage(@messageQueue, newMessage, title);
			}

		}

		public static string ToXML(Object oObject)
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
