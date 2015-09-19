using System;
using System.Xml;
using Phytel.API.DataDomain.ASE.DTO.Message;
using Phytel.ASE.Core;
using Phytel.Engage.Integrations.Configurations;
using Phytel.Engage.Integrations.DTO;
using Phytel.Engage.Integrations.Specifications;

namespace Phytel.Engage.Integrations.QueueProcess
{
    public class IntegrationProcess : QueueProcessBase
    {
        IMessageProcessor Processor { get; set; }

        public override void Execute(QueueMessage queueMessage)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(queueMessage.Body);

            var msg = new RegistryCompleteMessage
            {
                ContractDataBase = doc.DocumentElement.Attributes["contractdatabase"].Value,
                ContractId = doc.DocumentElement.Attributes["contractid"].Value,
                ReportDate = Convert.ToDateTime(doc.DocumentElement.Attributes["reportdate"].Value),
                RunType = doc.DocumentElement.Attributes["runtype"].Value
            };

            Processor.Process(msg);
        }

        public IntegrationProcess()
        {
            Processor = new MessageProcessor
            {
                IsApplicableContract = new IsApplicableContractSpecification<RegistryCompleteMessage>
                {
                    ContractProvider = new ApplicableContractProvider()
                }
            };
        }
    }
}
