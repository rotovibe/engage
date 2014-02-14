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
                            Barriers = t.Barriers,
                            Description = t.Description,
                            Order = t.Order,
                            PatientGoalId = request.Goal.Id,
                            StartDate = t.StartDate,
                            StatusId = t.Status,
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
                            AssignedTo = i.AssignedTo,
                            Attributes = GetAttributeData(i.Attributes),
                            Barriers = i.Barriers,
                            CategoryId = i.Category.ToString(),
                            Description = i.Description,
                            Id = i.Id,
                            Order = i.Order,
                            PatientGoalId = i.PatientGoalId,
                            StartDate = i.StartDate,
                            StatusId = i.Status,
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
                            CategoryId = b.Category,
                            Id = b.Id,
                            Name = b.Name,
                            PatientGoalId = b.PatientGoalId,
                            StatusId = b.Status,
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
    }
}
