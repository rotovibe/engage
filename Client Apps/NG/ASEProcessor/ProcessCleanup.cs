using System;
using Phytel.API.DataDomain.ASE.DTO.Message;
using Phytel.ASE.Core;

namespace Phytel.ASEProcessor
{
    class ProcessCleanup : QueueProcessBase
    {
        private string _DBConnName = "Phytel";

        public override void Execute(QueueMessage queueMessage)
        {
            try
            {
                if (base.Configuration.SelectSingleNode("//Phytel.ASE.Process/ProcessConfiguration/PhytelServicesConnName") != null)
                    _DBConnName = base.Configuration.SelectSingleNode("//Phytel.ASE.Process/ProcessConfiguration/PhytelServicesConnName").InnerText;

                Phytel.Services.ParameterCollection emptyParms = new Phytel.Services.ParameterCollection();
                Phytel.Services.SQLDataService.Instance.ExecuteProcedure(_DBConnName, false, "spPhy_PhytelMaintenance", emptyParms);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
