using System;
using AppDomain.Engage.Population.DTO.Referrals;
using Phytel.API.Common.Audit;

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
                ReferralDefinitionManager.PostReferralDefinition(request);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //AuditHelper.LogAuditData(request, "SQLUserId", null, HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }
    }
}