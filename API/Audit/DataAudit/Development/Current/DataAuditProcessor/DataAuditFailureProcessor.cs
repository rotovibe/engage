using System;
using System.IO;
using Phytel.API.DataDomain.ASE.Common.Enums;
using Phytel.API.DataDomain.ASE.DTO.Message;
using Phytel.ASE.Core;

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

                FileInfo file = new FileInfo(_filepath);
                file.Directory.Create();
                File.WriteAllText(string.Format(@"{0}\{1}.xml", _filepath, Guid.NewGuid()), queueMessage.Body);
            }
            catch(Exception ex)
            {
                base.LogError(ex, LogErrorCode.Error, LogErrorSeverity.Critical);
            }
        }
    }
}
