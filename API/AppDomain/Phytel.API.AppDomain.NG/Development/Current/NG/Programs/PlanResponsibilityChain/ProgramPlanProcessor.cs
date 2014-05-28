using Phytel.API.AppDomain.NG.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.PlanCOR
{
    public class ProgramPlanProcessor : PlanProcessor
    {
        public override void PlanElementHandler(object sender, PlanElementEventArg e)
        {
            try
            {
                if (e.PlanElement.GetType() == typeof(Program))
                {
                    if (e.Program.Completed)
                    {
                        e.Program.Completed = true;
                        e.Program.DateCompleted = System.DateTime.UtcNow;
                        e.Program.CompletedBy = e.UserId;
                        //e.Program.GraduatedFlag = true;
                        OnProcessIdEvent(e.Program);
                    }
                }
                else if (Successor != null)
                {
                    Successor.PlanElementHandler(this, e);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:ProgramPlanProcessor:PlanElementHandler()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
