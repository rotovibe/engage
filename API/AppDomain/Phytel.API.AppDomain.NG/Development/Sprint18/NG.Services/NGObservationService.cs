using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.Observation;
using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.Service
{
    public partial class NGService : ServiceStack.ServiceInterface.Service
    {
        public IObservationsManager Omgr { get; set; }

        public GetStandardObservationItemsResponse Get(GetStandardObservationItemsRequest request)
        {
            GetStandardObservationItemsResponse response = new GetStandardObservationItemsResponse();
            ObservationsManager om = new ObservationsManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = om.GetStandardObservationsRequest(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    om.LogException(ex);
            }
            finally
            {
                List<string> patientIds = new List<string>();
                patientIds.Add(request.PatientId);

                if(result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response; 
        }

        public GetAdditionalObservationItemResponse Get(GetAdditionalObservationItemRequest request)
        {
            GetAdditionalObservationItemResponse response = new GetAdditionalObservationItemResponse();
            ObservationsManager om = new ObservationsManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = om.GetAdditionalObservationsRequest(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    om.LogException(ex);
            }
            finally
            {
                List<string> patientIds = new List<string>();
                patientIds.Add(request.PatientId);
                
                if(result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }

            return response;
        }

        public GetObservationsResponse Get(GetObservationsRequest request)
        {
            GetObservationsResponse response = new GetObservationsResponse();
            ObservationsManager om = new ObservationsManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = om.GetObservations(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    om.LogException(ex);
            }
            finally
            {
                if(result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }

            return response;
        }

        public PostUpdateObservationItemsResponse Post(PostUpdateObservationItemsRequest request)
        {
            PostUpdateObservationItemsResponse response = new PostUpdateObservationItemsResponse();
            ObservationsManager om = new ObservationsManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = om.SavePatientObservations(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    om.LogException(ex);
            }
            finally
            {
                List<string> patientIds = new List<string>();
                patientIds.Add(request.PatientId);

                if(result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }

            return response;
        }

        public GetAllowedStatesResponse Get(GetAllowedStatesRequest request)
        {
            GetAllowedStatesResponse response = new GetAllowedStatesResponse();
            ObservationsManager om = new ObservationsManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = om.GetAllowedObservationStates(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    om.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }

        public GetPatientProblemsResponse Get(GetPatientProblemsRequest request)
        {
            GetPatientProblemsResponse response = new GetPatientProblemsResponse();
            ObservationsManager om = new ObservationsManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = om.GetPatientProblemsSummary(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    om.LogException(ex);
            }
            finally
            {
                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }

        public GetInitializeProblemResponse Get(GetInitializeProblemRequest request)
        {
            GetInitializeProblemResponse response = new GetInitializeProblemResponse();
            ObservationsManager om = new ObservationsManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = om.GetInitializeProblem(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    om.LogException(ex);
            }
            finally
            {
                List<string> patientIds = new List<string>();
                patientIds.Add(request.PatientId);

                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }

        public GetCurrentPatientObservationsResponse Get(GetCurrentPatientObservationsRequest request)
        {
            GetCurrentPatientObservationsResponse response = new GetCurrentPatientObservationsResponse();
            ObservationsManager om = new ObservationsManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = om.GetCurrentPatientObservations(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    om.LogException(ex);
            }
            finally
            {
                List<string> patientIds = new List<string>();
                patientIds.Add(request.PatientId);

                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }

        public GetHistoricalPatientObservationsResponse Get(GetHistoricalPatientObservationsRequest request)
        {
            GetHistoricalPatientObservationsResponse response = new GetHistoricalPatientObservationsResponse();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    var resultSet = Omgr.GetHistoricalPatientObservations(request);
                    response.PatientObservations = resultSet;
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    Omgr.LogException(ex);
            }
            finally
            {
                List<string> patientIds = new List<string>();
                patientIds.Add(request.PatientId);

                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }
    }
}