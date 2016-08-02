using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Search
{
    public interface ISearchRepositoryFactory
    {
        ISearchRepository GetRepository(IDataDomainRequest request, RepositoryType type);
    }
}