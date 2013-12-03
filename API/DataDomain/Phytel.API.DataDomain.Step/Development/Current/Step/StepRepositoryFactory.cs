using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Step
{
    public abstract class StepRepositoryFactory<T>
    {
        public static IStepRepository<T> GetStepRepository(string dbName, string productName)
        {
            IStepRepository<T> repo = null;

            //We only have 1 repository at this time, just return it
            repo = new MongoStepRepository<T>(dbName) as IStepRepository<T>;

            return repo;
        }
    }
}
