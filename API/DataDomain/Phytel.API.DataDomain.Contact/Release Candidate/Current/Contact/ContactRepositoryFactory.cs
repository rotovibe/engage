using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;
using Phytel.API.DataAudit;

namespace Phytel.API.DataDomain.Contact
{
    public class ContactRepositoryFactory : IContactRepositoryFactory
    {
        public IContactRepository GetRepository(IDataDomainRequest request, RepositoryType type)
        {
            IContactRepository repo = null;

            switch (type)
            {
                case RepositoryType.Contact:
                    {
                        repo = new MongoContactRepository(request.ContractNumber) as IContactRepository;
                        break;
                    }
            }

            repo.UserId = request.UserId;
            return repo;
        }
    }
}
