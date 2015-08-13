using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Utilization;
using Phytel.API.AppDomain.NG.Notes;
using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.Common.Audit;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using ServiceStack.Common.Web;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Service
{
    public class PatientUtilizationService : ServiceStack.ServiceInterface.Service
    {
        public ISecurityManager Security { get; set; }
        public IUtilizationManager UtilManager { get; set; }
        public IAuditUtil AuditUtil { get; set; }
        public ICommonFormatterUtil CommonFormatterUtil { get; set; }

        ///// <summary>
        ///// Example to return a HttpResult object with a decorated response.
        ///// </summary>
        ///// <param name="request">GetPatientUtilizationRequest</param>
        ///// <returns>object</returns>
        //public object Get(GetPatientUtilizationRequest request)
        //{
        //    GetPatientUtilizationResponse response = null;
        //    ValidateTokenResponse result = null;

        //    try
        //    {
        //        request.Token = base.Request.Headers["Token"] as string;
        //        result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
        //        if (result.UserId.Trim() != string.Empty)
        //        {
        //            request.UserId = result.UserId;
        //            response = UtilManager.GetPatientUtilization(request);
        //        }
        //        else
        //            throw new UnauthorizedAccessException();
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
        //        if ((ex is WebServiceException) == false)
        //            UtilManager.LogException(ex);
        //    }
        //    finally
        //    {
        //        List<string> patientIds = new List<string>();

        //        if (response.Utilization != null)
        //            patientIds.Add(response.Utilization.PatientId);

        //        if (result != null)
        //            AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, HttpContext.Current.Request, request.GetType().Name);
        //    }
        //    return new HttpResult(response, HttpStatusCode.OK)
        //    {
        //        StatusDescription = "this is really cool!",
        //        Location = "www.google.com"
        //    };
        //}

        public GetPatientUtilizationResponse Get(GetPatientUtilizationRequest request)
        {
            GetPatientUtilizationResponse response = null;
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = UtilManager.GetPatientUtilization(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    UtilManager.LogException(ex);
            }
            finally
            {
                List<string> patientIds = new List<string>();

                if (response.Utilization != null)
                    patientIds.Add(response.Utilization.PatientId);

                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }

        public DeletePatientUtilizationResponse Delete(DeletePatientUtilizationRequest request)
        {
            DeletePatientUtilizationResponse response = null;
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = UtilManager.DeletePatientUtilization(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    UtilManager.LogException(ex);
            }
            finally
            {
                List<string> patientIds = new List<string>();

                if (response.Success)
                    patientIds.Add(request.PatientId);

                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }

        public GetPatientUtilizationsResponse Get(GetPatientUtilizationsRequest request)
        {
            GetPatientUtilizationsResponse response = null;
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = UtilManager.GetPatientUtilizations(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    UtilManager.LogException(ex);
            }
            finally
            {
                List<string> patientIds = new List<string>();

                if (response.Utilizations != null)
                {
                    response.Utilizations.ForEach(u => patientIds.Add(u.PatientId));
                }

                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }

        public PostPatientUtilizationResponse Post(PostPatientUtilizationRequest request)
        {
            PostPatientUtilizationResponse response = null;
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    if (string.IsNullOrEmpty(request.Utilization.PatientId))
                        throw new Exception("Missing patientid");

                    if (string.IsNullOrEmpty(request.Utilization.VisitTypeId))
                        throw new Exception("Missing visittypeid");

                    request.UserId = result.UserId;
                    response = UtilManager.InsertPatientUtilization(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    UtilManager.LogException(ex);
            }
            finally
            {
                List<string> patientIds = new List<string>();

                if (response.Result)
                {
                    patientIds.Add(request.PatientId);
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, HttpContext.Current.Request, request.GetType().Name);
                }
            }
            return response;
        }

        public PutPatientUtilizationResponse Put(PutPatientUtilizationRequest request)
        {
            PutPatientUtilizationResponse response = null;
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    // refactor this validators to a pattern
                    if (string.IsNullOrEmpty(request.Utilization.Id))
                        throw new ArgumentException("Missing utilization id");

                    if (string.IsNullOrEmpty(request.Utilization.PatientId))
                        throw new ArgumentException("Missing patientid");

                    if (string.IsNullOrEmpty(request.Utilization.VisitTypeId))
                        throw new ArgumentException("Missing visittypeid");

                    request.UserId = result.UserId;
                    response = UtilManager.UpdatePatientUtilization(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    UtilManager.LogException(ex);
            }
            finally
            {
                List<string> patientIds = new List<string>();

                if (response.Result)
                {
                    patientIds.Add(request.PatientId);
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, HttpContext.Current.Request, request.GetType().Name);
                }
            }
            return response;
        }
    }
}