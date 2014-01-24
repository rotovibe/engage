using Phytel.API.DataDomain.Contact.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Contact;
using System;
using Phytel.API.Common.Format;

namespace Phytel.API.DataDomain.Contact
{
    public static class ContactDataManager
    {
        public static GetContactResponse GetContactByID(GetContactRequest request)
        {
            try
            {
                GetContactResponse result = new GetContactResponse();

                IContactRepository<GetContactResponse> repo = ContactRepositoryFactory<GetContactResponse>.GetContactRepository(request.ContractNumber, request.Context);
                result = repo.FindByID(request.ContactID) as GetContactResponse;

                // if cross-domain service call has error
                //if (result.Status != null)
                //{
                //    throw new ArgumentException(result.Status.Message, new Exception() { Source = result.Status.StackTrace });
                //}

                return (result != null ? result : new GetContactResponse());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static GetAllContactsResponse GetContactList(GetAllContactsRequest request)
        {
            try
            {
                GetAllContactsResponse result = new GetAllContactsResponse();

                IContactRepository<GetAllContactsResponse> repo = ContactRepositoryFactory<GetAllContactsResponse>.GetContactRepository(request.ContractNumber, request.Context);
               

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}   
