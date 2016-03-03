using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Cohort
{
    public abstract class CohortRepositoryFactory<T>
    {
        public static ICohortRepository<T> GetCohortRepository(string dbName, string productName, string userId)
        {
            ICohortRepository<T> repo = null;

            //We only have 1 repository at this time, just return it
            repo = new MongoCohortRepository<T>(dbName) as ICohortRepository<T>;
            repo.UserId = userId;
            return repo;
        }
    }
}
