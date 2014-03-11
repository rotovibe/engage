using Phytel.API.AppDomain.NG.DTO;
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
        public PostProcessActionResponse Post(PostProcessActionRequest request)
        {
            PostProcessActionResponse response = new PostProcessActionResponse();
            PlanManager intm = new PlanManager();

            try
            {
                request.Token = base.Request.Headers["Token"] as string;
                ValidateTokenResponse result = intm.IsUserValidated(request.Version, request.Token, request.ContractNumber);
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

                if (!string.IsNullOrEmpty(response.PatientId))
                {
                    patientIds = new List<string>();
                    patientIds.Add(response.PatientId);
                }

                AuditHelper.LogAuditData(request, patientIds, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response; 
        }
    }
}