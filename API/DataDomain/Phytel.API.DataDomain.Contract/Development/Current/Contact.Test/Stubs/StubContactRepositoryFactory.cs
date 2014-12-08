using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Contract.Test.Stubs
{
    public class StubContractRepositoryFactory : IContractRepositoryFactory
    {
        public IContractRepository GetRepository(Interface.IDataDomainRequest request, RepositoryType type)
        {
            IContractRepository cr = new StubMongoContractRepository() as IContractRepository;
            return cr;
        }
    }
}
