using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG
{
    public class EngageCareMemberCohortRuleFactory : ICareMemberCohortRuleFactory
    {
        private readonly IContactEndpointUtil _contactEndpointUtil;
        private readonly ILogger _logger;

        public EngageCareMemberCohortRuleFactory(IContactEndpointUtil contactEndpointUtil, ILogger logger)
        {
            _contactEndpointUtil = contactEndpointUtil;
            _logger = logger;
        }

        public List<ICareMemberCohortRule> GenerateEngageCareMemberCohortRules()
        {
            return new List<ICareMemberCohortRule>
            {
                new AssignedToMePCMRule(),
                new UnAssignedPCMRule(_contactEndpointUtil, _logger),
                new AssignedToMeRule()

            };
        }
    }
}
