using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.ServiceClient.Web;
using Phytel.API.AppDomain.NG.PlanSpecification;

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

                // create a responsibility chain to process each elemnt in the hierachy
                Actions action = GetProcessingAction(program.Modules, request.ActionId);

                // 1) check for special logic with response in action
                CheckPlanRules(action);

                // 2) set completion status for this action based on aggregate completion
                action.Completed = PlanElementUtil.SetCompletionStatus(action.Steps);
                action.Enabled = !action.Completed;

                // 3) set visibility of actions after action processing.
                Module mod = PlanElementUtil.FindElementById(program.Modules, action.ModuleId);
                PlanElementUtil.SetEnabledStatus(mod.Actions);

                // 4) ProcessAction(request);
                SaveAction(program);

                return response;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
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
