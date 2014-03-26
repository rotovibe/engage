using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Module
{
    public abstract class ModuleRepositoryFactory<T>
    {
        public static IModuleRepository<T> GetModuleRepository(string dbName, string productName, string userId)
        {
            IModuleRepository<T> repo = null;

            //We only have 1 repository at this time, just return it
            repo = new MongoModuleRepository<T>(dbName) as IModuleRepository<T>;
            repo.UserId = userId;
            return repo;
        }
    }
}
