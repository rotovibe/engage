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

        public static void SavePatientGoalTasks(PostPatientGoalRequest request)
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
                    Status = t.Status,
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
                throw;
            }
        }

        public static void SavePatientGoalInterventions(PostPatientGoalRequest request)
        {
            List<PatientInterventionData> pid = new List<PatientInterventionData>();
            request.Goal.Interventions.ForEach(i =>
            {
                pid.Add(new PatientInterventionData
                {
                    AssignedTo = i.AssignedTo,
                    Attributes = GetAttributeData(i.Attributes),
                    Barriers = i.Barriers,
                    Category = i.Category,
                    Description = i.Description,
                    Id = i.Id,
                    Order = i.Order,
                    PatientGoalId = i.PatientGoalId,
                    StartDate = i.StartDate,
                    Status = i.Status,
                    StatusDate = i.StatusDate
                });
            });

            pid.ForEach(pi =>
            {
                GoalsEndpointUtil.PostUpdateInterventionRequest(request, pi);
            });
        }
    }
}
