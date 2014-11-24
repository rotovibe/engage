using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.API.DataDomain.PatientSystem.Test
{
    class StubPatientSystemRepositoryFactory : IPatientSystemRepositoryFactory
    {
        public IPatientSystemRepository GetRepository(Interface.IDataDomainRequest request, DTO.RepositoryType type)
        {
            try
            {
                IPatientSystemRepository repo = new StubPatientSystemRepository(request.ContractNumber);
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
