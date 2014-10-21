using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Allergy;
using Phytel.API.DataDomain.Allergy.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using Phytel.API.Common;

namespace Phytel.API.AppDomain.NG.Allergy
{
    public class AllergyManager : ManagerBase, IAllergyManager
    {
        public IAllergyEndpointUtil EndpointUtil { get; set; }

        public List<DTO.Allergy.Allergy> GetAllergies(GetAllergiesRequest request)
        {
            try
            {
                List<DTO.Allergy.Allergy> result = new List<DTO.Allergy.Allergy>();
                var algy = EndpointUtil.GetAllergies(request);
                algy.ForEach(a => result.Add(new DTO.Allergy.Allergy {Id = a.Id}));
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetAllergies()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
