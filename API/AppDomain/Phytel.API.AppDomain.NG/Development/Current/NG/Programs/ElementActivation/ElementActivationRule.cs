using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;

namespace Phytel.API.AppDomain.NG.Programs.ElementActivation
{
    public abstract class ElementActivationRule
    {
        public abstract string Execute(string UserId, PlanElementEventArg arg, SpawnElement pe);
    }
}
