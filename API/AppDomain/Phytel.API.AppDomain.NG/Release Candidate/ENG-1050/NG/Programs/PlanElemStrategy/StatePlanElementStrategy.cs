using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.PlanElementStrategy
{
    public class StatePlanElementStrategy : IPlanElementStrategy
    {
        private readonly IElementAction _action;

        public StatePlanElementStrategy(IElementAction action)
        {
            _action = action;
        }

        public void Evoke()
        {
            _action.Execute();
        }
    }
}
