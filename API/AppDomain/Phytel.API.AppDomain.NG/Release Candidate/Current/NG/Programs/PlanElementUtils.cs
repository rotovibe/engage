﻿using MongoDB.Bson;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack.Common.Extensions;
using DD = Phytel.API.DataDomain.Program.DTO;
using Phytel.API.AppDomain.NG.Specifications;

namespace Phytel.API.AppDomain.NG
{
    public delegate void ProcessedElementInUtilEventHandlers(ProcessElementEventArgs e);

    public class PlanElementUtils : IPlanElementUtils
    {
        private readonly Specification<PlanElement> _isModifyAllowed;

        public PlanElementUtils()
        {
            _isModifyAllowed = new IsModifyAllowedSpecification<PlanElement>();
        }

        public event ProcessedElementInUtilEventHandlers _processedElementEvent;

        public void OnProcessIdEvent(object pe)
        {
            if (_processedElementEvent != null)
            {
                _processedElementEvent(new ProcessElementEventArgs {PlanElement = pe});
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

                if (completed == count)
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
                var mod = list.Where(r => ((IPlanElement) Convert.ChangeType(r, typeof (T))).Id == id).FirstOrDefault();
                return mod;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:FindElementById()::" + ex.Message, ex.InnerException);
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

                        if (x.GetType() == typeof (Module))
                        {
                            SetInitialActions(x, assignToId);
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
            if (((Module) x).Enabled == false) return;
            if (((Module) x).Actions == null) return;

            List<Actions> actionList =
                ((Module) x).Actions.Where(
                    a =>
                        a.Enabled == true && a.ElementState != (int) DD.ElementState.Completed
                        && a.ElementState != (int) DD.ElementState.InProgress).ToList();
            // create a specification for this to isolate the business logic.

            actionList.ForEach(z =>
            {
                //SetInitialProperties(((Module) x).AssignToId, (PlanElement) z, true);

                if (z.StateUpdatedOn == null)
                    z.StateUpdatedOn = DateTime.UtcNow;

                if (z.AssignDate == null)
                    z.AssignDate = DateTime.UtcNow;

                if (string.IsNullOrEmpty(z.AssignToId))
                    z.AssignToId = assignToId;

                if (string.IsNullOrEmpty(z.AssignById))
                    z.AssignById = DD.Constants.SystemContactId;

                OnProcessIdEvent(z);
            });
        }

        // NIGHT-876
        public void SetEnabledState<T>(List<T> list, T x, string assignToId, bool pEnabled)
        {
            try
            {
                if (list == null || x == null) return;

                var pe = ((IPlanElement) Convert.ChangeType(x, typeof (T)));
                if (string.IsNullOrEmpty(pe.Previous)) return;

                var prevElem = list.Find(r => ((IPlanElement) r).Id == pe.Previous);
                if (prevElem == null) return;

                if (((IPlanElement) prevElem).Completed != true)
                {
                    ((IPlanElement) x).Enabled = false;
                }
                else
                {
                    if (!((IPlanElement) x).Enabled)
                    {
                        ((IPlanElement) x).StateUpdatedOn = DateTime.UtcNow;
                        ((IPlanElement) x).Enabled = true;
                        if (((IPlanElement) x).AssignToId == null)
                            ((IPlanElement) x).AssignToId = assignToId;
                    }

                    // add this to a parent enabled specification
                    if (((IPlanElement) x).AssignDate == null)
                        ((IPlanElement) x).AssignDate = DateTime.UtcNow;

                    if (string.IsNullOrEmpty(((IPlanElement) x).AssignById))
                        ((IPlanElement) x).AssignById = DD.Constants.SystemContactId;

                    // only track elements who are enabled for now.
                    OnProcessIdEvent(Convert.ChangeType(x, typeof (T)));
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
                        if (s.StepTypeId != 7) return;
                        s.Enabled = false;
                        OnProcessIdEvent(s);
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:DisableCompleteButtonForAction()::" + ex.Message,
                    ex.InnerException);
            }
        }

        public void SpawnElementsInList(List<SpawnElement> list, Program program, string userId,
            DD.ProgramAttributeData progAttr)
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

        public void SetProgramAttributes(SpawnElement r, Program program, string userId,
            DD.ProgramAttributeData progAttr)
        {
            try
            {
                if (r.ElementType == 10)
                {
                    try
                    {
                        if (r.Tag == null)
                            throw new ArgumentException("Cannot set attribute of type " + r.ElementType +
                                                        ". Tag value is null.");

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
                    switch (state)
                    {
                        case 1:
                            program.ElementState = (int) DataDomain.Program.DTO.ElementState.Completed; //5;
                            program.StateUpdatedOn = System.DateTime.UtcNow;
                            progAttr.Eligibility = 1;
                            program.AttrEndDate = System.DateTime.UtcNow;
                            break;
                        case 2:
                            program.ElementState = (int) DataDomain.Program.DTO.ElementState.InProgress; //4;
                            program.StateUpdatedOn = System.DateTime.UtcNow;
                            progAttr.Eligibility = 2;
                            break;
                    }
                }
                else if (r.ElementType == 11)
                {
                    // eligibility reason
                    try
                    {
                        if (r.Tag == null)
                            throw new ArgumentException("Cannot set attribute of type " + r.ElementType +
                                                        ". Tag value is null.");

                        progAttr.IneligibleReason = (!string.IsNullOrEmpty(r.Tag)) ? r.Tag : null;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("AD:SetProgramAttributes()::IneligibleReason" + ex.Message,
                            ex.InnerException);
                    }
                }
                else if (r.ElementType == 12)
                {
                    try
                    {
                        if (r.Tag == null)
                            throw new ArgumentException("Cannot set attribute of type " + r.ElementType +
                                                        ". Tag value is null.");

                        program.ElementState = (!string.IsNullOrEmpty(r.Tag)) ? Convert.ToInt32(r.Tag) : 0;
                        program.StateUpdatedOn = System.DateTime.UtcNow;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("AD:SetProgramAttributes()::ElementState" + ex.Message, ex.InnerException);
                    }
                }
                else if (r.ElementType == 13)
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
                else if (r.ElementType == 14)
                {
                    //progAttr.AttrEndDate = System.DateTime.UtcNow;
                    program.AttrEndDate = System.DateTime.UtcNow;
                }
                else if (r.ElementType == 15)
                {
                    try
                    {
                        if (r.Tag == null)
                            throw new ArgumentException("Cannot set attribute of type " + r.ElementType +
                                                        ". Tag value is null.");

                        progAttr.Enrollment = (!string.IsNullOrEmpty(r.Tag)) ? Convert.ToInt32(r.Tag) : 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("AD:SetProgramAttributes()::Enrollment" + ex.Message, ex.InnerException);
                    }
                }
                else if (r.ElementType == 16)
                {

                    try
                    {
                        if (r.Tag == null)
                            throw new ArgumentException("Cannot set attribute of type " + r.ElementType +
                                                        ". Tag value is null.");

                        progAttr.OptOut = (!string.IsNullOrEmpty(r.Tag)) ? Convert.ToBoolean(r.Tag) : false;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("AD:SetProgramAttributes()::OptOut" + ex.Message, ex.InnerException);
                    }
                }
                    //else if (r.ElementType == 17)
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
                    //else if (r.ElementType == 18)
                    //{
                    //    // do something with opt out 
                    //    progAttr.OptOutDate = System.DateTime.UtcNow;
                    //}
                else if (r.ElementType == 19)
                {
                    try
                    {
                        if (r.Tag == null)
                            throw new ArgumentException("Cannot set attribute of type " + r.ElementType +
                                                        ". Tag value is null.");

                        progAttr.GraduatedFlag = (!string.IsNullOrEmpty(r.Tag)) ? Convert.ToInt32(r.Tag) : 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("AD:SetProgramAttributes()::GraduatedFlag" + ex.Message, ex.InnerException);
                    }
                }
                else if (r.ElementType == 20)
                {
                    try
                    {
                        if (r.Tag == null)
                            throw new ArgumentException("Cannot set attribute of type " + r.ElementType +
                                                        ". Tag value is null.");

                        progAttr.Locked = (!string.IsNullOrEmpty(r.Tag)) ? Convert.ToInt32(r.Tag) : 0;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("AD:SetProgramAttributes()::Locked" + ex.Message, ex.InnerException);
                    }
                }
                //else if (r.ElementType == 21)
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
                    if (m.Id.Equals(p))
                    {
                        SetInitialProperties(program.AssignToId, m, false);
                        var list = m.Actions.Where(a => a.Enabled == true
                                                        && a.ElementState != (int) DD.ElementState.Completed
                                                        && a.ElementState != (int) DD.ElementState.InProgress);
                        // create rule specification
                        list.ForEach(a =>
                        {
                            if (a.Enabled) SetInitialProperties(program.AssignToId, a, false);
                            OnProcessIdEvent(a);
                        });
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

        public void FindSpawnIdInActions(Program program, string p, Module m)
        {
            try
            {
                if (m.Actions != null)
                {
                    foreach (Actions a in m.Actions)
                    {
                        if (a.Id.Equals(p))
                        {
                            SetInitialProperties(m.AssignToId, a, false);
                            OnProcessIdEvent(a);
                            return;
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

        public void FindSpawnIdInSteps(Program program, string p, Actions a)
        {
            try
            {
                if (a.Steps == null) return;
                foreach (Step s in a.Steps.Where(s => s.Id.Equals(p)))
                {
                    SetInitialProperties(program.AssignToId, s, false);
                    OnProcessIdEvent(s);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:FindIdInSteps()::" + ex.Message, ex.InnerException);
            }
        }

        public void SetInitialProperties(string assignToId, IPlanElement m, bool dependent)
        {
            try
            {
                switch (dependent)
                {
                    case false:
                        {
                            SetInitialValues(assignToId, m);
                            break;
                        }
                    case true:
                        {
                            if (!m.Enabled)
                            {
                                SetInitialValues(assignToId, m);
                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:SetInitialProperties()::" + ex.Message, ex.InnerException);
            }
        }

        public void SetInitialValues(string assignToId, IPlanElement pe)
        {
            if (pe.Enabled) return;
            // NIGHT-835
            if (pe.AssignDate == null)
                pe.AssignDate = System.DateTime.UtcNow;

            //if (pe.AssignToId == null)
            pe.AssignToId = assignToId; // NIGHT-877

            pe.Enabled = true;
            pe.StateUpdatedOn = System.DateTime.UtcNow;
            pe.ElementState = (int) DD.ElementState.NotStarted; // 2;

            if (string.IsNullOrEmpty(pe.AssignById))
                pe.AssignById = DD.Constants.SystemContactId;
        }

        public Actions GetProcessingAction(List<Module> list, string actionId)
        {
            try
            {
                Actions query =
                    list.SelectMany(module => module.Actions).Where(action => action.Id == actionId).FirstOrDefault();
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:GetProcessingAction()::" + ex.Message, ex.InnerException);
            }
        }

        #region cohort patient

        public void RegisterCohortPatientViewProblemToPatient(string problemId, string patientId,
            IAppDomainRequest request)
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
                throw new Exception("AD:PlanElementUtil:RegisterCohortPatientViewProblemToPatient()::" + ex.Message,
                    ex.InnerException);
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

                if (modulesCompleted == p.Modules.Count)
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
                DD.ProgramAttributeData pAtt =
                    PlanElementEndpointUtil.GetProgramAttributes(_programAttributes.PlanElementId, request);
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

        public bool ModifyProgramAttributePropertiesForUpdate(DD.ProgramAttributeData pAtt,
            DD.ProgramAttributeData _pAtt)
        {
            bool dirty = false;
            try
            {
                //if (_pAtt.AssignedBy != null){ pAtt.AssignedBy = _pAtt.AssignedBy; dirty = true;}
                //if (_pAtt.AssignedOn != null){ pAtt.AssignedOn = _pAtt.AssignedOn; dirty = true;}
                if (_pAtt.CompletedBy != null)
                {
                    pAtt.CompletedBy = _pAtt.CompletedBy;
                    dirty = true;
                }
                if (_pAtt.DateCompleted != null)
                {
                    pAtt.DateCompleted = _pAtt.DateCompleted;
                    dirty = true;
                }
                if (_pAtt.DidNotEnrollReason != null)
                {
                    pAtt.DidNotEnrollReason = _pAtt.DidNotEnrollReason;
                    dirty = true;
                }
                //if (_pAtt.DisEnrollReason != null){ pAtt.DisEnrollReason = _pAtt.DisEnrollReason; dirty = true;}
                //if (_pAtt.EligibilityRequirements != null){ pAtt.EligibilityRequirements = _pAtt.EligibilityRequirements; dirty = true;}
                //if (_pAtt.EligibilityStartDate != null){ pAtt.EligibilityStartDate = _pAtt.EligibilityStartDate; dirty = true;}
                //if (_pAtt.EligibilityEndDate != null) { pAtt.EligibilityEndDate = _pAtt.EligibilityEndDate; dirty = true; }
                //if (_pAtt.AttrEndDate != null){ pAtt.AttrEndDate = _pAtt.AttrEndDate; dirty = true;}
                if (_pAtt.IneligibleReason != null)
                {
                    pAtt.IneligibleReason = _pAtt.IneligibleReason;
                    dirty = true;
                }
                if (_pAtt.OptOut != false)
                {
                    pAtt.OptOut = _pAtt.OptOut;
                    dirty = true;
                }
                //if (_pAtt.OptOutDate != null){ pAtt.OptOutDate = _pAtt.OptOutDate; dirty = true;}
                //if (_pAtt.OptOutReason != null){ pAtt.OptOutReason = _pAtt.OptOutReason; dirty = true;}
                if (_pAtt.OverrideReason != null)
                {
                    pAtt.OverrideReason = _pAtt.OverrideReason;
                    dirty = true;
                }
                if (_pAtt.Population != null)
                {
                    pAtt.Population = _pAtt.Population;
                    dirty = true;
                }
                if (_pAtt.RemovedReason != null)
                {
                    pAtt.RemovedReason = _pAtt.RemovedReason;
                    dirty = true;
                }
                //if (_pAtt.AttrStartDate != null){ pAtt.AttrStartDate = _pAtt.AttrStartDate; dirty = true;}
                if (_pAtt.Status != 0)
                {
                    pAtt.Status = _pAtt.Status;
                    dirty = true;
                }
                if (_pAtt.Completed != 0)
                {
                    pAtt.Completed = _pAtt.Completed;
                    dirty = true;
                }
                if (_pAtt.Eligibility != 0)
                {
                    pAtt.Eligibility = _pAtt.Eligibility;
                    dirty = true;
                }
                //if (_pAtt.EligibilityOverride != 0){ pAtt.EligibilityOverride = _pAtt.EligibilityOverride; dirty = true;}
                if (_pAtt.Enrollment != 0)
                {
                    pAtt.Enrollment = _pAtt.Enrollment;
                    dirty = true;
                }
                if (_pAtt.GraduatedFlag != 0)
                {
                    pAtt.GraduatedFlag = _pAtt.GraduatedFlag;
                    dirty = true;
                }
                if (_pAtt.Locked != 0)
                {
                    pAtt.Locked = _pAtt.Locked;
                    dirty = true;
                }

                return dirty;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:ModifyProgramAttributePropertiesForUpdate()::" + ex.Message,
                    ex.InnerException);
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
                throw new Exception("AD:PlanElementUtil:SetStartDateForProgramAttributes()::" + ex.Message,
                    ex.InnerException);
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


        public void HydratePlanElementLists(List<object> processedElements, PlanElements planElems)
        {
            try
            {
                if (processedElements != null && processedElements.Count > 0)
                {
                    foreach (Object obj in processedElements)
                    {
                        if (obj.GetType() == typeof (Program))
                        {
                            if (!planElems.Programs.Contains(obj))
                            {
                                Program p = CloneProgram((Program) obj);
                                planElems.Programs.Add(p);
                            }
                        }
                        else if (obj.GetType() == typeof (Module))
                        {
                            if (!planElems.Modules.Contains(obj))
                            {
                                Module m = CloneModule((Module) obj);
                                planElems.Modules.Add(m);
                            }
                        }
                        else if (obj.GetType() == typeof (Actions))
                        {
                            if (!planElems.Actions.Contains(obj))
                            {
                                Actions a = CloneAction((Actions) obj);
                                planElems.Actions.Add(a);
                            }
                        }
                        else if (obj.GetType() == typeof (Step))
                        {
                            if (!planElems.Steps.Contains(obj))
                            {
                                Step s = CloneStep((Step) obj);
                                planElems.Steps.Add(s);
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

        public Module CloneModule(Module md)
        {
            try
            {
                Module m = new Module
                {
                    Archived = md.Archived,
                    ArchivedDate = md.ArchivedDate,
                    ArchiveOriginId = md.ArchiveOriginId,
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

        public Actions CloneAction(Actions ac)
        {
            try
            {
                Actions a = new Actions
                {
                    Archived = ac.Archived,
                    ArchivedDate = ac.ArchivedDate,
                    ArchiveOriginId = ac.ArchiveOriginId,
                    AssignById = ac.AssignById,
                    AssignToId = ac.AssignToId, // NIGHT-877
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
                    Archived = st.Archived,
                    ArchivedDate = st.ArchivedDate,
                    ArchiveOriginId = st.ArchiveOriginId,
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

        public Program CloneProgram(Program pr)
        {
            try
            {
                Program p = new Program
                {
                    Archived = pr.Archived,
                    ArchivedDate = pr.ArchivedDate,
                    ArchiveOriginId = pr.ArchiveOriginId,
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
                            pe = InitializePlanElementSettings(pe, m, program);
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
                                        pe = InitializePlanElementSettings(pe, a, program);
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
                                                    pe = InitializePlanElementSettings(pe, s, program);
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

        // NIGHT-950,951
        public PlanElement InitializePlanElementSettings(PlanElement pe, PlanElement p, Program program)
        {
            try
            {
                if (p.Enabled == true) return p;
                p.Enabled = true;
                p.AssignById = DD.Constants.SystemContactId;
                p.AssignDate = System.DateTime.UtcNow;
                p.StateUpdatedOn = DateTime.UtcNow;
                p.AssignToId = program.AssignToId;
                pe = p;
                return pe;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:InitializePlanElementSettings()::" + ex.Message,
                    ex.InnerException);
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
                            if (a.ElementState == (int) DD.ElementState.InProgress ||
                                a.ElementState == (int) DD.ElementState.Completed)
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
                        Completed = (int) programAttributeData.Completed,
                        CompletedBy = programAttributeData.CompletedBy,
                        DateCompleted = programAttributeData.DateCompleted,
                        DidNotEnrollReason = programAttributeData.DidNotEnrollReason,
                        Eligibility = (int) programAttributeData.Eligibility,
                        //AttrEndDate = programAttributeData.AttrEndDate, Sprint 12
                        Enrollment = (int) programAttributeData.Enrollment,
                        GraduatedFlag = (int) programAttributeData.GraduatedFlag,
                        Id = programAttributeData.Id.ToString(),
                        IneligibleReason = programAttributeData.IneligibleReason,
                        Locked = (int) programAttributeData.Locked,
                        OptOut = programAttributeData.OptOut,
                        OverrideReason = programAttributeData.OverrideReason,
                        PlanElementId = programAttributeData.PlanElementId.ToString(),
                        Population = programAttributeData.Population,
                        RemovedReason = programAttributeData.RemovedReason,
                        //AttrStartDate = programAttributeData.AttrStartDate, Sprint 12
                        Status = (int) programAttributeData.Status
                    };
                }
                return programAttribute;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:getAttributes()::" + ex.Message, ex.InnerException);
            }
        }

        public bool UpdatePlanElementAttributes(Program pg, PlanElement planElement, string userId,
            PlanElements planElems)
        {
            try
            {
                var result = false;
                var pes =
                    Enumerable.Repeat<PlanElement>(pg, 1)
                        .Concat(pg.Modules)
                        .Concat(pg.Modules.SelectMany(m => m.Actions))
                        .ToList();

                var fPe = pes.First(pe => pe.Id == planElement.Id);
                if (fPe == null) return false;

                if (_isModifyAllowed.IsSatisfiedBy(fPe))
                {
                    ProcessPlanElementChanges(planElems, planElement, fPe, userId);
                    result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:IsActionInitial()::" + ex.Message, ex.InnerException);
            }
        }

        public void ProcessPlanElementChanges(PlanElements planElems, PlanElement samplePe, PlanElement fPe, string userId)
        {
            try
            {
                switch (fPe.GetType().Name)
                {
                    case "Actions":
                    {
                        SetPlanElementAttributes(planElems, fPe, samplePe, userId);
                        break;
                    }
                    case "Module":
                    {
                        var list =
                            Enumerable.Repeat(fPe, 1)
                                .Concat(
                                    ((Module) fPe).Actions.Where(
                                        a => (a.ElementState == 2 || a.ElementState == 4) && a.Enabled == true)
                                        .ToList<PlanElement>());
                        list.ForEach(pe => SetPlanElementAttributes(planElems, pe, samplePe, userId));
                        break;
                    }
                    case "Program":
                    {
                        var prog = fPe as Program;
                        var list =
                            Enumerable.Repeat<PlanElement>(prog, 1)
                                .Concat(
                                    prog.Modules.Where(
                                        a => (a.ElementState == 2 || a.ElementState == 4) && a.Enabled == true))
                                .Concat(
                                    prog.Modules.Where(m => m.Enabled == true).SelectMany(m => m.Actions)
                                        .Where(a => (a.ElementState == 2 || a.ElementState == 4) && a.Enabled == true))
                                .ToList();

                        list.ForEach(pe => SetPlanElementAttributes(planElems, pe, samplePe, userId));
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:ProcessPlanElementChanges()::" + ex.Message, ex.InnerException);
            }
        }

        private void SetPlanElementAttributes(PlanElements planElems, PlanElement pe, PlanElement samplePe,
            string userId)
        {
            pe.AssignToId = samplePe.AssignToId;
            pe.AssignById = userId;
            pe.AssignDate = DateTime.UtcNow;

            if (pe.GetType() == typeof (Program))
                planElems.Programs.Add(pe as Program);
            else if (pe.GetType() == typeof (Module))
                planElems.Modules.Add(pe as Module);
            else if (pe.GetType() == typeof (Actions))
                planElems.Actions.Add(pe as Actions);
        }


        public Actions CloneRepeatAction(Actions action, string assignedTo)
        {
            try
            {
                ISpecification<Step> removeSelectedResponse = new RemoveSelectedResponseSpecification<Step>();
                var idBag = new Dictionary<string, string>();
                var cAct = action.Clone<Actions>();
                
                cAct.ArchiveOriginId = action.Id;
                cAct.Id = ObjectId.GenerateNewId().ToString();

                // set archived action properties
                // archive parent action
                action.Order = 999;
                action.Archived = true;
                action.ArchivedDate = System.DateTime.UtcNow;

                // reinitialize properties
                cAct.ElementState = 4;
                cAct.AssignDate = System.DateTime.UtcNow;
                cAct.Enabled = true;
                cAct.StateUpdatedOn = System.DateTime.UtcNow;
                cAct.ElementState = (int) DD.ElementState.NotStarted;
                cAct.AssignById = DD.Constants.SystemContactId;
                cAct.AssignToId = assignedTo;
                cAct.Completed = false;
                cAct.CompletedBy = "CM";
                cAct.DateCompleted = null;

                // update spawnelements
                if (cAct.SpawnElement != null)
                    cAct.SpawnElement.ForEach(sp =>
                    {
                        if (!string.IsNullOrEmpty(sp.ElementId))
                            sp.ElementId = GetUpdatedId(idBag, sp.ElementId);
                    });

                cAct.Steps.ForEach(s =>
                {
                    s.ActionId = cAct.Id;
                    var newId = ObjectId.GenerateNewId().ToString();
                    if (!idBag.ContainsKey(s.Id))
                        idBag.Add(s.Id, newId);

                    s.Id = newId;

                    // set updated date
                    s.Completed = false;
                    s.DateCompleted = null;
                    s.ElementState = 4;
                    s.AssignDate = System.DateTime.UtcNow;
                    s.Enabled = true;
                    s.StateUpdatedOn = System.DateTime.UtcNow;
                    s.ElementState = (int) DD.ElementState.NotStarted;
                    s.AssignToId = assignedTo;

                    if (!string.IsNullOrEmpty(s.Next))
                        if (!idBag.ContainsKey(s.Next)) 
                            idBag.Add(s.Next, ObjectId.GenerateNewId().ToString());

                    if (!string.IsNullOrEmpty(s.Previous))
                        if (!idBag.ContainsKey(s.Previous))
                            idBag.Add(s.Previous, ObjectId.GenerateNewId().ToString());

                    // update stepids
                    s.Responses.ForEach(r =>
                    {
                        r.StepId = newId;
                        var rid = ObjectId.GenerateNewId().ToString();

                        if (!idBag.ContainsKey(r.Id))
                            idBag.Add(r.Id, rid);

                        r.Id = rid;

                        // reset the selectedresponseid by step type
                        s.SelectedResponseId = string.Empty;
                        if (removeSelectedResponse.IsSatisfiedBy(s))
                            s.SelectedResponseId = r.Id;

                        r.Selected = false;
                        r.Value = string.Empty;
                    });
                });

                try
                {
                    // go through and update references
                    cAct.Steps.ForEach(s =>
                    {
                        if (!string.IsNullOrEmpty(s.Next))
                            s.Next = GetUpdatedId(idBag, s.Next);

                        if (!string.IsNullOrEmpty(s.Previous))
                            s.Previous = GetUpdatedId(idBag, s.Previous);

                        if (s.SpawnElement != null)
                            s.SpawnElement.ForEach(sp =>
                            {
                                if (!string.IsNullOrEmpty(sp.ElementId))
                                    sp.ElementId = GetUpdatedId(idBag, sp.ElementId);
                            });

                        s.Responses.ForEach(rp =>
                        {
                            if (!string.IsNullOrEmpty(rp.NextStepId))
                                rp.NextStepId = GetUpdatedId(idBag, rp.NextStepId);

                            if (rp.SpawnElement != null)
                                rp.SpawnElement.ForEach(sp =>
                                {
                                    if (!string.IsNullOrEmpty(sp.ElementId))
                                        sp.ElementId = GetUpdatedId(idBag, sp.ElementId);
                                });
                        });
                    });
                }
                catch (Exception ex)
                {
                    throw new Exception("Update References Failed." + ex.Message, ex.InnerException);    
                }

                return cAct;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:CloneRepeatAction()::" + ex.Message, ex.InnerException);
            }
        }

        private string GetUpdatedId(Dictionary<string, string> bag, string id) {
            try
            {
                var rId = id;
                if (bag.ContainsKey(id))
                {
                    rId = bag[id];
                }
                return rId;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:PlanElementUtil:GetUpdatedId()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
