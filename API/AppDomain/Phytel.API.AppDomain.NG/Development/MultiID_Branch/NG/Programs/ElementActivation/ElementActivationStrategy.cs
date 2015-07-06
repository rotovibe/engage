using System.Collections.Generic;
using System.Linq;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.AppDomain.NG.Programs.ElementActivation
{
    public class ElementActivationStrategy : IElementActivationStrategy
    {
        private readonly List<IElementActivationRule> _rules;
        private readonly int[] _progAttributeTypes = new int[]{10, 11, 12, 13, 14, 15, 16, 19, 20, 21};

        public ElementActivationStrategy()
        {
            _rules = new List<IElementActivationRule>
            {
                // add other implementations here
                new ToDoActivationRule(),
                new ProblemActivationRule(),
                new GoalActivationRule(),
                new InterventionActivationRule(),
                new TaskActivationRule()
            };
        }

        public SpawnType Run(PlanElementEventArg e, SpawnElement rse, string userId, ProgramAttributeData pad)
        {
            SpawnType alert = null;

            alert = _progAttributeTypes.Contains(rse.ElementType)
                ? new ProgramAttributeActivationRule().Execute(userId, e, rse, pad)
                : _rules.First(r => r.ElementType == rse.ElementType).Execute(userId, e, rse, pad);

            return alert;
        }
    }
}
