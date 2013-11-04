using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Patient
{
    public abstract class PatientRepositoryFactory<T>
    {
        public static IPatientRepository<T> GetPatientRepository(string dbName, string productName)
        {
            IPatientRepository<T> repo = null;

            //We only have 1 repository at this time, just return it
            repo = new MongoPatientRepository<T>(dbName) as IPatientRepository<T>;

            return repo;
        }
    }
}
