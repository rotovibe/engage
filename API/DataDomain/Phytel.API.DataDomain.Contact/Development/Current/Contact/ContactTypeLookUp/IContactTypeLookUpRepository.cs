using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Contact.MongoDB.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact.ContactTypeLookUp
{
    public interface IContactTypeLookUpRepository : IRepository
    {

        object GetContactTypeLookUps(ContactLookUpGroupType type);
        string SaveContactTypeLookUp(ContactTypeLookUpData request, string userId);

    }
}
