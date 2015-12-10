using Phytel.Engage.Integrations.Repo.Bridge;

namespace Phytel.Engage.Integrations.Repo.Repositories
{
    public interface IRepository
    {
        object SelectAll();
        object Insert(object list);
    }
}