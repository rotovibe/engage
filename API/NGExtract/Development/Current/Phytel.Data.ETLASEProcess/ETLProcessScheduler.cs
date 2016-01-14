using Phytel.Data.ETL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Phytel.API.DataDomain.ASE.DTO.Message;

namespace Phytel.Data.ETLASEProcess
{
    public class ETLProcessScheduler : Phytel.ASE.Core.QueueProcessBase
    {
        public override void Execute(QueueMessage queueMessage)
        {
//<Phytel.ASE.Process>
//<ProcessConfiguration>
//<Contracts publishkey="etlcontractready">
//<Contract>InHealth001</Contract>
//    <Contract>OrlandHealth001</Contract>
//</Contracts>
//</ProcessConfiguration>
//</Phytel.ASE.Process>


            XmlNodeList list =
                base.Configuration.SelectNodes("//Phytel.ASE.Process/ProcessConfiguration/Contracts/Contract");

            var key = base.Configuration.SelectSingleNode("//Phytel.ASE.Process/ProcessConfiguration/Contracts").Attributes.GetNamedItem("publishkey").Value;

            foreach (XmlNode n in list)
            {
                var str = n.InnerText;
                var bodyStr = "<Contract>" + str + "</Contract>";
                base.LogDebug("Sending Contract " + str + " to bus.");
                Phytel.ASE.Core.APIService.PutMessageOnBus(base.APIURL, key, bodyStr);
            }


        }
    }
}
