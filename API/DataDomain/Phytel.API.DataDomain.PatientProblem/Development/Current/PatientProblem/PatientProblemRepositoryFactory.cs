using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientProblem
{
    public abstract class PatientProblemRepositoryFactory<T>
    {
        public static IPatientProblemRepository<T> GetPatientProblemRepository(string dbName, string productName)
        {
            IPatientProblemRepository<T> repo = null;

            //We only have 1 repository at this time, just return it
            repo = new MongoPatientProblemRepository<T>(dbName) as IPatientProblemRepository<T>;

            return repo;
        }
    }
}
