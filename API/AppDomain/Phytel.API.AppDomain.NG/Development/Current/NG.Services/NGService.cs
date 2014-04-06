using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Phytel.API.AppDomain.NG.Service
{
    public partial class NGService : ServiceStack.ServiceInterface.Service
    {
        private ISecurityManager _security;
        private INGManager _ngm;

        public GetPatientResponse Post(GetPatientRequest request)
        {
            GetPatientResponse response = new GetPatientResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = _ngm.GetPatient(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;

                if (response.Patient != null)
                {
                   patientIds = new List<string>();
                    patientIds.Add(response.Patient.Id);                    
                }

                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }

            return response; 
        }

        public GetPatientResponse Get(GetPatientRequest request)
        {
            GetPatientResponse response = new GetPatientResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = _ngm.GetPatient(request);
                }
                else
                    throw new UnauthorizedAccessException();

            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;

                if (response.Patient != null)
                {
                    patientIds = new List<string>();
                    patientIds.Add(response.Patient.Id);               
                }

                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }

            return response;
        }

        public GetPatientSSNResponse Get(GetPatientSSNRequest request)
        {
            GetPatientSSNResponse response = new GetPatientSSNResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = _ngm.GetPatientSSN(request);
                }
                else
                    throw new UnauthorizedAccessException();

            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;

                if (response != null)
                {
                    patientIds = new List<string>();
                    patientIds.Add(request.PatientId);
                }

                if(result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
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
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.PatientProblems = _ngm.GetPatientProblems(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                if(result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);               
            }
            
            return response; 
        }

        public GetAllProblemsResponse Get(GetAllProblemsRequest request)
        {
            GetAllProblemsResponse response = new GetAllProblemsResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.Problems = _ngm.GetProblems(request);
                }
                else
                    throw new UnauthorizedAccessException();               
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                if(result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);               
            }
            
            return response; 
        }

        public GetAllCohortsResponse Get(GetAllCohortsRequest request)
        {
            GetAllCohortsResponse response = new GetAllCohortsResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.Cohorts = _ngm.GetCohorts(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public PutPatientFlaggedUpdateResponse Post(PutPatientFlaggedUpdateRequest request)
        {
            PutPatientFlaggedUpdateResponse response = new PutPatientFlaggedUpdateResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = _ngm.PutPatientFlaggedUpdate(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public PutPatientBackgroundResponse Post(PutPatientBackgroundRequest request)
        {
            PutPatientBackgroundResponse response = new PutPatientBackgroundResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = _ngm.UpdateBackground(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }

            return response;
        }

        public PutPatientDetailsUpdateResponse Post(PutPatientDetailsUpdateRequest request)
        {
            PutPatientDetailsUpdateResponse response = new PutPatientDetailsUpdateResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = _ngm.PutPatientDetailsUpdate(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public GetCohortPatientsResponse Get(GetCohortPatientsRequest request)
        {
            GetCohortPatientsResponse response = new GetCohortPatientsResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = _ngm.GetCohortPatients(request);
                }
                else
                    throw new UnauthorizedAccessException();

            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;

                if (response.Patients != null)
                {
                    patientIds = response.Patients.Select(x => x.Id).ToList();
                }

                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public GetAllSettingsResponse Get(GetAllSettingsRequest request)
        {
            GetAllSettingsResponse response = new GetAllSettingsResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = _ngm.GetAllSettings(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public GetActiveProgramsResponse Get(GetActiveProgramsRequest request)
        {
            GetActiveProgramsResponse response = new GetActiveProgramsResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId != null)
                {
                    if (result.UserId.Trim() != string.Empty)
                    {
                        request.UserId = result.UserId;
                        response = _ngm.GetActivePrograms(request);
                    }
                    else
                        throw new UnauthorizedAccessException();
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public PostPatientToProgramsResponse Post(PostPatientToProgramsRequest request)
        {
            PostPatientToProgramsResponse response = new PostPatientToProgramsResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = _ngm.PostPatientToProgram(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);               
            }
            
            return response; 
        }

        public GetPatientProgramDetailsSummaryResponse Get(GetPatientProgramDetailsSummaryRequest request)
        {
            GetPatientProgramDetailsSummaryResponse response = new GetPatientProgramDetailsSummaryResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = _ngm.GetPatientProgramDetailsSummary(request);
                }
                else
                    throw new UnauthorizedAccessException();

            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);               
            }
            
            return response; 
        }

        public GetPatientProgramsResponse Get(GetPatientProgramsRequest request)
        {
            GetPatientProgramsResponse response = new GetPatientProgramsResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = _ngm.GetPatientPrograms(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;

                if (response.Programs != null)
                {
                    patientIds = response.Programs.Select(x => x.PatientId).ToList();
                }
                
                if(result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);

            }
            
            return response; 
        }

        #region Contact
        public GetContactResponse Get(GetContactRequest request)
        {
            GetContactResponse response = new GetContactResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.Contact = _ngm.GetContactByPatientId(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public GetAllCareManagersResponse Get(GetAllCareManagersRequest request)
        {
            GetAllCareManagersResponse response = new GetAllCareManagersResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.Contacts = _ngm.GetCareManagers(request);
                    response.Version = request.Version;
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public PutUpdateContactResponse Post(PutUpdateContactRequest request)
        {
            PutUpdateContactResponse response = new PutUpdateContactResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = _ngm.PutUpdateContact(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }
        #endregion

        #region LookUps ContactRelated
        public GetAllCommModesResponse Get(GetAllCommModesRequest request)
        {
            GetAllCommModesResponse response = new GetAllCommModesResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.CommModes = _ngm.GetAllCommModes(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public GetAllStatesResponse Get(GetAllStatesRequest request)
        {
            GetAllStatesResponse response = new GetAllStatesResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.States = _ngm.GetAllStates(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public GetAllTimesOfDaysResponse Get(GetAllTimesOfDaysRequest request)
        {
            GetAllTimesOfDaysResponse response = new GetAllTimesOfDaysResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.TimesOfDays = _ngm.GetAllTimesOfDays(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public GetAllTimeZonesResponse Get(GetAllTimeZonesRequest request)
        {
            GetAllTimeZonesResponse response = new GetAllTimeZonesResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.TimeZones = _ngm.GetAllTimeZones(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public GetAllCommTypesResponse Get(GetAllCommTypesRequest request)
        {
            GetAllCommTypesResponse response = new GetAllCommTypesResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.CommTypes = _ngm.GetAllCommTypes(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public GetAllLanguagesResponse Get(GetAllLanguagesRequest request)
        {
            GetAllLanguagesResponse response = new GetAllLanguagesResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.Languages = _ngm.GetAllLanguages(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        #endregion

        #region LookUps GoalRelated
        public GetLookUpsResponse Get(GetLookUpsRequest request)
        {
            GetLookUpsResponse response = new GetLookUpsResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.LookUps = _ngm.GetLookUps(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }
        #endregion

        #region PatientNote
        public PostPatientNoteResponse Post(PostPatientNoteRequest request)
        {
            PostPatientNoteResponse response = new PostPatientNoteResponse();
            NotesManager ntm = new NotesManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ntm.InsertPatientNote(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }

            return response;
        }

        public GetPatientNoteResponse Get(GetPatientNoteRequest request)
        {
            GetPatientNoteResponse response = new GetPatientNoteResponse();
            NotesManager ntm = new NotesManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.Note = ntm.GetPatientNote(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;

                if (response.Note != null)
                    patientIds.Add(response.Note.PatientId);

                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }

        public GetAllPatientNotesResponse Get(GetAllPatientNotesRequest request)
        {
            GetAllPatientNotesResponse response = new GetAllPatientNotesResponse();
            NotesManager ntm = new NotesManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.Notes = ntm.GetAllPatientNotes(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;

                if (response.Notes != null)
                {
                    patientIds = response.Notes.Select(x => x.PatientId).ToList();
                }

                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }

        public PostDeletePatientNoteResponse Post(PostDeletePatientNoteRequest request)
        {
            PostDeletePatientNoteResponse response = null;
            NotesManager ntm = new NotesManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = _security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ntm.DeletePatientNote(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    _ngm.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }
        #endregion

        public NGService()
        {
            _security = new SecurityManager();
            _ngm = new NGManager();
        }

        public NGService(ISecurityManager sm, INGManager ngm)
        {
            _security = sm;
            _ngm = ngm;
        }
    }
}