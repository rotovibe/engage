namespace Phytel.Engage.Integrations.Repo.Repositories
{
    public interface IRepositoryFactory
    {
        IRepository GetRepository(string context, RepositoryType type);
    }
}