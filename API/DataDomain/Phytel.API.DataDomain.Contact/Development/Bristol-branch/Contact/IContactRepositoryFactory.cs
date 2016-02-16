using Phytel.API.Interface;
using System;
namespace Phytel.API.DataDomain.Contact
{
    public interface IContactRepositoryFactory
    {
        IContactRepository GetRepository(IDataDomainRequest request, RepositoryType type);
    }
}
