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
                RelatedChanges.Clear();
                PostProcessActionResponse response = new PostProcessActionResponse();

                Program p = PlanElementEndpointUtil.RequestPatientProgramDetail(request);
                Actions action = request.Action;
                NGUtils.UpdateProgramAction(action, p);

                if (action.Completed)
                {
                    //// create a responsibility chain to process each elemnt in the hierachy
                    ProgramPlanProcessor pChain = InitializeProgramChain();
                    //// process steps in action
                    action.Steps.ForEach(s =>
                    {
                        pChain.ProcessRequest((IPlanElement)s, p, request.UserId, request.PatientId, action);
                    });

                    pChain.ProcessRequest((IPlanElement)action, p, request.UserId, request.PatientId, action);
                    Module mod = PlanElementUtil.FindElementById(p.Modules, action.ModuleId);

                    if (mod != null)
                    {
                        // set enabled status for action dependencies
                        PlanElementUtil.SetEnabledStatusByPrevious(mod.Actions);
                        // set enable/visibility of actions after action processing.
                        pChain.ProcessRequest((IPlanElement)mod, p, request.UserId, request.PatientId, action);
                    }
                    // set module visibility for modules
                    PlanElementUtil.SetEnabledStatusByPrevious(p.Modules);
                }
                else
                {
                    action.ElementState = 3;
                }

                // save
                PlanElementEndpointUtil.SaveAction(request, p);

                response.Program = p;
                response.RelatedChanges = RelatedChanges;
                response.PatientId = request.PatientId;
                response.Version = request.Version;

                return response;
            }
            catch (Exception wse)
            {
                throw wse;
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
