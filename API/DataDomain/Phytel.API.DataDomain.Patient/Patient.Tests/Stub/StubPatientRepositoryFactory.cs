using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Patient.Test.Stub
{
    public class StubPatientRepositoryFactory : IPatientRepositoryFactory
    {
        public IPatientRepository GetRepository(IDataDomainRequest request, RepositoryType type)
        {
            IPatientRepository repo = null;

            switch (type)
            {
                case RepositoryType.Patient:
                    {
                        repo = new StubPatientRespository(request.ContractNumber) as IPatientRepository;
                        break;
                    }
                case RepositoryType.CohortPatientView:
                    {
                        repo = new StubCohortPatientViewRepository(request.ContractNumber) as IPatientRepository;
                        break;
                    }
            }

            repo.UserId = request.UserId;
            return repo;
        }
    }
}
