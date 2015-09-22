namespace Phytel.Engage.Integrations.UOW
{
    public interface IImportUOW<T>
    {
        void Add(T obj);
        void Commit();
    }
}