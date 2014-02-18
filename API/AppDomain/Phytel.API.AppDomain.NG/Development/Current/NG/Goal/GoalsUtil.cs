﻿using Phytel.API.AppDomain.NG.DTO;
using System;
using System.Collections.Generic;
using AD = Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.AppDomain.NG.DTO.Goal;

namespace Phytel.API.AppDomain.NG
{
    public static class GoalsUtil
    {

        public static bool SavePatientGoalTasks(PostPatientGoalRequest request)
        {
            bool result = false;
            try
            {
                if (request.Goal.Tasks != null && request.Goal.Tasks.Count > 0)
                {
            List<PatientTaskData> ptd = new List<PatientTaskData>();
            request.Goal.Tasks.ForEach(t =>
            {
                ptd.Add(new PatientTaskData
                {
                    Id = t.Id,
                    Attributes = GetAttributeData(t.CustomAttributes),
                    Barriers = t.BarrierIds,
                    Description = t.Description,
                    PatientGoalId = request.Goal.Id,
                    StartDate = t.StartDate,
                            StatusId = t.StatusId,
                    StatusDate = t.StatusDate,
                    TargetDate = t.TargetDate,
                    TargetValue = t.TargetValue
                });
            });

            ptd.ForEach(td =>
            {
                GoalsEndpointUtil.PostUpdateTaskRequest(request, td);
            });
        }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:SavePatientGoalTasks():" + ex.Message, ex.InnerException);
            }
        }

        public static List<CustomAttributeData> GetAttributeData(List<DTO.CustomAttribute> list)
        {
            try
            {
                List<CustomAttributeData> ad = new List<CustomAttributeData>();
                if (list != null && list.Count > 0)
                {

                    list.ForEach(a =>
                    {
                        ad.Add(new CustomAttributeData
                        {
                            Id = a.Id,
                            Values = a.Values
                        });
                    });
                }
                return ad;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetAttributeData():" + ex.Message, ex.InnerException);
            }
        }

        public static bool SavePatientGoalInterventions(PostPatientGoalRequest request)
        {
            bool result = false;
            try
            {
                if (request.Goal.Interventions != null && request.Goal.Interventions.Count > 0)
        {
            List<PatientInterventionData> pid = new List<PatientInterventionData>();
            request.Goal.Interventions.ForEach(i =>
            {
                pid.Add(new PatientInterventionData
                {
                    AssignedToId = i.AssignedToId,
                    Barriers = i.BarrierIds,
                    CategoryId = i.CategoryId,
                    Description = i.Description,
                    Id = i.Id,
                    PatientGoalId = i.PatientGoalId,
                    StartDate = i.StartDate,
                            StatusId = i.StatusId,
                    StatusDate = i.StatusDate
                });
            });

            pid.ForEach(pi =>
            {
                        result = GoalsEndpointUtil.PostUpdateInterventionRequest(request, pi);
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:SavePatientGoalInterventions():" + ex.Message, ex.InnerException);
            }
        }

        public static bool SavePatientGoalBarriers(PostPatientGoalRequest request)
        {
            bool result = false;
            try
            {
                if (request.Goal.Barriers != null && request.Goal.Barriers.Count > 0)
                {
                    List<PatientBarrierData> pbd = new List<PatientBarrierData>();
                    request.Goal.Barriers.ForEach(b =>
                    {
                        pbd.Add(new PatientBarrierData
                        {
                            CategoryId = b.CategoryId,
                            Id = b.Id,
                            Name = b.Name,
                            PatientGoalId = b.PatientGoalId,
                            StatusId = b.StatusId,
                            StatusDate = b.StatusDate
                        });
                    });

                    pbd.ForEach(bd =>
                    {
                        result = GoalsEndpointUtil.PostUpdateBarrierRequest(request, bd);
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:SavePatientGoalBarriers():" + ex.Message, ex.InnerException);
            }
        }
        internal static bool DeletePatientGoalBarriers(PostDeletePatientGoalRequest request)
        {
            bool result = false;
            try
            {
                result = GoalsEndpointUtil.DeleteBarrierRequest(request, request.PatientGoalId);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:DeletePatientGoalBarriers():" + ex.Message, ex.InnerException);
            }
        }

        internal static bool DeletePatientGoalTasks(PostDeletePatientGoalRequest request)
        {
            bool result = false;
            try
            {

                GoalsEndpointUtil.DeleteTaskRequest(request, request.PatientGoalId);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:DeletePatientGoalTasks():" + ex.Message, ex.InnerException);
            }
        }

        internal static bool DeletePatientGoalInterventions(PostDeletePatientGoalRequest request)
        {
            bool result = false;
            try
            {
                result = GoalsEndpointUtil.DeleteInterventionRequest(request, request.PatientGoalId);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:DeletePatientGoalInterventions():" + ex.Message, ex.InnerException);
            }
        }

        public static List<CustomAttribute> GetCustomAttributeDetails(List<CustomAttributeData> list, List<CustomAttribute> attributesLibrary)
        {
            List<CustomAttribute> attrList = null;
            if (list != null && list.Count > 0)
            {
                attrList = new List<CustomAttribute>();
                foreach(CustomAttributeData cad in list)
                {
                    var libraryItem = attributesLibrary.Find(i => i.Id == cad.Id);
                    if (libraryItem != null)
                    {
                        attrList.Add(new CustomAttribute { 
                              Id = cad.Id, Name = libraryItem.Name, ControlType = libraryItem.ControlType, Options = libraryItem.Options, Order = libraryItem.Order, Required = libraryItem.Required, Values =  cad.Values
                        });    
                    }
                }
            }
            return attrList;
        }

        public static List<PatientBarrier> GetBarriers(List<PatientBarrierData> list)
        {
            List<PatientBarrier> barrierList = null;
            if (list != null && list.Count > 0)
            {
                barrierList = new List<PatientBarrier>();
                foreach (PatientBarrierData b in list)
                {
                    barrierList.Add(new PatientBarrier 
                        { 
                            Id = b.Id, 
                            StatusId = b.StatusId,
                            CategoryId = b.CategoryId,
                            Name = b.Name,
                            PatientGoalId = b.PatientGoalId, 
                            StatusDate = b.StatusDate
                        });
                }
            }
            return barrierList;
        }

        public static List<PatientTask> GetTasks(List<PatientTaskData> list, List<CustomAttribute> taskAttributesLibrary)
        {
            List<PatientTask> taskList = null;
            if (list != null && list.Count > 0)
            {
                taskList = new List<PatientTask>();
                foreach (PatientTaskData t in list)
                {
                    taskList.Add(new PatientTask
                    {
                        Id = t.Id,
                        PatientGoalId = t.PatientGoalId,
                        TargetValue = t.TargetValue,
                        StatusId = t.StatusId,
                        TargetDate = t.TargetDate,
                        CustomAttributes = GetCustomAttributeDetails(t.Attributes, taskAttributesLibrary),
                        BarrierIds = t.Barriers,
                        Description = t.Description,
                        StatusDate = t.StatusDate,
                        StartDate  = t.StartDate
                    });
                }
            }
            return taskList;
        }

        public static List<PatientIntervention> GetInterventions(List<PatientInterventionData> list)
        {
            List<PatientIntervention> interventionList = null;
            if (list != null && list.Count > 0)
            {
                interventionList = new List<PatientIntervention>();
                foreach (PatientInterventionData i in list)
                {
                    interventionList.Add(new PatientIntervention
                    {
                        Id = i.Id,
                        PatientGoalId = i.PatientGoalId,
                        CategoryId = i.CategoryId,
                        StatusId = i.StatusId,
                        AssignedToId = i.AssignedToId,
                        BarrierIds = i.Barriers,
                        Description = i.Description,
                        StatusDate = i.StatusDate,
                        StartDate = i.StartDate
                    });
                }
            }
            return interventionList;
        }

        public static List<ChildView> GetChildView(List<ChildViewData> list)
        {
            List<ChildView> cvList = null;
            if (list != null && list.Count > 0)
            {
                cvList = new List<ChildView>();
                foreach (ChildViewData c in list)
                {
                    cvList.Add(new ChildView {Id = c.Id, PatientGoalId = c.PatientGoalId , Name = c.Name , Description = c.Description, StatusId = c.StatusId });
                }
            }
            return cvList;
        }

        internal static PatientGoal GetPatientGoalForInitialize(GetInitializeGoalRequest request, PatientGoalData pgd)
        {
            PatientGoal pg = null;
            if (pgd != null)
            {
                pg = new PatientGoal
                {
                    CustomAttributes = GoalsEndpointUtil.GetAttributesLibraryByType(request, 1), //GetAttributesForInitialize(pgd.Attributes), // change this call when attributes are ready
                    EndDate = pgd.EndDate,
                    Id = pgd.Id,
                    Name = pgd.Name,
                    PatientId = pgd.PatientId,
                    SourceId = pgd.SourceId,
                    StartDate = pgd.StartDate,
                    StatusId = pgd.StatusId,
                    TargetDate = pgd.TargetDate,
                    TargetValue = pgd.TargetValue,
                    TypeId = pgd.TypeId
                };
            }
            return pg;
        }

        internal static PatientTask GetPatientTaskForInitialize(GetInitializeTaskRequest request, PatientTaskData ptd)
        {
            PatientTask pt = null;
            if (ptd != null)
            {
                pt = new PatientTask
                {
                    CustomAttributes = GoalsEndpointUtil.GetAttributesLibraryByType(request, 2),
                    Id = ptd.Id,
                    StartDate = ptd.StartDate,
                    StatusId = ptd.StatusId,
                    TargetDate = ptd.TargetDate,
                    TargetValue = ptd.TargetValue,
                };
            }
            return pt;
        }

        internal static List<AD.Goal.Option> ConvertToOptionList(Dictionary<int, string> dictionary)
        {
            try
            {
                List<Option> options = new List<Option>();

                foreach (KeyValuePair<int, string> e in dictionary)
                {
                    options.Add(new Option { Display = e.Value, Value = e.Key });
                }

                return options;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
