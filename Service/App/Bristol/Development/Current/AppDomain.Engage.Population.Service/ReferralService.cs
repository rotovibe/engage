using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppDomain.Engage.Population.DTO.Referrals;
using Phytel.API.Common.Audit;
using Phytel.API.DataAudit;

namespace AppDomain.Engage.Population.Service
{
    public class ReferralService : ServiceBase
    {
        // used to retrieve the referral/registry/cohort definition.
        public IReferralManager ReferralManager { get; set; }
        public IAuditHelpers AuditHelpers { get; set; }

        public PostReferralDefinitionResponse Post(PostReferralDefinitionRequest request)
        {
            var response =new PostReferralDefinitionResponse();
            try
            {
                //ReferralManager.
            }
            catch (Exception ex)
            {

            }
            finally
            {
                AuditHelper.LogAuditData(request, "SQLUserId", null, HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }
    }
}