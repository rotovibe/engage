using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientGoal
{
    public abstract class PatientGoalRepositoryFactory<T>
    {
        public static IPatientGoalRepository<T> GetPatientGoalRepository(string dbName, string productName)
        {
            try
            {
                IPatientGoalRepository<T> repo = null;

                //We only have 1 repository at this time, just return it
                repo = new MongoPatientGoalRepository<T>(dbName) as IPatientGoalRepository<T>;

                return repo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IPatientGoalRepository<T> GetPatientBarrierRepository(string dbName, string productName)
        {
            try
            {
                IPatientGoalRepository<T> repo = null;

                //We only have 1 repository at this time, just return it
                repo = new MongoPatientBarrierRepository<T>(dbName) as IPatientGoalRepository<T>;

                return repo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IPatientGoalRepository<T> GetPatientTaskRepository(string dbName, string productName)
        {
            try
            {
                IPatientGoalRepository<T> repo = null;

                //We only have 1 repository at this time, just return it
                repo = new MongoPatientTaskRepository<T>(dbName) as IPatientGoalRepository<T>;

                return repo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static IPatientGoalRepository<T> GetPatientInterventionRepository(string dbName, string productName)
        {
            try
            {
                IPatientGoalRepository<T> repo = null;

                //We only have 1 repository at this time, just return it
                repo = new MongoPatientInterventionRepository<T>(dbName) as IPatientGoalRepository<T>;

                return repo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static IAttributeRepository<T> GetAttributeLibraryRepository(string dbName, string productName)
        {
            try
            {
                IAttributeRepository<T> repo = null;

                //We only have 1 repository at this time, just return it
                repo = new MongoAttributeLibraryRepository<T>(dbName) as IAttributeRepository<T>;

                return repo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
