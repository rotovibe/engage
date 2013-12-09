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
        public static IStepRepository<T> GetStepRepository(string dbName, string productName, string type)
        {
            IStepRepository<T> repo = null;
            if (type.Equals("yesno", StringComparison.InvariantCultureIgnoreCase))
            {
                repo = new MongoYesNoStepRepository<T>(dbName) as IStepRepository<T>;
            }
            else if (type.Equals("text", StringComparison.InvariantCultureIgnoreCase))
            {
                repo = new MongoTextStepRepository<T>(dbName) as IStepRepository<T>;
            }
            return repo;
        }
    }
}
