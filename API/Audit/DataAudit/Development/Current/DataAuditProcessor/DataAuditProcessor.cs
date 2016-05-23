using System;
using System.Xml;
using MongoDB.Driver;
using Phytel.API.DataDomain.ASE.Common.Enums;
using Phytel.API.DataDomain.ASE.DTO.Message;
using Phytel.ASE.Core;

namespace Phytel.API.DataAuditProcessor
{
    public class DataAuditProcessor: QueueProcessBase
    {
        string _DBConnName = "";
        private const string ConfigXPathPrefix = "//Phytel.ASE.Process/ProcessConfiguration/";

        public override void Execute(QueueMessage queueMessage)
        {
            try
            {
                _DBConnName = base.Configuration.SelectSingleNode("//Phytel.ASE.Process/ProcessConfiguration/PhytelServicesConnName").InnerText;

                DataAudit.DataAudit da = MessageQueueHelper.DeserializeXmlObject(queueMessage.Body, typeof(DataAudit.DataAudit)) as DataAudit.DataAudit;

                MongoDatabase db = Phytel.Services.MongoService.Instance.GetDatabase(_DBConnName, da.Contract, true, "Audit");
                db.GetCollection(da.EntityType).Insert(da);
                
                XmlNodeList queues = Configuration.SelectNodes(ConfigXPathPrefix + "AdditionalQueues/Queue");
                if (queues != null)
                {
                    queueMessage.Routes.ForEach(t =>
                    {
                        t.RetryProcessInterval = GetRetryInterval();
                        t.MaxProcessRetryCount = 4;
                    });

                    foreach (XmlNode queue in queues)
                    {
                        try
                        {
                            string queueName = queue.InnerText;
                            RouteMessage(queueMessage, queueName);
                        }
                        catch (Exception ex)
                        {
                            LogError(ex, LogErrorCode.Error, LogErrorSeverity.Critical);
                            RetryMessage = true;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                base.LogError(ex, LogErrorCode.Error, LogErrorSeverity.Critical);
                throw;
            }
        }

        private int GetRetryInterval()
        {
            int retryInterval;
            var interval = Configuration.SelectSingleNode(ConfigXPathPrefix + "RetryInterval");
            if(interval != null && Int32.TryParse(interval.InnerText, out retryInterval))
            {
                return retryInterval;
            }
            return 1;
        }
    }
}
