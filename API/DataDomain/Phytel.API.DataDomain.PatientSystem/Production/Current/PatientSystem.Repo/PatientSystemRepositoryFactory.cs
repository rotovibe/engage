using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientSystem
{
    public enum RepositoryType
    {
        PatientSystem,
        System
    }
    
    
    public class PatientNoteRepositoryFactory : IPatientSystemRepositoryFactory
    {
        private readonly string _contract;
        private readonly string _userId;
        private readonly PatientSystemMongoContext _context;

        public PatientNoteRepositoryFactory(string contract, string userid)
        {
            _contract = contract;
            _userId = userid;
            _context = new PatientSystemMongoContext(_contract);
        }

        public IMongoPatientSystemRepository GetRepository(RepositoryType type)
        {
            try
            {
                IMongoPatientSystemRepository repo = null;

                switch (type)
                {
                    case RepositoryType.PatientSystem:
                        {
                            repo = new MongoPatientSystemRepository<PatientSystemMongoContext>(_context) { UserId = _userId, ContractDBName = _contract };
                            break;
                        }
                    case RepositoryType.System:
                        {
                            repo = new MongoSystemRepository<PatientSystemMongoContext>(_context) { UserId = _userId, ContractDBName = _contract };
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
