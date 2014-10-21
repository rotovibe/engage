using Phytel.API.AppDomain.NG.Allergy;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Allergy;
using Phytel.API.AppDomain.Security.DTO;
using Phytel.API.Common.Audit;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Phytel.API.AppDomain.NG.Service
{
    public partial class AllergyService : ServiceStack.ServiceInterface.Service
    {
        public ISecurityManager Security {get; set;}
        public IAllergyManager AllergyManager {get; set;}
        public IAuditUtil AuditUtil { get; set; }
        public ICommonFormatterUtil CommonFormatterUtil { get; set; }

        private const string unknownBrowserType = "Unknown browser";
        private const string unknownUserHostAddress = "Unknown IP";

        public GetAllergiesResponse Get(GetAllergiesRequest request)
        {
            GetAllergiesResponse response = new GetAllergiesResponse();
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
                    response.Allergies = AllergyManager.GetAllergies(request);
                }
                else
                    throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                CommonFormatterUtil.FormatExceptionResponse(response, base.Response, ex);
                if ((ex is WebServiceException) == false)
                    AllergyManager.LogException(ex);
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
    }
}