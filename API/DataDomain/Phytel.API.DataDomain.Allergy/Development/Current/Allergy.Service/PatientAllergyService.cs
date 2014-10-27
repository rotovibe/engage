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

        public GetPatientAllergiesDataResponse Get(GetPatientAllergiesDataRequest request)
        {
            var response = new GetPatientAllergiesDataResponse { Version = request.Version };

            try
            {
                RequireUserId(request);
                response.PatientAllergiesData = Manager.GetPatientAllergyList(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }
    }
}