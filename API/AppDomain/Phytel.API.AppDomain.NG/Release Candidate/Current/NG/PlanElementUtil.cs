using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanSpecification;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using DD = Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.AppDomain.NG
{
    public static class PlanElementUtil
    {
        /// <summary>
        /// Checks to see if completion status count is equal to the list count. Return true if equal.
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="list">PlanElement list</param>
        /// <returns></returns>
        public static bool SetCompletionStatus<T>(List<T> list)
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

        public static T FindElementById<T>(List<T> list, string id)
        {
            var mod = list.Where(r => ((IPlanElement)Convert.ChangeType(r, typeof(T))).Id == id).FirstOrDefault();
            return mod;
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
                throw new Exception("AppDomain:SetEnabledStatusByPrevious():" + ex.Message, ex.InnerException);
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
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:SetEnabledState():" + ex.Message, ex.InnerException);
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
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:DisableCompleteButtonForAction():" + ex.Message, ex.InnerException);
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
                        SetEnabledStateRecursion(r.ElementId, program);
                    }
                    else
                    {
                        SetProgramAttributes(r, program, userId, progAttr );
                    }
                });
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:SpawnElementsInList():" + ex.Message, ex.InnerException);
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
                    progAttr.Eligibility = (!string.IsNullOrEmpty(r.Tag))? Convert.ToInt32(r.Tag) : 0;

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

                    progAttr.Enrollment = (!string.IsNullOrEmpty(r.Tag)) ? Convert.ToInt32(r.Tag) : 0;
                }
                else if (r.ElementType.Equals(16))
                {
                    if (r.Tag == null)
                        throw new ArgumentException("Cannot set attribute of type " + r.ElementType + ". Tag value is null.");

                    progAttr.OptOut = (!string.IsNullOrEmpty(r.Tag))? r.Tag : null;
                }
                else if (r.ElementType.Equals(17))
                {
                    // do something with opt out
                    if (r.Tag == null)
                        throw new ArgumentException("Cannot set attribute of type " + r.ElementType + ". Tag value is null.");

                    progAttr.OptOutReason = (!string.IsNullOrEmpty(r.Tag))? r.Tag : null;
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

                    progAttr.GraduatedFlag = (!string.IsNullOrEmpty(r.Tag))? Convert.ToInt32(r.Tag) : 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:SpawnElementsInList():" + ex.Message, ex.InnerException);
            }
        }

        private static void SetEnabledStateRecursion(string p, Program program)
        {
            foreach (Module m in program.Modules)
            {
                if (m.Id.ToString().Equals(p))
                {
                    SetInitialProperties(m);
                }
                else
                {
                    FindIdInActions(p, m);
                }
            }
        }

        private static void FindIdInActions(string p, Module m)
        {
            if (m.Actions != null)
            {
                foreach (Actions a in m.Actions)
                {
                    if (a.Id.ToString().Equals(p))
                    {
                        SetInitialProperties(a);
                    }
                    else
                    {
                        FindIdInSteps(p, a);
                    }
                }
            }
        }

        private static void FindIdInSteps(string p, Actions a)
        {
            if (a.Steps != null)
            {
                foreach (Step s in a.Steps)
                {
                    if (s.Id.ToString().Equals(p))
                    {
                        SetInitialProperties(s);
                    }
                }
            }
        }

        private static void SetInitialProperties(IPlanElement m)
        {
            m.Enabled = true;
            m.AssignDate = System.DateTime.UtcNow;
            m.ElementState = 0;
            m.AssignBy = "System";
        }

        public static Actions GetProcessingAction(List<Module> list, string actionId)
        {
            Actions query = list.SelectMany(module => module.Actions).Where(action => action.Id == actionId).FirstOrDefault();
            return query;
        }

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
                throw new Exception("AppDomain:SpawnElementsInList():" + ex.Message, ex.InnerException);
            }
        }

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
                throw new Exception("AppDomain:RegisterProblemCodeToPatient():" + ex.Message, ex.InnerException);
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
                throw new Exception("AppDomain:GetCohortPatientViewRecord():" + ex.Message, ex.InnerException);
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
                throw new Exception("AppDomain:SaveReportingAttributes():" + ex.Message, ex.InnerException);
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
                if (_pAtt.OptOut != null){ pAtt.OptOut = _pAtt.OptOut; dirty = true;}
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
                throw new Exception("AppDomain:ModifyProgramAttributePropertiesForUpdate():" + ex.Message, ex.InnerException);
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
                throw new Exception("AppDomain:SetStartDateForProgramAttributes():" + ex.Message, ex.InnerException);
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
                _programAttributes.OptOut = null;
                _programAttributes.EligibilityOverride = 1;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
