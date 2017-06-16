using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.WebHost.Endpoints;
using System;

namespace Phytel.API.AppDomain.NG.PlanCOR
{
    public class ModulePlanProcessor : PlanProcessor
    {
        public ModulePlanProcessor()
        {
            ProgramAttributes = new ProgramAttributeData();
            if (AppHostBase.Instance != null)
                AppHostBase.Instance.Container.AutoWire(this);
        }

        public override void PlanElementHandler(object sender, PlanElementEventArg e)
        {
            try
            {
                if (e.PlanElement.GetType() == typeof(Module))
                {
                    Module module = e.PlanElement as Module;
                    //PlanElementUtil.SetProgramInformation(_programAttributes, e.Program);
                    ProgramAttributes.PlanElementId = e.Program.Id;

                    if (module.Actions != null)
                    {
                        module.Completed = PEUtils.GetCompletionStatus(module.Actions);
                        if (module.Completed)
                        {
                            module.CompletedBy = e.UserId;
                            module.StateUpdatedOn = DateTime.UtcNow;
                            module.ElementState = (int)ElementState.Completed;
                            module.DateCompleted = System.DateTime.UtcNow;

                            // look at spawnelement and trigger enabled state.
                            if (module.SpawnElement != null)
                            {
                                PEUtils.SpawnElementsInList(module.SpawnElement, e.Program, e.UserId, ProgramAttributes);
                                module.SpawnElement.ForEach(
                                    rse => { if (rse.ElementType > 100) HandlePlanElementActions(e, e.UserId, rse); });
                            }

                            // save any program attribute changes
                            PEUtils.SaveReportingAttributes(ProgramAttributes, e.DomainRequest);
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
