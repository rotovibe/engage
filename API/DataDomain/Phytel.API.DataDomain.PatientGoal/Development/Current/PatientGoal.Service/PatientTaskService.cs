using System;
using System.Net;
using Phytel.API.DataDomain.PatientGoal;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.Common.Format;

namespace Phytel.API.DataDomain.PatientGoal.Service
{
    /// <summary>
    /// Task oriented service calls.
    /// </summary>
    public partial class PatientGoalService : ServiceStack.ServiceInterface.Service
    {
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

        //public GetAllPatientGoalsResponse Post(GetAllPatientGoalsRequest request)
        //{
        //    GetAllPatientGoalsResponse response = new GetAllPatientGoalsResponse();
        //    try
        //    {
        //        response = PatientGoalDataManager.GetPatientGoalList(request);
        //        response.Version = request.Version;
        //    }
        //    catch (Exception ex)
        //    {
        //        //TODO: Log this to C3 database via ASE
        //        base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
        //    }
        //    return response;
        //}
    }
}