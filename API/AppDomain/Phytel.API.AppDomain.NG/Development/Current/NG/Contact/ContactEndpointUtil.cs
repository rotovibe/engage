using System;
using System.Configuration;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
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

        #endregion


       
    }
}
