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

        public UpsertBatchContactDataResponse UpsertContacts(UpsertBatchContactDataRequest request)
        {
            UpsertBatchContactDataResponse response = new UpsertBatchContactDataResponse();
            if (request.ContactsData != null && request.ContactsData.Count > 0)
            {
                List<HttpObjectResponse<ContactData>> list = new List<HttpObjectResponse<ContactData>>();
                IContactRepository repo = Factory.GetRepository(request, RepositoryType.Contact);
                request.ContactsData.ForEach(p =>
                {
                    HttpStatusCode code = HttpStatusCode.OK;
                    ContactData psData = null;
                    string message = string.Empty;
                    try
                    {
                        if (string.IsNullOrEmpty(p.PatientId))
                        {
                            code = HttpStatusCode.BadRequest;
                            message = string.Format("PatientId is missing for PatientId : {0}", p.PatientId);
                        }
                        else
                        {
                            ContactData data = (ContactData)repo.GetContactByPatientId(p.PatientId);
                            if (data == null)
                            {
                                PutContactDataRequest insertReq = new PutContactDataRequest
                                {
                                    PatientId = p.PatientId,
                                    Context = request.Context,
                                    ContractNumber = request.ContractNumber,
                                    ContactData = p,
                                    UserId = request.UserId,
                                    Version = request.Version
                                };
                                string id = (string)repo.Insert(insertReq);
                                if (!string.IsNullOrEmpty(id))
                                {
                                    code = HttpStatusCode.Created;
                                    psData = new ContactData { Id = id, PatientId = p.PatientId };
                                }
                            }
                            else
                            {
                                p.Id = data.Id;
                                PutUpdateContactDataRequest updateReq = new PutUpdateContactDataRequest
                                {
                                    ContactData = p,
                                    Context = request.Context,
                                    ContractNumber = request.ContractNumber,
                                    UserId = request.UserId,
                                    Version = request.Version
                                };
                                PutUpdateContactDataResponse updateRes = repo.Update(request) as PutUpdateContactDataResponse;
                                if (updateRes != null && updateRes.SuccessData)
                                {
                                    code = HttpStatusCode.NoContent;
                                    psData = new ContactData { Id = p.Id, PatientId = p.PatientId };
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        code = HttpStatusCode.InternalServerError;
                        message = string.Format("PatientId: {0}, Message: {1}, StackTrace: {2}", p.PatientId, ex.Message, ex.StackTrace);
                    }
                    list.Add(new HttpObjectResponse<ContactData> { Code = code, Body = (ContactData)psData, Message = message });
                });
                response.Responses = list;
            }
            return response;
        }
    }
}   
