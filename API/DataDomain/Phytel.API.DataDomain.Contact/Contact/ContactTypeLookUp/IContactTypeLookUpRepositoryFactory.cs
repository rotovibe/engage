using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact.ContactTypeLookUp
{
    public interface IContactTypeLookUpRepositoryFactory
    {
        IContactTypeLookUpRepository GetContactTypeLookUpRepository(IDataDomainRequest request, RepositoryType type);
    }
}
