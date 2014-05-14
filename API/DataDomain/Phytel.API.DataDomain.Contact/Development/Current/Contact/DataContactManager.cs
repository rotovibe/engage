using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.Interface;
using MongoDB.Bson;
using System.Configuration;

namespace Phytel.API.DataDomain.Contact
{
    public class ContactDataManager : IContactDataManager
    {
        protected static readonly int Limit = Convert.ToInt32(ConfigurationManager.AppSettings["RecentLimit"]);

        public IContactRepositoryFactory Factory { get; set; }

        public ContactData GetContactByPatientId(GetContactByPatientIdDataRequest request)
        {
            ContactData result = null;
            try
            {
                IContactRepository repo = Factory.GetRepository(request, RepositoryType.Contact);
                
                result = repo.FindContactByPatientId(request) as ContactData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public ContactData GetContactByUserId(GetContactByUserIdDataRequest request)
        {
            ContactData result = null;
            try
            {
                IContactRepository repo = Factory.GetRepository(request, RepositoryType.Contact);

                result = repo.FindContactByUserId(request) as ContactData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public SearchContactsDataResponse SearchContacts(SearchContactsDataRequest request)
        {
            SearchContactsDataResponse response = new SearchContactsDataResponse();
            try
            {
                IContactRepository repo = Factory.GetRepository(request, RepositoryType.Contact);

                response.Contacts = repo.SearchContacts(request) as List<ContactData>;
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public PutContactDataResponse InsertContact(PutContactDataRequest request)
        {
            PutContactDataResponse response = null;
            try
            {
                IContactRepository repo = Factory.GetRepository(request, RepositoryType.Contact);
                
                response = repo.Insert(request) as PutContactDataResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public PutUpdateContactDataResponse UpdateContact(PutUpdateContactDataRequest request)
        {
            PutUpdateContactDataResponse response = null;
            try
            {
                IContactRepository repo = Factory.GetRepository(request, RepositoryType.Contact);

                response = repo.Update(request) as PutUpdateContactDataResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public GetAllCareManagersDataResponse GetCareManagers(GetAllCareManagersDataRequest request)
        {
            GetAllCareManagersDataResponse response = new GetAllCareManagersDataResponse();
            try
            {
                IContactRepository repo = Factory.GetRepository(request, RepositoryType.Contact);

                response.Contacts = repo.FindCareManagers() as List<ContactData>;
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public PutRecentPatientResponse AddRecentPatient(PutRecentPatientRequest request)
        {
            PutRecentPatientResponse response = new PutRecentPatientResponse { SuccessData = false, Version = 1.0 };
            try
            {
                string patientId;
                int limit = Limit;

                if (request.PatientId != null && request.PatientId.Length > 0)
                {
                    patientId = request.PatientId;

                    IContactRepository repo = Factory.GetRepository(request, RepositoryType.Contact);
                    // find contact
                    ContactData mContact = (ContactData)repo.FindByID(request.ContactId);

                    if (mContact.RecentsList == null)
                    {
                        mContact.RecentsList = new List<string>();
                    }

                    // update recent list
                    MruList mruList = new MruList { Limit = limit, RecentList = mContact.RecentsList };
                    mruList.AddPatient(patientId);

                    if (repo.UpdateRecentList(request, mruList.RecentList))
                    {
                        response.SuccessData = true;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ContactData GetContactByContactId(GetContactByContactIdDataRequest request)
        {
            ContactData result = null;
            try
            {
                IContactRepository repo = Factory.GetRepository(request, RepositoryType.Contact);

                result = repo.FindByID(request.ContactId) as ContactData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}   
