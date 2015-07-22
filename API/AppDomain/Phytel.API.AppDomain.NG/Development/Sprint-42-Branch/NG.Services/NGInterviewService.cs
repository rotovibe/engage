using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.Service
{
    public partial class NGService
    {
        public PostProcessActionResponse Post(PostProcessActionRequest request)
        {
            var response = new PostProcessActionResponse();
            var intm = new PlanManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = intm.ProcessActionResults(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    intm.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;
                if (!string.IsNullOrEmpty(response.PatientId)){patientIds = new List<string> {response.PatientId};}

                if(result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response; 
        }

        public PostSaveActionResponse Post(PostSaveActionRequest request)
        {
            var response = new PostSaveActionResponse();
            var intm = new PlanManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = intm.SaveActionResults(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    intm.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;
                if (!string.IsNullOrEmpty(response.PatientId)){ patientIds = new List<string> {response.PatientId}; }

                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }

        public PostRepeatActionResponse Post(PostRepeatActionRequest request)
        {
            var response = new PostRepeatActionResponse();
            var intm = new PlanManager();
            ValidateTokenResponse result = null;

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response = intm.RepeatAction(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    intm.LogException(ex);
            }
            finally
            {
                List<string> patientIds = null;
                if (!string.IsNullOrEmpty(response.PatientId)) { patientIds = new List<string> { response.PatientId }; }

                if (result != null)
                    AuditHelper.LogAuditData(request, result.SQLUserId, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }
    }
}