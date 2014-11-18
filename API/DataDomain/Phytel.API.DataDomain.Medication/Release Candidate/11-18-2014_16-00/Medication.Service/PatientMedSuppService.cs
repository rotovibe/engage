using System;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.DataDomain.Medication.Service
{
    public class PatientMedSuppService : ServiceBase
    {
        //protected readonly IPatientMedSuppDataManager Manager;
        public IPatientMedSuppDataManager Manager { get; set; }

        //public PatientMedSuppService(IPatientMedSuppDataManager mgr)
        //{
        //    Manager = mgr;
        //}

        #region Posts
        public GetPatientMedSuppsDataResponse Post(GetPatientMedSuppsDataRequest request)
        {
            var response = new GetPatientMedSuppsDataResponse { Version = request.Version };

            try
            {
                RequireUserId(request);
                response.PatientMedSuppsData = Manager.GetPatientMedSupps(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }
        #endregion

        #region Puts
        public PutPatientMedSuppDataResponse Put(PutPatientMedSuppDataRequest request)
        {
            PutPatientMedSuppDataResponse response = new PutPatientMedSuppDataResponse { Version = request.Version };

            try
            {
                RequireUserId(request);
                response.PatientMedSuppData = Manager.SavePatientMedSupps(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }
        #endregion

        #region Delete & UndoDelete
        public DeleteMedSuppsByPatientIdDataResponse Delete(DeleteMedSuppsByPatientIdDataRequest request)
        {
            DeleteMedSuppsByPatientIdDataResponse response = new DeleteMedSuppsByPatientIdDataResponse { Version = request.Version };
            try
            {
                RequireUserId(request);
                response = Manager.DeletePatientMedSupps(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public UndoDeletePatientMedSuppsDataResponse Put(UndoDeletePatientMedSuppsDataRequest request)
        {
            UndoDeletePatientMedSuppsDataResponse response = new UndoDeletePatientMedSuppsDataResponse();
            try
            {
                RequireUserId(request);
                response = Manager.UndoDeletePatientMedSupps(request);
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