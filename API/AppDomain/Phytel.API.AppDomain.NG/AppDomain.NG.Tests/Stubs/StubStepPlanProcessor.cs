using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.AppDomain.NG.Test.Stubs
{
    public delegate void SpawnEventHandler(object s, SpawnEventArgs e);

    public class StubStepPlanProcessor : PlanProcessor, IStepPlanProcessor
    {
        public StubStepPlanProcessor()
        {
            ProgramAttributes = new ProgramAttributeData();
            if (AppHostBase.Instance != null)
                AppHostBase.Instance.Container.AutoWire(this);
        }

        public override void PlanElementHandler(object sender, PlanElementEventArg e)
        {
            try
            {
                if (e.PlanElement.GetType() == typeof(Step))
                {
                    ProgramAttributes.PlanElementId = e.Program.Id;
                    var s = (Step)e.PlanElement;

                    if (e.Action.Completed)
                    {
                        if (s.Completed)
                        {
                            // see if responses have spawn elements
                            if (s.Responses != null)
                                SetCompletedStepResponses(e, s);

                            if (s.SpawnElement != null)
                            {
                                PEUtils.SpawnElementsInList(s.SpawnElement, e.Program, e.UserId, ProgramAttributes);
                                s.SpawnElement.ForEach(
                                    rse => { if (rse.ElementType > 100) HandlePlanElementActions(e, e.UserId, rse); });
                            }
                        }
                        else
                        {
                            if (s.Responses != null)
                                s.Responses.ForEach(r => { r.Delete = true; });
                        }

                        // save program properties
                        PEUtils.SaveReportingAttributes(ProgramAttributes, e.DomainRequest);
                        // raise process event to register the id
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
                throw new Exception("AD:StepPlanProcessor:PlanElementHandler()::" + ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Handles the response initialization to deletion of false and evokes any spawnelements.
        /// </summary>
        /// <param name="e">PlanElementEventArg</param>
        /// <param name="s">Step</param>
        public void SetCompletedStepResponses(PlanElementEventArg e, Step s)
        {
            try
            {
                s.Responses.ForEach(r =>
                {
                    r.Delete = false;
                    if ((s.StepTypeId == 4) || (s.StepTypeId == 1))
                    {
                        if (s.SelectedResponseId.Equals(r.Id))
                            HandleResponseSpawnElements(s, r, e, e.UserId);
                    }
                    else
                    {
                        HandleResponseSpawnElements(s, r, e, e.UserId);
                    }
                });
            }
            catch (Exception ex)
            {
                throw new Exception("AD:StepPlanProcessor:StepCompletedStepResponse()::" + ex.Message, ex.InnerException);
            }
        }

        public void HandleResponseSpawnElements(Step s, Response r, PlanElementEventArg e, string userId)
        {
            try
            {
                if (r.SpawnElement != null)
                {
                    if (PEUtils.ResponseSpawnAllowed(s, r))
                    {
                        r.SpawnElement.ForEach(rse => HandlePlanElementActions(e, userId, rse));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:StepPlanProcessor:HandleResponseSpawnElements()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
