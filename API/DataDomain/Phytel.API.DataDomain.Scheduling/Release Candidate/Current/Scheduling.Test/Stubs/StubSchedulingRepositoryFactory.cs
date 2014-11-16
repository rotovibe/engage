using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Scheduling;
using Phytel.API.DataDomain.Scheduling.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.ToDo.Test.Stubs
{
    class StubSchedulingRepositoryFactory : ISchedulingRepositoryFactory
    {
        public ISchedulingRepository GetRepository(IDataDomainRequest request, RepositoryType type)
        {
            try
            {
                ISchedulingRepository repo = null;

                switch (type)
                {
                    case RepositoryType.ToDo:
                        {
                            repo = new StubMongoToDoRepository(request.ContractNumber);
                            break;
                        }

                    case RepositoryType.Schedule:
                        {
                            repo = new StubMongoScheduleRepository(request.ContractNumber);
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
