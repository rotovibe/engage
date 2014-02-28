using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientNote
{
    public abstract class PatientNoteRepositoryFactory<T>
    {
        public static IPatientNoteRepository<T> GetPatientNoteRepository(string dbName, string productName)
        {
            try
            {
                IPatientNoteRepository<T> repo = null;

                //We only have 1 repository at this time, just return it
                repo = new MongoPatientNoteRepository<T>(dbName) as IPatientNoteRepository<T>;

                return repo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
