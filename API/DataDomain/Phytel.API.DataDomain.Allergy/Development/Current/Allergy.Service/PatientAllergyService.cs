using System;
using Phytel.API.DataDomain.Allergy.DTO;

namespace Phytel.API.DataDomain.Allergy.Service
{
    public class PatientAllergyService : ServiceBase
    {
        protected readonly IPatientAllergyDataManager Manager;

        public PatientAllergyService(IPatientAllergyDataManager mgr)
        {
            Manager = mgr;
        }

        public GetPatientAllergyResponse Get(GetPatientAllergyRequest request)
        {
            var response = new GetPatientAllergyResponse { Version = request.Version };

            try
            {
                RequireUserId(request);
                response.Allergies = Manager.GetPatientAllergyList(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }
    }
}