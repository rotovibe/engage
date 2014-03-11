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
        public GetPatientResponse Post(GetPatientRequest request)
        {
            GetPatientResponse response = new GetPatientResponse();
            NGManager ngm = new NGManager();
            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.GetPatient(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;

                if (response.Patient != null)
                {
                   patientIds = new List<string>();
                    patientIds.Add(response.Patient.Id);                    
                }
                
                AuditHelper.LogAuditData(request, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }

            return response; 
        }

        public GetPatientResponse Get(GetPatientRequest request)
        {
            GetPatientResponse response = new GetPatientResponse();
            NGManager ngm = new NGManager();

            IRequestContext req = base.RequestContext;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.GetPatient(request);
                }
                else
                    throw new UnauthorizedAccessException();

            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;

                if (response.Patient != null)
                {
                    patientIds = new List<string>();
                    patientIds.Add(response.Patient.Id);               
                }

                AuditHelper.LogAuditData(request, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }

            return response;
        }

        public GetPatientSSNResponse Get(GetPatientSSNRequest request)
        {
            GetPatientSSNResponse response = new GetPatientSSNResponse();
            NGManager ngm = new NGManager();

            IRequestContext req = base.RequestContext;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.GetPatientSSN(request);
                }
                else
                    throw new UnauthorizedAccessException();

            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;

                if (response != null)
                {
                    patientIds = new List<string>();
                    patientIds.Add(request.PatientId);
                }

                AuditHelper.LogAuditData(request, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
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
            NGManager ngm = new NGManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.PatientProblems = ngm.GetPatientProblems(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
               AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);               
            }
            
            return response; 
        }

        public GetAllProblemsResponse Get(GetAllProblemsRequest request)
        {
            GetAllProblemsResponse response = new GetAllProblemsResponse();
            NGManager ngm = new NGManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.Problems = ngm.GetProblems(request);
                }
                else
                    throw new UnauthorizedAccessException();               
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);               
            }
            
            return response; 
        }

        public GetAllCohortsResponse Get(GetAllCohortsRequest request)
        {
            GetAllCohortsResponse response = new GetAllCohortsResponse();
            NGManager ngm = new NGManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.Cohorts = ngm.GetCohorts(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public PutPatientFlaggedUpdateResponse Post(PutPatientFlaggedUpdateRequest request)
        {
            PutPatientFlaggedUpdateResponse response = new PutPatientFlaggedUpdateResponse();
            NGManager ngm = new NGManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.PutPatientFlaggedUpdate(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public PutPatientBackgroundResponse Post(PutPatientBackgroundRequest request)
        {
            PutPatientBackgroundResponse response = new PutPatientBackgroundResponse();
            NGManager ngm = new NGManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.UpdateBackground(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }

            return response;
        }

        public PutPatientDetailsUpdateResponse Post(PutPatientDetailsUpdateRequest request)
        {
            PutPatientDetailsUpdateResponse response = new PutPatientDetailsUpdateResponse();
            NGManager ngm = new NGManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.PutPatientDetailsUpdate(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public GetCohortPatientsResponse Get(GetCohortPatientsRequest request)
        {
            GetCohortPatientsResponse response = new GetCohortPatientsResponse();
            NGManager ngm = new NGManager();
            request.Token = base.Request.Headers["Token"] as string;
            try
            {
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.GetCohortPatients(request);
                }
                else
                    throw new UnauthorizedAccessException();

            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;

                if (response.Patients != null)
                {
                    patientIds = response.Patients.Select(x => x.Id).ToList();
                }

                AuditHelper.LogAuditData(request, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public GetAllSettingsResponse Get(GetAllSettingsRequest request)
        {
            GetAllSettingsResponse response = new GetAllSettingsResponse();
            NGManager ngm = new NGManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.GetAllSettings(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                    AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public GetActiveProgramsResponse Get(GetActiveProgramsRequest request)
        {
            GetActiveProgramsResponse response = new GetActiveProgramsResponse();
            NGManager ngm = new NGManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
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
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public PostPatientToProgramsResponse Post(PostPatientToProgramsRequest request)
        {
            PostPatientToProgramsResponse response = new PostPatientToProgramsResponse();
            NGManager ngm = new NGManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.PostPatientToProgram(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);               
            }
            
            return response; 
        }

        public GetPatientProgramDetailsSummaryResponse Get(GetPatientProgramDetailsSummaryRequest request)
        {
            GetPatientProgramDetailsSummaryResponse response = new GetPatientProgramDetailsSummaryResponse();
            NGManager ngm = new NGManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.GetPatientProgramDetailsSummary(request);
                }
                else
                    throw new UnauthorizedAccessException();

            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                 AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);               
            }
            
            return response; 
        }

        public GetPatientProgramsResponse Get(GetPatientProgramsRequest request)
        {
            GetPatientProgramsResponse response = new GetPatientProgramsResponse();
            NGManager ngm = new NGManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.GetPatientPrograms(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;

                if (response.Programs != null)
                {
                    patientIds = response.Programs.Select(x => x.PatientId).ToList();
                }
                
                AuditHelper.LogAuditData(request, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);

            }
            
            return response; 
        }

        #region Contact
        public GetContactResponse Get(GetContactRequest request)
        {
            GetContactResponse response = new GetContactResponse();
            NGManager ngm = new NGManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.Contact = ngm.GetContactByPatientId(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public GetAllCareManagersResponse Get(GetAllCareManagersRequest request)
        {
            GetAllCareManagersResponse response = new GetAllCareManagersResponse();
            NGManager ngm = new NGManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.Contacts = ngm.GetCareManagers(request);
                    response.Version = request.Version;
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public PutUpdateContactResponse Post(PutUpdateContactRequest request)
        {
            PutUpdateContactResponse response = new PutUpdateContactResponse();
            NGManager ngm = new NGManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.PutUpdateContact(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }
        #endregion

        #region LookUps ContactRelated
        public GetAllCommModesResponse Get(GetAllCommModesRequest request)
        {
            GetAllCommModesResponse response = new GetAllCommModesResponse();
            NGManager ngm = new NGManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.CommModes = ngm.GetAllCommModes(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public GetAllStatesResponse Get(GetAllStatesRequest request)
        {
            GetAllStatesResponse response = new GetAllStatesResponse();
            NGManager ngm = new NGManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.States = ngm.GetAllStates(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public GetAllTimesOfDaysResponse Get(GetAllTimesOfDaysRequest request)
        {
            GetAllTimesOfDaysResponse response = new GetAllTimesOfDaysResponse();
            NGManager ngm = new NGManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.TimesOfDays = ngm.GetAllTimesOfDays(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public GetAllTimeZonesResponse Get(GetAllTimeZonesRequest request)
        {
            GetAllTimeZonesResponse response = new GetAllTimeZonesResponse();
            NGManager ngm = new NGManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.TimeZones = ngm.GetAllTimeZones(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public GetAllCommTypesResponse Get(GetAllCommTypesRequest request)
        {
            GetAllCommTypesResponse response = new GetAllCommTypesResponse();
            NGManager ngm = new NGManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.CommTypes = ngm.GetAllCommTypes(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                 AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public GetAllLanguagesResponse Get(GetAllLanguagesRequest request)
        {
            GetAllLanguagesResponse response = new GetAllLanguagesResponse();
            NGManager ngm = new NGManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.Languages = ngm.GetAllLanguages(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        #endregion

        #region LookUps GoalRelated
        public GetLookUpsResponse Get(GetLookUpsRequest request)
        {
            GetLookUpsResponse response = new GetLookUpsResponse();
            NGManager ngm = new NGManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.LookUps = ngm.GetLookUps(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }
        #endregion


        #region PatientNote
        public PostPatientNoteResponse Post(PostPatientNoteRequest request)
        {
            PostPatientNoteResponse response = new PostPatientNoteResponse();
            NotesManager ngm = new NotesManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.InsertPatientNote(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }

            return response;
        }

        public GetPatientNoteResponse Get(GetPatientNoteRequest request)
        {
            GetPatientNoteResponse response = new GetPatientNoteResponse();
            NotesManager ngm = new NotesManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.Note = ngm.GetPatientNote(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;

                if (response.Note != null)
                    patientIds.Add(response.Note.PatientId);

                AuditHelper.LogAuditData(request, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }

        public GetAllPatientNotesResponse Get(GetAllPatientNotesRequest request)
        {
            GetAllPatientNotesResponse response = new GetAllPatientNotesResponse();
            NotesManager ngm = new NotesManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.Notes = ngm.GetAllPatientNotes(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;

                if (response.Notes != null)
                {
                    patientIds = response.Notes.Select(x => x.PatientId).ToList();
                }

                AuditHelper.LogAuditData(request, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }

        public PostDeletePatientNoteResponse Post(PostDeletePatientNoteRequest request)
        {
            PostDeletePatientNoteResponse response = null;
            NotesManager ngm = new NotesManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.DeletePatientNote(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ngm.LogException(ex);
            }
            finally
            {
                AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }
        #endregion

    }
}