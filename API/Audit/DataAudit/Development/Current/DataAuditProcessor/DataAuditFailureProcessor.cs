using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Phytel.Framework.ASE.Process;
using Phytel.API.DataAudit;
using Phytel.Mongo.Linq;
using System.Configuration;
using MongoDB.Driver;

namespace Phytel.API.DataAuditProcessor
{
    public class DataAuditFailureProcessor: QueueProcessBase
    {
        string _filepath = "";
        
        public override void Execute(QueueMessage queueMessage)
        {
            try
            {
                _filepath = base.Configuration.SelectSingleNode("//Phytel.ASE.Process/ProcessConfiguration/FilePath").InnerText;

                //format & write the queuemessage body to the filepath
            }
            catch(Exception ex)
            {
                base.LogError(ex, Framework.ASE.Data.Common.LogErrorCode.Error, Framework.ASE.Data.Common.LogErrorSeverity.Critical);
            }
        }

        //private void SetupBaseProperties()
        //{
        //    _message = _bodyDom.SelectSingleNode(string.Format(_xpath, "Message"));
        //    _entityid = GetDomValue("EntityID");
        //    _userid = GetDomValue("UserId");
        //    _contractnumber = GetDomValue("Contract");
        //    _entitytype = GetDomValue("EntityType");
        //    _type = GetDomValue("Type");
        //    _timestamp = DateTime.Parse(GetDomValue("TimeStamp").ToString());
        //    _entity = GetDomValue("Entity");
        //}

        //private string GetDomValue(string fieldName)
        //{
        //    XmlNode xnode = _bodyDom.SelectSingleNode(string.Format(_xpath, fieldName));

        //    if (xnode != null)
        //        return xnode.InnerText;
        //    else
        //        return string.Empty;
        //}

    }
}
