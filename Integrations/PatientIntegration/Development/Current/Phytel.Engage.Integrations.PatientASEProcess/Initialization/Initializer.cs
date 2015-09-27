namespace Phytel.Engage.Integrations.Process.Initialization
{
    public abstract class Initializer<T> : IInitializer<T>
    {
        public abstract T Build();
    }
}
