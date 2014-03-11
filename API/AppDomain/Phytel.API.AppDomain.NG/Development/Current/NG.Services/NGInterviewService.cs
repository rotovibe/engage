using System;
using System.Net;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.ServiceHost;
using Phytel.API.Common.Format;
using Phytel.API.AppDomain.Security.DTO;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceInterface.Cors;
using Phytel.API.Common.Audit;
using System.Collections.Generic;
using System.Web;
using Phytel.API.DataAudit;

namespace Phytel.API.AppDomain.NG.Service
{
    public partial class NGService : ServiceStack.ServiceInterface.Service
    {
        public PostProcessActionResponse Post(PostProcessActionRequest request)
        {
            PostProcessActionResponse response = new PostProcessActionResponse();
            try
            {
                PlanManager intm = new PlanManager();
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
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
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