using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientProblem.DTO;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Interface;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DD = Phytel.API.DataDomain.Program.DTO;
using AD = Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientGoal.DTO;

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
                    Attributes = GetAttributeData(t.Attributes),
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

        public static List<AttributeData> GetAttributeData(List<DTO.Attribute> list)
        {
            try
            {
                List<AttributeData> ad = new List<AttributeData>();
                if (list != null && list.Count > 0)
                {

                    list.ForEach(a =>
                    {
                        ad.Add(new AttributeData
                        {
                            Value = a.Value,
                            Order = a.Order,
                            Name = a.Name,
                            ControlType = a.ControlType
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
                        result = GoalsEndpointUtil.DeleteBarrierRequest(request, bd.Id);
                    });
                }
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
                if (request.Goal.Tasks != null && request.Goal.Tasks.Count > 0)
                {
                    List<PatientTaskData> ptd = new List<PatientTaskData>();
                    request.Goal.Tasks.ForEach(t =>
                    {
                        ptd.Add(new PatientTaskData
                        {
                            Id = t.Id,
                            Attributes = GetAttributeData(t.Attributes),
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
                        GoalsEndpointUtil.DeleteTaskRequest(request, td.Id);
                    });
                }
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
                if (request.Goal.Interventions != null && request.Goal.Interventions.Count > 0)
                {
                    List<PatientInterventionData> pid = new List<PatientInterventionData>();
                    request.Goal.Interventions.ForEach(i =>
                    {
                        pid.Add(new PatientInterventionData
                        {
                            AssignedToId = i.AssignedToId,
                            Barriers = i.BarrierIds,
                            CategoryId = i.CategoryId != null ? i.CategoryId.ToString() : null,
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
                        result = GoalsEndpointUtil.DeleteInterventionRequest(request, pi.Id);
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:DeletePatientGoalInterventions():" + ex.Message, ex.InnerException);
            }
        }

        public static List<Phytel.API.AppDomain.NG.DTO.Attribute> GetAttributes(List<AttributeData> list)
        {
            List<Phytel.API.AppDomain.NG.DTO.Attribute> attr = null;
            if (list != null && list.Count > 0)
            {
                attr = new List<Phytel.API.AppDomain.NG.DTO.Attribute>();
                foreach(AttributeData ad in list)
                {
                    attr.Add(new Phytel.API.AppDomain.NG.DTO.Attribute { Name = ad.Name, Value = ad.Value, ControlType = ad.ControlType, Order = ad.Order });
                }
            }
            return attr;
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
                            PatientGoalId = b.Name, 
                            StatusDate = b.StatusDate
                        });
                }
            }
            return barrierList;
        }

        public static List<PatientTask> GetTasks(List<PatientTaskData> list)
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
                        Attributes = GetAttributes(t.Attributes),
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
    }
}
