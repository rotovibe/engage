using DataDomain.PatientNote.Repo;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientNote.Repo
{
    public interface IPatientNoteRepositoryFactory
    {
        IMongoPatientNoteRepository GetRepository(RepositoryType type);
    }
}