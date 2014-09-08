using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientNote
{
    public interface IPatientNoteRepositoryFactory
    {
        IPatientNoteRepository GetRepository(IDataDomainRequest request, RepositoryType type);
    }
}

