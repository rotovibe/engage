using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.AppDomain.NG.PlanElementStrategy;
using Phytel.API.AppDomain.NG.Programs;
using Phytel.API.AppDomain.NG.Specifications;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.WebHost.Endpoints;
using Program = Phytel.API.AppDomain.NG.DTO.Program;

namespace Phytel.API.AppDomain.NG
{
    public class PlanManager : ManagerBase, IPlanManager
    {
        public IPlanElementUtils PEUtils { get; set; }
        public IEndpointUtils EndPointUtils { get; set; }
        public List<string> RelatedChanges { get; set; }
        public List<object> ProcessedElements { get; set; }
        public IsInitialActionSpecification<Program> IsInitialAction { get; set; }
        public IStepPlanProcessor StepProcessor { get; set; }

        public PlanManager()
        {
            RelatedChanges = new List<string>();
            ProcessedElements = new List<object>();
            IsInitialAction = new IsInitialActionSpecification<Program>();

            if (AppHostBase.Instance != null)
                AppHostBase.Instance.Container.AutoWire(this);
        }

        public PostProcessActionResponse ProcessActionResults(PostProcessActionRequest request)
        {
            try
            {
                // need to refactor this into a mediator.
                RelatedChanges.Clear();
                ProcessedElements.Clear();
                PostProcessActionResponse response = new PostProcessActionResponse();
                response.PlanElems = new PlanElements();

                Program p = EndPointUtils.RequestPatientProgramDetail(request);

                Actions action = request.Action;

                if (action.Completed)
                {
                    // 1) pre-process
                    // set program starting date
                    if (IsInitialAction.IsSatisfiedBy(p))
                    {
                        p.AttrStartDate = DateTime.UtcNow;
                        //PlanElementUtil.SetStartDateForProgramAttributes(request.ProgramId, request);
                    }

                    // get module reference
                    Module mod = PEUtils.FindElementById(p.Modules, action.ModuleId);

                    // set module to in progress
                    if (mod.ElementState == (int)ElementState.NotStarted) //!= 4
                    {
                        mod.ElementState = (int)ElementState.InProgress; //4;
                        mod.StateUpdatedOn = DateTime.UtcNow;
                    }

                    // 2) set in progress state
                    //new ResponseSpawnAllowed<Step>().IsSatisfiedBy(s)
                    if (PEUtils.IsActionInitial(p))
                    //if (new IsActionInitialSpecification<Program>().IsSatisfiedBy(p))
                    {
                        // set program to in progress
                        if (p.ElementState == (int)ElementState.NotStarted)
                        {
                            p.ElementState = (int)ElementState.InProgress; //4;
                            p.StateUpdatedOn = DateTime.UtcNow;
                        }
                    }

                    // 3) set action state to completed
                    action.ElementState = (int)ElementState.Completed;
                    action.StateUpdatedOn = DateTime.UtcNow;

                    // 4) insert action update
                    var act = NGUtils.UpdateProgramAction(action, p);

                    //// create a responsibility chain to process each elemnt in the hierachy
                    ProgramPlanProcessor pChain = InitializeProgramChain();

                    // 5)  process steps
                    action.Steps.ForEach(
                        s =>
                            pChain.ProcessWorkflow((IPlanElement)s, p, request.UserId, request.PatientId, action,
                                request));

                    // 6) process action
                    pChain.ProcessWorkflow((IPlanElement)action, p, request.UserId, request.PatientId, action, request);

                    // 7) process module
                    if (mod != null)
                    {
                        // set enabled status for action dependencies
                        PEUtils.SetEnabledStatusByPrevious(mod.Actions, mod.AssignToId, mod.Enabled);
                        // set enable/visibility of actions after action processing.
                        pChain.ProcessWorkflow((IPlanElement)mod, p, request.UserId, request.PatientId, action, request);
                        AddUniquePlanElementToProcessedList(mod);
                    }

                    // post processing //
                    // 8) set module visibility for modules
                    PEUtils.SetEnabledStatusByPrevious(p.Modules, p.AssignToId, p.Enabled);

                    // 9) evaluate program status
                    if (PEUtils.IsProgramCompleted(p, request.UserId))
                    {
                        p.Completed = true;
                        pChain.ProcessWorkflow((IPlanElement)p, p, request.UserId, request.PatientId, action, request);
                    }

                    // 10) register changed action 
                    AddUniquePlanElementToProcessedList(action);
                }
                else
                {
                    // need to update this on the p level.
                    action.ElementState = (int)ElementState.InProgress; //4; // in progress
                }

                AddUniquePlanElementToProcessedList(p);

                // save
                var pdetail = EndPointUtils.SaveAction(request, action.Id, p, false);

                // create element changed lists 
                PEUtils.HydratePlanElementLists(ProcessedElements, response.PlanElems);

                response.PlanElems.Attributes = PEUtils.GetAttributes(pdetail.Attributes);
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

                Program p = EndPointUtils.RequestPatientProgramDetail(request);
                Actions action = request.Action;

                // set elementstates to in progress
                Module mod = PEUtils.FindElementById(p.Modules, action.ModuleId);
                
                // set to in progress
                if (mod.ElementState == (int)ElementState.NotStarted) //!= 4
                {
                    mod.ElementState = (int)ElementState.InProgress; //4;
                    mod.StateUpdatedOn = DateTime.UtcNow;
                }
                
                if (PEUtils.IsActionInitial(p))
                {
                    // set program to in progress
                    if (p.ElementState == (int)ElementState.NotStarted)
                    {
                        p.ElementState = (int)ElementState.InProgress; //4;
                        p.StateUpdatedOn = System.DateTime.UtcNow;
                    }
                }

                NGUtils.UpdateProgramAction(action, p);

                AddUniquePlanElementToProcessedList(mod);

                // save
                EndPointUtils.SaveAction(request, action.Id, p, false);

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

        public PostRepeatActionResponse RepeatAction(PostRepeatActionRequest request)
        {
            try
            {
                RelatedChanges.Clear();
                ProcessedElements.Clear();
                var response = new PostRepeatActionResponse();
                response.PlanElems = new PlanElements();

                var p = EndPointUtils.RequestPatientProgramDetail(request);             

                // get module reference
                Module mod = PEUtils.FindElementById(p.Modules, request.Action.ModuleId);
                IPlanElementStrategy strategy = new StatePlanElementStrategy(new SetModulePropertiesForRepeat(mod));                

                // clone and insert new action
                var cAction = PEUtils.CloneRepeatAction(request.Action, mod.AssignToId);
                cAction.CompletedBy = "CM";
                AddUniquePlanElementToProcessedList(cAction);

                // insert action update
                var action = NGUtils.UpdateProgramAction(request.Action, p);   
                AddUniquePlanElementToProcessedList(request.Action);

                // insert action into module and update references
                ReplaceActionReferenceInModule(request.Action.Id, cAction, mod);
                strategy.Evoke();
                AddUniquePlanElementToProcessedList(mod);

                // save
                var pdetail = EndPointUtils.SaveAction(request, cAction.Id, p, true);

                // create element changed lists 
                PEUtils.HydratePlanElementLists(ProcessedElements, response.PlanElems);

                response.PlanElems.Attributes = PEUtils.GetAttributes(pdetail.Attributes);
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

        public void ReplaceActionReferenceInModule(string oActionId, Actions newAction, Module mod)
        {
            try
            {
                var cActionId = newAction.Id;
                // replace all references to old action
                mod.Actions.ForEach(a =>
                {
                    //previous
                    if (!string.IsNullOrEmpty(a.Previous))
                        if (a.Previous.Equals(oActionId)) a.Previous = cActionId;

                    //next
                    if (!string.IsNullOrEmpty(a.Next))
                        if (a.Next.Equals(oActionId)) a.Next = cActionId;

                    // spawnelements
                    ReplaceSpawnElementReferences(a.SpawnElement, oActionId, cActionId);

                    // steps
                    a.Steps.ForEach(stp =>
                    {
                        // step spawnelements
                        ReplaceSpawnElementReferences(stp.SpawnElement, oActionId, cActionId);

                        // responses
                        if (stp.Responses != null)
                            stp.Responses.ForEach(
                                rsp => ReplaceSpawnElementReferences(rsp.SpawnElement, oActionId, cActionId));
                    });
                });

                // add new action
                if (!mod.Actions.Contains(newAction))
                    mod.Actions.Add(newAction);
            }
            catch (Exception ex)
            {
                throw new Exception("AD:InterviewManager:ReplaceActionReferenceInModule()::" + ex.Message,
                    ex.InnerException);
            }
        }

        public void ReplaceSpawnElementReferences(List<SpawnElement> list, string oActionId, string cActionId)
        {
            try
            {
                if (list == null || list.Count == 0) return;

                list.ForEach(se =>
                {
                    if (se.ElementId.Equals(oActionId))
                        se.ElementId = cActionId;
                });
            }
            catch (Exception ex)
            {
                throw new Exception("AD:InterviewManager:ReplaceSpawnElementReferences()::" + ex.Message,
                    ex.InnerException);
            }
        }

        private ProgramPlanProcessor InitializeProgramChain()
        {
            ProgramPlanProcessor progProc = new ProgramPlanProcessor();
            ModulePlanProcessor modProc = new ModulePlanProcessor();
            ActionPlanProcessor actProc = new ActionPlanProcessor();
            IStepPlanProcessor stepProc = StepProcessor;
            
            // initialize all spawn events.
            stepProc.SpawnEvent += stepProc__spawnEvent;
            modProc.SpawnEvent += stepProc__spawnEvent;
            actProc.SpawnEvent += stepProc__spawnEvent;
            progProc.SpawnEvent += stepProc__spawnEvent;
            
            stepProc.ProcessedElementEvent += Proc__processedIdEvent;
            PEUtils._processedElementEvent += PlanElementUtil__processedElementEvent;
            progProc.Successor = modProc;
            modProc.Successor = actProc;
            actProc.Successor = (PlanProcessor)stepProc;

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

        private void stepProc__spawnEvent(object s, SpawnEventArgs e)
        {
            try
            {
                // add goal/task/intervention payload to processlist
                if (e.Tags != null)
                {
                    if (e.Tags.Count > 0)
                    {
                        e.Tags.ForEach(t =>
                        {
                            if (t != null)
                            {
                                if (t.GetType() == typeof (List<object>))
                                    ((List<object>) t).ForEach(r =>
                                    {
                                        if (r != null) AddUniquePlanElementToProcessedList(r);
                                    });
                                else
                                    AddUniquePlanElementToProcessedList(t);
                            }
                        });
                    }
                }
                RelatedChanges.Add(e.Name);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "AD:InterviewManager:stepProc__spawnEvent():: count: " + e.Tags.Count + " " + ex.Message,
                    ex.InnerException);
            }
        }
    }
}
