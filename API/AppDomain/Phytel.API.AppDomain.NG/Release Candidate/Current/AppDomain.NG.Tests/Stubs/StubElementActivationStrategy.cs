using System.Collections.Generic;
using System.Linq;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.AppDomain.NG.Programs.ElementActivation;
using Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.AppDomain.NG.Test.Stubs
{
    public class StubElementActivationStrategy : IElementActivationStrategy
    {
        private readonly List<IElementActivationRule> _rules;

        public StubElementActivationStrategy()
        {
            _rules = new List<IElementActivationRule>
            {
                new StubToDoActivationRule(),
                new StubProblemActivationRule(),
                new GoalActivationRule(){ EndpointUtil = new StubEndpointUtils(), PlanUtils = new StubPlanElementUtils(), GoalsEndpointUtil = new StubGoalsEndpointUtils()},
                new InterventionActivationRule(){ EndpointUtil = new StubEndpointUtils(), PlanUtils = new StubPlanElementUtils(), GoalsEndpointUtil = new StubGoalsEndpointUtils()},
                new TaskActivationRule(){ EndpointUtil = new StubEndpointUtils(), PlanUtils = new StubPlanElementUtils(), GoalsEndpointUtil = new StubGoalsEndpointUtils()}
            };
        }

        public SpawnType Run(PlanElementEventArg e, SpawnElement rse, string userId, ProgramAttributeData pad)
        {
            var alert = _rules.First(r => r.ElementType == rse.ElementType).Execute(userId, e, rse, pad);
            return alert;
        }
    }
}
