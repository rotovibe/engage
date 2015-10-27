using Phytel.Engage.Integrations.DomainEvents;

namespace Phytel.Engage.Integrations.Process
{
    public interface ILoggerEvent
    {
        void Logger_EtlEvent(object sender, LogStatus e);
    }
}