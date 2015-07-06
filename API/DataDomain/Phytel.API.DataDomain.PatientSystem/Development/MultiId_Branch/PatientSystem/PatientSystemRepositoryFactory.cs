using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientSystem
{
    public class PatientSystemRepositoryFactory : IPatientSystemRepositoryFactory
    {
        public IPatientSystemRepository GetRepository(IDataDomainRequest request, RepositoryType type)
        {
            try
            {
                IPatientSystemRepository repo = null;

                switch (type)
                {
                    case RepositoryType.PatientSystem:
                        {
                            repo = new MongoPatientSystemRepository(request.ContractNumber);
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
