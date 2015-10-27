using Phytel.Engage.Integrations.QueueProcess;

namespace Phytel.Engage.Integrations.Process.Initialization
{
    public abstract class Initializer<T> : IInitializer<T>
    {
        public abstract T Build(IntegrationProcess process);
    }
}
