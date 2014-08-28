using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;

namespace Phytel.API.AppDomain.NG.Programs.ElementActivation
{
    public class ElementActivationStrategy : IElementActivationStrategy
    {
        private readonly List<IElementActivationRule> _rules;

        public ElementActivationStrategy()
        {
            _rules = new List<IElementActivationRule>
            {
                new ToDoActivationRule(),
                new ProblemActivationRule()
            };
        }

        public string Run(PlanElementEventArg e, SpawnElement rse, string userId)
        {
            var alert = _rules.First(r => r.ElementType == rse.ElementType).Execute(userId, e, rse);
            return alert;
        }
    }
}
