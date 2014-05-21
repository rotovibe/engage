using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.WebHost.Endpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.PlanCOR
{
    public class ActionPlanProcessor : PlanProcessor
    {
        private readonly ProgramAttributeData _programAttributes;
        public IPlanElementUtils PeUtils { get; set; }

        public ActionPlanProcessor()
        {
            _programAttributes = new ProgramAttributeData();
            if (AppHostBase.Instance != null)
                AppHostBase.Instance.Container.AutoWire(this);
        }

        public override void PlanElementHandler(object sender, PlanElementEventArg e)
        {
            try
            {
                if (e.PlanElement.GetType() == typeof(Actions))
                {
                    Actions action = e.PlanElement as Actions;
                    //PlanElementUtil.SetProgramInformation(_programAttributes, e.Program);
                    _programAttributes.PlanElementId = e.Program.Id;

                    if (action != null && action.Completed)
                    {
                        action.CompletedBy = e.UserId;
                        action.ElementState = 5;
                        action.DateCompleted = System.DateTime.UtcNow;
                        PeUtils.DisableCompleteButtonForAction(action.Steps);

                        //2) look at spawnelement and trigger enabled state.
                        if (action.SpawnElement != null)
                        {
                            PeUtils.SpawnElementsInList(action.SpawnElement, e.Program, e.UserId, _programAttributes);
                        }

                        // save any program attribute changes
                        PeUtils.SaveReportingAttributes(_programAttributes, e.DomainRequest);
                        OnProcessIdEvent(action);
                    }
                }
                else if (Successor != null)
                {
                    Successor.PlanElementHandler(this, e);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:ActionPlanProcessor:PlanElementHandler()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
