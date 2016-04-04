using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Medication
{
    public abstract class MedicationRepositoryFactory<T>
    {
        public static IMedicationRepository<T> GetMedicationRepository(string dbName, string productName, string userId)
        {
            try
            {
                IMedicationRepository<T> repo = null;

                //We only have 1 repository at this time, just return it
                repo = new MongoMedicationRepository<T>(dbName) as IMedicationRepository<T>;
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
