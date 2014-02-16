using System;
using System.Net;
using Phytel.API.DataDomain.PatientGoal;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.Common.Format;

namespace Phytel.API.DataDomain.PatientGoal.Service
{
    public class PatientGoalService : ServiceStack.ServiceInterface.Service
    {

        public PutInitializeGoalDataResponse Put(PutInitializeGoalDataRequest request)
        {
            PutInitializeGoalDataResponse response = new PutInitializeGoalDataResponse();
            try
            {
                response = PatientGoalDataManager.InitializeGoal(request);
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public PutInitializeBarrierDataResponse Put(PutInitializeBarrierDataRequest request)
        {
            PutInitializeBarrierDataResponse response = new PutInitializeBarrierDataResponse();
            try
            {
                response = PatientGoalDataManager.InitializeBarrier(request);
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public GetPatientGoalDataResponse Get(GetPatientGoalDataRequest request)
        {
            GetPatientGoalDataResponse response = new GetPatientGoalDataResponse();
            try
            {
                response = PatientGoalDataManager.GetPatientGoal(request);
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public GetAllPatientGoalsDataResponse Get(GetAllPatientGoalsDataRequest request)
        {
            GetAllPatientGoalsDataResponse response = new GetAllPatientGoalsDataResponse();
            try
            {
                response = PatientGoalDataManager.GetPatientGoalList(request);
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public PutPatientGoalDataResponse Put(PutPatientGoalDataRequest request)
        {
            PutPatientGoalDataResponse response = new PutPatientGoalDataResponse();
            try
            {
                response = PatientGoalDataManager.PutPatientGoal(request);
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public PutInitializeTaskResponse Put(PutInitializeTaskRequest request)
        {
            PutInitializeTaskResponse response = new PutInitializeTaskResponse();
            try
            {
                response = PatientGoalDataManager.InsertNewPatientTask(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public PutUpdateTaskResponse Put(PutUpdateTaskRequest request)
        {
            PutUpdateTaskResponse response = new PutUpdateTaskResponse();
            try
            {
                response = PatientGoalDataManager.UpdatePatientTask(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public PutInitializeInterventionResponse Put(PutInitializeInterventionRequest request)
        {
            PutInitializeInterventionResponse response = new PutInitializeInterventionResponse();
            try
            {
                response = PatientGoalDataManager.InsertNewPatientIntervention(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public PutUpdateInterventionResponse Put(PutUpdateInterventionRequest request)
        {
            PutUpdateInterventionResponse response = new PutUpdateInterventionResponse();
            try
            {
                response = PatientGoalDataManager.UpdatePatientIntervention(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public PutUpdateBarrierResponse Put(PutUpdateBarrierRequest request)
        {
            PutUpdateBarrierResponse response = new PutUpdateBarrierResponse();
            try
            {
                response = PatientGoalDataManager.UpdatePatientBarrier(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public DeletePatientGoalDataResponse Delete(DeletePatientGoalDataRequest request)
        {
            DeletePatientGoalDataResponse response = new DeletePatientGoalDataResponse();
            try
            {
                response = PatientGoalDataManager.DeletePatientGoal(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public DeleteTaskResponse Delete(DeleteTaskRequest request)
        {
            DeleteTaskResponse response = new DeleteTaskResponse();
            try
            {
                response = PatientGoalDataManager.DeleteTask(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public DeleteInterventionResponse Delete(DeleteInterventionRequest request)
        {
            DeleteInterventionResponse response = new DeleteInterventionResponse();
            try
            {
                response = PatientGoalDataManager.DeleteIntervention(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public DeleteBarrierResponse Delete(DeleteBarrierRequest request)
        {
            DeleteBarrierResponse response = new DeleteBarrierResponse();
            try
            {
                response = PatientGoalDataManager.DeleteBarrier(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }
    }
}