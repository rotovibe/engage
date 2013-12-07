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
        public static IStepRepository<T> GetStepRepository(string dbName, string productName, string table)
        {
            IStepRepository<T> repo = null;
            if (table.Equals("YesNo", StringComparison.InvariantCultureIgnoreCase))
            {
                repo = new MongoYesNoRepository<T>(dbName) as IStepRepository<T>;
            }
            else if (table.Equals("Text", StringComparison.InvariantCultureIgnoreCase))
            {
                repo = new MongoTextRepository<T>(dbName) as IStepRepository<T>;
            }
            return repo;
        }
    }
}
