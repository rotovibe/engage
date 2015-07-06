using System;
using Phytel.API.DataDomain.PatientNote.Repo;

namespace DataDomain.PatientNote.Repo
{
    public enum RepositoryType
    {
        PatientNote,
        Utilization
    }
    
    public class PatientNoteRepositoryFactory : IPatientNoteRepositoryFactory
    {
        private readonly string _contract;
        private readonly string _userId;
        private readonly PatientNoteMongoContext _context;

        public PatientNoteRepositoryFactory(string contract, string userid)
        {
            _contract = contract;
            _userId = userid;
            _context = new PatientNoteMongoContext(_contract);
        }

        public IMongoPatientNoteRepository GetRepository(RepositoryType type)
        {
            try
            {
                IMongoPatientNoteRepository repo = null;

                switch (type)
                {
                    case RepositoryType.PatientNote:
                        {
                            repo = new MongoPatientNoteRepository<PatientNoteMongoContext>(_context) { UserId = _userId, ContractDBName = _contract};
                            break;
                        }
                    case RepositoryType.Utilization:
                        {
                            repo = new MongoPatientUtilizationRepository<PatientNoteMongoContext>(_context){UserId = _userId, ContractDBName = _contract};
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

