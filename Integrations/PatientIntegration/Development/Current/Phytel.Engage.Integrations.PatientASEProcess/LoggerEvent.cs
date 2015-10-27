using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.ASE.Common.Enums;
using Phytel.ASE.Core;
using Phytel.Engage.Integrations.DomainEvents;
using Phytel.Engage.Integrations.QueueProcess;
using LogType = Phytel.Engage.Integrations.DomainEvents.LogType;

namespace Phytel.Engage.Integrations.Process
{
    public class LoggerEvent : ILoggerEvent
    {
        public IntegrationProcess Process { get; set; }

        public void Logger_EtlEvent(object sender, LogStatus e)
        {
            //if (e.Type == LogType.Error)
            //    Process.LogError("[" + _contractName + "] : " + e.Message, LogErrorCode.Error, LogErrorSeverity.Critical, string.Empty);
            //else
            //    Process.LogDebug("[" + _contractName + "] : " + e.Message);
        }
    }
}
