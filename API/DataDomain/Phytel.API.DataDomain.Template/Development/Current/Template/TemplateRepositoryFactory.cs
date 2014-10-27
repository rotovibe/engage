using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Template
{
    public abstract class TemplateRepositoryFactory<T>
    {
        public static ITemplateRepository<T> GetTemplateRepository(string dbName, string productName, string userId)
        {
            try
            {
                ITemplateRepository<T> repo = null;

                //We only have 1 repository at this time, just return it
                repo = new MongoTemplateRepository<T>(dbName) as ITemplateRepository<T>;
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
