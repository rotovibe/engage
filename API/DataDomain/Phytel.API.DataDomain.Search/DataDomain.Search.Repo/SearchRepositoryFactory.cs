using System;
using Phytel.API.DataDomain.Search.DTO;
using Phytel.API.Interface;
using ServiceStack.WebHost.Endpoints;

namespace DataDomain.Search.Repo
{
    public enum RepositoryType
    {
        Search,
        Lucene
    }

    public abstract class SearchRepositoryFactory
    {

        public static IMongoSearchRepository GetSearchRepository(IDataDomainRequest request, RepositoryType type)
        {
            try
            {
                IMongoSearchRepository repo = null;

                switch (type)
                {
                    case RepositoryType.Search:
                    {
                        var context = new SearchMongoContext(request.ContractNumber);
                        repo = new MongoSearchRepository<SearchMongoContext>(context){UserId = request.UserId,ContractDBName = request.ContractNumber};
                        break;
                    }
                    case RepositoryType.Lucene:
                    {
                        repo =  new LuceneSearchRepository() { UserId = request.UserId, ContractDBName = request.ContractNumber };
                        break;
                    }
                }
                return repo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
