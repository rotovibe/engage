using System;
using System.Web;
using AppDomain.Engage.Population.DTO.Context;
using AppDomain.Engage.Population.DTO.Referrals;
using Phytel.API.AppDomain.Platform.Security.DTO;
using Phytel.API.ASE.Client.Interface;
using Phytel.API.Common.Audit;
using Phytel.API.DataAudit;
using Phytel.Services.API.Platform.Filter;
using Phytel.Services.API.Platform.Filter.Attributes;
using Phytel.Services.API.Provider;
using ServiceStack.ServiceClient.Web;

namespace AppDomain.Engage.Population.Service
{
    [IsAuthenticatedFilter("br")]
    public class ReferralDefinitionService : ServiceBase
    {
        private readonly IHostContextProxy _hostContextProxy;
        private readonly IAuditLogger _auditLogger;
        private readonly IASEClient _client;
        private UserContext _userContext;
        // used to retrieve the referral/registry/cohort definition.
        public IReferralDefinitionManager ReferralDefinitionManager { get; set; }
        public IAuditHelpers AuditHelpers { get; set; }

        public ReferralDefinitionService(IHostContextProxy hostContextProxy, IAuditLogger auditLogger, IASEClient client)
        {
            _hostContextProxy = hostContextProxy;
            _userContext = BuildUserContext(_hostContextProxy.GetItem(FilterAttributesConstants.UserSessionName) as AppDomainSessionInfo);
            _client = client;
            _auditLogger = auditLogger;
        }

        public PostReferralDefinitionResponse Post(PostReferralDefinitionRequest request)
        {
            ReferralDefinitionManager.UserContext = _userContext; // initialize this for every service handler.
            var response = new PostReferralDefinitionResponse();
            try
            {
                response = ReferralDefinitionManager.PostReferralDefinition(request.ReferralDefinitionData);

                // FYI : use this to log any patients that you might see.
                //_auditLogger.Patients = new List<string> { "patient1", "patient2", "patient3" };
            }
            catch (Exception ex)
            {
                FormatException(request, new PostReferralDefinitionResponse(), _client, ex);
            }
            return response;
        }
    }
}