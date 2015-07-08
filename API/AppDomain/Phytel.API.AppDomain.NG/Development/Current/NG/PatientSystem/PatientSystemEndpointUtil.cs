using System;
using System.Collections.Generic;
using System.Configuration;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG
{
    public class PatientSystemEndpointUtil : IPatientSystemEndpointUtil
    {
        #region endpoint addresses
        protected readonly string DDPatientSystemUrl = ConfigurationManager.AppSettings["DDPatientSystemUrl"];
        #endregion

        public List<SystemSourceData> GetSystemSources(GetActiveSystemSourcesRequest request)
        {
            try
            {
                List<SystemSourceData> result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/SystemSource", "GET")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/SystemSource",
                                    DDPatientSystemUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);

                GetSystemSourcesDataResponse dataDomainResponse = client.Get<GetSystemSourcesDataResponse>(url);
                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.SystemSourcesData;
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
