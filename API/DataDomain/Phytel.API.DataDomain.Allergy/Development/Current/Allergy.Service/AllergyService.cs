using System;
using Phytel.API.DataDomain.Allergy.DTO;

namespace Phytel.API.DataDomain.Allergy.Service
{
    public class AllergyService : ServiceBase
    {
        protected readonly IAllergyDataManager Manager;

        public AllergyService(IAllergyDataManager mgr)
        {
            Manager = mgr;
        }

        public PostNewAllergyResponse Put(PostNewAllergyRequest request)
        {
            var response = new PostNewAllergyResponse { Version = request.Version };
            try
            {
                RequireUserId(request);
                response.Allergy = Manager.PutNewAllergy(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public GetAllAllergysResponse Get(GetAllAllergysRequest request)
        {
            var response = new GetAllAllergysResponse { Version = request.Version };
            try
            {
                RequireUserId(request);
                response.Allergys = Manager.GetAllergyList(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }
    }
}