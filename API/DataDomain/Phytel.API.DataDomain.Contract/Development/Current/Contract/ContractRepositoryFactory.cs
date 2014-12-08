using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;
using Phytel.API.DataAudit;

namespace Phytel.API.DataDomain.Contract
{
    public class ContractRepositoryFactory : IContractRepositoryFactory
    {
        public IContractRepository GetRepository(IDataDomainRequest request, RepositoryType type)
        {
            IContractRepository repo = null;

            switch (type)
            {
                case RepositoryType.Contract:
                    {
                        repo = new MongoContractRepository(request.ContractNumber) as IContractRepository;
                        break;
                    }
            }

            repo.UserId = request.UserId;
            return repo;
        }
    }
}
