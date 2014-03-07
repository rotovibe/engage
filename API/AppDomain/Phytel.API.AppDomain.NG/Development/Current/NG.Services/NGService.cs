using System;
using System.Net;
using System.Linq;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.ServiceHost;
using Phytel.API.Common.Format;
using Phytel.API.AppDomain.Security.DTO;
using ServiceStack.ServiceClient.Web;
using Phytel.API.Common.Audit;
using System.Collections.Generic;
using Phytel.API.DataAudit;
using System.Web;

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
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
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
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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

            IRequestContext req = base.RequestContext;

            try
            {
                NGManager ngm = new NGManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
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
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
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
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
            try
            {
                NGManager ngm = new NGManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
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
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
            try
            {
                NGManager ngm = new NGManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
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
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
            try
            {
                NGManager ngm = new NGManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
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
                //TODO: Log this to C3 database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
            try
            {
                NGManager ngm = new NGManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
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
                //TODO: Log this to C3 database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
            try
            {
                NGManager ngm = new NGManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
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
                //TODO: Log this to C3 database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
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
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
            try
            {
                NGManager ngm = new NGManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
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
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
            try
            {
                NGManager ngm = new NGManager();
                request.Token = base.Request.Headers["Token"] as string;
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
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
            try
            {
                NGManager ngm = new NGManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
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
                //TODO: Log this to C3 database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
            try
            {
                NGManager ngm = new NGManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
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
                //TODO: Log this to C3 database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
            try
            {
                NGManager ngm = new NGManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
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
                //TODO: Log this to C3 database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
            try
            {
                NGManager ngm = new NGManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
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
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
            try
            {
                NGManager ngm = new NGManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
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
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
            try
            {
                NGManager ngm = new NGManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
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
                //TODO: Log this to C3 database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
            try
            {
                NGManager ngm = new NGManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
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
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
            try
            {
                NGManager ngm = new NGManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
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
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
            try
            {
                NGManager ngm = new NGManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
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
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
            try
            {
                NGManager ngm = new NGManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
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
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
            try
            {
                NGManager ngm = new NGManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
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
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
            try
            {
                NGManager ngm = new NGManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
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
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
            try
            {
                NGManager ngm = new NGManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
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
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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
            try
            {
                NotesManager ngm = new NotesManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = ngm.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ngm.InsertPatientNote(request, result.UserId);
                }
                else
                    throw new UnauthorizedAccessException();

                AuditHelper.LogAuditData(request, null, System.Web.HttpContext.Current.Request, request.GetType().Name);

                return response;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                return response;
            }
        }

        public GetPatientNoteResponse Get(GetPatientNoteRequest request)
        {
            GetPatientNoteResponse response = new GetPatientNoteResponse();
            try
            {
                NotesManager nManager = new NotesManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = nManager.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.Note = nManager.GetPatientNote(request);
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

        public GetAllPatientNotesResponse Get(GetAllPatientNotesRequest request)
        {
            GetAllPatientNotesResponse response = new GetAllPatientNotesResponse();
            try
            {
                NotesManager nManager = new NotesManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = nManager.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.Notes = nManager.GetAllPatientNotes(request);
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

        public PostDeletePatientNoteResponse Post(PostDeletePatientNoteRequest request)
        {
            PostDeletePatientNoteResponse response = null;
            try
            {
                NotesManager nManager = new NotesManager();
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = nManager.IsUserValidated(request.Version, request.Token);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = nManager.DeletePatientNote(request);
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
        #endregion

    }
}