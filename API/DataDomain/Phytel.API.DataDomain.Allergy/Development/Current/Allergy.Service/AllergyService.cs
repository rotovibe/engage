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

        #region Initialize
        public PutInitializeAllergyDataResponse Put(PutInitializeAllergyDataRequest request)
        {
            PutInitializeAllergyDataResponse response = new PutInitializeAllergyDataResponse { Version = request.Version };

            try
            {
                RequireUserId(request);
                response.AllergyData = Manager.InitializeAllergy(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }
        #endregion

        #region Puts
        public PutAllergyDataResponse Put(PutAllergyDataRequest request)
        {
            PutAllergyDataResponse response = new PutAllergyDataResponse { Version = request.Version };

            try
            {
                RequireUserId(request);
                response.AllergyData = Manager.UpdateAllergy(request);
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