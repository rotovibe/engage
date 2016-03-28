using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Contact.DTO.ContactTypeLookUp;

namespace Phytel.API.DataDomain.Contact.ContactTypeLookUp
{
    public interface IContactTypeLookUpManager
    {
        GetContactTypeLookUpDataResponse GetContactTypeLookUps(GetContactTypeLookUpDataRequest request);
        PutContactTypeLookUpDataResponse SavContactTypeLookUp(PutContactTypeLookUpDataRequest request);
    }
}
