using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.CareMember.DTO;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using Phytel.API.Common;

namespace Phytel.API.AppDomain.NG
{
    public class CareMembersManager : ManagerBase
    {
        #region endpoint addresses
        protected static readonly string DDCareMemberUrl = ConfigurationManager.AppSettings["DDCareMemberUrl"];
        protected static readonly string DDContactServiceUrl = ConfigurationManager.AppSettings["DDContactServiceUrl"];
        protected static readonly string DDPatientServiceURL = ConfigurationManager.AppSettings["DDPatientServiceUrl"];
        #endregion

        public CareMember GetCareMember(GetCareMemberRequest request)
        {
            try
            {
                CareMember result = null;
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/CareMember/{Id}", "GET")]
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/CareMember/{5}",
                    DDCareMemberUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.Id), request.UserId);

                GetCareMemberDataResponse ddResponse = client.Get<GetCareMemberDataResponse>(url);

                if (ddResponse != null && ddResponse.CareMember != null)
                {
                    CareMemberData n = ddResponse.CareMember;
                    List<string> contactIds = new List<string>();
                    contactIds.Add(n.ContactId);
                    List<ContactData> contactsData  = getContactDetails(contactIds, request.Version, request.ContractNumber, request.UserId);
                    string pfName = string.Empty;
                    string gender = string.Empty;
                    var contact = contactsData.Find(a => a.Id == n.ContactId);
                    if(contact != null)
                    {
                        pfName  = contact.PreferredName;
                        gender = contact.Gender;
                    }
                    result = new CareMember
                    {
                        Id = n.Id,
                        PatientId = n.PatientId,
                        ContactId = n.ContactId,
                        PreferredName  = pfName,
                        Gender = gender,
                        Primary = n.Primary,
                        TypeId = n.TypeId
                    };
                }
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetCareMember()::" + ex.Message, ex.InnerException);
            }
        }

        public List<CareMember> GetAllCareMembers(GetAllCareMembersRequest request)
        {
            try
            {
                List<CareMember> result = null;
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/CareMembers", "GET")]
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/CareMembers",
                    DDCareMemberUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId), request.UserId);

                GetAllCareMembersDataResponse ddResponse = client.Get<GetAllCareMembersDataResponse>(url);

                if (ddResponse != null && ddResponse.CareMembers != null)
                {
                    result = new List<CareMember>();
                    List<CareMemberData> dataList = ddResponse.CareMembers;
                    List<string> contactIds = new List<string>();
                    if(dataList.Count > 0)
                    {
                        foreach (CareMemberData ctm in dataList)
                        {
                            contactIds.Add(ctm.ContactId);
                        }
                    }
                    List<ContactData> contactsData  = getContactDetails(contactIds, request.Version, request.ContractNumber, request.UserId);
                    foreach (CareMemberData n in dataList)
                    {
                        string pfName = string.Empty;
                        string gender = string.Empty;
                        var contact = contactsData.Find(a => a.Id == n.ContactId);
                        if(contact != null)
                        {
                            pfName  = contact.PreferredName;
                            gender = contact.Gender;
                        }
                        result.Add(new CareMember
                        {
                            Id = n.Id,
                            PatientId = n.PatientId,
                            ContactId = n.ContactId,
                            PreferredName = pfName,
                            Gender = gender,
                            Primary = n.Primary,
                            TypeId = n.TypeId
                        });
                    }
                }
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetAllCareMembers()::" + ex.Message, ex.InnerException);
            }
        }

        public PostCareMemberResponse InsertCareMember(PostCareMemberRequest request)
        {
            try
            {
                PostCareMemberResponse response = new PostCareMemberResponse();
                if (request.CareMember == null)
                    throw new Exception("The CareMember property cannot be null in the request.");

                if (!atleastOneCareMemberExists(request))
                {
                    //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/CareMember/Insert", "PUT")]
                    IRestClient client = new JsonServiceClient();
                    string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/patient/{4}/CareMember/Insert",
                                                                                    DDCareMemberUrl,
                                                                                    "NG",
                                                                                    request.Version,
                                                                                    request.ContractNumber,
                                                                                    request.PatientId), request.UserId);

                    CareMemberData cmData = new CareMemberData
                    {
                        PatientId = request.CareMember.PatientId,
                        ContactId = request.CareMember.ContactId,
                        TypeId = request.CareMember.TypeId,
                        Primary = request.CareMember.Primary
                    };
                    PutCareMemberDataResponse dataDomainResponse =
                        client.Put<PutCareMemberDataResponse>(url, new PutCareMemberDataRequest
                                                                                    {
                                                                                        CareMember = cmData,
                                                                                        Context = "NG",
                                                                                        ContractNumber = request.ContractNumber,
                                                                                        Version = request.Version,
                                                                                        UserId = request.UserId,
                                                                                        PatientId = request.PatientId
                                                                                    } as object);
                    if (dataDomainResponse != null && !(string.IsNullOrEmpty(dataDomainResponse.Id)))
                    {
                        response.Id = dataDomainResponse.Id;
                        response.Version = dataDomainResponse.Version;

                        if (request.CareMember.Primary)
                        {
                            //Upsert the PrimaryCare Manager's ContactId in the CohortPatientView collection.
                            upsertCohortPatientView(request.PatientId, request.CareMember.ContactId, response.Version, request.ContractNumber, request.UserId);
                        }
                    }
                }
                return response;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:InsertCareMember()::" + ex.Message, ex.InnerException);
            }
        }

        private bool atleastOneCareMemberExists(PostCareMemberRequest request)
        {
            bool result = false;
            //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/CareMembers", "GET")]
            IRestClient client = new JsonServiceClient();
            string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/CareMembers",
                DDCareMemberUrl,
                "NG",
                request.Version,
                request.ContractNumber,
                request.PatientId), request.UserId);

            GetAllCareMembersDataResponse ddResponse = client.Get<GetAllCareMembersDataResponse>(url);

            if (ddResponse != null && ddResponse.CareMembers != null && ddResponse.CareMembers.Count > 0)
            {
                result = true;
            }
            return result;
        }

        public PostUpdateCareMemberResponse UpdateCareMember(PostUpdateCareMemberRequest request)
        {
            try
            {
                if (request.CareMember == null)
                    throw new Exception("The CareMember property cannot be null in the request.");

                PostUpdateCareMemberResponse response = new PostUpdateCareMemberResponse();
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/CareMember/Update", "PUT")]
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/patient/{4}/CareMember/Update",
                                                                                DDCareMemberUrl,
                                                                                "NG",
                                                                                request.Version,
                                                                                request.ContractNumber,
                                                                                request.PatientId), request.UserId);

                CareMemberData cmData = new CareMemberData
                {
                    Id = request.CareMember.Id,
                    PatientId = request.CareMember.PatientId,
                    ContactId = request.CareMember.ContactId,
                    TypeId = request.CareMember.TypeId,
                    Primary = request.CareMember.Primary
                };
                PutUpdateCareMemberDataResponse dataDomainResponse =
                    client.Put<PutUpdateCareMemberDataResponse>(url, new PutUpdateCareMemberDataRequest
                                                                                {
                                                                                    CareMember = cmData,
                                                                                    Context = "NG",
                                                                                    ContractNumber = request.ContractNumber,
                                                                                    Version = request.Version,
                                                                                    UserId = request.UserId,
                                                                                    PatientId = request.PatientId
                                                                                } as object);
                if (dataDomainResponse != null && dataDomainResponse.Updated)
                {
                    response.Version = dataDomainResponse.Version;

                    if (request.CareMember.Primary)
                    {
                        //Upsert the PrimaryCare Manager's ContactId in the CohortPatientView collection.
                        upsertCohortPatientView(request.PatientId, request.CareMember.ContactId, response.Version, request.ContractNumber, request.UserId);
                    }
                }
                return response;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:UpdateCareMember()::" + ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Get the contact Details for a list of Contact ids.
        /// </summary>
        /// <param name="contactIds">List of Contact Ids.</param>
        /// <param name="version">version of the request.</param>
        /// <param name="contractNumber">contract number of the request.</param>
        /// <param name="userId">user id making the request.</param>
        /// <returns>List of contact data.</returns>
        private List<ContactData> getContactDetails(List<string> contactIds, double version, string contractNumber, string userId)
        {

            List<ContactData> contactsData = new List<ContactData>();
            if(contactIds.Count > 0)
            {
                GetContactsByContactIdsDataResponse contactDataResponse = null;
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Contact",
                                                        DDContactServiceUrl,
                                                        "NG",
                                                        version,
                                                        contractNumber), userId);

                //[Route("/{Context}/{Version}/{ContractNumber}/Contact", "POST")]
                contactDataResponse = client.Post<GetContactsByContactIdsDataResponse>(url, new GetContactsByContactIdsDataRequest()
                                                        {
                                                            ContactIds = contactIds,
                                                            Context = "NG",
                                                            ContractNumber = contractNumber,
                                                            UserId = userId,
                                                            Version = version
                                                        } as object
                                                        );

                if (contactDataResponse != null && contactDataResponse.Contacts != null)
                {
                    contactsData = contactDataResponse.Contacts;
                }
            }
            return contactsData;
        }

        private void upsertCohortPatientView(string patientId, string contactId, double version, string contractNumber, string userId)
        {
            string context = "NG";
            try
            {
                IRestClient client = new JsonServiceClient();
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/patient/{4}/cohortpatientview/",
                    DDPatientServiceURL,
                    context,
                    version,
                    contractNumber,
                    patientId), userId);

                GetCohortPatientViewResponse getResponse =
                    client.Get<GetCohortPatientViewResponse>(url);

                if (getResponse != null && getResponse.CohortPatientView != null)
                {
                    CohortPatientViewData cpvd = getResponse.CohortPatientView;
                    // check to see if primary care manager's contactId exists in the searchfield
                    if (!cpvd.SearchFields.Exists(sf => sf.FieldName == Constants.PCM))
                    {
                        cpvd.SearchFields.Add(new SearchFieldData
                        {
                            Value = contactId,
                            Active = true,
                            FieldName = Constants.PCM
                        });
                    }
                    else
                    {
                        cpvd.SearchFields.ForEach(sf =>
                        {
                            if (sf.FieldName == Constants.PCM)
                            {
                                sf.Value = contactId;
                                sf.Active = true;
                            }
                        });
                    }

                    string cohortPatientViewURL = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/patient/{4}/cohortpatientview/update",
                        DDPatientServiceURL,
                        context,
                        version,
                        contractNumber,
                        patientId), userId);

                    PutUpdateCohortPatientViewResponse putResponse =
                        client.Put<PutUpdateCohortPatientViewResponse>(cohortPatientViewURL, new PutUpdateCohortPatientViewRequest
                        {
                            CohortPatientView = cpvd,
                            ContractNumber = contractNumber,
                            PatientID = patientId
                        } as object);
                }
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:UpsertCohortPatientView()::" + ex.Message, ex.InnerException);
            }
        }
    }

}
