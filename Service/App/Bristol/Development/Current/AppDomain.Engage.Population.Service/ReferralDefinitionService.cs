using System;
using System.Web;
using AppDomain.Engage.Population.DTO.Referrals;
using Phytel.API.Common.Audit;
using Phytel.API.DataAudit;
using ServiceStack.ServiceClient.Web;

namespace AppDomain.Engage.Population.Service
{
    public class ReferralDefinitionService : ServiceBase
    {
        // used to retrieve the referral/registry/cohort definition.
        public IReferralDefinitionManager ReferralDefinitionManager { get; set; }
        public IAuditHelpers AuditHelpers { get; set; }  

        public PostReferralDefinitionResponse Post(PostReferralDefinitionRequest request)
        {
            var response = new PostReferralDefinitionResponse();
            try
            {
                response = ReferralDefinitionManager.PostReferralDefinition(request);
            }
            catch (WebServiceException ex)
            {
                throw new Exception(ex.ErrorMessage);
            }
            finally
            {
                //AuditHelper.LogAuditData(request, "SQLUserId", null, HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }
    }
}