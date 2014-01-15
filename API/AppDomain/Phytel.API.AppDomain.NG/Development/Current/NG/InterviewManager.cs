using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.ServiceClient.Web;
using Phytel.API.AppDomain.NG.PlanSpecification;
using Phytel.API.AppDomain.NG.PlanCOR;

namespace Phytel.API.AppDomain.NG
{
    public class PlanManager : ManagerBase
    {

        public PostProcessActionResponse ProcessActionResults(PostProcessActionRequest request)
        {
            try
            {
                PostProcessActionResponse response = new PostProcessActionResponse();
                Program program = request.Program;

                //// create a responsibility chain to process each elemnt in the hierachy
                ProgramPlanProcessor pChain = InitializeProgramChain();

                Actions action = GetProcessingAction(program.Modules, request.ActionId);
                pChain.ProcessRequest((IPlanElement)action, program, request.UserId);

                // process modules
                program.Modules.ForEach(m =>
                {
                    pChain.ProcessRequest((IPlanElement)m, program, request.UserId);
                });

                // set module settings
                // 3) set enable/visibility of actions after action processing.
                Module mod = PlanElementUtil.FindElementById(program.Modules, action.ModuleId);
                PlanElementUtil.SetEnabledStatusByPrevious(mod.Actions);

                // 4) ProcessAction(request);
                //SaveAction(program);

                return response;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        private ProgramPlanProcessor InitializeProgramChain()
        {
            ProgramPlanProcessor progProc = new ProgramPlanProcessor();
            ModulePlanProcessor modProc = new ModulePlanProcessor();
            ActionPlanProcessor actProc = new ActionPlanProcessor();
            StepPlanProcessor stepProc = new StepPlanProcessor();
            progProc.Successor = modProc;
            modProc.Successor = actProc;
            actProc.Successor = stepProc;

            return progProc;
        }

        private void SaveAction(Program action)
        {
            
        }

        private void CheckPlanRules(Actions action)
        {
            // check for any special rules or objectives
        }

        private Actions GetProcessingAction(List<Module> list, string actionId)
        {
            Actions query = list.SelectMany(module => module.Actions).Where(action => action.Id == actionId).FirstOrDefault();
            return query;
        }
    }
}
