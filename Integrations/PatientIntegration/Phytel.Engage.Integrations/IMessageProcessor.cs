using Phytel.Engage.Integrations.DTO;

namespace Phytel.Engage.Integrations
{
    public interface IMessageProcessor
    {
        void Process(RegistryCompleteMessage message);
    }
}