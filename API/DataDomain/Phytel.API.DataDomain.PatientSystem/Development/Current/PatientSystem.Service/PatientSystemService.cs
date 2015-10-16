using System;
using System.Net;
using Phytel.API.DataDomain.PatientSystem;
using Phytel.API.DataDomain.PatientSystem.DTO;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Common.Format;
using System.Configuration;
using System.Web;

namespace Phytel.API.DataDomain.PatientSystem.Service
{
    public class PatientSystemService : ServiceBase
    {
        public IPatientSystemDataManager Manager { get; set; }

        #region Gets
        public GetPatientSystemDataResponse Get(GetPatientSystemDataRequest request)
        {
            GetPatientSystemDataResponse response = new GetPatientSystemDataResponse();
            try
            {
                RequireUserId(request); 
                response = Manager.GetPatientSystem(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public GetPatientSystemsDataResponse Get(GetPatientSystemsDataRequest request)
        {
            GetPatientSystemsDataResponse response = new GetPatientSystemsDataResponse();
            try
            {
                RequireUserId(request);
                response.PatientSystemsData = Manager.GetPatientSystems(request); 
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public GetAllPatientSystemDataResponse Get(GetAllPatientSystemDataRequest request)
        {
            var response = new GetAllPatientSystemDataResponse();
            try
            {
                RequireUserId(request);
                response.PatientSystemsOldData = Manager.GetAllPatientSystems();
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        } 
        #endregion

        #region POST
        public InsertPatientSystemDataResponse Post(InsertPatientSystemDataRequest request)
        {
            InsertPatientSystemDataResponse response = new InsertPatientSystemDataResponse();
            try
            {
                RequireUserId(request);
                response.PatientSystemData = Manager.InsertPatientSystem(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public InsertPatientSystemsDataResponse Post(InsertPatientSystemsDataRequest request)
        {
            InsertPatientSystemsDataResponse response = new InsertPatientSystemsDataResponse();
            try
            {
                RequireUserId(request);
                response.PatientSystemsData = Manager.InsertPatientSystems(request); 
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public GetPatientSystemByIdsDataResponse Post(GetPatientSystemByIdsDataRequest request)
        {
            GetPatientSystemByIdsDataResponse response = new GetPatientSystemByIdsDataResponse();
            try
            {
                RequireUserId(request);
                response.PatientSystems = Manager.GetPatientSystemsByIds(request); 
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public InsertBatchPatientSystemsDataResponse Post(InsertBatchPatientSystemsDataRequest request)
        {
            InsertBatchPatientSystemsDataResponse response = new InsertBatchPatientSystemsDataResponse();
            try
            {
                RequireUserId(request);
                response.Responses = Manager.InsertBatchPatientSystems(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public InsertBatchEngagePatientSystemsDataResponse Post(InsertBatchEngagePatientSystemsDataRequest request)
        {
            InsertBatchEngagePatientSystemsDataResponse response = new InsertBatchEngagePatientSystemsDataResponse();
            try
            {
                RequireUserId(request);
                response.Result = Manager.InsertBatchEngagePatientSystems(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }
        #endregion

        #region PUT
        public UpdatePatientSystemDataResponse Put(UpdatePatientSystemDataRequest request)
        {
            UpdatePatientSystemDataResponse response = new UpdatePatientSystemDataResponse();
            try
            {
                RequireUserId(request);
                response.Success = Manager.UpdatePatientSystem(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        } 

        public UpdatePatientSystemsDataResponse Put(UpdatePatientSystemsDataRequest request)
        {
            UpdatePatientSystemsDataResponse response = new UpdatePatientSystemsDataResponse();
            try
            {
                RequireUserId(request);
                response.PatientSystemsData = Manager.UpdatePatientSystems(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        } 
        #endregion

        #region Delete & UndoDelete
        public DeletePatientSystemByPatientIdDataResponse Delete(DeletePatientSystemByPatientIdDataRequest request)
        {
            DeletePatientSystemByPatientIdDataResponse response = new DeletePatientSystemByPatientIdDataResponse();
            try
            {
                RequireUserId(request); 
                response = Manager.DeletePatientSystemByPatientId(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public DeletePatientSystemsDataResponse Delete(DeletePatientSystemsDataRequest request)
        {
            DeletePatientSystemsDataResponse response = new DeletePatientSystemsDataResponse();
            try
            {
                RequireUserId(request);
                Manager.DeletePatientSystems(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public UndoDeletePatientSystemsDataResponse Put(UndoDeletePatientSystemsDataRequest request)
        {
            UndoDeletePatientSystemsDataResponse response = new UndoDeletePatientSystemsDataResponse();
            try
            {
                RequireUserId(request); 
                response = Manager.UndoDeletePatientSystems(request);
                response.Version = request.Version;
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