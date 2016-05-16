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

        public void  Process(RegistryCompleteMessage message)
        {
            try
            {
                if (!IsApplicableContract.IsSatisfiedBy(message)) return;

                LoggerDomainEvent.Raise(LogStatus.Create("*** Atmosphere Import Start ***", true));
                LoggerDomainEvent.Raise(LogStatus.Create("Atmosphere Patient Import for " + message.ContractDataBase + " started.", true));
                PatientsUow.Initialize(message.ContractDataBase);
                PatientsUow.Commit(message.ContractDataBase);
                LoggerDomainEvent.Raise(LogStatus.Create("Atmosphere Patient Import for "+ message.ContractDataBase +" completed.", true));
                LoggerDomainEvent.Raise(LogStatus.Create("*** Atmosphere Import Completed ***", true));
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(LogStatus.Create("MessageProcessor:Process(): " + ex.Message, false));
                throw;
            }
        }
    }
}
