using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Action
{
    public abstract class ActionRepositoryFactory<T>
    {
        public static IActionRepository<T> GetActionRepository(string dbName, string productName, string userId)
        {
            IActionRepository<T> repo = null;

            //We only have 1 repository at this time, just return it
            repo = new MongoActionRepository<T>(dbName) as IActionRepository<T>;
            repo.UserId = userId;
            return repo;
        }
    }
}
