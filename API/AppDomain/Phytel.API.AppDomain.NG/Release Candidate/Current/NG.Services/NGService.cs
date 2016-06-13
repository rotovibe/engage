using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.Common.Audit;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using Phytel.API.AppDomain.NG.DTO.Context;
using Phytel.API.AppDomain.NG.DTO.Note.Context;
using Phytel.API.AppDomain.NG.Notes;
using ServiceStack.Common.Web;
using ServiceStack.FluentValidation;
using ServiceStack.FluentValidation.Results;
using ServiceStack.Text;

namespace Phytel.API.AppDomain.NG.Service
{
    public partial class NGService : ServiceStack.ServiceInterface.Service
    {
        public ISecurityManager Security {get; set;}
        public INGManager NGManager {get; set;}
        public IAuditUtil AuditUtil { get; set; }
        public INotesManager NotesManager { get; set; }
        public ICommonFormatterUtil CommonFormatterUtil { get; set; }
        public IServiceContext ServiceContext { get; set; }

        private const string unknownBrowserType = "Unknown browser";
        private const string unknownUserHostAddress = "Unknown IP";

        // example REST response standard
        //public HttpResult Get(AppDomainRequest request)
        //{
            // to Comply wiht the REST serivces standard. We will specify the HttpResult as the reutrn object for service calls in the future
            //var res = new HttpResult {Response = response, StatusCode = System.Net.HttpStatusCode.Created};
            //return ResolveEventArgs;
        //}

        public GetPatientResponse Post(GetPatientRequest request)
        {
            GetPatientResponse response = new GetPatientResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = NGManager.GetPatient(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
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
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = NGManager.GetPatient(request);
                }
                else
                    throw new UnauthorizedAccessException();

            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
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
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = NGManager.GetPatientSSN(request);
                }
                else
                    throw new UnauthorizedAccessException();

            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
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

        public PostDeletePatientResponse Post(PostDeletePatientRequest request)
        {
            PostDeletePatientResponse response = new PostDeletePatientResponse();
            //NGManager ngm = new NGManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = NGManager.DeletePatient(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;
                if (request.Id != null)
                {
                    patientIds = new List<string>();
                    patientIds.Add(request.Id);
                }
                
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
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
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.Problems = NGManager.GetProblems(request);
                }
                else
                    throw new UnauthorizedAccessException();               
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
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
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.Cohorts = NGManager.GetCohorts(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
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
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = NGManager.PutPatientFlaggedUpdate(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
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
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = NGManager.UpsertPatient(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;
                if (!string.IsNullOrEmpty(response.Id))
                {
                    patientIds = new List<string>();
                    patientIds.Add(response.Id);
                }
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
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
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = NGManager.GetCohortPatients(request);
                }
                else
                    throw new UnauthorizedAccessException();

            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
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

        public GetInitializePatientResponse Get(GetInitializePatientRequest request)
        {
            GetInitializePatientResponse response = new GetInitializePatientResponse();
            ValidateTokenResponse result = null;
            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = NGManager.GetInitializePatient(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
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

        public GetAllSettingsResponse Get(GetAllSettingsRequest request)
        {
            GetAllSettingsResponse response = new GetAllSettingsResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = NGManager.GetAllSettings(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
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
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId != null)
                {
                    if (result.UserId.Trim() != string.Empty)
                    {
                        request.UserId = result.UserId;
                        response = NGManager.GetActivePrograms(request);
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
                    NGManager.LogException(ex);
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
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = NGManager.PostPatientToProgram(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);               
            }
            
            return response; 
        }

        public PostProgramAttributesChangeResponse Post(PostProgramAttributesChangeRequest request)
        {
            PostProgramAttributesChangeResponse response = new PostProgramAttributesChangeResponse();
            ValidateTokenResponse result = null;

            try
            {
                if (base.Request != null)
                {
                    request.Token = base.Request.Headers["Token"] as string;
                }

                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = NGManager.PostProgramAttributeChanges(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
            }
            finally
            {
                if (result != null)
                {
                    string browser = (base.Request != null) ? base.Request.UserAgent : unknownBrowserType;
                    string hostAddress = (base.Request != null) ? base.Request.UserHostAddress : unknownUserHostAddress;
                    AuditUtil.LogAuditData(request, result.SQLUserId, null, browser, hostAddress, request.GetType().Name);
                }
            }

            return response;
        }

        public GetPatientProgramDetailsSummaryResponse Get(GetPatientProgramDetailsSummaryRequest request)
        {
            GetPatientProgramDetailsSummaryResponse response = new GetPatientProgramDetailsSummaryResponse();
            ValidateTokenResponse result = null;

            try
            {
                if (base.Request != null)
                {
                    request.Token = base.Request.Headers["Token"] as string;
                }
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = NGManager.GetPatientProgramDetailsSummary(request);
                }
                else
                    throw new UnauthorizedAccessException();

            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
            }
            finally
            {
                if (result != null)
                {
                    string browser = (base.Request != null) ? base.Request.UserAgent : unknownBrowserType;
                    string hostAddress = (base.Request != null)? base.Request.UserHostAddress : unknownUserHostAddress;
                    AuditUtil.LogAuditData(request, result.SQLUserId, null, browser, hostAddress, request.GetType().Name);
                }
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
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = NGManager.GetPatientPrograms(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
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

        public GetPatientActionDetailsResponse Get(GetPatientActionDetailsRequest request)
        {
            GetPatientActionDetailsResponse response = new GetPatientActionDetailsResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = NGManager.GetPatientActionDetails(request);
                }
                else
                    throw new UnauthorizedAccessException();

            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
            }
            finally
            {
                if (result != null)
                {
                    string browser = (base.Request != null) ? base.Request.UserAgent : unknownBrowserType;
                    string hostAddress = (base.Request != null) ? base.Request.UserHostAddress : unknownUserHostAddress;
                    AuditUtil.LogAuditData(request, result.SQLUserId, null, browser, hostAddress, request.GetType().Name);
                }
            }

            return response;
        }

        public PostRemovePatientProgramResponse Post(PostRemovePatientProgramRequest request)
        {
            PostRemovePatientProgramResponse response = new PostRemovePatientProgramResponse();
            //NGManager ngm = new NGManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = NGManager.RemovePatientProgram(request);
                }
                else
                   throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;
                if (request.PatientId != null)
                {
                    patientIds = new List<string>();
                    patientIds.Add(request.PatientId);
                }

                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }

        #region Contact
        public GetContactByPatientIdResponse Get(GetContactByPatientIdRequest request)
        {
            GetContactByPatientIdResponse response = new GetContactByPatientIdResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.Contact = NGManager.GetContactByPatientId(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }
       
       
        public GetRecentPatientsResponse Get(GetRecentPatientsRequest request)
        {
            GetRecentPatientsResponse response = new GetRecentPatientsResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = NGManager.GetRecentPatients(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;
                if (response.Patients != null && response.Patients.Count >  0)
                {
                    patientIds = new List<string>();
                    foreach (CohortPatient cp in response.Patients)
                    {
                        patientIds.Add(cp.Id);
                    }
                }
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }



        public SearchContactsResponse Post(SearchContactsRequest request)
        {
            if(request == null)
                throw new ArgumentNullException("request");

            if (request.ContactTypeIds.IsNullOrEmpty() && request.ContactStatuses.IsNullOrEmpty())
                throw new ValidationException(
                    new List<ValidationFailure>
                {
                    new ValidationFailure("ContactTypeIds or ContactStatuses","Must provide ContactTypeIds or ContactStatuses","Contacts.Search"),
                    
                });


            var response = new SearchContactsResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    var dataResponse = NGManager.SearchContacts(request);
                    response.Contacts = dataResponse.Contacts;
                    response.TotalCount = dataResponse.TotalCount;
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
            }
            finally
            {

                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }

        public UpdateContactResponse Put(DTO.UpdateContactRequest request)
        {
            UpdateContactResponse response = new UpdateContactResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = NGManager.PutUpdateContact(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request,
                        request.GetType().Name);
            }

            return response;
        }

        public InsertContactResponse Post(InsertContactRequest request)
        {
            InsertContactResponse response = new InsertContactResponse();

            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = NGManager.InsertContact(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request,
                        request.GetType().Name);
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
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.CommModes = NGManager.GetAllCommModes(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
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
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.States = NGManager.GetAllStates(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
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
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.TimesOfDays = NGManager.GetAllTimesOfDays(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
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
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.TimeZones = NGManager.GetAllTimeZones(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
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
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.CommTypes = NGManager.GetAllCommTypes(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
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
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.Languages = NGManager.GetAllLanguages(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        #endregion

        #region ContactTypeLookUp
       
        public GetContactTypeLookupResponse Get(GetContactTypeLookupRequest request)
        {
            GetContactTypeLookupResponse response = new GetContactTypeLookupResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = NGManager.GetContactTypeLookup(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }

            return response;
        }

        #endregion 

        #region LookUps refactored
        public GetLookUpsResponse Get(GetLookUpsRequest request)
        {
            GetLookUpsResponse response = new GetLookUpsResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.LookUps = NGManager.GetLookUps(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            
            return response; 
        }

        public GetLookUpDetailsResponse Get(GetLookUpDetailsRequest request)
        {
            GetLookUpDetailsResponse response = new GetLookUpDetailsResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.LookUpDetails = NGManager.GetLookUpDetails(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }

            return response;
        }
        #endregion

        #region LookUps Program
        public GetAllObjectivesResponse Get(GetAllObjectivesRequest request)
        {
            GetAllObjectivesResponse response = new GetAllObjectivesResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.Objectives = NGManager.GetAllObjectives(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
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
            ValidateTokenResponse result = null;

            try
            {
                if (base.Request != null)
                {
                    request.Token = base.Request.Headers["Token"] as string;
                }

                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = NotesManager.InsertPatientNote(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
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
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
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
                    NGManager.LogException(ex);
            }
            finally
            {
                List<string> patientIds = new List<string>();

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
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    ServiceContext.UserId = result.UserId;
                    ServiceContext.Tag = new PatientNoteContext
                    {
                        Count = request.Count,
                        PatientId = request.PatientId,
                        UserId = result.UserId
                    };

                    response.Notes = NotesManager.GetAllPatientNotes(ServiceContext);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
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
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
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
                    NGManager.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }

        public UpdatePatientNoteResponse Put(UpdatePatientNoteRequest request)
        {
            UpdatePatientNoteResponse response = new UpdatePatientNoteResponse();
            NotesManager ntm = new NotesManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = ntm.UpdatePatientNote(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    NGManager.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }

            return response;
        }
        #endregion
    }
}