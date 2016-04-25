using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG
{
    public class EngageCareMemberCohortRuleFactory : ICareMemberCohortRuleFactory
    {
        private readonly IContactEndpointUtil _contactEndpointUtil;

        public EngageCareMemberCohortRuleFactory(IContactEndpointUtil contactEndpointUtil)
        {
            _contactEndpointUtil = contactEndpointUtil;
        }

        public List<ICareMemberCohortRule> GenerateEngageCareMemberCohortRules()
        {
            return new List<ICareMemberCohortRule>
            {
                new AssignedToMePCMRule(),
                new UnAssignedPCMRule(_contactEndpointUtil),
                new AssignedToMeRule()

            };
        }
    }
}
