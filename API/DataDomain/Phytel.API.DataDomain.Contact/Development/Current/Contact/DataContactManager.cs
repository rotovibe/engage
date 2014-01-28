using System;
using Phytel.API.DataDomain.Contact.DTO;

namespace Phytel.API.DataDomain.Contact
{
    public static class ContactDataManager
    {
        public static ContactData GetContactByPatientId(GetContactDataRequest request)
        {
            ContactData result = null;
            try
            {
                IContactRepository<GetContactDataResponse> repo = ContactRepositoryFactory<GetContactDataResponse>.GetContactRepository(request.ContractNumber, request.Context);
                result = repo.FindContactByPatientId(request) as ContactData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}   
