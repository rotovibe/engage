using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Phytel.API.AppDomain.NG.Allergy;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.Common.Audit;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using ServiceStack.ServiceClient.Web;


namespace Phytel.API.AppDomain.NG.Service
{
    public partial class NGContactService : ServiceStack.ServiceInterface.Service
    {
        public ISecurityManager Security { get; set; }
        public IContactManager ContactManager { get; set; }
        public IAuditUtil AuditUtil { get; set; }
        public ICommonFormatterUtil CommonFormatterUtil { get; set; }

        private const string unknownBrowserType = "Unknown browser";
        private const string unknownUserHostAddress = "Unknown IP";
        
        #region Contact

        #endregion

        #region CareTeam
        public GetCareTeamResponse Get(GetCareTeamRequest request)
        {
            GetCareTeamResponse response = new GetCareTeamResponse();
            try
            {
                if (base.Request != null)
                {
                    request.Token = base.Request.Headers["Token"] as string;
                }
                ValidateTokenResponse result = null;
                result = Security.IsUserValidated(request.Version, request.Token, request.ContractNumber);
                if (result.UserId.Trim() != string.Empty)
                {
                    request.UserId = result.UserId;
                    response.CareTeam = ContactManager.GetCareTeam(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    ContactManager.LogException(ex);
            }
            finally
            {
                //List<string> patientIds = new List<string>();
                //patientIds.Add(request.PatientId);
                //if (result != null)
                //{
                //    string browser = (base.Request != null) ? base.Request.UserAgent : unknownBrowserType;
                //    string hostAddress = (base.Request != null) ? base.Request.UserHostAddress : unknownUserHostAddress;
                //    AuditUtil.LogAuditData(request, result.SQLUserId, patientIds, browser, hostAddress, request.GetType().Name);
                //}
            }
            return response;
        }

        #endregion
    }

}