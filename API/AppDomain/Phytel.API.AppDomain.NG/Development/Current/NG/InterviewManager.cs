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
        public List<object> ProcessedElements { get; set; }

        public PlanManager()
        {
            RelatedChanges = new List<string>();
            ProcessedElements = new List<object>();
        }

        public PostProcessActionResponse ProcessActionResults(PostProcessActionRequest request)
        {
            try
            {
                // need to refactor this into a mediator.
                RelatedChanges.Clear();
                ProcessedElements.Clear();
                PostProcessActionResponse response = new PostProcessActionResponse();

                Program p = PlanElementEndpointUtil.RequestPatientProgramDetail(request);
                Actions action = request.Action;
                NGUtils.UpdateProgramAction(action, p);

                if (action.Completed)
                {
                    // pre-process
                    // set program starting date
                    if (action.Order == 1)
                    {
                        //p.StartDate = System.DateTime.UtcNow;
                        PlanElementUtil.SetStartDateForProgramAttributes(request.ProgramId, request);
                    }

                    // get module reference
                    Module mod = PlanElementUtil.FindElementById(p.Modules, action.ModuleId);
                    // set to in progress
                    mod.ElementState = 4;
                    // set program to in progress
                    p.ElementState = 4;


                    //// create a responsibility chain to process each elemnt in the hierachy
                    ProgramPlanProcessor pChain = InitializeProgramChain();

                    //// process steps in action
                    action.Steps.ForEach(s => { pChain.ProcessWorkflow((IPlanElement)s, p, request.UserId, request.PatientId, action, request); });

                    //// process action
                    pChain.ProcessWorkflow((IPlanElement)action, p, request.UserId, request.PatientId, action, request);
                    
                    if (mod != null)
                    {
                        // set enabled status for action dependencies
                        PlanElementUtil.SetEnabledStatusByPrevious(mod.Actions);
                        // set enable/visibility of actions after action processing.
                        pChain.ProcessWorkflow((IPlanElement)mod, p, request.UserId, request.PatientId, action, request);
                        AddUniquePlanElementToProcessedList(mod);
                    }

                    // set module visibility for modules
                    PlanElementUtil.SetEnabledStatusByPrevious(p.Modules);

                    // evaluate program status
                    if (PlanElementUtil.IsProgramCompleted(p, request.UserId))
                    {
                        p.Completed = true;
                        pChain.ProcessWorkflow((IPlanElement)p, p, request.UserId, request.PatientId, action, request);
                    }
                    AddUniquePlanElementToProcessedList(action);
                }
                else
                {
                    // need to update this on the p level.
                    action.ElementState = 4; // in progress
                }

                AddUniquePlanElementToProcessedList(p);

                // save
                PlanElementEndpointUtil.SaveAction(request, action.Id, p);

                // create element changed lists 
                PlanElementUtil.HydratePlanElementLists(ProcessedElements, response);

                //response.Program = p;
                response.RelatedChanges = RelatedChanges;
                response.PatientId = request.PatientId;
                response.Version = request.Version;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:InterviewManager:ProcessActionResults()::" + ex.Message, ex.InnerException);
            }
        }

        public PostSaveActionResponse SaveActionResults(PostSaveActionRequest request)
        {
            try
            {
                // need to refactor this into a mediator.
                RelatedChanges.Clear();
                PostSaveActionResponse response = new PostSaveActionResponse();

                Program p = PlanElementEndpointUtil.RequestPatientProgramDetail(request);
                Actions action = request.Action;
                NGUtils.UpdateProgramAction(action, p);

                // set elementstates to in progress
                Module mod = PlanElementUtil.FindElementById(p.Modules, action.ModuleId);
                mod.ElementState = 4;
                p.ElementState = 4;
                AddUniquePlanElementToProcessedList(mod);

                // save
                PlanElementEndpointUtil.SaveAction(request, action.Id, p);

                //response.Program = p;
                response.Saved = true;
                response.RelatedChanges = RelatedChanges;
                response.PatientId = request.PatientId;
                response.Version = request.Version;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:InterviewManager:ProcessActionResults()::" + ex.Message, ex.InnerException);
            }
        }

        private ProgramPlanProcessor InitializeProgramChain()
        {
            ProgramPlanProcessor progProc = new ProgramPlanProcessor();
            ModulePlanProcessor modProc = new ModulePlanProcessor();
            ActionPlanProcessor actProc = new ActionPlanProcessor();
            StepPlanProcessor stepProc = new StepPlanProcessor();
            stepProc._spawnEvent += stepProc__spawnEvent;
            stepProc._processedElementEvent += Proc__processedIdEvent;
            PlanElementUtil._processedElementEvent += PlanElementUtil__processedElementEvent;
            progProc.Successor = modProc;
            modProc.Successor = actProc;
            actProc.Successor = stepProc;

            return progProc;
        }

        void PlanElementUtil__processedElementEvent(ProcessElementEventArgs e)
        {
            AddUniquePlanElementToProcessedList(e.PlanElement);
        }

        void Proc__processedIdEvent(object s, ProcessElementEventArgs e)
        {
            AddUniquePlanElementToProcessedList(e.PlanElement);
        }

        private void AddUniquePlanElementToProcessedList(object e)
        {
            try
            {
                if (!ProcessedElements.Contains(e))
                    ProcessedElements.Add(e);
            }
            catch (Exception ex)
            {
                throw new Exception("AD:InterviewManager:AddUniquePlanElementToProcessedList()::" + ex.Message, ex.InnerException);
            }
        }

        void stepProc__spawnEvent(object s, SpawnEventArgs e)
        {
            RelatedChanges.Add(e.Name);
        }
    }
}
