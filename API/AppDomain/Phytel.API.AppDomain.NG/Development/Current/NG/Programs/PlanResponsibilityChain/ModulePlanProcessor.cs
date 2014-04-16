using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Program.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.PlanCOR
{
    public class ModulePlanProcessor : PlanProcessor
    {
        private ProgramAttribute _programAttributes;

        public ModulePlanProcessor()
        {
            _programAttributes = new ProgramAttribute();
        }

        public override void PlanElementHandler(object sender, PlanElementEventArg e)
        {
            try
            {
                if (e.PlanElement.GetType().Equals(typeof(Module)))
                {
                    Module module = e.PlanElement as Module;
                    //PlanElementUtil.SetProgramInformation(_programAttributes, e.Program);
                    _programAttributes.PlanElementId = e.Program.Id;

                    if (module.Actions != null)
                    {
                        module.Completed = PlanElementUtil.SetCompletionStatus(module.Actions);
                        if (module.Completed)
                        {
                            module.CompletedBy = e.UserId;
                            module.ElementState = 5;
                            module.DateCompleted = System.DateTime.UtcNow;
                            // look at spawnelement and trigger enabled state.
                            if (module.SpawnElement != null)
                            {
                                PlanElementUtil.SpawnElementsInList(module.SpawnElement, e.Program, e.UserId, _programAttributes);
                            }
                            // save any program attribute changes
                            PlanElementUtil.SaveReportingAttributes(_programAttributes, e.DomainRequest);
                            OnProcessIdEvent(module);
                        }
                    }
                }
                else if (Successor != null)
                {
                    Successor.PlanElementHandler(this, e);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:ModulePlanProcessor:PlanElementHandler()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
