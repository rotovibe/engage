using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.PlanElementStrategy
{
    public class StatePlanElementStrategy : IPlanElementStrategy
    {
        private readonly IPlanElementAction _action;

        public StatePlanElementStrategy(IPlanElementAction action)
        {
            _action = action;
        }

        public void Evoke()
        {
            _action.Execute();
        }
    }
}
