using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.LookUp
{
    public class LookUpRepositoryFactory : ILookUpRepositoryFactory
    {
        public ILookUpRepository GetRepository(IDataDomainRequest request, RepositoryType type)
        {
            ILookUpRepository repo = null;

            switch (type)
            {
                case RepositoryType.LookUp:
                    {
                        repo = new MongoLookUpRepository(request.ContractNumber) as ILookUpRepository;
                        break;
                    }
                case RepositoryType.Setting:
                    {
                        repo = new MongoSettingRepository(request.ContractNumber) as ILookUpRepository;
                        break;
                    }
            }

            repo.UserId = request.UserId;
            return repo;
        }


    }
}
