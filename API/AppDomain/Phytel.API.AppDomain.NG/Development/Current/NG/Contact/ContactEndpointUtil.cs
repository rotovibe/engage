using System;
using System.Collections.Generic;
using System.Configuration;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG
{
    public class ContactEndpointUtil : IContactEndpointUtil
    {
        #region endpoint addresses
        protected readonly string DDContactServiceUrl = ConfigurationManager.AppSettings["DDContactServiceUrl"];
        #endregion

        #region Contact

        public ContactData GetContactByContactId(GetContactByContactIdRequest request)
        {
            ContactData result = null;
            try
            {
                
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Contact/{ContactId}", "GET")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Contact/{4}",
                        DDContactServiceUrl,
                        "NG",
                        request.Version,
                        request.ContractNumber,
                        request.ContactId), request.UserId);

                GetContactByContactIdDataResponse dataDomainResponse = client.Get<GetContactByContactIdDataResponse>(url);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.Contact;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ContactData> GetContactsByContactIds(List<string> contactIds, double version, string contractNumber, string userId)
        {

            List<ContactData> result = null;
            if (contactIds.Count > 0)
            {
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Contact", "POST")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Contact",
                                                        DDContactServiceUrl,
                                                        "NG",
                                                        version,
                                                        contractNumber), userId);

                //[Route("/{Context}/{Version}/{ContractNumber}/Contact", "POST")]
                GetContactsByContactIdsDataResponse contactDataResponse = client.Post<GetContactsByContactIdsDataResponse>(url, new GetContactsByContactIdsDataRequest()
                {
                    ContactIds = contactIds,
                    Context = "NG",
                    ContractNumber = contractNumber,
                    UserId = userId,
                    Version = version
                } as object);

                if (contactDataResponse != null && contactDataResponse.Contacts != null)
                {
                    result = contactDataResponse.Contacts;
                }
            }
            return result;
        }

        #endregion

        #region CareTeam

        public CareTeamData GetCareTeam(DTO.GetCareTeamRequest request)
        {
            try
            {
                CareTeamData result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Contacts/{ContactId}/CareTeams", "GET")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Contacts/{4}/CareTeams",
                                    DDContactServiceUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber,
                                    request.ContactId), request.UserId);

                GetCareTeamDataResponse dataDomainResponse = client.Get<GetCareTeamDataResponse>(url);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.CareTeamData;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SaveCareTeamDataResponse SaveCareTeam(SaveCareTeamRequest request)
        {
            SaveCareTeamDataResponse response = null;
            try
            {

                IRestClient client = new JsonServiceClient();
                // '/{Context}/{Version}/{ContractNumber}/Contacts/{ContactId}/CareTeams
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Contacts/{4}/CareTeams",
                                                                                DDContactServiceUrl,
                                                                                "NG",
                                                                                request.Version,
                                                                                request.ContractNumber, request.ContactId), request.UserId);
                 response =
                    client.Post<SaveCareTeamDataResponse>(url, new SaveCareTeamDataRequest
                    {
                        CareTeamData = Mapper.Map<CareTeamData>(request.CareTeam),
                        Version = request.Version,
                        ContactId = request.ContactId,
                        ContractNumber = request.ContractNumber,
                        Context = "NG"
                    } as object);
              
            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:SaveCareTeam()::" + wse.Message, wse.InnerException);
            }

            return response;
        }

        public UpdateCareTeamMemberResponse UpdateCareTeamMember(UpdateCareTeamMemberRequest request)
        {
            UpdateCareTeamMemberResponse response = null;
            try
            {
                IRestClient client = new JsonServiceClient();
                //[Route("/{Version}/{ContractNumber}/Contacts/{ContactId}/CareTeams/{CareTeamId}/CareTeamMembers/{Id}", "PUT")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Contacts/{4}/CareTeams/{5}/CareTeamMembers/{6}",
                                                                                DDContactServiceUrl,
                                                                                "NG",
                                                                                request.Version,
                                                                                request.ContractNumber, request.ContactId, request.CareTeamId, request.Id), request.UserId);
                var dataDomainResponse =
                    client.Put<UpdateCareTeamMemberResponse>(url, new UpdateCareTeamMemberDataRequest
                    {
                        CareTeamMemberData = Mapper.Map<CareTeamMemberData>(request.CareTeamMember),
                        Version = request.Version,
                        ContactId = request.ContactId,
                        CareTeamId = request.CareTeamId,
                        Id = request.Id,
                        ContractNumber = request.ContractNumber,
                        Context = "NG"
                    } as object);
                if (dataDomainResponse != null)
                {
                    response.Version = dataDomainResponse.Version;
                    response.Status = dataDomainResponse.Status;
                }

            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:UpdateCareTeamMember()::" + wse.Message, wse.InnerException);
            }
            return response;
        }

        public DeleteCareTeamMemberDataResponse DeleteCareTeamMember(DeleteCareTeamMemberRequest request)
        {
            DeleteCareTeamMemberDataResponse response = null;
            try
            {

                IRestClient client = new JsonServiceClient();
                //[Route("/{Version}/{ContractNumber}/Contacts/{ContactId}/CareTeams/{CareTeamId}/CareTeamMembers/{Id}", "DELETE")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Contacts/{4}/CareTeams/{5}/CareTeamMembers/{6}",
                                                                                DDContactServiceUrl,
                                                                                "NG",
                                                                                request.Version,
                                                                                request.ContractNumber, request.ContactId, request.CareTeamId, request.Id), request.UserId);
                var dataDomainResponse = client.Delete<DeleteCareTeamMemberDataResponse>(url);

                if (dataDomainResponse != null)
                {
                    response.Version = dataDomainResponse.Version;
                    response.Status = dataDomainResponse.Status;
                }

            }
            catch (WebServiceException wse)
            {
                throw new WebServiceException("AD:DeleteCareTeamMember()::" + wse.Message, wse.InnerException);
            }

            return response;
        }
        #endregion
    }
}
