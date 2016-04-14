using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                GetCareTeamDataResponse dataDomainResponse = client.Post<GetCareTeamDataResponse>(url, new GetCareTeamDataRequest
                {
                    Context = "NG",
                    ContractNumber = request.ContractNumber,
                    ContactId = request.ContactId,
                    UserId = request.UserId,
                    Version = request.Version
                } as object);

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
        #endregion
    }
}
