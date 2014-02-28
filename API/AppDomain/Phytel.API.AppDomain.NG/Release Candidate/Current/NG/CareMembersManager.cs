using System;
using System.Collections.Generic;
using System.Configuration;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.CareMember.DTO;
using Phytel.API.DataDomain.Contact.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG
{
    public class CareMembersManager : ManagerBase
    {
        #region endpoint addresses
        protected static readonly string DDCareMemberUrl = ConfigurationManager.AppSettings["DDCareMemberUrl"];
        protected static readonly string DDContactServiceUrl = ConfigurationManager.AppSettings["DDContactServiceUrl"];
        #endregion

        public CareMember GetCareMember(GetCareMemberRequest request)
        {
            try
            {
                CareMember result = null;
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/CareMember/{Id}", "GET")]
                IRestClient client = new JsonServiceClient();
                GetCareMemberDataResponse ddResponse = client.Get<GetCareMemberDataResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/CareMember/{5}?UserId={6}",
                    DDCareMemberUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.Id,
                    request.UserId));

                if (ddResponse != null && ddResponse.CareMember != null)
                {
                    CareMemberData n = ddResponse.CareMember;
                    List<string> contactIds = new List<string>();
                    contactIds.Add(n.ContactId);
                    List<ContactData> contactsData  = getContactDetails(contactIds, request.Version, request.ContractNumber, request.UserId);
                    string pfName = string.Empty;
                    string gender = string.Empty;
                    var contact = contactsData.Find(a => a.ContactId == n.ContactId);
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
                throw new WebServiceException("AD:GetCareMember()" + ex.Message, ex.InnerException);
            }
        }

        public List<CareMember> GetAllCareMembers(GetAllCareMembersRequest request)
        {
            try
            {
                List<CareMember> result = null;
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/CareMembers", "GET")]
                IRestClient client = new JsonServiceClient();
                GetAllCareMembersDataResponse ddResponse = client.Get<GetAllCareMembersDataResponse>(
                    string.Format("{0}/{1}/{2}/{3}/Patient/{4}/CareMembers?UserId={5}",
                    DDCareMemberUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.UserId));

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
                        var contact = contactsData.Find(a => a.ContactId == n.ContactId);
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
                throw new WebServiceException("AD:GetAllCareMembers()" + ex.Message, ex.InnerException);
            }
        }

        public PostCareMemberResponse InsertCareMember(PostCareMemberRequest request)
        {
            try
            {
                if (request.CareMember == null)
                    throw new Exception("The CareMember property cannot be null in the request.");

                PostCareMemberResponse response = new PostCareMemberResponse();
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/CareMember/Insert", "PUT")]
                IRestClient client = new JsonServiceClient();
                CareMemberData cmData = new CareMemberData
                {
                    PatientId = request.CareMember.PatientId,
                    ContactId = request.CareMember.ContactId,
                    TypeId = request.CareMember.TypeId,
                    Primary = request.CareMember.Primary
                };
                PutCareMemberDataResponse dataDomainResponse =
                    client.Put<PutCareMemberDataResponse>(string.Format("{0}/{1}/{2}/{3}/patient/{4}/CareMember/Insert?UserId={5}",
                                                                                DDCareMemberUrl,
                                                                                "NG",
                                                                                request.Version,
                                                                                request.ContractNumber,
                                                                                request.PatientId,
                                                                                request.UserId), new PutCareMemberDataRequest
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
                }

                return response;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
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
                CareMemberData cmData = new CareMemberData
                {
                    Id = request.CareMember.Id,
                    PatientId = request.CareMember.PatientId,
                    ContactId = request.CareMember.ContactId,
                    TypeId = request.CareMember.TypeId,
                    Primary = request.CareMember.Primary
                };
                PutUpdateCareMemberDataResponse dataDomainResponse =
                    client.Put<PutUpdateCareMemberDataResponse>(string.Format("{0}/{1}/{2}/{3}/patient/{4}/CareMember/Update?UserId={5}",
                                                                                DDCareMemberUrl,
                                                                                "NG",
                                                                                request.Version,
                                                                                request.ContractNumber,
                                                                                request.PatientId,
                                                                                request.UserId), new PutUpdateCareMemberDataRequest
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
                }
                return response;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
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
        private List<ContactData> getContactDetails(List<string> contactIds, string version, string contractNumber, string userId)
        {

            List<ContactData> contactsData = new List<ContactData>();
            if(contactIds.Count > 0)
            {
                SearchContactsDataResponse contactDataResponse = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Contact", "POST")]
                contactDataResponse = client.Post<SearchContactsDataResponse>(string.Format("{0}/{1}/{2}/{3}/Contact",
                                                        DDContactServiceUrl,
                                                        "NG",
                                                        version,
                                                        contractNumber), new SearchContactsDataRequest
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
    }

}
