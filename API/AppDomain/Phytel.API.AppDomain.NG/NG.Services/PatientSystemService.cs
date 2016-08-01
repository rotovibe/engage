using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Context;
using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.Common.Audit;
using Phytel.API.Common.Format;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Service
{
    public partial class PatientSystemService : ServiceStack.ServiceInterface.Service
    {
        public ISecurityManager Security {get; set;}
        public IPatientSystemManager PatientSystemManager { get; set; }
        public IAuditUtil AuditUtil { get; set; }
        public ICommonFormatterUtil CommonFormatterUtil { get; set; }
        public IServiceContext ServiceContext { get; set; }

        private const string unknownBrowserType = "Unknown browser";
        private const string unknownUserHostAddress = "Unknown IP";

        #region System
        
        #region Get
        public GetActiveSystemsResponse Get(GetActiveSystemsRequest request)
        {
            GetActiveSystemsResponse response = new GetActiveSystemsResponse();
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
                    ServiceContext.UserId = result.UserId;
                    response.Systems = PatientSystemManager.GetActiveSystems(ServiceContext);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    PatientSystemManager.LogException(ex);
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
        #endregion

        #endregion

        #region PatientSystem

        #region Get
        public GetPatientSystemsResponse Get(GetPatientSystemsRequest request)
        {
            GetPatientSystemsResponse response = new GetPatientSystemsResponse();
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
                    ServiceContext.UserId = result.UserId;
                    response.PatientSystems = PatientSystemManager.GetPatientSystems(ServiceContext, request.PatientId);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    PatientSystemManager.LogException(ex);
            }
            finally
            {
                if (result != null)
                {
                    List<string> patientIds = null;
                    if (response.PatientSystems != null)
                    {
                        patientIds = response.PatientSystems.Select(x => x.PatientId).ToList();
                    }
                    string browser = (base.Request != null) ? base.Request.UserAgent : unknownBrowserType;
                    string hostAddress = (base.Request != null) ? base.Request.UserHostAddress : unknownUserHostAddress;
                    AuditUtil.LogAuditData(request, result.SQLUserId, patientIds, browser, hostAddress, request.GetType().Name);
                }
            }

            return response;
        }
        #endregion

        #region Post
        public InsertPatientSystemsResponse Post(InsertPatientSystemsRequest request)
        {
            InsertPatientSystemsResponse response = new InsertPatientSystemsResponse();
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
                    ServiceContext.UserId = result.UserId;
                    ServiceContext.Tag = request.PatientSystems;
                    response.PatientSystems = PatientSystemManager.InsertPatientSystems(ServiceContext, request.PatientId);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    PatientSystemManager.LogException(ex);
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
        #endregion

        #region Put
        public UpdatePatientSystemsResponse Put(UpdatePatientSystemsRequest request)
        {
            UpdatePatientSystemsResponse response = new UpdatePatientSystemsResponse();
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
                    ServiceContext.UserId = result.UserId;
                    ServiceContext.Tag = request.PatientSystems;
                    response.PatientSystems = PatientSystemManager.UpdatePatientSystems(ServiceContext, request.PatientId);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    PatientSystemManager.LogException(ex);
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
        #endregion

        #region Delete
        public DeletePatientSystemsResponse Delete(DeletePatientSystemsRequest request)
        {
            DeletePatientSystemsResponse response = new DeletePatientSystemsResponse();
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
                    PatientSystemManager.DeletePatientSystems(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    PatientSystemManager.LogException(ex);
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
        #endregion

        #endregion
    }
}