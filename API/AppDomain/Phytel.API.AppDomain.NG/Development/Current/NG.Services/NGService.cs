using System;
using System.Net;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.ServiceHost;
using Phytel.API.Common.Format;

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

                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
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

                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
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

                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
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

                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
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

                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
        }

        public PutPatientFlaggedUpdateResponse Put(PutPatientFlaggedUpdateRequest request)
        {
            PutPatientFlaggedUpdateResponse response = new PutPatientFlaggedUpdateResponse();
            try
            {
                NGManager ngm = new NGManager();

                bool result = ngm.IsUserValidated(request.Version, request.Token);
                if (result)
                {
                    response = ngm.PutPatientFlaggedUpdate(request);
                }
                else
                    throw new UnauthorizedAccessException();

                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
        }

        public PutPatientPriorityUpdateResponse Put(PutPatientPriorityUpdateRequest request)
        {
            PutPatientPriorityUpdateResponse response = new PutPatientPriorityUpdateResponse();
            try
            {
                NGManager ngm = new NGManager();

                bool result = ngm.IsUserValidated(request.Version, request.Token);
                if (result)
                {
                    response = ngm.PutPatientPriorityUpdate(request);
                }
                else
                    throw new UnauthorizedAccessException();

                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
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

                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
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
                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
        }

    }
}
