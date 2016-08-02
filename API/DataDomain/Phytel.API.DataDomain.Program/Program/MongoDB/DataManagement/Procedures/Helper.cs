using System;
using System.Configuration;
using MongoDB.Bson;
using Phytel.API.DataDomain.CareMember.DTO;
using ServiceStack.Service;

namespace Phytel.API.DataDomain.Program.MongoDB.DataManagement.Procedures
{
    public static class Helper
    {
        public static string DDCareMemberUrl = ConfigurationManager.AppSettings["DDCareMemberUrl"];
        
        public static ObjectId GetPatientsPrimaryCareManager(GetPrimaryCareManagerDataRequest request, IRestClient client)
        {
            try
            {

                ObjectId pcmObjectId = ObjectId.Empty;
                // [Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/PrimaryCareManager", "GET")]
                string url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/PrimaryCareManager",
                    DDCareMemberUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId), request.UserId);

                GetCareMemberDataResponse ddResponse = client.Get<GetCareMemberDataResponse>(url);

                if (ddResponse != null && ddResponse.CareMember != null)
                {
                    pcmObjectId = ObjectId.Parse(ddResponse.CareMember.ContactId);
                }
                return pcmObjectId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
