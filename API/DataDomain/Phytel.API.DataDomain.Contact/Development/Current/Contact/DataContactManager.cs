using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.Interface;

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

        public static ContactData GetContactByUserId(GetContactByUserIdDataRequest request)
        {
            ContactData result = null;
            try
            {
                IContactRepository<GetContactByUserIdDataResponse> repo = ContactRepositoryFactory<GetContactByUserIdDataResponse>.GetContactRepository(request.ContractNumber, request.Context);
                result = repo.FindContactByUserId(request) as ContactData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static SearchContactsDataResponse SearchContacts(SearchContactsDataRequest request)
        {
            SearchContactsDataResponse response = new SearchContactsDataResponse();
            try
            {
                IContactRepository<List<ContactData>> repo = ContactRepositoryFactory<List<ContactData>>.GetContactRepository(request.ContractNumber, request.Context);
                response.Contacts = repo.SearchContacts(request) as List<ContactData>;
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public static PutContactDataResponse InsertContact(PutContactDataRequest request)
        {
            PutContactDataResponse response = null;
            try
            {
                IContactRepository<PutContactDataResponse> repo = ContactRepositoryFactory<PutContactDataResponse>.GetContactRepository(request.ContractNumber, request.Context);
                response = repo.Insert(request) as PutContactDataResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public static PutUpdateContactDataResponse UpdateContact(PutUpdateContactDataRequest request)
        {
            PutUpdateContactDataResponse response = null;
            try
            {
                IContactRepository<PutUpdateContactDataResponse> repo = ContactRepositoryFactory<PutUpdateContactDataResponse>.GetContactRepository(request.ContractNumber, request.Context);
                response = repo.Update(request) as PutUpdateContactDataResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public static GetAllCareManagersDataResponse GetCareManagers(GetAllCareManagersDataRequest request)
        {
            GetAllCareManagersDataResponse response = new GetAllCareManagersDataResponse();
            try
            {
                IContactRepository<List<ContactData>> repo = ContactRepositoryFactory<List<ContactData>>.GetContactRepository(request.ContractNumber, request.Context);
                response.Contacts = repo.FindCareManagers() as List<ContactData>;
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }
}   
