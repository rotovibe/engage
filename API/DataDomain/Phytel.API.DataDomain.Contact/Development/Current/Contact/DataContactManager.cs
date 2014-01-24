using System;
using Phytel.API.DataDomain.Contact.DTO;

namespace Phytel.API.DataDomain.Contact
{
    public static class ContactDataManager
    {
        public static GetContactDataResponse GetContactByPatientId(GetContactDataRequest request)
        {
            try
            {
                GetContactDataResponse result = new GetContactDataResponse();

                IContactRepository<GetContactDataResponse> repo = ContactRepositoryFactory<GetContactDataResponse>.GetContactRepository(request.ContractNumber, request.Context);
                result = repo.FindByID(request.PatientId) as GetContactDataResponse;
                return (result != null ? result : new GetContactDataResponse());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}   
