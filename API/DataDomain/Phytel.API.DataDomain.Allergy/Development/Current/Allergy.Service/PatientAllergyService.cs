using System;
using Phytel.API.DataDomain.Allergy.DTO;

namespace Phytel.API.DataDomain.Allergy.Service
{
    public class PatientAllergyService : ServiceBase
    {
        //protected readonly IPatientAllergyDataManager Manager;
        public IPatientAllergyDataManager Manager { get; set;}

        //public PatientAllergyService(IPatientAllergyDataManager mgr)
        //{
        //    Manager = mgr;
        //}

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
        public PutPatientAllergiesDataResponse Put(PutPatientAllergiesDataRequest request)
        {
            PutPatientAllergiesDataResponse response = new PutPatientAllergiesDataResponse { Version = request.Version };

            try
            {
                RequireUserId(request);
                response.PatientAllergiesData = Manager.UpdatePatientAllergies(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }
        #endregion

        #region Delete & UndoDelete
        public DeleteAllergiesByPatientIdDataResponse Delete(DeleteAllergiesByPatientIdDataRequest request)
        {
            DeleteAllergiesByPatientIdDataResponse response = new DeleteAllergiesByPatientIdDataResponse { Version = request.Version };
            try
            {
                RequireUserId(request);
                response = Manager.DeletePatientAllergies(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public UndoDeletePatientAllergiesDataResponse Put(UndoDeletePatientAllergiesDataRequest request)
        {
            UndoDeletePatientAllergiesDataResponse response = new UndoDeletePatientAllergiesDataResponse();
            try
            {
                RequireUserId(request);
                response = Manager.UndoDeletePatientAllergies(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public DeletePatientAllergyDataResponse Delete(DeletePatientAllergyDataRequest request)
        {
            DeletePatientAllergyDataResponse response = new DeletePatientAllergyDataResponse { Version = request.Version };
            try
            {
                RequireUserId(request);
                response = Manager.Delete(request);
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