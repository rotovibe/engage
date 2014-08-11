using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;
using Phytel.API.DataDomain.PatientObservation.MongoDB.DTO;
using Phytel.API.DataDomain.PatientObservation.DTO;

namespace Phytel.API.DataDomain.PatientObservation
{
    public class PatientObservationRepositoryFactory : IPatientObservationRepositoryFactory
    {
        public IPatientObservationRepository GetRepository(IDataDomainRequest request, RepositoryType type)
        {
            IPatientObservationRepository repo = null;

            switch (type)
            {
                case RepositoryType.PatientObservation:
                {
                    repo =
                        new MongoPatientObservationRepository(request.ContractNumber);
                    break;
                }
                case RepositoryType.Observation:
                {
                    repo =
                        new MongoObservationRepository(request.ContractNumber);
                    break;
                }
            }

            if (repo != null)
                repo.UserId = request.UserId;

            return repo;
        }
    }
}
