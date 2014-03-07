using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.ServiceClient.Web;
using Phytel.API.AppDomain.NG.PlanSpecification;
using Phytel.API.AppDomain.NG.PlanCOR;
using ServiceStack.Service;
using DD = Phytel.API.DataDomain.Program.DTO;
using System.Configuration;

namespace Phytel.API.AppDomain.NG
{
    public class PlanManager : ManagerBase
    {
        public List<string> RelatedChanges { get; set; }

        public PlanManager()
        {
            RelatedChanges = new List<string>();
        }

        public PostProcessActionResponse ProcessActionResults(PostProcessActionRequest request)
        {
            try
            {
                // need to refactor this into a mediator.
                RelatedChanges.Clear();
                PostProcessActionResponse response = new PostProcessActionResponse();

                Program p = PlanElementEndpointUtil.RequestPatientProgramDetail(request);
                Actions action = request.Action;
                NGUtils.UpdateProgramAction(action, p);

                if (action.Completed)
                {
                    // set program starting date
                    if (action.Order == 1)
                    {
                        //p.StartDate = System.DateTime.UtcNow;
                        PlanElementUtil.SetStartDateForProgramAttributes(request.ProgramId, request);
                    }

                    //// create a responsibility chain to process each elemnt in the hierachy
                    ProgramPlanProcessor pChain = InitializeProgramChain();
                    //// process steps in action
                    action.Steps.ForEach(s =>
                    {
                        pChain.ProcessWorkflow((IPlanElement)s, p, request.UserId, request.PatientId, action, request);
                    });

                    pChain.ProcessWorkflow((IPlanElement)action, p, request.UserId, request.PatientId, action, request);
                    Module mod = PlanElementUtil.FindElementById(p.Modules, action.ModuleId);

                    if (mod != null)
                    {
                        // set enabled status for action dependencies
                        PlanElementUtil.SetEnabledStatusByPrevious(mod.Actions);
                        // set enable/visibility of actions after action processing.
                        pChain.ProcessWorkflow((IPlanElement)mod, p, request.UserId, request.PatientId, action, request);
                    }
                    // set module visibility for modules
                    PlanElementUtil.SetEnabledStatusByPrevious(p.Modules);

                    // evaluate program status
                    if (PlanElementUtil.IsProgramCompleted(p, request.UserId))
                    {
                        p.Completed = true;
                        pChain.ProcessWorkflow((IPlanElement)p, p, request.UserId, request.PatientId, action, request);
                    }
                }
                else
                {
                    // need to update this on the p level.
                    action.ElementState = 4; // in progress
                }

                // save
                PlanElementEndpointUtil.SaveAction(request, action.Id, p);

                response.Program = p;
                response.RelatedChanges = RelatedChanges;
                response.PatientId = request.PatientId;
                response.Version = request.Version;

                return response;
            }
            catch (Exception wse)
            {
                throw;
            }
        }

        private ProgramPlanProcessor InitializeProgramChain()
        {
            ProgramPlanProcessor progProc = new ProgramPlanProcessor();
            ModulePlanProcessor modProc = new ModulePlanProcessor();
            ActionPlanProcessor actProc = new ActionPlanProcessor();
            StepPlanProcessor stepProc = new StepPlanProcessor();
            stepProc._spawnEvent += stepProc__spawnEvent;
            progProc.Successor = modProc;
            modProc.Successor = actProc;
            actProc.Successor = stepProc;

            return progProc;
        }

        void stepProc__spawnEvent(object s, SpawnEventArgs e)
        {
            this.RelatedChanges.Add(e.Name);
        }
    }
}
