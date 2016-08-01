using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.CareMember.DTO;

namespace Phytel.API.DataDomain.CareMember.Test
{
    public class StubCareMemberRepositoryFactory : ICareMemberRepositoryFactory
    {
        public ICareMemberRepository GetRepository(Interface.IDataDomainRequest request, RepositoryType type)
        {
            ICareMemberRepository cr = new StubMongoCareMemberRepository() as ICareMemberRepository;
            return cr;
        }
    }
}
