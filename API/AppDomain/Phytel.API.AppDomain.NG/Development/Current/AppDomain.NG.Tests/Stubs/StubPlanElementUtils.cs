using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Program.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Common.Extensions;

namespace Phytel.API.AppDomain.NG.Test.Stubs
{
    public class StubPlanElementUtils : IPlanElementUtils
    {
        public event ProcessedElementInUtilEventHandlers _processedElementEvent;

        public DTO.PlanElement ActivatePlanElement(string p, DTO.Program program)
        {
            throw new NotImplementedException();
        }

        public void DisableCompleteButtonForAction(List<DTO.Step> list)
        {
        }

        public T FindElementById<T>(List<T> list, string id)
        {
            return list[0];
        }

        public void FindSpawnIdInActions(DTO.Program program, string p, DTO.Module m)
        {
            try
            {
                if (m.Actions != null)
                {
                    foreach (Actions a in m.Actions)
                    {
                        if (a.Id.Equals(p))
                        {
                            SetInitialProperties(program.AssignToId, a, false);
                            OnProcessIdEvent(a);
                        }
                        else
                        {
                            FindSpawnIdInSteps(program, p, a);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:FindIdInActions()::" + ex.Message, ex.InnerException);
            }
        }

        public void FindSpawnIdInSteps(DTO.Program program, string p, DTO.Actions a)
        {
            try
            {
                if (a.Steps != null)
                {
                    foreach (Step s in a.Steps)
                    {
                        if (s.Id.Equals(p))
                        {
                            SetInitialProperties(program.AssignToId, s, false);
                            OnProcessIdEvent(s);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:FindIdInSteps()::" + ex.Message, ex.InnerException);
            }
        }

        public Phytel.API.DataDomain.Patient.DTO.CohortPatientViewData GetCohortPatientViewRecord(string patientId, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.Actions GetProcessingAction(List<DTO.Module> list, string actionId)
        {
            throw new NotImplementedException();
        }

        public void HydratePlanElementLists(List<object> processedElements, DTO.PlanElements planElems)
        {
            try
            {
                if (processedElements != null && processedElements.Count > 0)
                {
                    planElems = new PlanElements();

                    foreach (Object obj in processedElements)
                    {
                        if (obj.GetType() == typeof(DTO.Program))
                        {
                            if (!planElems.Programs.Contains(obj))
                            {
                                DTO.Program p = CloneProgram((DTO.Program)obj);
                                planElems.Programs.Add(p);
                            }
                        }
                        else if (obj.GetType() == typeof(Module))
                        {
                            if (!planElems.Modules.Contains(obj))
                            {
                                Module m = CloneModule((Module)obj);
                                planElems.Modules.Add(m);
                            }
                        }
                        else if (obj.GetType() == typeof(Actions))
                        {
                            if (!planElems.Actions.Contains(obj))
                            {
                                Actions a = CloneAction((Actions)obj);
                                planElems.Actions.Add(a);
                            }
                        }
                        else if (obj.GetType() == typeof(Step))
                        {
                            if (!planElems.Steps.Contains(obj))
                            {
                                Step s = CloneStep((Step)obj);
                                planElems.Steps.Add(s);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:StubPlanElementUtil:HydratePlanElementLists()::" + ex.Message, ex.InnerException);
            }
        }

        public Module CloneModule(Module md)
        {
            try
            {
                Module m = new Module
                {
                    AssignById = md.AssignById,
                    AssignToId = md.AssignToId, // 950
                    AssignDate = md.AssignDate,
                    Completed = md.Completed,
                    CompletedBy = md.CompletedBy,
                    DateCompleted = md.DateCompleted,
                    Description = md.Description,
                    ElementState = md.ElementState,
                    StateUpdatedOn = md.StateUpdatedOn,
                    Enabled = md.Enabled,
                    Id = md.Id,
                    Name = md.Name,
                    Next = md.Next,
                    Objectives = md.Objectives,
                    Order = md.Order,
                    Previous = md.Previous,
                    ProgramId = md.ProgramId,
                    SourceId = md.SourceId,
                    SpawnElement = md.SpawnElement,
                    Status = md.Status,
                    Text = md.Text,
                    Actions = new List<Actions>()
                };
                return m;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:CloneModule()::" + ex.Message, ex.InnerException);
            }
        }

        private Actions CloneAction(Actions ac)
        {
            try
            {
                Actions a = new Actions
                {
                    AssignById = ac.AssignById,
                    AssignDate = ac.AssignDate,
                    Completed = ac.Completed,
                    CompletedBy = ac.CompletedBy,
                    DateCompleted = ac.DateCompleted,
                    Description = ac.Description,
                    ElementState = ac.ElementState,
                    StateUpdatedOn = ac.StateUpdatedOn,
                    Enabled = ac.Enabled,
                    Id = ac.Id,
                    ModuleId = ac.ModuleId,
                    Name = ac.Name,
                    Next = ac.Next,
                    Objectives = ac.Objectives,
                    Order = ac.Order,
                    Previous = ac.Previous,
                    SourceId = ac.SourceId,
                    SpawnElement = ac.SpawnElement,
                    Status = ac.Status,
                    Text = ac.Text,
                    Steps = new List<Step>()
                };
                return a;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:CloneAction()::" + ex.Message, ex.InnerException);
            }
        }

        private Step CloneStep(Step st)
        {
            try
            {
                Step s = new Step
                {
                    ActionId = st.ActionId,
                    AssignById = st.AssignById,
                    AssignDate = st.AssignDate,
                    Completed = st.Completed,
                    CompletedBy = st.CompletedBy,
                    ControlType = st.ControlType,
                    DateCompleted = st.DateCompleted,
                    Description = st.Description,
                    ElementState = st.ElementState,
                    StateUpdatedOn = st.StateUpdatedOn,
                    Enabled = st.Enabled,
                    Ex = st.Ex,
                    Header = st.Header,
                    Id = st.Id,
                    IncludeTime = st.IncludeTime,
                    Next = st.Next,
                    Notes = st.Notes,
                    Order = st.Order,
                    Previous = st.Previous,
                    Question = st.Question,
                    Responses = st.Responses,
                    SelectedResponseId = st.SelectedResponseId,
                    SelectType = st.SelectType,
                    SourceId = st.SourceId,
                    SpawnElement = st.SpawnElement,
                    Status = st.Status,
                    StepTypeId = st.StepTypeId,
                    Text = st.Text,
                    Title = st.Title
                };
                return s;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:CloneStep()::" + ex.Message, ex.InnerException);
            }
        }

        public Phytel.API.AppDomain.NG.DTO.Program CloneProgram(Phytel.API.AppDomain.NG.DTO.Program pr)
        {
            try
            {
                Phytel.API.AppDomain.NG.DTO.Program p = new Phytel.API.AppDomain.NG.DTO.Program
                {
                    AssignById = pr.AssignById,
                    AssignDate = pr.AssignDate,
                    AssignToId = pr.AssignToId,
                    Client = pr.Client,
                    Completed = pr.Completed,
                    CompletedBy = pr.CompletedBy,
                    Previous = pr.Previous,
                    ContractProgramId = pr.ContractProgramId,
                    DateCompleted = pr.DateCompleted,
                    Description = pr.Description,
                    //DidNotEnrollReason = pr.DidNotEnrollReason,
                    //DisEnrollReason = pr.DisEnrollReason,
                    ElementState = pr.ElementState,
                    StateUpdatedOn = pr.StateUpdatedOn,
                    Eligibility = pr.Eligibility,
                    EligibilityEndDate = pr.EligibilityEndDate,
                    //EligibilityOverride = pr.EligibilityOverride,
                    EligibilityRequirements = pr.EligibilityRequirements,
                    EligibilityStartDate = pr.EligibilityStartDate,
                    Enabled = pr.Enabled,
                    StartDate = pr.StartDate,
                    EndDate = pr.EndDate,
                    AttrStartDate = pr.AttrStartDate,
                    AttrEndDate = pr.AttrEndDate,
                    AuthoredBy = pr.AuthoredBy,
                    TemplateName = pr.TemplateName,
                    TemplateVersion = pr.TemplateVersion,
                    ProgramVersion = pr.ProgramVersion,
                    ProgramVersionUpdatedOn = pr.ProgramVersionUpdatedOn,
                    //Enrollment = pr.Enrollment,
                    //GraduatedFlag = pr.GraduatedFlag,
                    Id = pr.Id,
                    //IneligibleReason = pr.IneligibleReason,
                    Name = pr.Name,
                    Next = pr.Next,
                    Objectives = pr.Objectives,
                    //OptOut = pr.OptOut,
                    //OptOutDate = pr.OptOutDate,
                    //OptOutReason = pr.OptOutReason,
                    Order = pr.Order,
                    //OverrideReason = pr.OverrideReason,
                    PatientId = pr.PatientId,
                    ProgramState = pr.ProgramState,
                    //RemovedReason = pr.RemovedReason,
                    ShortName = pr.ShortName,
                    SourceId = pr.SourceId,
                    SpawnElement = pr.SpawnElement,
                    Status = pr.Status,
                    Text = pr.Text,
                    Version = pr.Version,
                    Modules = new List<Module>()
                };
                return p;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:CloneProgram()::" + ex.Message, ex.InnerException);
            }
        }

        public DTO.PlanElement InitializePlanElementSettings(DTO.PlanElement pe, DTO.PlanElement p)
        {
            throw new NotImplementedException();
        }

        public bool IsActionInitial(DTO.Program p)
        {
            try
            {
                bool result = true;
                if (p.Modules != null)
                {
                    foreach (Module m in p.Modules)
                    {
                        foreach (Actions a in m.Actions)
                        {
                            if (a.ElementState == (int)ElementState.InProgress ||
                                a.ElementState == (int)ElementState.Completed)
                            {
                                result = false;
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:IsActionInitial()::" + ex.Message, ex.InnerException);
            }
        }

        public bool IsProgramCompleted(DTO.Program p, string userId)
        {
            return true;
        }

        public bool ModifyProgramAttributePropertiesForUpdate(Phytel.API.DataDomain.Program.DTO.ProgramAttributeData pAtt, Phytel.API.DataDomain.Program.DTO.ProgramAttributeData _pAtt)
        {
            throw new NotImplementedException();
        }

        public void OnProcessIdEvent(object pe)
        {
        }

        public void RegisterCohortPatientViewProblemToPatient(string problemId, string patientId, Interface.IAppDomainRequest request)
        {
        }

        public bool ResponseSpawnAllowed(DTO.Step s, DTO.Response r)
        {
            throw new NotImplementedException();
        }

        public void SaveReportingAttributes(Phytel.API.DataDomain.Program.DTO.ProgramAttributeData _programAttributes, Interface.IAppDomainRequest request)
        {
        }

        public bool SetCompletionStatus<T>(List<T> list)
        {
            return true;
        }

        public void SetElementEnabledState(string p, DTO.Program program)
        {
            try
            {
                foreach (Module m in program.Modules)
                {
                    if (m.Id.Equals(p))
                    {
                        SetInitialProperties(program.AssignToId, m, false);
                        var list = m.Actions.Where(a => a.Enabled == true
                                                        && a.ElementState != (int)ElementState.Completed
                                                        && a.ElementState != (int)ElementState.InProgress); // create rule specification

                        list.ForEach(a => { if (a.Enabled) SetInitialProperties(program.AssignToId, a, false); });
                        OnProcessIdEvent(m);
                    }
                    else
                    {
                        FindSpawnIdInActions(program, p, m);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:SetElementEnabledState()::" + ex.Message, ex.InnerException);
            }
        }

        public void SetEnabledState<T>(List<T> list, T x)
        {
            throw new NotImplementedException();
        }

        public void SetEnabledStatusByPrevious<T>(List<T> actions)
        {
            throw new NotImplementedException();
        }

        public void SetInitialProperties(string assignToId, IPlanElement m, bool dependent)
        {
            try
            {
                // NIGHT-835
                if (m.AssignDate == null)
                    m.AssignDate = System.DateTime.UtcNow;

                m.Enabled = true;
                m.StateUpdatedOn = System.DateTime.UtcNow;
                m.AssignToId = assignToId; // NIGHT-877
                m.ElementState = (int)ElementState.NotStarted; // 2;
                //m.AssignById = Constants.SystemContactId;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:SetInitialProperties()::" + ex.Message, ex.InnerException);
            }
        }

        public void SetProgramAttributes(DTO.SpawnElement r, DTO.Program program, string userId, Phytel.API.DataDomain.Program.DTO.ProgramAttributeData progAttr)
        {
            throw new NotImplementedException();
        }

        public void SetProgramInformation(Phytel.API.DataDomain.Program.DTO.ProgramAttributeData _programAttributes, DTO.Program p)
        {
            throw new NotImplementedException();
        }

        public void SetStartDateForProgramAttributes(string programId, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public void SpawnElementsInList(List<DTO.SpawnElement> list, DTO.Program program, string userId, Phytel.API.DataDomain.Program.DTO.ProgramAttributeData progAttr)
        {
            try
            {
                list.ForEach(r =>
                {
                    if (r.ElementType < 10)
                    {
                        SetElementEnabledState(r.ElementId, program);
                    }
                    else
                    {
                        SetProgramAttributes(r, program, userId, progAttr);
                    }
                });
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:SpawnElementsInList()::" + ex.Message, ex.InnerException);
            }
        }

        public DTO.ProgramAttribute GetAttributes(DataDomain.Program.DTO.ProgramAttributeData programAttributeData)
        {
            return new DTO.ProgramAttribute();
        }

        public PlanElement InitializePlanElementSettings(PlanElement pe, PlanElement p, DTO.Program program)
        {
            throw new NotImplementedException();
        }

        public void SetEnabledState<T>(List<T> list, T x, string assignToId, bool pEnabled)
        {
            try
            {
                if (list == null || x == null) return;

                var pe = ((IPlanElement)Convert.ChangeType(x, typeof(T)));
                if (string.IsNullOrEmpty(pe.Previous)) return;

                var prevElem = list.Find(r => ((IPlanElement)r).Id == pe.Previous);
                if (prevElem == null) return;

                if (((IPlanElement)prevElem).Completed != true)
                {
                    ((IPlanElement)x).Enabled = false;
                }
                else
                {
                    if (!((IPlanElement)x).Enabled)
                    {
                        ((IPlanElement)x).StateUpdatedOn = DateTime.UtcNow;
                        ((IPlanElement)x).Enabled = true;
                        if (((IPlanElement)x).AssignToId == null)
                            ((IPlanElement)x).AssignToId = assignToId;
                    }

                    // add this to a parent enabled specification
                    if (((IPlanElement)x).AssignDate == null)
                        ((IPlanElement)x).AssignDate = DateTime.UtcNow;

                    if (string.IsNullOrEmpty(((IPlanElement)x).AssignById))
                        ((IPlanElement)x).AssignById = DataDomain.Program.DTO.Constants.SystemContactId;

                    // only track elements who are enabled for now.
                    OnProcessIdEvent(Convert.ChangeType(x, typeof(T)));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:SetEnabledState()::" + ex.Message, ex.InnerException);
            }
        }

        public void SetEnabledStatusByPrevious<T>(List<T> planElements, string assignToId, bool pEnabled)
        {
            try
            {
                if (planElements != null)
                    planElements.ForEach(x =>
                    {
                        //var pe = ((IPlanElement) Convert.ChangeType(x, typeof (T)));
                        //var atVal = !string.IsNullOrEmpty(pe.AssignToId) ? pe.AssignToId : assignToId;

                        SetEnabledState(planElements, x, assignToId, pEnabled);

                        if (x.GetType() == typeof(Module))
                        {
                            SetInitialActions(x, null);
                        }
                    });
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:SetEnabledStatusByPrevious()::" + ex.Message, ex.InnerException);
            }
        }

        public void SetInitialActions(object x, string assignToId)
        {
            throw new NotImplementedException();
        }

        Actions IPlanElementUtils.CloneAction(Actions md)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePlanElementAttributes(DTO.Program pg, PlanElement planElement, string userId, PlanElements planElems)
        {
            throw new NotImplementedException();
        }

        public void ProcessPlanElementChanges(PlanElements planElems, PlanElement samplePe, PlanElement fPe, string userId)
        {
            throw new NotImplementedException();
        }


        public Actions CloneRepeatAction(Actions action, string assignedById)
        {
            throw new NotImplementedException();
        }


        public void SetInitialValues(string assignToId, IPlanElement pe)
        {
            throw new NotImplementedException();
        }
    }
}
