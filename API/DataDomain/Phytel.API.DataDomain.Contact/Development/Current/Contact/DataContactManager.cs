using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.Interface;
using MongoDB.Bson;
using System.Configuration;
using Phytel.API.Common;
using System.Net;

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

        public string InsertContact(PutContactDataRequest request)
        {
            string id = null;
            try
            {
                IContactRepository repo = Factory.GetRepository(request, RepositoryType.Contact);
                id = (string)repo.Insert(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return id;
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

        public GetContactByContactIdDataResponse GetContactByContactId(GetContactByContactIdDataRequest request)
        {
            GetContactByContactIdDataResponse result = new GetContactByContactIdDataResponse();
            try
            {
                IContactRepository repo = Factory.GetRepository(request, RepositoryType.Contact);

                result.Contact = repo.FindByID(request.ContactId) as ContactData;
                result.Limit = Limit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DeleteContactByPatientIdDataResponse DeleteContactByPatientId(DeleteContactByPatientIdDataRequest request)
        {
            DeleteContactByPatientIdDataResponse response = null;
            bool success = false;
            try
            {
                response = new DeleteContactByPatientIdDataResponse();

                IContactRepository repo = Factory.GetRepository(request, RepositoryType.Contact);
                ContactData contact = repo.GetContactByPatientId(request.PatientId) as ContactData;
                List<ContactWithUpdatedRecentList> contactsWithUpdatedRecentList = null;
                if (contact != null)
                {
                    request.Id = contact.Id;
                    repo.Delete(request);
                    response.DeletedId = request.Id;
                    success = true;

                    // Remove this deleted contact(PatientId) from RecentList of  other contacts(users logged in).

                    List<ContactData> contactsWithAPatientInRecentList = repo.FindContactsWithAPatientInRecentList(request.PatientId) as List<ContactData>;
                    if (contactsWithAPatientInRecentList != null && contactsWithAPatientInRecentList.Count > 0)
                    {
                        contactsWithUpdatedRecentList = new List<ContactWithUpdatedRecentList>();
                        contactsWithAPatientInRecentList.ForEach(c =>
                        {
                            PutRecentPatientRequest recentPatientRequest = new PutRecentPatientRequest { 
                                ContactId = c.Id,
                                Context = request.Context,
                                ContractNumber = request.ContractNumber,
                                UserId = request.UserId,
                                Version = request.Version
                            };
                            int index = c.RecentsList.IndexOf(request.PatientId);
                            if (c.RecentsList.Remove(request.PatientId))
                            {
                                if (repo.UpdateRecentList(recentPatientRequest, c.RecentsList))
                                {
                                    contactsWithUpdatedRecentList.Add(new ContactWithUpdatedRecentList { ContactId = recentPatientRequest.ContactId, PatientIndex = index });
                                    success = true;
                                }
                            }
                        });
                    }
                }
                response.ContactWithUpdatedRecentLists = contactsWithUpdatedRecentList;
                response.Success = success;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }

        public UndoDeleteContactDataResponse UndoDeleteContact(UndoDeleteContactDataRequest request)
        {
            UndoDeleteContactDataResponse response = null;
            try
            {
                response = new UndoDeleteContactDataResponse();
                IContactRepository repo = Factory.GetRepository(request, RepositoryType.Contact);
                if (request.Id != null)
                {
                    repo.UndoDelete(request);
                    // Add the deleted contact back into the RecentList of  other contacts(users logged in) who had him/her before the delete action.
                    var undeletedContact = repo.FindByID(request.Id) as ContactData;
                    if(undeletedContact != null)
                    {
                        if(request.ContactWithUpdatedRecentLists != null && request.ContactWithUpdatedRecentLists.Count > 0)
                        {
                            request.ContactWithUpdatedRecentLists.ForEach(c =>
                            {
                               var contactData = repo.FindByID(c.ContactId) as ContactData;
                                if(contactData != null)
                                {
                                    contactData.RecentsList.Insert(c.PatientIndex, undeletedContact.PatientId);
                                    PutRecentPatientRequest recentPatientRequest = new PutRecentPatientRequest
                                    {
                                        ContactId = c.ContactId,
                                        Context = request.Context,
                                        ContractNumber = request.ContractNumber,
                                        UserId = request.UserId,
                                        Version = request.Version
                                    };
                                    repo.UpdateRecentList(recentPatientRequest, contactData.RecentsList);
                                }
                            });
                        }
                    }
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }

        public List<HttpObjectResponse<ContactData>> InsertBatchContacts(InsertBatchContactDataRequest request)
        {
            List<HttpObjectResponse<ContactData>> list = null;
            try
            {
                if (request.ContactsData != null && request.ContactsData.Count > 0)
                {
                    list = new List<HttpObjectResponse<ContactData>>();
                    IContactRepository repo = Factory.GetRepository(request, RepositoryType.Contact);
                    BulkInsertResult result = (BulkInsertResult)repo.InsertAll(request.ContactsData.Cast<object>().ToList());
                    if (result != null)
                    {
                        if (result.ProcessedIds != null && result.ProcessedIds.Count > 0)
                        {
                            // Get the Contacts that were newly inserted. 
                            List<ContactData> insertedContacts = repo.Select(result.ProcessedIds) as List<ContactData>;
                            if (insertedContacts != null && insertedContacts.Count > 0)
                            {
                                insertedContacts.ForEach(r =>
                                {
                                    list.Add(new HttpObjectResponse<ContactData> { Code = HttpStatusCode.Created, Body = (ContactData)new ContactData { Id = r.Id, PatientId = r.PatientId } });
                                });
                            }
                        }
                        result.ErrorMessages.ForEach(e =>
                        {
                            list.Add(new HttpObjectResponse<ContactData> { Code = HttpStatusCode.InternalServerError, Message = e });
                        });
                    }
                }
            }
            catch (Exception ex) { throw ex; }
            return list;
        }
    }
}   
