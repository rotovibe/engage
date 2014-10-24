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