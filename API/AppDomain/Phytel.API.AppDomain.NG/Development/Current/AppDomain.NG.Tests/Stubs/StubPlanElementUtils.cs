﻿using Phytel.API.AppDomain.NG.DTO;
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

        public void FindIdInActions(DTO.Program program, string p, DTO.Module m)
        {
            try
            {
                if (m.Actions != null)
                {
                    foreach (Actions a in m.Actions)
                    {
                        if (a.Id.Equals(p))
                        {
                            SetInitialProperties(program.AssignToId, a);
                            OnProcessIdEvent(a);
                        }
                        else
                        {
                            FindIdInSteps(program, p, a);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:FindIdInActions()::" + ex.Message, ex.InnerException);
            }
        }

        public void FindIdInSteps(DTO.Program program, string p, DTO.Actions a)
        {
            try
            {
                if (a.Steps != null)
                {
                    foreach (Step s in a.Steps)
                    {
                        if (s.Id.Equals(p))
                        {
                            SetInitialProperties(program.AssignToId, s);
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

        public void HydratePlanElementLists(List<object> processedElements, DTO.PostProcessActionResponse response)
        {
            try
            {
                if (processedElements != null && processedElements.Count > 0)
                {
                    response.PlanElems = new PlanElements();

                    foreach (Object obj in processedElements)
                    {
                        if (obj.GetType() == typeof(DTO.Program))
                        {
                            if (!response.PlanElems.Programs.Contains(obj))
                            {
                                DTO.Program p = CloneProgram((DTO.Program)obj);
                                response.PlanElems.Programs.Add(p);
                            }
                        }
                        else if (obj.GetType() == typeof(Module))
                        {
                            if (!response.PlanElems.Modules.Contains(obj))
                            {
                                Module m = CloneModule((Module)obj);
                                response.PlanElems.Modules.Add(m);
                            }
                        }
                        else if (obj.GetType() == typeof(Actions))
                        {
                            if (!response.PlanElems.Actions.Contains(obj))
                            {
                                Actions a = CloneAction((Actions)obj);
                                response.PlanElems.Actions.Add(a);
                            }
                        }
                        else if (obj.GetType() == typeof(Step))
                        {
                            if (!response.PlanElems.Steps.Contains(obj))
                            {
                                Step s = CloneStep((Step)obj);
                                response.PlanElems.Steps.Add(s);
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
            return true;
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
            throw new NotImplementedException();
        }

        public void RegisterCohortPatientViewProblemToPatient(string problemId, string patientId, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
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
                        SetInitialProperties(program.AssignToId, m);
                        var list = m.Actions.Where(a => a.Enabled == true
                                                        && a.ElementState != (int)ElementState.Completed
                                                        && a.ElementState != (int)ElementState.InProgress); // create rule specification

                        list.ForEach(a => { if (a.Enabled) SetInitialProperties(program.AssignToId, a); });
                        OnProcessIdEvent(m);
                    }
                    else
                    {
                        FindIdInActions(program, p, m);
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

        public void SetInitialProperties(string assignToId, DTO.IPlanElement m)
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
            throw new NotImplementedException();
        }

        public void SetEnabledStatusByPrevious<T>(List<T> planElements, string assignToId, bool pEnabled)
        {
        }


        public void SetInitialActions(object x, string assignToId)
        {
            throw new NotImplementedException();
        }


        Actions IPlanElementUtils.CloneAction(Actions md)
        {
            throw new NotImplementedException();
        }


        public bool UpdatePlanElementAttributes(DTO.Program pg, PlanElement planElement, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
