using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientGoal
{
    public class PatientGoalRepositoryFactory : IPatientGoalRepositoryFactory
    {
        public IPatientGoalRepository GetRepository(IDataDomainRequest request, RepositoryType type)
        {
            try
            {
                IPatientGoalRepository repo = null;

                switch (type)
                {
                    case RepositoryType.PatientGoal:
                    {
                        repo = new MongoPatientGoalRepository(request.ContractNumber);
                        break;
                    }
                    case RepositoryType.PatientBarrier:
                    {
                        repo = new MongoPatientBarrierRepository(request.ContractNumber);
                        break;
                    }
                    case RepositoryType.PatientTask:
                    {
                        repo = new MongoPatientTaskRepository(request.ContractNumber);
                        break;
                    }
                    case RepositoryType.PatientIntervention:
                    {
                        repo = new MongoPatientInterventionRepository(request.ContractNumber);
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
