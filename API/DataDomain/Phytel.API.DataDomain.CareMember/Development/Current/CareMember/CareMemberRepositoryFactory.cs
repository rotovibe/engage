using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.CareMember.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.CareMember
{
    public class CareMemberRepositoryFactory : ICareMemberRepositoryFactory
    {
        public ICareMemberRepository GetRepository(IDataDomainRequest request, RepositoryType type)
        {
            ICareMemberRepository repo = null;

            switch (type)
            {
                case RepositoryType.CareMember:
                    {
                        repo = new MongoCareMemberRepository(request.ContractNumber) as ICareMemberRepository;
                        break;
                    }
            }

            repo.UserId = request.UserId;
            return repo;
        }
    }
}
