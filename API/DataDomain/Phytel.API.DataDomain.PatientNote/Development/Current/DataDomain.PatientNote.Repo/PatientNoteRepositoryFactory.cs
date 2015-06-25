using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientNote.Repo
{
    public enum RepositoryType
    {
        PatientNote
    }
    
    public abstract class PatientNoteRepositoryFactory
    {
        public static IMongoPatientNoteRepository GetRepository(IDataDomainRequest request, RepositoryType type)
        {
            try
            {
                IMongoPatientNoteRepository repo = null;

                switch (type)
                {
                    case RepositoryType.PatientNote:
                        {
                            var context = new PatientNoteMongoContext(request.ContractNumber);
                            repo = new MongoPatientNoteRepository<PatientNoteMongoContext>(context) { UserId = request.UserId, ContractDBName = request.ContractNumber };
                            break;
                        }
                }
                return repo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

