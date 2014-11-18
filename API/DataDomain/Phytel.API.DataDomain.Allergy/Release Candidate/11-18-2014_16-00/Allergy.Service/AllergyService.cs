using System;
using Phytel.API.DataDomain.Allergy.DTO;

namespace Phytel.API.DataDomain.Allergy.Service
{
    public class AllergyService : ServiceBase
    {
        //protected readonly IAllergyDataManager Manager;
        public IAllergyDataManager Manager { get; set; }

        //public AllergyService(IAllergyDataManager mgr)
        //{
        //    Manager = mgr;
        //}

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