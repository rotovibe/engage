using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.C3User
{
    public abstract class C3UserRepositoryFactory<T>
    {
        public static IC3UserRepository<T> GetC3UserRepository(string productName)
        {
            IC3UserRepository<T> repo = null;

            //We only have 1 repository at this time, just return it
            repo = new C3UserRepository<T>() as IC3UserRepository<T>;

            return repo;
        }
    }
}
