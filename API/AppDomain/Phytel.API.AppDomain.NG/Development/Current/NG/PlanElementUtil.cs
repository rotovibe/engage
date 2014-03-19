using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.AppDomain.NG.PlanSpecification;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using DD = Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.AppDomain.NG
{
    public delegate void ProcessedElementInUtilEventHandler(ProcessElementEventArgs e);

    public static class PlanElementUtil
    {
        public static event ProcessedElementInUtilEventHandler _processedElementEvent;

        private static void OnProcessIdEvent(object pe)
        {
            if (_processedElementEvent != null)
            {
                _processedElementEvent(new ProcessElementEventArgs { PlanElement = pe });
            }
        }

        /// <summary>
        /// Checks to see if completion status count is equal to the list count. Return true if equal.
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="list">PlanElement list</param>
        /// <returns></returns>
        public static bool SetCompletionStatus<T>(List<T> list)
        {
            try
            {
                bool result = false;
                int completed = list.FindAll(new Completed<T>().IsSatisfiedBy).Count();
                int count = list.Count;

                if (completed.Equals(count))
                {
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:SetCompletionStatus()::" + ex.Message, ex.InnerException);
            }
        }

        public static T FindElementById<T>(List<T> list, string id)
        {
            try
            {
                var mod = list.Where(r => ((IPlanElement)Convert.ChangeType(r, typeof(T))).Id == id).FirstOrDefault();
                return mod;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:FindElementById()::" + ex.Message, ex.InnerException);
            }
        }

        public static void SetEnabledStatusByPrevious<T>(List<T> actions)
        {
            try
            {
                if (actions != null)
                {
                    actions.ForEach(x =>
                    {
                        SetEnabledState(actions, x);
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:SetEnabledStatusByPrevious()::" + ex.Message, ex.InnerException);
            }
        }

        public static void SetEnabledState<T>(List<T> list, T x)
        {
            try
            {
                if (list != null && x != null)
                {
                    IPlanElement pe = ((IPlanElement)Convert.ChangeType(x, typeof(T)));

                    if (!string.IsNullOrEmpty(pe.Previous))
                    {
                        var prevElem = list.Find(r => ((IPlanElement)r).Id == pe.Previous);
                        if (prevElem != null)
                        {
                            if (((IPlanElement)prevElem).Completed != true)
                            {
                                ((IPlanElement)x).Enabled = false;
                            }
                            else
                            {
                                ((IPlanElement)x).Enabled = true;
                                // only track elements who are enabled for now.
                                OnProcessIdEvent(Convert.ChangeType(x, typeof(T)));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:SetEnabledState()::" + ex.Message, ex.InnerException);
            }
        }

        internal static void DisableCompleteButtonForAction(List<Step> list)
        {
            try
            {
                if (list != null)
                {
                    list.ForEach(s =>
                    {
                        if (s.StepTypeId.Equals(7))
                        {
                            s.Enabled = false;
                            OnProcessIdEvent(s);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:DisableCompleteButtonForAction()::" + ex.Message, ex.InnerException);
            }
        }

        internal static void SpawnElementsInList(List<SpawnElement> list, Program program, string userId, DD.ProgramAttribute progAttr)
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
                        SetProgramAttributes(r, program, userId, progAttr );
                    }
                });
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:SpawnElementsInList()::" + ex.Message, ex.InnerException);
            }
        }

        public static void SetProgramAttributes(SpawnElement r, Program program, string userId, DD.ProgramAttribute progAttr)
        {
            try
            {
                if (r.ElementType.Equals(10))
                {
                    if (r.Tag == null)
                        throw new ArgumentException("Cannot set attribute of type " + r.ElementType + ". Tag value is null.");

                    // eligibility series
                    try
                    {
                        progAttr.Eligibility = (!string.IsNullOrEmpty(r.Tag)) ? Convert.ToInt32(r.Tag) : 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("AD:SetProgramAttributes()::Eligibility" + ex.Message, ex.InnerException);
                    }

                    int state; // no = 1, yes = 2
                    bool isNum = int.TryParse(r.Tag, out state);
                    if (isNum)
                    {
                        // program is closed due to ineligibility
                        if (state.Equals(1)) // not eligible
                        {
                            program.ElementState = 5;
                            progAttr.Eligibility = 1;
                            progAttr.EndDate = System.DateTime.UtcNow;
                        }
                        else if (state.Equals(2)) // eligible
                        {
                            program.ElementState = 4;
                            progAttr.Eligibility = 2;
                        }
                    }
                }
                else if (r.ElementType.Equals(11))
                {
                    // eligibility reason
                    if (r.Tag == null)
                        throw new ArgumentException("Cannot set attribute of type " + r.ElementType + ". Tag value is null.");

                    progAttr.IneligibleReason = (!string.IsNullOrEmpty(r.Tag))?r.Tag : null;
                }
                else if (r.ElementType.Equals(14))
                {
                    progAttr.EndDate = System.DateTime.UtcNow;
                }
                else if (r.ElementType.Equals(15))
                {
                    if (r.Tag == null)
                        throw new ArgumentException("Cannot set attribute of type " + r.ElementType + ". Tag value is null.");

                    try
                    {
                        progAttr.Enrollment = (!string.IsNullOrEmpty(r.Tag)) ? Convert.ToInt32(r.Tag) : 0;
                    }
                    catch(Exception ex)
                    {
                        throw new Exception("AD:SetProgramAttributes()::Enrollment" + ex.Message, ex.InnerException);
                    }
                }
                else if (r.ElementType.Equals(16))
                {
                    if (r.Tag == null)
                        throw new ArgumentException("Cannot set attribute of type " + r.ElementType + ". Tag value is null.");

                    try
                    {
                        progAttr.OptOut = (!string.IsNullOrEmpty(r.Tag)) ? Convert.ToBoolean(r.Tag) : false;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("AD:SetProgramAttributes()::OptOut" + ex.Message, ex.InnerException);
                    }
                }
                else if (r.ElementType.Equals(17))
                {
                    // do something with opt out
                    if (r.Tag == null)
                        throw new ArgumentException("Cannot set attribute of type " + r.ElementType + ". Tag value is null.");

                    progAttr.OptOutReason = (!string.IsNullOrEmpty(r.Tag)) ? r.Tag : null;
                }
                else if (r.ElementType.Equals(18))
                {
                    // do something with opt out 
                    progAttr.OptOutDate = System.DateTime.UtcNow;
                }
                else if (r.ElementType.Equals(19))
                {
                    if (r.Tag == null)
                        throw new ArgumentException("Cannot set attribute of type " + r.ElementType + ". Tag value is null.");

                    try
                    {
                        progAttr.GraduatedFlag = (!string.IsNullOrEmpty(r.Tag)) ? Convert.ToInt32(r.Tag) : 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("AD:SetProgramAttributes()::GraduatedFlag" + ex.Message, ex.InnerException);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:SetProgramAttributes()::" + ex.Message, ex.InnerException);
            }
        }

        private static void SetElementEnabledState(string p, Program program)
        {
            try
            {
                foreach (Module m in program.Modules)
                {
                    if (m.Id.ToString().Equals(p))
                    {
                        SetInitialProperties(m);
                        OnProcessIdEvent(m);
                    }
                    else
                    {
                        FindIdInActions(p, m);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:SetEnabledStateRecursion()::" + ex.Message, ex.InnerException);
            }
        }

        private static void FindIdInActions(string p, Module m)
        {
            try
            {
                if (m.Actions != null)
                {
                    foreach (Actions a in m.Actions)
                    {
                        if (a.Id.ToString().Equals(p))
                        {
                            SetInitialProperties(a);
                            OnProcessIdEvent(a);
                        }
                        else
                        {
                            FindIdInSteps(p, a);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:FindIdInActions()::" + ex.Message, ex.InnerException);
            }
        }

        private static void FindIdInSteps(string p, Actions a)
        {
            try
            {
                if (a.Steps != null)
                {
                    foreach (Step s in a.Steps)
                    {
                        if (s.Id.ToString().Equals(p))
                        {
                            SetInitialProperties(s);
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

        private static void SetInitialProperties(IPlanElement m)
        {
            try
            {
                m.Enabled = true;
                m.AssignDate = System.DateTime.UtcNow;
                m.ElementState = 0;
                m.AssignBy = "System";
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:SetInitialProperties()::" + ex.Message, ex.InnerException);
            }
        }

        public static Actions GetProcessingAction(List<Module> list, string actionId)
        {
            try
            {
                Actions query = list.SelectMany(module => module.Actions).Where(action => action.Id == actionId).FirstOrDefault();
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:GetProcessingAction()::" + ex.Message, ex.InnerException);
            }
        }

        #region cohort patient
        internal static void RegisterCohortPatientViewProblemToPatient(string problemId, string patientId, IAppDomainRequest request)
        {
            try
            {
                CohortPatientViewData cpvd = GetCohortPatientViewRecord(patientId, request);
                // check to see if problem exists in the searchfield
                if (!cpvd.SearchFields.Exists(sf => sf.Value == problemId))
                {
                    cpvd.SearchFields.Add(new SearchFieldData
                    {
                        Value = problemId,
                        Active = true,
                        FieldName = Constants.Problem
                    });
                }
                else
                {
                    cpvd.SearchFields.ForEach(sf =>
                    {
                        if (sf.Value == problemId)
                        {
                            if (sf.Active == false)
                                sf.Active = true;
                        }
                    });
                }

                PlanElementEndpointUtil.UpdateCohortPatientViewProblem(cpvd, patientId, request);
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:RegisterCohortPatientViewProblemToPatient()::" + ex.Message, ex.InnerException);
            }
        }

        private static CohortPatientViewData GetCohortPatientViewRecord(string patientId, IAppDomainRequest request)
        {
            try
            {
                CohortPatientViewData cpvd = PlanElementEndpointUtil.RequestCohortPatientViewData(patientId, request);
                return cpvd;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:GetCohortPatientViewRecord()::" + ex.Message, ex.InnerException);
            }
        }
        #endregion

        #region program related tasks
        internal static bool IsProgramCompleted(Program p, string userId)
        {
            try
            {
                bool result = false;
                int modulesCompleted = 0;
                p.Modules.ForEach(m =>
                {
                    if (m.Completed)
                    {
                        modulesCompleted++;
                    }
                });

                if (modulesCompleted.Equals(p.Modules.Count))
                {
                    result = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:IsProgramCompleted()::" + ex.Message, ex.InnerException);
            }
        }

        internal static void SaveReportingAttributes(DD.ProgramAttribute _programAttributes, IAppDomainRequest request)
        {
            try
            {
                // 1) get program attribute
                DD.ProgramAttribute pAtt = PlanElementEndpointUtil.GetProgramAttributes(_programAttributes.PlanElementId, request);
                // 2) update existing attributes
                if (pAtt != null)
                {
                    bool dirty = ModifyProgramAttributePropertiesForUpdate(pAtt, _programAttributes);
                    if (dirty)
                    {
                        PlanElementEndpointUtil.UpdateProgramAttributes(pAtt, request);
                    }
                }
                else
                {
                    PlanElementEndpointUtil.InsertNewProgramAttribute(_programAttributes, request);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:SaveReportingAttributes()::" + ex.Message, ex.InnerException);
            }
        }

        private static bool ModifyProgramAttributePropertiesForUpdate(DD.ProgramAttribute pAtt, DD.ProgramAttribute _pAtt)
        {
            bool dirty = false;
            try
            {
                if (_pAtt.AssignedBy != null){ pAtt.AssignedBy = _pAtt.AssignedBy; dirty = true;}
                if (_pAtt.AssignedOn != null){ pAtt.AssignedOn = _pAtt.AssignedOn; dirty = true;}
                if (_pAtt.AuthoredBy != null){ pAtt.AuthoredBy = _pAtt.AuthoredBy; dirty = true;}
                if (_pAtt.CompletedBy != null){ pAtt.CompletedBy = _pAtt.CompletedBy; dirty = true;}
                if (_pAtt.DateCompleted != null){ pAtt.DateCompleted = _pAtt.DateCompleted; dirty = true;}
                if (_pAtt.DidNotEnrollReason != null){ pAtt.DidNotEnrollReason = _pAtt.DidNotEnrollReason; dirty = true;}
                if (_pAtt.DisEnrollReason != null){ pAtt.DisEnrollReason = _pAtt.DisEnrollReason; dirty = true;}
                if (_pAtt.EligibilityRequirements != null){ pAtt.EligibilityRequirements = _pAtt.EligibilityRequirements; dirty = true;}
                if (_pAtt.EligibilityStartDate != null){ pAtt.EligibilityStartDate = _pAtt.EligibilityStartDate; dirty = true;}
                if (_pAtt.EndDate != null){ pAtt.EndDate = _pAtt.EndDate; dirty = true;}
                if (_pAtt.IneligibleReason != null){ pAtt.IneligibleReason = _pAtt.IneligibleReason; dirty = true;}
                if (_pAtt.OptOut != false){ pAtt.OptOut = _pAtt.OptOut; dirty = true;}
                if (_pAtt.OptOutDate != null){ pAtt.OptOutDate = _pAtt.OptOutDate; dirty = true;}
                if (_pAtt.OptOutReason != null){ pAtt.OptOutReason = _pAtt.OptOutReason; dirty = true;}
                if (_pAtt.OverrideReason != null){ pAtt.OverrideReason = _pAtt.OverrideReason; dirty = true;}
                if (_pAtt.Population != null){ pAtt.Population = _pAtt.Population; dirty = true;}
                if (_pAtt.RemovedReason != null){ pAtt.RemovedReason = _pAtt.RemovedReason; dirty = true;}
                if (_pAtt.StartDate != null){ pAtt.StartDate = _pAtt.StartDate; dirty = true;}
                if (_pAtt.Status != 0){ pAtt.Status = _pAtt.Status; dirty = true;}
                if (_pAtt.Completed != 0){ pAtt.Completed = _pAtt.Completed; dirty = true;}
                if (_pAtt.Eligibility != 0){ pAtt.Eligibility = _pAtt.Eligibility; dirty = true;}
                if (_pAtt.EligibilityEndDate != null){ pAtt.EligibilityEndDate = _pAtt.EligibilityEndDate; dirty = true;}
                if (_pAtt.EligibilityOverride != 0){ pAtt.EligibilityOverride = _pAtt.EligibilityOverride; dirty = true;}
                if (_pAtt.Enrollment != 0){ pAtt.Enrollment = _pAtt.Enrollment; dirty = true;}
                if (_pAtt.GraduatedFlag != 0){ pAtt.GraduatedFlag = _pAtt.GraduatedFlag; dirty = true;}
                if (_pAtt.Locked != 0) { pAtt.Locked = _pAtt.Locked; dirty = true; }

                return dirty;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:ModifyProgramAttributePropertiesForUpdate()::" + ex.Message, ex.InnerException);
            }
        }

        internal static void SetStartDateForProgramAttributes(string programId, IAppDomainRequest request)
        {
            try
            {
                DD.ProgramAttribute pa = new DD.ProgramAttribute
                {
                    PlanElementId = programId,
                    StartDate = System.DateTime.UtcNow
                };

                // 1) get program attribute
                DD.ProgramAttribute pAtt = PlanElementEndpointUtil.GetProgramAttributes(pa.PlanElementId, request);
                // 2) update existing attributes
                if (pAtt != null)
                {
                    ModifyProgramAttributePropertiesForUpdate(pAtt, pa);
                    PlanElementEndpointUtil.UpdateProgramAttributes(pAtt, request);
                }
                else
                {
                    PlanElementEndpointUtil.InsertNewProgramAttribute(pa, request);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:SetStartDateForProgramAttributes()::" + ex.Message, ex.InnerException);
            }
        }

        internal static void SetProgramInformation(DD.ProgramAttribute _programAttributes, Program p)
        {
            try
            {
                _programAttributes.PlanElementId = p.Id;
                _programAttributes.Status = p.Status;
                _programAttributes.StartDate = p.StartDate;
                _programAttributes.EndDate = null;
                _programAttributes.Eligibility = p.Eligibility;
                _programAttributes.Enrollment = p.Enrollment;
                _programAttributes.GraduatedFlag = 1;
                _programAttributes.OptOut = false;
                _programAttributes.EligibilityOverride = 1;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:SetProgramInformation()::" + ex.Message, ex.InnerException);
            }
        }
        #endregion


        internal static void HydratePlanElementLists(List<object> ProcessedElements, PostProcessActionResponse response)
        {
            try
            {
                if (ProcessedElements != null && ProcessedElements.Count > 0)
                {
                    response.PlanElems = new PlanElements();

                    foreach (Object obj in ProcessedElements)
                    {
                        if (obj.GetType().Equals(typeof(Program)))
                        {
                            if (!response.PlanElems.Programs.Contains(obj))
                            {
                                Program p = PlanElementUtil.CloneProgram((Program)obj);
                                response.PlanElems.Programs.Add(p);
                            }
                        }
                        else if (obj.GetType().Equals(typeof(Module)))
                        {
                            if (!response.PlanElems.Modules.Contains(obj))
                            {
                                Module m = PlanElementUtil.CloneModule((Module)obj);
                                response.PlanElems.Modules.Add(m);
                            }
                        }
                        else if (obj.GetType().Equals(typeof(Actions)))
                        {
                            if (!response.PlanElems.Actions.Contains(obj))
                            {
                                Actions a = PlanElementUtil.CloneAction((Actions)obj);
                                response.PlanElems.Actions.Add(a);
                            }
                        }
                        else if (obj.GetType().Equals(typeof(Step)))
                        {
                            if (!response.PlanElems.Steps.Contains(obj))
                            {
                                Step s = PlanElementUtil.CloneStep((Step)obj);
                                response.PlanElems.Steps.Add(s);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:HydratePlanElementLists()::" + ex.Message, ex.InnerException);
            }
        }

        private static Module CloneModule(Module md)
        {
            try
            {
                Module m = new Module
                {
                    AssignBy = md.AssignBy,
                    AssignDate = md.AssignDate,
                    Completed = md.Completed,
                    CompletedBy = md.CompletedBy,
                    DateCompleted = md.DateCompleted,
                    Description = md.Description,
                    ElementState = md.ElementState,
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

        private static Actions CloneAction(Actions ac)
        {
            try
            {
                Actions a = new Actions
                {
                    AssignBy = ac.AssignBy,
                    AssignDate = ac.AssignDate,
                    Completed = ac.Completed,
                    CompletedBy = ac.CompletedBy,
                    DateCompleted = ac.DateCompleted,
                    Description = ac.Description,
                    ElementState = ac.ElementState,
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

        private static Step CloneStep(Step st)
        {
            try
            {
                Step s = new Step
                {
                    ActionId = st.ActionId,
                    AssignBy = st.AssignBy,
                    AssignDate = st.AssignDate,
                    Completed = st.Completed,
                    CompletedBy = st.CompletedBy,
                    ControlType = st.ControlType,
                    DateCompleted = st.DateCompleted,
                    Description = st.Description,
                    ElementState = st.ElementState,
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

        private static Program CloneProgram(Program pr)
        {
            try
            {
                Program p = new Program
                {
                    AssignBy = pr.AssignBy,
                    AssignDate = pr.AssignDate,
                    Client = pr.Client,
                    Completed = pr.Completed,
                    CompletedBy = pr.CompletedBy,
                    Previous = pr.Previous,
                    ContractProgramId = pr.ContractProgramId,
                    DateCompleted = pr.DateCompleted,
                    Description = pr.Description,
                    DidNotEnrollReason = pr.DidNotEnrollReason,
                    DisEnrollReason = pr.DisEnrollReason,
                    ElementState = pr.ElementState,
                    Eligibility = pr.Eligibility,
                    EligibilityEndDate = pr.EligibilityEndDate,
                    EligibilityOverride = pr.EligibilityOverride,
                    EligibilityRequirements = pr.EligibilityRequirements,
                    EligibilityStartDate = pr.EligibilityStartDate,
                    Enabled = pr.Enabled,
                    EndDate = pr.EndDate,
                    Enrollment = pr.Enrollment,
                    GraduatedFlag = pr.GraduatedFlag,
                    Id = pr.Id,
                    IneligibleReason = pr.IneligibleReason,
                    Name = pr.Name,
                    Next = pr.Next,
                    ObjectivesInfo = pr.ObjectivesInfo,
                    OptOut = pr.OptOut,
                    OptOutDate = pr.OptOutDate,
                    OptOutReason = pr.OptOutReason,
                    Order = pr.Order,
                    OverrideReason = pr.OverrideReason,
                    PatientId = pr.PatientId,
                    ProgramState = pr.ProgramState,
                    RemovedReason = pr.RemovedReason,
                    ShortName = pr.ShortName,
                    SourceId = pr.SourceId,
                    SpawnElement = pr.SpawnElement,
                    StartDate = pr.StartDate,
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
    }
}
