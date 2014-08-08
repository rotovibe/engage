using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;
using Phytel.API.DataDomain.Patient.MongoDB.DTO;

namespace Phytel.API.DataDomain.Patient
{
    public class PatientRepositoryFactory : IPatientRepositoryFactory
    {
        public IPatientRepository GetRepository(IDataDomainRequest request, RepositoryType type)
        {
            IPatientRepository repo = null;

            switch (type)
            {
                case RepositoryType.Patient:
                    {
                        repo = new MongoPatientRepository(request.ContractNumber) as IPatientRepository;
                        break;
                    }
                case RepositoryType.CohortPatientView:
                    {
                        repo = new MongoCohortPatientViewRepository(request.ContractNumber) { Utils = new DTOUtils() } as IPatientRepository;
                        break;
                    }
                case RepositoryType.PatientUser:
                    {
                        repo = new MongoPatientUserRepository(request.ContractNumber) as IPatientRepository;
                        break;
                    }
            }

            repo.UserId = request.UserId;
            return repo;
        }
    }
}
