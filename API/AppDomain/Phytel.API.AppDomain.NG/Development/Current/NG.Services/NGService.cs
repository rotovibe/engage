using System;
using System.Net;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.ServiceHost;
using Phytel.API.Common.Format;
using Phytel.API.AppDomain.Security.DTO;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Service
{
    public partial class NGService : ServiceStack.ServiceInterface.Service
    {
        public GetPatientResponse Post(GetPatientRequest request)
        {
            GetPatientResponse response = new GetPatientResponse();
            try
            {
                NGManager ngm = new NGManager();

                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.GetPatient(request);
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

        public GetPatientResponse Get(GetPatientRequest request)
        {
            GetPatientResponse response = new GetPatientResponse();
            try
            {
                NGManager ngm = new NGManager();

                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.GetPatient(request);
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

                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.PatientProblems = ngm.GetPatientProblems(request);
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

        public GetAllProblemsResponse Get(GetAllProblemsRequest request)
        {
            GetAllProblemsResponse response = new GetAllProblemsResponse();
            try
            {
                NGManager ngm = new NGManager();

                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.Problems = ngm.GetProblems(request);
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

        public GetAllCohortsResponse Get(GetAllCohortsRequest request)
        {
            GetAllCohortsResponse response = new GetAllCohortsResponse();
            try
            {
                NGManager ngm = new NGManager();

                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
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

        public PutPatientFlaggedUpdateResponse Post(PutPatientFlaggedUpdateRequest request)
        {
            PutPatientFlaggedUpdateResponse response = new PutPatientFlaggedUpdateResponse();
            try
            {
                NGManager ngm = new NGManager();

                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
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

        public PutPatientDetailsUpdateResponse Post(PutPatientDetailsUpdateRequest request)
        {
            PutPatientDetailsUpdateResponse response = new PutPatientDetailsUpdateResponse();
            try
            {
                NGManager ngm = new NGManager();

                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.PutPatientDetailsUpdate(request);
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
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.GetCohortPatients(request, base.RequestContext);
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

                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
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

        public GetActiveProgramsResponse Get(GetActiveProgramsRequest request)
        {
            GetActiveProgramsResponse response = new GetActiveProgramsResponse();
            try
            {
                NGManager ngm = new NGManager();

                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
                if (result.UserId != null)
                {
                    if (result.UserId.Trim() != string.Empty)
                    {
                        request.UserId = result.UserId;
                        response = ngm.GetActivePrograms(request);
                    }
                    else
                        throw new UnauthorizedAccessException();
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

        public PostPatientToProgramsResponse Post(PostPatientToProgramsRequest request)
        {
            PostPatientToProgramsResponse response = new PostPatientToProgramsResponse();
            try
            {
                NGManager ngm = new NGManager();

                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.PostPatientToProgram(request);
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

        public GetPatientProgramDetailsSummaryResponse Get(GetPatientProgramDetailsSummaryRequest request)
        {
            GetPatientProgramDetailsSummaryResponse response = new GetPatientProgramDetailsSummaryResponse();
            try
            {
                NGManager ngm = new NGManager();

                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.GetPatientProgramDetailsSummary(request);
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

        public GetContactResponse Get(GetContactRequest request)
        {
            GetContactResponse response = new GetContactResponse();
            try
            {
                NGManager ngm = new NGManager();

                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.GetContact(request);
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