using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Program
{
    public abstract class ProgramRepositoryFactory<T>
    {
        public static IProgramRepository<T> GetProgramRepository(string dbName, string productName)
        {
            IProgramRepository<T> repo = null;

            //We only have 1 repository at this time, just return it
            repo = new MongoProgramRepository<T>(dbName) as IProgramRepository<T>;

            return repo;
        }

        public static IProgramRepository<T> GetPatientProgramRepository(string dbName, string productName)
        {
            IProgramRepository<T> repo = null;

            //We only have 1 repository at this time, just return it
            repo = new MongoPatientProgramRepository<T>(dbName) as IProgramRepository<T>;

            return repo;
        }

        public static IProgramRepository<T> GetContractProgramRepository(string dbName, string productName)
        {
            IProgramRepository<T> repo = null;

            //We only have 1 repository at this time, just return it
            repo = new MongoContractProgramRepository<T>(dbName) as IProgramRepository<T>;

            return repo;
        }

        public static IProgramRepository<T> GetStepResponseRepository(string dbName, string productName)
        {
            IProgramRepository<T> repo = null;

            //We only have 1 repository at this time, just return it
            repo = new MongoResponseRepository<T>(dbName) as IProgramRepository<T>;

            return repo;
        }

        public static IProgramRepository<T> GetPatientProgramStepResponseRepository(string dbName, string productName)
        {
            IProgramRepository<T> repo = null;

            //We only have 1 repository at this time, just return it
            repo = new MongoPatientProgramResponseRepository<T>(dbName) as IProgramRepository<T>;

            return repo;
        }

        public static IProgramRepository<T> GetProgramAttributesRepository(string dbName, string productName)
        {
            IProgramRepository<T> repo = null;

            //We only have 1 repository at this time, just return it
            repo = new MongoPatientProgramAttributeRepository<T>(dbName) as IProgramRepository<T>;

            return repo;
        }

        internal static IProgramRepository<T> GetTempContractResponsesRepository(string dbName, string productName)
        {
            IProgramRepository<T> repo = null;

            //We only have 1 repository at this time, just return it
            repo = new MongoTempContractProgramResponseRepository<T>(dbName) as IProgramRepository<T>;

            return repo;
        }
    }
}
