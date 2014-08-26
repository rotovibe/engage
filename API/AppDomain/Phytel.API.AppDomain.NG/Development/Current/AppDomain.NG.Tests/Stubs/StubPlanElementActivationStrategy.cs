using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.AppDomain.NG.Programs.PlanElemStrategy;

namespace Phytel.API.AppDomain.NG.Test.Stubs
{
    public class StubPlanElementActivationStrategy : IPlanElementActivationStrategy
    {
        private readonly List<IPlanElementActivationRule> _rules;

        public StubPlanElementActivationStrategy()
        {
            _rules = new List<IPlanElementActivationRule>
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
