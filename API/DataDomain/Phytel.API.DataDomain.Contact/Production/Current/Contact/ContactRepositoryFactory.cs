using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact
{
    public abstract class ContactRepositoryFactory<T>
    {
        public static IContactRepository<T> GetContactRepository(string dbName, string productName, string userId)
        {
            try
            {
                IContactRepository<T> repo = null;

                //We only have 1 repository at this time, just return it
                repo = new MongoContactRepository<T>(dbName) as IContactRepository<T>;
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
