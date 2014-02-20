using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.LookUp
{
    public abstract class LookUpRepositoryFactory<T>
    {
        public static ILookUpRepository<T> GetLookUpRepository(string dbName, string productName)
        {
            ILookUpRepository<T> repo = null;
            repo = new MongoLookUpRepository<T>(dbName) as ILookUpRepository<T>;
            return repo;
        }
    }
}
