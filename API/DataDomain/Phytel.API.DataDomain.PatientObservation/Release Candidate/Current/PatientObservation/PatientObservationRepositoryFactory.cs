using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientObservation
{
    public abstract class PatientObservationRepositoryFactory<T>
    {
        public static IPatientObservationRepository<T> GetPatientObservationRepository(string dbName, string productName)
        {
            try
            {
                IPatientObservationRepository<T> repo = null;

                //We only have 1 repository at this time, just return it
                repo = new MongoPatientObservationRepository<T>(dbName) as IPatientObservationRepository<T>;

                return repo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static IPatientObservationRepository<T> GetObservationRepository(string dbName, string productName)
        {
            try
            {
                IPatientObservationRepository<T> repo = null;

                //We only have 1 repository at this time, just return it
                repo = new MongoObservationRepository<T>(dbName) as IPatientObservationRepository<T>;

                return repo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
