using System;
using System.Net;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.Service
{
    public class NGService : ServiceStack.ServiceInterface.Service
    {
        public GetPatientResponse Post(GetPatientRequest request)
        {
            GetPatientResponse response = new GetPatientResponse();
            try
            {
                NGManager ngm = new NGManager();

                //request.Token = base.Request.Headers["APIToken"] as string;
                bool result = ngm.IsUserValidated(request.Version, request.Token);
                if (result)
                    response = ngm.GetPatient(request);
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
                response.Status.StackTrace = ex.StackTrace;
            }
            return response;
        }

        public GetPatientResponse Get(GetPatientRequest request)
        {
            GetPatientResponse response = new GetPatientResponse();
            try
            {
                NGManager ngm = new NGManager();

                //request.Token = base.Request.Headers["APIToken"] as string;
                bool result = ngm.IsUserValidated(request.Version, request.Token);
                if (result)
                    response = ngm.GetPatient(request);
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
                response.Status.StackTrace = ex.StackTrace;
            }
            return response;
        }

        /// <summary>
        ///     ServiceStack's GET endpoint for getting active problems for a patient
        /// </summary>
        /// <param name="request">PatientProblemResponse object</param>
        /// <returns>PatientProblemResponse object</returns>
        public GetAllPatientProblemsResponse Get(GetAllPatientProblemsRequest request)
        {
            GetAllPatientProblemsResponse response = new GetAllPatientProblemsResponse();
            try
            {
                NGManager ngm = new NGManager();

                bool result = ngm.IsUserValidated(request.Version, request.Token);
                if (result)
                    response.PatientProblems = ngm.GetPatientProblems(request);
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public GetAllProblemsResponse Get(GetAllProblemsRequest request)
        {
            GetAllProblemsResponse response = new GetAllProblemsResponse();
            try
            {
                NGManager ngm = new NGManager();

                bool result = ngm.IsUserValidated(request.Version, request.Token);
                if (result)
                    response.Problems = ngm.GetProblems(request);
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public GetAllCohortsResponse Get(GetAllCohortsRequest request)
        {
            GetAllCohortsResponse response = new GetAllCohortsResponse();
            try
            {
                NGManager ngm = new NGManager();

                bool result = ngm.IsUserValidated(request.Version, request.Token);
                if (result)
                {
                    response.Cohorts = ngm.GetCohorts(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public GetCohortPatientsResponse Get(GetCohortPatientsRequest request)
        {
            GetCohortPatientsResponse response = new GetCohortPatientsResponse();
            NGManager ngm = new NGManager();

            try
            {
                bool result = ngm.IsUserValidated(request.Version, request.Token);
                if (result)
                {
                    response = ngm.GetCohortPatients(request, base.RequestContext  );
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("GetCohortPatientsRequest", ex.Message);
                response.Status.StackTrace = ex.StackTrace;
            }
            return response;
        }

        public GetAllSettingsResponse Get(GetAllSettingsRequest request)
        {
            GetAllSettingsResponse response = new GetAllSettingsResponse();
            try
            {
                NGManager ngm = new NGManager();

                bool result = ngm.IsUserValidated(request.Version, request.Token);
                if (result)
                {
                    response = ngm.GetAllSettings(request);
                }
                else
                    throw new UnauthorizedAccessException();
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
