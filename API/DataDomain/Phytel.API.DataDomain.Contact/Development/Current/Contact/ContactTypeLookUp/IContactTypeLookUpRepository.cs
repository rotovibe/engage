using Phytel.API.DataDomain.Contact.MongoDB.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact.ContactTypeLookUp
{
    public interface IContactTypeLookUpRepository : IRepository
    {

        object GetContactTypeLookUps(GroupType type);
        
    }
}
