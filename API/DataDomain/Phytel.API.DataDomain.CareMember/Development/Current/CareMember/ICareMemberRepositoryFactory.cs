using Phytel.API.DataDomain.CareMember.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.CareMember
{
    public interface ICareMemberRepositoryFactory
    {
        ICareMemberRepository GetRepository(IDataDomainRequest request, RepositoryType type);
    }
}
