using Phytel.API.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.ProgramDesign
{
    public abstract class ProgramDesignRepositoryFactory<T>
    {
        public static IProgramDesignRepository<T> GetProgramDesignRepository(string dbName, string productName, string userId)
        {
            try
            {
                IProgramDesignRepository<T> repo = null;

                //We only have 1 repository at this time, just return it
                repo = new MongoProgramDesignRepository<T>(dbName) as IProgramDesignRepository<T>;
                repo.UserId = userId;
                return repo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IProgramDesignRepository<T> GetProgramRepository(string dbName, string productName, string userId)
        {
            IProgramDesignRepository<T> repo = null;

            //We only have 1 repository at this time, just return it
            repo = new MongoProgramRepository<T>(dbName) as IProgramDesignRepository<T>;
            repo.UserId = userId;
            return repo;
        }

        public static IProgramDesignRepository<T> GetContractProgramRepository(string dbName, string productName, string userId)
        {
            IProgramDesignRepository<T> repo = null;

            //We only have 1 repository at this time, just return it
            repo = new MongoContractProgramRepository<T>(dbName) as IProgramDesignRepository<T>;
            repo.UserId = userId;
            return repo;
        }

        public static IProgramDesignRepository<T> GetStepResponseRepository(string dbName, string productName, string userId)
        {
            IProgramDesignRepository<T> repo = null;

            //We only have 1 repository at this time, just return it
            repo = new MongoResponseRepository<T>(dbName) as IProgramDesignRepository<T>;
            repo.UserId = userId;
            return repo;
        }

        public static IProgramDesignRepository<T> GetModuleRepository(string dbName, string productName, string userId)
        {
            IProgramDesignRepository<T> repo = null;

            //We only have 1 repository at this time, just return it
            repo = new MongoModuleRepository<T>(dbName) as IProgramDesignRepository<T>;
            repo.UserId = userId;
            return repo;
        }

        public static IProgramDesignRepository<T> GetActionRepository(string dbName, string productName, string userId)
        {
            IProgramDesignRepository<T> repo = null;

            //We only have 1 repository at this time, just return it
            repo = new MongoActionRepository<T>(dbName) as IProgramDesignRepository<T>;
            repo.UserId = userId;
            return repo;
        }

        public static IProgramDesignRepository<T> GetStepRepository(string dbName, string productName, string type)
        {
            IProgramDesignRepository<T> repo = null;
            if (type.Equals("yesno", StringComparison.InvariantCultureIgnoreCase))
            {
                repo = new MongoYesNoStepRepository<T>(dbName) as IProgramDesignRepository<T>;
            }
            else if (type.Equals("text", StringComparison.InvariantCultureIgnoreCase))
            {
                repo = new MongoTextStepRepository<T>(dbName) as IProgramDesignRepository<T>;
            }
            return repo;
        }

        //public static IProgramDesignRepository<T> GetProgramAttributesRepository(string dbName, string productName, string userId)
        //{
        //    IProgramDesignRepository<T> repo = null;

        //    //We only have 1 repository at this time, just return it
        //    repo = new MongoPatientProgramAttributeRepository<T>(dbName) as IProgramDesignRepository<T>;
        //    repo.UserId = userId;
        //    return repo;
        //}
    }
}
