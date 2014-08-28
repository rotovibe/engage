using System.Collections.Generic;
using System.Linq;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.AppDomain.NG.Programs.ElementActivation;

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
                new StubProblemActivationRule()
            };
        }

        public string Run(PlanElementEventArg e, SpawnElement rse, string userId)
        {
            var alert = _rules.First(r => r.ElementType == rse.ElementType).Execute(userId, e, rse);
            return alert;
        }
    }
}
