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

        #region Posts
        public GetPatientAllergiesDataResponse Post(GetPatientAllergiesDataRequest request)
        {
            var response = new GetPatientAllergiesDataResponse { Version = request.Version };

            try
            {
                RequireUserId(request);
                response.PatientAllergiesData = Manager.GetPatientAllergies(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        } 
        #endregion

        #region Initialize
        public PutInitializePatientAllergyDataResponse Put(PutInitializePatientAllergyDataRequest request)
        {
            PutInitializePatientAllergyDataResponse response = new PutInitializePatientAllergyDataResponse { Version = request.Version };

            try
            {
                RequireUserId(request);
                response.PatientAllergyData = Manager.InitializePatientAllergy(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }
        #endregion

        #region Puts
        public PutPatientAllergyDataResponse Put(PutPatientAllergyDataRequest request)
        {
            PutPatientAllergyDataResponse response = new PutPatientAllergyDataResponse { Version = request.Version };

            try
            {
                RequireUserId(request);
                response.PatientAllergyData = Manager.UpdateSinglePatientAllergy(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }


        public PutPatientAllergiesDataResponse Put(PutPatientAllergiesDataRequest request)
        {
            PutPatientAllergiesDataResponse response = new PutPatientAllergiesDataResponse { Version = request.Version };

            try
            {
                RequireUserId(request);
                response.PatientAllergiesData = Manager.UpdateBulkPatientAllergies(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }
        #endregion
    }
}