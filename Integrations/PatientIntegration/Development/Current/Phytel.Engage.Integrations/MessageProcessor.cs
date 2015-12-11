using System;
using Phytel.Engage.Integrations.DomainEvents;
using Phytel.Engage.Integrations.DTO;
using Phytel.Engage.Integrations.Specifications;
using Phytel.Engage.Integrations.UOW;

namespace Phytel.Engage.Integrations
{
    public class MessageProcessor : IMessageProcessor
    {
        public IIsApplicableContract<RegistryCompleteMessage> IsApplicableContract { get; set; }
        public IImportUow PatientsUow { get; set; }

        public void Process(RegistryCompleteMessage message)
        {
            try
            {
                if (!IsApplicableContract.IsSatisfiedBy(message))
                {
                    LoggerDomainEvent.Raise(LogStatus.Create("Integration for this contract is not registered. Closing import process.", true));
                    return;
                }

                LoggerDomainEvent.Raise(LogStatus.Create("Initializing Entity records from Atmosphere...", true));
                PatientsUow.Initialize(message.ContractDataBase);
                PatientsUow.Commit(message.ContractDataBase);
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(LogStatus.Create("MessageProcessor:Process(): " + ex.Message, false));
                throw;
            }
        }
    }
}
