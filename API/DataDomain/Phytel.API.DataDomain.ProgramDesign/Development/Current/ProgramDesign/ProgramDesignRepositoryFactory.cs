using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

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
    }
}
