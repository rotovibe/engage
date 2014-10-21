using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Allergy;
using Phytel.API.DataDomain.Allergy.DTO;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.Interface;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.AppDomain.NG.Allergy
{
    public class AllergyEndpointUtil : IAllergyEndpointUtil
    {
        #region endpoint addresses
        protected readonly string DDAllergyUrl = ConfigurationManager.AppSettings["DDAllergyUrl"];
        #endregion

        public List<DdAllergy> GetAllergies(GetAllergiesRequest request)
        {
            try
            {
                List<DdAllergy> result = null;
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Allergy", "GET")]
                var url = Common.Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Allergy",
                                    DDAllergyUrl,
                                    "NG",
                                    request.Version,
                                    request.ContractNumber), request.UserId);

                GetAllAllergysResponse dataDomainResponse = client.Get<GetAllAllergysResponse>(url);

                if (dataDomainResponse != null)
                {
                    result = dataDomainResponse.Allergys;
                }

                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetAllergies()::" + ex.Message, ex.InnerException);
            }
        }

    }
}
