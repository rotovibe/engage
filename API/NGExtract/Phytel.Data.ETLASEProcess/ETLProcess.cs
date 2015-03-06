using Phytel.Data.ETL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Phytel.Data.ETLASEProcess
{
    public class ETLProcess : Phytel.ASE.Core.QueueProcessBase
    {
        public override void Execute(API.DataDomain.ASE.DTO.Message.QueueMessage queueMessage)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(queueMessage.Body);
            var contStr = doc.DocumentElement.InnerText;

            // add contstr to constructor
            ETLProcessor pro = new ETLProcessor(contStr);
            pro.EtlEvent += pro_EtlEvent;
            pro.Rebuild();
        }

        void pro_EtlEvent(object sender, ETLEventArgs e)
        {
            if (e.IsError)
                base.LogError(e.Message, API.DataDomain.ASE.Common.Enums.LogErrorCode.Error, API.DataDomain.ASE.Common.Enums.LogErrorSeverity.Critical, string.Empty);
            else
                base.LogDebug(e.Message);
        }
    }
}
