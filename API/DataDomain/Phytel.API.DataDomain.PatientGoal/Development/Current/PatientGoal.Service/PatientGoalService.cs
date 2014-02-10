using System;
using System.Net;
using Phytel.API.DataDomain.PatientGoal;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.Common.Format;

namespace Phytel.API.DataDomain.PatientGoal.Service
{
    public partial class PatientGoalService : ServiceStack.ServiceInterface.Service
    {
        public GetPatientGoalResponse Post(GetPatientGoalRequest request)
        {
            GetPatientGoalResponse response = new GetPatientGoalResponse();
            try
            {
                response = PatientGoalDataManager.GetPatientGoalByID(request);
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

        public GetPatientGoalResponse Get(GetPatientGoalRequest request)
        {
            GetPatientGoalResponse response = new GetPatientGoalResponse();
            try
            {
                response = PatientGoalDataManager.GetPatientGoalByID(request);
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

        public GetAllPatientGoalsResponse Post(GetAllPatientGoalsRequest request)
        {
            GetAllPatientGoalsResponse response = new GetAllPatientGoalsResponse();
            try
            {
                response = PatientGoalDataManager.GetPatientGoalList(request);
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