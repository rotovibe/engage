using Phytel.API.DataDomain.Scheduling.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Scheduling
{
    public interface ISchedulingRepositoryFactory
    {
        ISchedulingRepository GetRepository(IDataDomainRequest request, RepositoryType type);
    }
}

