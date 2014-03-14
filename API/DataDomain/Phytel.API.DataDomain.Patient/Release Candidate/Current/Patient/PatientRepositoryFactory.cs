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
        ///  this needs to be refactored to provide a generic IPatientRepository. 
        ///  There are overrides to the IPatientRepository that prevent this.
        public static IPatientRepository<T> GetPatientRepository(string dbName, string productName, string userId)
        {
            IPatientRepository<T> repo = null;
            repo = new MongoPatientRepository<T>(dbName) as IPatientRepository<T>;
            repo.UserId = userId;
            return repo;
        }

        public static IPatientRepository<T> GetCohortPatientViewRepository(string dbName, string productName, string userId)
        {
            IPatientRepository<T> repo = null;
            repo = new MongoCohortPatientViewRepository<T>(dbName) as IPatientRepository<T>;
            repo.UserId = userId;
            return repo;
        }
    }
}
