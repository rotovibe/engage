using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientNote
{
    public class PatientNoteRepositoryFactory : IPatientNoteRepositoryFactory
    {
        public IPatientNoteRepository GetRepository(IDataDomainRequest request, RepositoryType type)
        {
            try
            {
                IPatientNoteRepository repo = null;

                switch (type)
                {
                    case RepositoryType.PatientNote:
                        {
                            repo = new MongoPatientNoteRepository(request.ContractNumber);
                            break;
                        }
                }

                repo.UserId = request.UserId;
                return repo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
