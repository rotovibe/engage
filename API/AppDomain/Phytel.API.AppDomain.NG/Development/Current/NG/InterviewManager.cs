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
        protected static readonly string DDProgramServiceUrl = ConfigurationManager.AppSettings["DDProgramServiceUrl"];

        public PostProcessActionResponse ProcessActionResults(PostProcessActionRequest request)
        {
            try
            {
                PostProcessActionResponse response = new PostProcessActionResponse();

                Program p = RequestPatientProgramDetail(request);
                Actions action = request.Action;
                NGUtils.UpdateProgramAction(action, p);

                //// create a responsibility chain to process each elemnt in the hierachy
                ProgramPlanProcessor pChain = InitializeProgramChain();

                //// process steps in action
                action.Steps.ForEach(s =>
                {
                    pChain.ProcessRequest((IPlanElement)s, p, request.UserId, request.PatientId);
                });

                pChain.ProcessRequest((IPlanElement)action, p, request.UserId, request.PatientId);


                Module mod = PlanElementUtil.FindElementById(p.Modules, action.ModuleId);

                if (mod != null)
                {
                    // set enabled status for action dependencies
                    PlanElementUtil.SetEnabledStatusByPrevious(mod.Actions);

                    // set enable/visibility of actions after action processing.
                    pChain.ProcessRequest((IPlanElement)mod, p, request.UserId, request.PatientId);
                }

                // set module visibility for modules
                PlanElementUtil.SetEnabledStatusByPrevious(p.Modules);

                // save
                SaveAction(request, p);

                response.Program = p;
                response.Version = request.Version;

                return response;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
        }

        private Program RequestPatientProgramDetail(PostProcessActionRequest request)
        {
            Program pd = null;
            IRestClient client = new JsonServiceClient();
            DD.GetProgramDetailsSummaryResponse resp =
                client.Get<DD.GetProgramDetailsSummaryResponse>(
                string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Program/{5}/Details/?Token={6}",
                DDProgramServiceUrl,
                "NG",
                request.Version,
                request.ContractNumber,
                request.PatientId,
                request.ProgramId,
                request.Token));

            pd = NGUtils.FormatProgramDetail(resp.Program);

            return pd;
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

        private DD.ProgramDetail SaveAction(PostProcessActionRequest request, Program p)
        {
            try
            {
                DD.ProgramDetail pD = NGUtils.FormatProgramDetail(p);

                IRestClient client = new JsonServiceClient();
                DD.PutProgramActionProcessingResponse response = client.Put<DD.PutProgramActionProcessingResponse>(
                    string.Format(@"{0}/{1}/{2}/{3}/Patient/{4}/Programs/{5}/Update",
                    DDProgramServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.ProgramId,
                    request.Token), new DD.PutProgramActionProcessingRequest { Program = pD, UserId = request.UserId });

                return response.program;
            }
            catch (WebServiceException wse)
            {
                Exception ae = new Exception(wse.ResponseBody, wse.InnerException);
                throw ae;
            }
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
