using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.AppDomain.NG.Programs.ElementActivation
{
    public abstract class ElementActivationRule
    {
        public abstract SpawnType Execute(string userId, PlanElementEventArg arg, SpawnElement pe, ProgramAttributeData pad);

        protected PlanElement HandlePlanElementActivation(IPlanElementUtils planUtils, PlanElementEventArg e, SpawnElement rse)
        {
            try
            {
                PlanElement pe = planUtils.ActivatePlanElement(rse.ElementId, e.Program);
                return pe;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:StepPlanProcessor:HandlePlanElementActivation()::" + ex.Message,
                    ex.InnerException);
            }
        }
    }
}
