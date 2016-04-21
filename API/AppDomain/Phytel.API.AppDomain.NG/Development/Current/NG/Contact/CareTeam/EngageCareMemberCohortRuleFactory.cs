using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG
{
    public class EngageCareMemberCohortRuleFactory : ICareMemberCohortRuleFactory
    {
        public List<ICareMemberCohortRule> GenerateEngageCareMemberCohortRules()
        {
            return new List<ICareMemberCohortRule>
            {
                new AssignedToMePCMRule(),
                new UnAssignedPCMRule(),
                new AssignedToMeRule()

            };
        }
    }
}
