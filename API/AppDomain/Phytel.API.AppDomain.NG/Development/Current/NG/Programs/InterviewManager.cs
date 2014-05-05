using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.ServiceClient.Web;
using Phytel.API.AppDomain.NG.PlanCOR;
using ServiceStack.Service;
using DD = Phytel.API.DataDomain.Program.DTO;
using System.Configuration;
using Phytel.API.AppDomain.NG.Programs;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.AppDomain.NG.Specifications;

namespace Phytel.API.AppDomain.NG
{
    public class PlanManager : ManagerBase, IPlanManager
    {
        public IPlanElementUtils PEUtils { get; set; }
        public List<string> RelatedChanges { get; set; }
        public List<object> ProcessedElements { get; set; }
        public IsInitialActionSpecification<Program> IsInitialAction { get; set; }

        public PlanManager()
        {
            RelatedChanges = new List<string>();
            ProcessedElements = new List<object>();
            IsInitialAction = new IsInitialActionSpecification<Program>();
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

                if (action.Completed)
                {
                    // pre-process
                    // set program starting date
                    if (IsInitialAction.IsSatisfiedBy(p))
                    {
                        p.AttrStartDate = System.DateTime.UtcNow;
                        //PlanElementUtil.SetStartDateForProgramAttributes(request.ProgramId, request);
                    }

                    // get module reference
                    Module mod = PlanElementUtil.FindElementById(p.Modules, action.ModuleId);
                    // set to in progress
                    mod.ElementState = 4;
                    
                    // set in progress state
                    //new ResponseSpawnAllowed<Step>().IsSatisfiedBy(s)
                    if (PlanElementUtil.IsActionInitial(p))
                    //if (new IsActionInitialSpecification<Program>().IsSatisfiedBy(p))
                    {
                        // set program to in progress
                        p.ElementState = 4;
                    }

                    // insert action update
                    NGUtils.UpdateProgramAction(action, p);

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
                DD.ProgramDetail pdetail = PlanElementEndpointUtil.SaveAction(request, action.Id, p);

                // create element changed lists 
                PlanElementUtil.HydratePlanElementLists(ProcessedElements, response);

                response.PlanElems.Attributes = PlanElementUtil.GetAttributes(pdetail.Attributes);
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

                // set elementstates to in progress
                Module mod = PlanElementUtil.FindElementById(p.Modules, action.ModuleId);
                // set to in progress
                mod.ElementState = 4;
                
                if (PlanElementUtil.IsActionInitial(p))
                //if (new IsActionInitialSpecification<Program>().IsSatisfiedBy(p))
                {
                    // set program to in progress
                    p.ElementState = 4;
                }
                
                NGUtils.UpdateProgramAction(action, p);

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
