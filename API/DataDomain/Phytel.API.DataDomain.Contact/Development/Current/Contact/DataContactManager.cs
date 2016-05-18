using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.Interface;
using MongoDB.Bson;
using System.Configuration;
using Phytel.API.Common;
using System.Net;
using Phytel.API.DataAudit;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using Phytel.API.Common.CustomObject;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Contact
{
    public class ContactDataManager : IContactDataManager
    {
        protected static readonly int Limit = Convert.ToInt32(ConfigurationManager.AppSettings["RecentLimit"]);
        protected static readonly string DDLookupServiceUrl = ConfigurationManager.AppSettings["DDLookupServiceUrl"];

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

        public GetContactsByContactIdsDataResponse GetContactsByContactId(GetContactsByContactIdsDataRequest request)
        {
            var response = new GetContactsByContactIdsDataResponse();
            try
            {
                IContactRepository repo = Factory.GetRepository(request, RepositoryType.Contact);

                response.Contacts = repo.GetContactsByContactIds(request) as List<ContactData>;
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public string InsertContact(InsertContactDataRequest request)
        {
            string id = null;
            try
            {
                if (request == null)
                    throw new ArgumentNullException("request");
                CheckForRequiredFields(request.ContactData);
                IContactRepository repo = Factory.GetRepository(request, RepositoryType.Contact);
                if (repo == null)
                    throw new Exception("The repository should not be null");
                id = (string)repo.Insert(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return id;
        }

        private void CheckForRequiredFields(ContactData c)
        {
            if (c != null)
            {
                if (string.IsNullOrEmpty(c.ContactTypeId))
                    throw new Exception("The Contact Type Id cannot be null.");
                else
                {
                    if (string.Compare(c.ContactTypeId, Constants.PersonContactTypeId, true) == 0 && (string.IsNullOrEmpty(c.FirstName) || string.IsNullOrEmpty(c.LastName)))
                        throw new Exception("A contact of Person type cannot have empty First and Last name.");
                }
            }
            else
            {
                throw new ArgumentNullException("Contact");
            }
        }

        public UpdateContactDataResponse UpdateContact(UpdateContactDataRequest request)
        {
            UpdateContactDataResponse response = null;
            try
            {
                if (request == null)
                    throw new ArgumentNullException("request");
                CheckForRequiredFields(request.ContactData);
                IContactRepository repo = Factory.GetRepository(request, RepositoryType.Contact);
                if (repo == null)
                    throw new Exception("The repository should not be null");
                response = repo.Update(request) as UpdateContactDataResponse;
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

            if(request == null)
                throw new ArgumentNullException("request");
            try
            {
                IContactRepository repo = Factory.GetRepository(request, RepositoryType.Contact);

                if(repo == null)
                    throw new Exception("The repository should not be null");

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
                            PutRecentPatientRequest recentPatientRequest = new PutRecentPatientRequest
                            {
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
                    if (undeletedContact != null)
                    {
                        if (request.ContactWithUpdatedRecentLists != null && request.ContactWithUpdatedRecentLists.Count > 0)
                        {
                            request.ContactWithUpdatedRecentLists.ForEach(c =>
                            {
                                var contactData = repo.FindByID(c.ContactId) as ContactData;
                                if (contactData != null)
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
                    List<ContactData> contactData = request.ContactsData;
                    //#region Get the default timeZone.
                    //string defaultTimeZoneId = getDefaultTimeZone(request);
                    //#endregion  
                    #region Get all the available comm modes in the lookup.
                    List<CommModeData> commModeData = new List<CommModeData>();
                    List<IdNamePair> modesLookUp = getAllCommModes(request);
                    if (modesLookUp != null && modesLookUp.Count > 0)
                    {
                        foreach (IdNamePair l in modesLookUp)
                        {
                            commModeData.Add(new CommModeData { ModeId = l.Id, OptOut = false, Preferred = false });
                        }
                    }
                    #endregion
                    // Populate default CommModes and default timeZones to the Contact data before inserting.
                    contactData.ForEach(c =>
                    {
                        c.Modes = commModeData;
                        //c.TimeZoneId = defaultTimeZoneId;
                    });
                    list = new List<HttpObjectResponse<ContactData>>();
                    IContactRepository repo = Factory.GetRepository(request, RepositoryType.Contact);
                    BulkInsertResult result = (BulkInsertResult)repo.InsertAll(contactData.Cast<object>().ToList());
                    if (result != null)
                    {
                        if (result.ProcessedIds != null && result.ProcessedIds.Count > 0)
                        {
                            // Get the Contacts that were newly inserted. 
                            List<ContactData> insertedContacts = repo.Select(result.ProcessedIds) as List<ContactData>;
                            if (insertedContacts != null && insertedContacts.Count > 0)
                            {
                                #region DataAudit
                                List<string> insertedContactIds = insertedContacts.Select(p => p.Id).ToList();
                                AuditHelper.LogDataAudit(request.UserId, MongoCollectionName.Contact.ToString(), insertedContactIds, Common.DataAuditType.Insert, request.ContractNumber);
                                #endregion

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

        private List<IdNamePair> getAllCommModes(IDataDomainRequest request)
        {
            List<IdNamePair> response = null;
            try
            {
                response = new List<IdNamePair>();
                string DDLookupServiceUrl = ConfigurationManager.AppSettings["DDLookupServiceUrl"];
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/commmodes",
                                                                    DDLookupServiceUrl,
                                                                    "NG",
                                                                    request.Version,
                                                                    request.ContractNumber), request.UserId);

                // [Route("/{Context}/{Version}/{ContractNumber}/commmodes", "GET")]
                Phytel.API.DataDomain.LookUp.DTO.GetAllCommModesDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetAllCommModesDataResponse>(url);
                List<IdNamePair> dataList = dataDomainResponse.CommModes;
                if (dataList != null && dataList.Count > 0)
                {
                    response = dataList;
                }
            }
            catch (Exception ex) { throw ex; }
            return response;
        }

        private string getDefaultTimeZone(IDataDomainRequest request)
        {
            string timeZoneId = null;
            try
            {
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/TimeZone/Default",
                                                                        DDLookupServiceUrl,
                                                                        "NG",
                                                                        request.Version,
                                                                        request.ContractNumber), request.UserId);

                //  [Route("/{Context}/{Version}/{ContractNumber}/TimeZone/Default", "GET")]
                Phytel.API.DataDomain.LookUp.DTO.GetTimeZoneDataResponse dataDomainResponse = client.Get<Phytel.API.DataDomain.LookUp.DTO.GetTimeZoneDataResponse>(url);
                if (dataDomainResponse != null && dataDomainResponse.TimeZone != null)
                {
                    timeZoneId = dataDomainResponse.TimeZone.Id;
                }
            }
            catch (Exception ex) { throw ex; }
            return timeZoneId;
        }

        public SearchContactsDataResponse SearchContacts(SearchContactsDataRequest request)
        {
            var response = new SearchContactsDataResponse();
            try
            {
                var repo = Factory.GetRepository(request, RepositoryType.Contact);

                var searchTotalCount = repo.GetSearchContactsCount(request);
                if (searchTotalCount > 0)
                {
                    response.Contacts = repo.SearchContacts(request) as List<ContactData>;
                }
                else
                {
                    response.Contacts = new List<ContactData>();
                }

                
                response.Version = request.Version;
                response.TotalCount = searchTotalCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public SyncContactInfoDataResponse SyncContactInfo(SyncContactInfoDataRequest request)
        {
            var response = new SyncContactInfoDataResponse();
            try
            {
                var repo = Factory.GetRepository(request, RepositoryType.Contact);
                var isSuccessful = repo.SyncContact(request);

                response.IsSuccessful = isSuccessful;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public DereferencePatientDataResponse DereferencePatient(DereferencePatientDataRequest request)
        {
            var response = new DereferencePatientDataResponse();
            try
            {
                var repo = Factory.GetRepository(request, RepositoryType.Contact);
                var isSuccessful = repo.DereferencePatient(request);

                response.IsSuccessful = isSuccessful;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        //??
        public UndoDereferencePatientDataResponse UndoDereferencePatient(UndoDereferencePatientDataRequest request)
        {
            var response = new UndoDereferencePatientDataResponse();
            try
            {
                var repo = Factory.GetRepository(request, RepositoryType.Contact);
                var isSuccessful = repo.UnDereferencePatient(request);

                response.IsSuccessful = isSuccessful;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }
}
