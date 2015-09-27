namespace Phytel.Engage.Integrations.Process.Initialization
{
    public interface IInitializer<T>
    {
        T Build();
    }
}