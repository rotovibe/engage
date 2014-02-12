using System;
using System.Net;
using Phytel.API.DataDomain.PatientGoal;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.Common.Format;

namespace Phytel.API.DataDomain.PatientGoal.Service
{
    public partial class PatientGoalService : ServiceStack.ServiceInterface.Service
    {
        public GetPatientGoalDataResponse Post(GetPatientGoalDataRequest request)
        {
            GetPatientGoalDataResponse response = new GetPatientGoalDataResponse();
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

        public GetPatientGoalDataResponse Get(GetPatientGoalDataRequest request)
        {
            GetPatientGoalDataResponse response = new GetPatientGoalDataResponse();
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

        public GetAllPatientGoalsDataResponse Post(GetAllPatientGoalsDataRequest request)
        {
            GetAllPatientGoalsDataResponse response = new GetAllPatientGoalsDataResponse();
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