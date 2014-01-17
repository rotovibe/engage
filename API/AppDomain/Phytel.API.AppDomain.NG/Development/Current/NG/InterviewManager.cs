﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.ServiceClient.Web;
using Phytel.API.AppDomain.NG.PlanSpecification;
using Phytel.API.AppDomain.NG.PlanCOR;
using ServiceStack.Service;
using Phytel.API.DataDomain.Program.DTO;
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

                //// create a responsibility chain to process each elemnt in the hierachy
                ProgramPlanProcessor pChain = InitializeProgramChain();

                Actions action = GetProcessingAction(request.Program.Modules, request.ActionId);
                pChain.ProcessRequest((IPlanElement)action, request.Program, request.UserId);

                // process modules
                request.Program.Modules.ForEach(m =>
                {
                    pChain.ProcessRequest((IPlanElement)m, request.Program, request.UserId);
                });

                // need to get module references to control state
                // 3) set enable/visibility of actions after action processing.
                Module mod = PlanElementUtil.FindElementById(request.Program.Modules, action.ModuleId);
                PlanElementUtil.SetEnabledStatusByPrevious(mod.Actions);

                // 4) save
                SaveAction(request);

                response.Program = request.Program;
                response.Version = request.Version;

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

        private ProgramDetail SaveAction(PostProcessActionRequest request)
        {
            try
            {
                ProgramDetail pD = NGUtils.FormatProgramDetail(request.Program);

                IRestClient client = new JsonServiceClient();
                PutProgramActionProcessingResponse response = client.Put<PutProgramActionProcessingResponse>(
                    string.Format(@"{0}/{1}/{2}/{3}/Patient/{4}/Programs/{5}/Update",
                    DDProgramServiceUrl,
                    "NG",
                    request.Version,
                    request.ContractNumber,
                    request.PatientId,
                    request.Program.Id,
                    request.Token), new PutProgramActionProcessingRequest { Program = pD, UserId = request.UserId });

                return response.program;
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain: SaveAction():" + ex.Message, ex.InnerException);
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
