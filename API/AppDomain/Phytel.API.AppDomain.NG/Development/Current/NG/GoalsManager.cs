using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.ServiceClient.Web;
using Phytel.API.AppDomain.NG.PlanSpecification;
using Phytel.API.AppDomain.NG.PlanCOR;
using ServiceStack.Service;
using DD = Phytel.API.DataDomain.Program.DTO;
using System.Configuration;
using Phytel.API.DataDomain.PatientGoal.DTO;

namespace Phytel.API.AppDomain.NG
{
    public class GoalsManager : ManagerBase
    {

        public PostInitializeGoalResponse PostInitialGoalRequest(PostInitializeGoalRequest request)
        {
            try
            {
                PostInitializeGoalResponse response = new PostInitializeGoalResponse();
                string id = GoalsEndpointUtil.PostInitialGoalRequest(request);
                response.Id = id;
                response.Version = request.Version;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PostInitializeBarrierResponse PostInitialBarrierRequest(PostInitializeBarrierRequest request)
        {
            try
            {
                PostInitializeBarrierResponse response = new PostInitializeBarrierResponse();
                string id = GoalsEndpointUtil.PostInitialBarrierRequest(request);
                response.Id = id;
                response.Version = request.Version;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public GetInitializeTaskResponse GetInitialTask(GetInitializeTaskRequest request)
        {
            try
            {
                GetInitializeTaskResponse itr = new GetInitializeTaskResponse();
                string id = GoalsEndpointUtil.GetInitialTaskRequest(request);
                itr.Id = id;
                itr.Version = request.Version;
                return itr;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public GetInitializeInterventionResponse GetInitialIntervention(GetInitializeInterventionRequest request)
        {
            try
            {
                GetInitializeInterventionResponse itr = new GetInitializeInterventionResponse();
                string id = GoalsEndpointUtil.GetInitialInterventionRequest(request);
                itr.Id = id;
                itr.Version = request.Version;
                return itr;
            }
            catch
            {
                throw;
            }
        }

        public PostPatientGoalResponse SavePatientGoal(PostPatientGoalRequest request)
        {
            try
            {
                PostPatientGoalResponse pgr = new PostPatientGoalResponse();
                
                // save goals
                bool result = GoalsEndpointUtil.PostUpdateGoalRequest(request);

                // save barriers

                // save tasks
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

                // save interventions
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

                pgr.Result = result;

                return pgr;
            }
            catch
            {
                throw;
            }
        }

        private List<AttributeData> GetAttributeData(List<DTO.Attribute> list)
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
    }
}
