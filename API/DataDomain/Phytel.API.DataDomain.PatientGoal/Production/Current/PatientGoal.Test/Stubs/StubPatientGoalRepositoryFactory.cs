using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientGoal.Test.Stubs
{
    public class StubPatientGoalRepositoryFactory : IPatientGoalRepositoryFactory
    {
        public IGoalRepository GetRepository(IDataDomainRequest request, RepositoryType type)
        {
            try
            {
                IGoalRepository repo = null;

                switch (type)
                {
                    case RepositoryType.PatientGoal:
                    {
                        repo = new StubMongoPatientGoalRepository(request.ContractNumber);
                        break;
                    }
                    //case RepositoryType.PatientBarrier:
                    //{
                    //    repo = new MongoPatientBarrierRepository(request.ContractNumber);
                    //    break;
                    //}
                    //case RepositoryType.PatientTask:
                    //{
                    //    repo = new MongoPatientTaskRepository(request.ContractNumber);
                    //    break;
                    //}
                    //case RepositoryType.PatientIntervention:
                    //{
                    //    repo = new MongoPatientInterventionRepository(request.ContractNumber);
                    //    break;
                    //}
                    //case RepositoryType.Goal:
                    //{
                    //    repo = new MongoGoalRepository(request.ContractNumber);
                    //    break;
                    //}
                    //case RepositoryType.Task:
                    //{
                    //    repo = new MongoTaskRepository(request.ContractNumber);
                    //    break;
                    //}
                    //case RepositoryType.Intervention:
                    //{
                    //    repo = new MongoInterventionRepository(request.ContractNumber);
                    //    break;
                    //}
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
