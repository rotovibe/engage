using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Contact.Test.Stubs
{
    public class StubContactRepositoryFactory : IContactRepositoryFactory
    {
        public IContactRepository GetRepository(Interface.IDataDomainRequest request, RepositoryType type)
        {
            IContactRepository cr = new StubMongoContactRepository() as IContactRepository;
            return cr;
        }
    }
}
