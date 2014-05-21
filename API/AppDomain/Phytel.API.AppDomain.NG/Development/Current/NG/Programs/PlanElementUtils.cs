using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using DD = Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Common;
using Phytel.API.AppDomain.NG.Specifications;

namespace Phytel.API.AppDomain.NG
{
    public delegate void ProcessedElementInUtilEventHandlers(ProcessElementEventArgs e);

    public class PlanElementUtils : IPlanElementUtils
    {
        public event ProcessedElementInUtilEventHandlers _processedElementEvent;
        private const string SystemId = "5368ff2ad4332316288f3e3e";

        public void OnProcessIdEvent(object pe)
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
        public bool SetCompletionStatus<T>(List<T> list)
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

        public T FindElementById<T>(List<T> list, string id)
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

        public void SetEnabledStatusByPrevious<T>(List<T> actions)
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

        public void SetEnabledState<T>(List<T> list, T x)
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
                                ((IPlanElement) x).AssignById = SystemId;
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

        public void DisableCompleteButtonForAction(List<Step> list)
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

        public void SpawnElementsInList(List<SpawnElement> list, Program program, string userId, DD.ProgramAttributeData progAttr)
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

        public void SetProgramAttributes(SpawnElement r, Program program, string userId, DD.ProgramAttributeData progAttr)
        {
            try
            {
                if (r.ElementType.Equals(10))
                {
                    // eligibility series
                    try
                    {
                        if (r.Tag == null)
                            throw new ArgumentException("Cannot set attribute of type " + r.ElementType + ". Tag value is null.");

                        progAttr.Eligibility = (!string.IsNullOrEmpty(r.Tag)) ? Convert.ToInt32(r.Tag) : 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("AD:SetProgramAttributes()::Eligibility" + ex.Message, ex.InnerException);
                    }

                    int state; // no = 1, yes = 2
                    var isNum = int.TryParse(r.Tag, out state);
                    if (!isNum) return;
                    // program is closed due to ineligibility
                    if (state.Equals(1)) // not eligible
                    {
                        program.ElementState = 5;
                        program.StateUpdatedOn = System.DateTime.UtcNow;
                        progAttr.Eligibility = 1;
                        //progAttr.AttrEndDate = System.DateTime.UtcNow;
                        program.AttrEndDate = System.DateTime.UtcNow;
                    }
                    else if (state.Equals(2)) // eligible
                    {
                        program.ElementState = 4;
                        program.StateUpdatedOn = System.DateTime.UtcNow;
                        progAttr.Eligibility = 2;
                    }
                }
                else if (r.ElementType.Equals(11))
                {
                    // eligibility reason
                    try
                    {
                        if (r.Tag == null)
                            throw new ArgumentException("Cannot set attribute of type " + r.ElementType + ". Tag value is null.");

                        progAttr.IneligibleReason = (!string.IsNullOrEmpty(r.Tag)) ? r.Tag : null;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("AD:SetProgramAttributes()::IneligibleReason" + ex.Message, ex.InnerException);
                    }
                }
                else if (r.ElementType.Equals(12))
                {
                    try
                    {
                        if (r.Tag == null)
                            throw new ArgumentException("Cannot set attribute of type " + r.ElementType + ". Tag value is null.");

                        program.ElementState = (!string.IsNullOrEmpty(r.Tag)) ? Convert.ToInt32(r.Tag) : 0;
                        program.StateUpdatedOn = System.DateTime.UtcNow;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("AD:SetProgramAttributes()::ElementState" + ex.Message, ex.InnerException);
                    }
                }
                else if (r.ElementType.Equals(13))
                {
                    try
                    {
                        // need to revisit in the future.
                        //if (r.Tag == null)
                        //    throw new ArgumentException("Cannot set attribute of type " + r.ElementType + ". Tag value is null.");

                        if (string.IsNullOrEmpty(r.Tag))
                        {
                            //progAttr.AttrStartDate = System.DateTime.UtcNow;
                            program.AttrStartDate = System.DateTime.UtcNow;
                        }
                        else
                        {
                            DateTime date;
                            if (DateTime.TryParse(r.Tag.ToString(), out date))
                            {
                                //progAttr.AttrStartDate = date;
                                program.AttrStartDate = date;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("AD:SetProgramAttributes()::StartDate" + ex.Message, ex.InnerException);
                    }
                }
                else if (r.ElementType.Equals(14))
                {
                    //progAttr.AttrEndDate = System.DateTime.UtcNow;
                    program.AttrEndDate = System.DateTime.UtcNow;
                }
                else if (r.ElementType.Equals(15))
                {
                    try
                    {
                        if (r.Tag == null)
                            throw new ArgumentException("Cannot set attribute of type " + r.ElementType + ". Tag value is null.");

                        progAttr.Enrollment = (!string.IsNullOrEmpty(r.Tag)) ? Convert.ToInt32(r.Tag) : 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("AD:SetProgramAttributes()::Enrollment" + ex.Message, ex.InnerException);
                    }
                }
                else if (r.ElementType.Equals(16))
                {

                    try
                    {
                        if (r.Tag == null)
                            throw new ArgumentException("Cannot set attribute of type " + r.ElementType + ". Tag value is null.");

                        progAttr.OptOut = (!string.IsNullOrEmpty(r.Tag)) ? Convert.ToBoolean(r.Tag) : false;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("AD:SetProgramAttributes()::OptOut" + ex.Message, ex.InnerException);
                    }
                }
                //else if (r.ElementType.Equals(17))
                //{
                //    // do something with opt out
                //    try
                //    {
                //        if (r.Tag == null)
                //            throw new ArgumentException("Cannot set attribute of type " + r.ElementType + ". Tag value is null.");

                //        progAttr.OptOutReason = (!string.IsNullOrEmpty(r.Tag)) ? r.Tag : null;
                //    }
                //    catch (Exception ex)
                //    {
                //        throw new Exception("AD:SetProgramAttributes()::OptOutReason" + ex.Message, ex.InnerException);
                //    }
                //}
                //else if (r.ElementType.Equals(18))
                //{
                //    // do something with opt out 
                //    progAttr.OptOutDate = System.DateTime.UtcNow;
                //}
                else if (r.ElementType.Equals(19))
                {
                    try
                    {
                        if (r.Tag == null)
                            throw new ArgumentException("Cannot set attribute of type " + r.ElementType + ". Tag value is null.");

                        progAttr.GraduatedFlag = (!string.IsNullOrEmpty(r.Tag)) ? Convert.ToInt32(r.Tag) : 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("AD:SetProgramAttributes()::GraduatedFlag" + ex.Message, ex.InnerException);
                    }
                }
                else if (r.ElementType.Equals(20))
                {
                    try
                    {
                        if (r.Tag == null)
                            throw new ArgumentException("Cannot set attribute of type " + r.ElementType + ". Tag value is null.");

                        progAttr.Locked = (!string.IsNullOrEmpty(r.Tag)) ? Convert.ToInt32(r.Tag) : 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("AD:SetProgramAttributes()::Locked" + ex.Message, ex.InnerException);
                    }
                }
                //else if (r.ElementType.Equals(21))
                //{
                //    try
                //    {
                //        if (r.Tag == null)
                //            throw new ArgumentException("Cannot set attribute of type " + r.ElementType + ". Tag value is null.");

                //        progAttr.EligibilityOverride = (!string.IsNullOrEmpty(r.Tag)) ? Convert.ToInt32(r.Tag) : 0;
                //    }
                //    catch (Exception ex)
                //    {
                //        throw new Exception("AD:SetProgramAttributes()::EligibilityOverride" + ex.Message, ex.InnerException);
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:SetProgramAttributes()::" + ex.Message, ex.InnerException);
            }
        }

        public void SetElementEnabledState(string p, Program program)
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
                throw new Exception("AD:PlanElementUtil:SetElementEnabledState()::" + ex.Message, ex.InnerException);
            }
        }

        public void FindIdInActions(string p, Module m)
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

        public void FindIdInSteps(string p, Actions a)
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

        public void SetInitialProperties(IPlanElement m)
        {
            try
            {
                m.Enabled = true;
                m.StateUpdatedOn = System.DateTime.UtcNow;
                m.AssignDate = System.DateTime.UtcNow;
                m.ElementState = 2;
                m.AssignById = SystemId;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:SetInitialProperties()::" + ex.Message, ex.InnerException);
            }
        }

        public Actions GetProcessingAction(List<Module> list, string actionId)
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
        public void RegisterCohortPatientViewProblemToPatient(string problemId, string patientId, IAppDomainRequest request)
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

        public CohortPatientViewData GetCohortPatientViewRecord(string patientId, IAppDomainRequest request)
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
        public bool IsProgramCompleted(Program p, string userId)
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

        public void SaveReportingAttributes(DD.ProgramAttributeData _programAttributes, IAppDomainRequest request)
        {
            try
            {
                // 1) get program attribute
                DD.ProgramAttributeData pAtt = PlanElementEndpointUtil.GetProgramAttributes(_programAttributes.PlanElementId, request);
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

        public bool ModifyProgramAttributePropertiesForUpdate(DD.ProgramAttributeData pAtt, DD.ProgramAttributeData _pAtt)
        {
            bool dirty = false;
            try
            {
                //if (_pAtt.AssignedBy != null){ pAtt.AssignedBy = _pAtt.AssignedBy; dirty = true;}
                //if (_pAtt.AssignedOn != null){ pAtt.AssignedOn = _pAtt.AssignedOn; dirty = true;}
                if (_pAtt.CompletedBy != null){ pAtt.CompletedBy = _pAtt.CompletedBy; dirty = true;}
                if (_pAtt.DateCompleted != null){ pAtt.DateCompleted = _pAtt.DateCompleted; dirty = true;}
                if (_pAtt.DidNotEnrollReason != null){ pAtt.DidNotEnrollReason = _pAtt.DidNotEnrollReason; dirty = true;}
                //if (_pAtt.DisEnrollReason != null){ pAtt.DisEnrollReason = _pAtt.DisEnrollReason; dirty = true;}
                //if (_pAtt.EligibilityRequirements != null){ pAtt.EligibilityRequirements = _pAtt.EligibilityRequirements; dirty = true;}
                //if (_pAtt.EligibilityStartDate != null){ pAtt.EligibilityStartDate = _pAtt.EligibilityStartDate; dirty = true;}
                //if (_pAtt.EligibilityEndDate != null) { pAtt.EligibilityEndDate = _pAtt.EligibilityEndDate; dirty = true; }
                //if (_pAtt.AttrEndDate != null){ pAtt.AttrEndDate = _pAtt.AttrEndDate; dirty = true;}
                if (_pAtt.IneligibleReason != null){ pAtt.IneligibleReason = _pAtt.IneligibleReason; dirty = true;}
                if (_pAtt.OptOut != false){ pAtt.OptOut = _pAtt.OptOut; dirty = true;}
                //if (_pAtt.OptOutDate != null){ pAtt.OptOutDate = _pAtt.OptOutDate; dirty = true;}
                //if (_pAtt.OptOutReason != null){ pAtt.OptOutReason = _pAtt.OptOutReason; dirty = true;}
                if (_pAtt.OverrideReason != null){ pAtt.OverrideReason = _pAtt.OverrideReason; dirty = true;}
                if (_pAtt.Population != null){ pAtt.Population = _pAtt.Population; dirty = true;}
                if (_pAtt.RemovedReason != null){ pAtt.RemovedReason = _pAtt.RemovedReason; dirty = true;}
                //if (_pAtt.AttrStartDate != null){ pAtt.AttrStartDate = _pAtt.AttrStartDate; dirty = true;}
                if (_pAtt.Status != 0){ pAtt.Status = _pAtt.Status; dirty = true;}
                if (_pAtt.Completed != 0){ pAtt.Completed = _pAtt.Completed; dirty = true;}
                if (_pAtt.Eligibility != 0){ pAtt.Eligibility = _pAtt.Eligibility; dirty = true;}
                //if (_pAtt.EligibilityOverride != 0){ pAtt.EligibilityOverride = _pAtt.EligibilityOverride; dirty = true;}
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

        public void SetStartDateForProgramAttributes(string programId, IAppDomainRequest request)
        {
            try
            {
                DD.ProgramAttributeData pa = new DD.ProgramAttributeData
                {
                    PlanElementId = programId,
                    //AttrStartDate = System.DateTime.UtcNow
                };

                // 1) get program attribute
                DD.ProgramAttributeData pAtt = PlanElementEndpointUtil.GetProgramAttributes(pa.PlanElementId, request);
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

        public void SetProgramInformation(DD.ProgramAttributeData _programAttributes, Program p)
        {
            try
            {
                _programAttributes.PlanElementId = p.Id;
                _programAttributes.Status = p.Status;
                //_programAttributes.AttrStartDate = p.StartDate;
                //_programAttributes.AttrEndDate = null;
                _programAttributes.Eligibility = p.Eligibility;
                //_programAttributes.Enrollment = p.Enrollment;
                _programAttributes.GraduatedFlag = 1;
                _programAttributes.OptOut = false;
                //_programAttributes.EligibilityOverride = 1;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:SetProgramInformation()::" + ex.Message, ex.InnerException);
            }
        }
        #endregion


        public void HydratePlanElementLists(List<object> ProcessedElements, PostProcessActionResponse response)
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
                                Program p = CloneProgram((Program)obj);
                                response.PlanElems.Programs.Add(p);
                            }
                        }
                        else if (obj.GetType().Equals(typeof(Module)))
                        {
                            if (!response.PlanElems.Modules.Contains(obj))
                            {
                                Module m = CloneModule((Module)obj);
                                response.PlanElems.Modules.Add(m);
                            }
                        }
                        else if (obj.GetType().Equals(typeof(Actions)))
                        {
                            if (!response.PlanElems.Actions.Contains(obj))
                            {
                                Actions a = CloneAction((Actions)obj);
                                response.PlanElems.Actions.Add(a);
                            }
                        }
                        else if (obj.GetType().Equals(typeof(Step)))
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
                throw new Exception("AD:PlanElementUtil:HydratePlanElementLists()::" + ex.Message, ex.InnerException);
            }
        }

        private Module CloneModule(Module md)
        {
            try
            {
                Module m = new Module
                {
                    AssignById = md.AssignById,
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

        public Program CloneProgram(Program pr)
        {
            try
            {
                Program p = new Program
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

        public bool ResponseSpawnAllowed(Step s, Response r)
        {
            try
            {
                bool pass = true;
                if (new ResponseSpawnAllowed<Step>().IsSatisfiedBy(s))
                {
                    if (string.IsNullOrEmpty(r.Value) || r.Value.ToLower().Equals("false"))
                    {
                        pass = false;
                    }
                }
                return pass;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:ResponseSpawnAllowed()::" + ex.Message, ex.InnerException);
            }
        }

        public PlanElement ActivatePlanElement(string p, Program program)
        {
            try
            {
                PlanElement pe = null;
                if (program.Modules != null)
                {
                    foreach (Module m in program.Modules)
                    {
                        if (m.Id.Equals(p))
                        {
                            pe = InitializePlanElementSettings(pe, m);
                            break;
                        }
                        else
                        {
                            if (m.Actions != null)
                            {
                                foreach (Actions a in m.Actions)
                                {
                                    if (a.Id.Equals(p))
                                    {
                                        pe = InitializePlanElementSettings(pe, a);
                                        break;
                                    }
                                    else
                                    {
                                        if (a.Steps != null)
                                        {
                                            foreach (Step s in a.Steps)
                                            {
                                                if (s.Id.Equals(p))
                                                {
                                                    pe = InitializePlanElementSettings(pe, s);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return pe;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:ActivatePlanElement()::" + ex.Message, ex.InnerException);
            }
        }

        public PlanElement InitializePlanElementSettings(PlanElement pe, PlanElement p)
        {
            try
            {
                p.Enabled = true;
                p.AssignById = SystemId;
                p.AssignDate = System.DateTime.UtcNow;
                pe = p;
                return pe;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:InitializePlanElementSettings()::" + ex.Message, ex.InnerException);
            }
        }

        public bool IsActionInitial(Program p)
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
                            if (a.ElementState == 4 || a.ElementState == 5)
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

        public ProgramAttribute GetAttributes(DD.ProgramAttributeData programAttributeData)
        {
            ProgramAttribute programAttribute = null;
            
            try
            {
                if (programAttributeData != null)
                {
                    programAttribute = new ProgramAttribute
                    {
                        // AssignedBy = programAttributeData.AssignedBy,
                        //AssignedOn = programAttributeData.AssignedOn, Sprint 12
                        AuthoredBy = programAttributeData.AuthoredBy,
                        Completed = (int)programAttributeData.Completed,
                        CompletedBy = programAttributeData.CompletedBy,
                        DateCompleted = programAttributeData.DateCompleted,
                        DidNotEnrollReason = programAttributeData.DidNotEnrollReason,
                        Eligibility = (int)programAttributeData.Eligibility,
                        //AttrEndDate = programAttributeData.AttrEndDate, Sprint 12
                        Enrollment = (int)programAttributeData.Enrollment,
                        GraduatedFlag = (int)programAttributeData.GraduatedFlag,
                        Id = programAttributeData.Id.ToString(),
                        IneligibleReason = programAttributeData.IneligibleReason,
                        Locked = (int)programAttributeData.Locked,
                        OptOut = programAttributeData.OptOut,
                        OverrideReason = programAttributeData.OverrideReason,
                        PlanElementId = programAttributeData.PlanElementId.ToString(),
                        Population = programAttributeData.Population,
                        RemovedReason = programAttributeData.RemovedReason,
                        //AttrStartDate = programAttributeData.AttrStartDate, Sprint 12
                        Status = (int)programAttributeData.Status
                    };
                }
                return programAttribute;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:getAttributes()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
