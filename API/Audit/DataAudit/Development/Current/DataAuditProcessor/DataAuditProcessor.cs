using System;
using MongoDB.Driver;
using Phytel.API.DataDomain.ASE.Common.Enums;
using Phytel.API.DataDomain.ASE.DTO.Message;
using Phytel.ASE.Core;
using Phytel.Services.Mongo;

namespace Phytel.API.DataAuditProcessor
{
    public class DataAuditProcessor: QueueProcessBase
    {
        string _DBConnName = "";

        public override void Execute(QueueMessage queueMessage)
        {
            try
            {
                _DBConnName = base.Configuration.SelectSingleNode("//Phytel.ASE.Process/ProcessConfiguration/PhytelServicesConnName").InnerText;

                DataAudit.DataAudit da = MessageQueueHelper.DeserializeXmlObject(queueMessage.Body, typeof(DataAudit.DataAudit)) as DataAudit.DataAudit;

                MongoDatabase db = MongoService.Instance.GetDatabase(_DBConnName, da.Contract, true, "Audit");
                db.GetCollection(da.EntityType).Insert(da);
            }
            catch(Exception ex)
            {
                base.LogError(ex, LogErrorCode.Error, LogErrorSeverity.Critical);
                throw;
            }
        }
    }
}
