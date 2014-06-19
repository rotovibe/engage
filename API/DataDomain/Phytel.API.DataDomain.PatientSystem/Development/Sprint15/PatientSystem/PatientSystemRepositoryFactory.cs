using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientSystem
{
    public abstract class PatientSystemRepositoryFactory<T>
    {
        public static IPatientSystemRepository<T> GetPatientSystemRepository(string dbName, string productName, string userId)
        {
            IPatientSystemRepository<T> repo = null;

            //We only have 1 repository at this time, just return it
            repo = new MongoPatientSystemRepository<T>(dbName) as IPatientSystemRepository<T>;
            repo.UserId = userId;
            return repo;
        }
    }
}
