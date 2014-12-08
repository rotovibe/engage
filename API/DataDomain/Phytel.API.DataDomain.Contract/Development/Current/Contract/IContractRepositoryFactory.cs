using Phytel.API.Interface;
using System;
namespace Phytel.API.DataDomain.Contract
{
    public interface IContractRepositoryFactory
    {
        IContractRepository GetRepository(IDataDomainRequest request, RepositoryType type);
    }
}
