using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Search
{
    public abstract class SearchRepositoryFactory<T>
    {
        public static ISearchRepository<T> GetSearchRepository(string dbName, string productName, string userId)
        {
            try
            {
                ISearchRepository<T> repo = null;

                //We only have 1 repository at this time, just return it
                repo = new MongoSearchRepository<T>(dbName) as ISearchRepository<T>;
                repo.UserId = userId;
                return repo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
