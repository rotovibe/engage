using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG
{
    public class EngageCareMemberCohortRuleFactory : ICareMemberCohortRuleFactory
    {
        private readonly IContactEndpointUtil _contactEndpointUtil;
        private readonly ILogger _logger;
        private readonly ICohortRuleUtil _cohortRuleUtil;

        public EngageCareMemberCohortRuleFactory(IContactEndpointUtil contactEndpointUtil, ILogger logger, ICohortRuleUtil cohortRuleUtil)
        {
            _contactEndpointUtil = contactEndpointUtil;
            _logger = logger;
            _cohortRuleUtil = cohortRuleUtil;
        }

        public List<ICareMemberCohortRule> GenerateEngageCareMemberCohortRules()
        {
            return new List<ICareMemberCohortRule>
            {
                new AssignedToMePCMRule(_contactEndpointUtil, _logger,_cohortRuleUtil),
                new UnAssignedPCMRule(_contactEndpointUtil, _logger,_cohortRuleUtil),
                new AssignedToMeRule(_contactEndpointUtil, _logger,_cohortRuleUtil)

            };
        }
    }
}
