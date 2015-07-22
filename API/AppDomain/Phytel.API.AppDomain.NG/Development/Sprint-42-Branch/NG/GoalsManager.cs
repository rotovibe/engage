using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using ServiceStack.ServiceClient.Web;
using Phytel.API.AppDomain.NG.PlanCOR;
using ServiceStack.Service;
using DD = Phytel.API.DataDomain.Program.DTO;
using System.Configuration;
using Phytel.API.DataDomain.PatientGoal.DTO;

namespace Phytel.API.AppDomain.NG
{
    public class GoalsManager : ManagerBase
    {

        #region Initialize
        public GetInitializeGoalResponse GetInitialGoalRequest(GetInitializeGoalRequest request)
        {
            try
            {
                GetInitializeGoalResponse response = new GetInitializeGoalResponse();
                PatientGoalData pg = (PatientGoalData)GoalsEndpointUtil.GetInitialGoalRequest(request);
                response.Goal = GoalsUtil.GetPatientGoalForInitialize(request, pg);
                response.Version = request.Version;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetInitialGoalRequest()::" + ex.Message, ex.InnerException);
            }
        }

        public GetInitializeBarrierResponse GetInitialBarrierRequest(GetInitializeBarrierRequest request)
        {
            try
            {
                GetInitializeBarrierResponse response = new GetInitializeBarrierResponse();
                string id = GoalsEndpointUtil.GetInitialBarrierRequest(request);
                response.Id = id;
                response.Version = request.Version;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetInitialBarrierRequest()::" + ex.Message, ex.InnerException);
            }
        }

        public GetInitializeTaskResponse GetInitialTask(GetInitializeTaskRequest request)
        {
            try
            {
                GetInitializeTaskResponse itr = new GetInitializeTaskResponse();
                PatientTaskData ptd = (PatientTaskData)GoalsEndpointUtil.GetInitialTaskRequest(request);
                PatientTask task = GoalsUtil.GetPatientTaskForInitialize(request, ptd);
                itr.Task = task;
                itr.Version = request.Version;
                return itr;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetInitialTask()::" + ex.Message, ex.InnerException);
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
            catch (Exception ex)
            {
                throw new Exception("AD:GetInitialIntervention()::" + ex.Message, ex.InnerException);
            }
        } 
        #endregion

        #region Gets
        public GetPatientGoalResponse GetPatientGoal(GetPatientGoalRequest request)
        {
            try
            {
                GetPatientGoalResponse response = new GetPatientGoalResponse();
                response.Goal = GoalsEndpointUtil.GetPatientGoal(request);
                response.Version = request.Version;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetPatientGoal()::" + ex.Message, ex.InnerException);
            }
        }

        public GetAllPatientGoalsResponse GetAllPatientGoals(GetAllPatientGoalsRequest request)
        {
            try
            {
                GetAllPatientGoalsResponse response = new GetAllPatientGoalsResponse();
                response.Goals = GoalsEndpointUtil.GetAllPatientGoals(request);
                response.Version = request.Version;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetAllPatientGoals()::" + ex.Message, ex.InnerException);
            }
        }

        public GetInterventionsResponse GetInterventions(GetInterventionsRequest request)
        {
            try
            {
                GetInterventionsResponse response = new GetInterventionsResponse();
                response.Interventions = GoalsEndpointUtil.GetInterventions(request);
                response.Version = request.Version;
                return response;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetInterventions()::" + ex.Message, ex.InnerException);
            }
        }

        public GetTasksResponse GetTasks(GetTasksRequest request)
        {
            try
            {
                GetTasksResponse response = new GetTasksResponse();
                response.Tasks = GoalsEndpointUtil.GetTasks(request);
                response.Version = request.Version;
                return response;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetTasks()::" + ex.Message, ex.InnerException);
            }
        } 
        #endregion

        #region Save
        public PostPatientGoalResponse SavePatientGoal(PostPatientGoalRequest request)
        {
            try
            {
                PostPatientGoalResponse pgr = new PostPatientGoalResponse();
                pgr.Goal = GoalsEndpointUtil.PostUpdateGoalRequest(request);
                pgr.Version = request.Version;
                return pgr;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:SavePatientGoal()::" + ex.Message, ex.InnerException);
            }
        }

        public PostPatientInterventionResponse SavePatientIntervention(PostPatientInterventionRequest request)
        {
            try
            {
                PostPatientInterventionResponse response = new PostPatientInterventionResponse();
                response.Intervention = GoalsEndpointUtil.PostUpdateInterventionRequest(request);
                response.Version = request.Version;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:SavePatientIntervention()::" + ex.Message, ex.InnerException);
            }
        }

        public PostPatientTaskResponse SavePatientTask(PostPatientTaskRequest request)
        {
            try
            {
                PostPatientTaskResponse response = new PostPatientTaskResponse();
                response.Task = GoalsEndpointUtil.PostUpdateTaskRequest(request);
                response.Version = request.Version;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:SavePatientTask()::" + ex.Message, ex.InnerException);
            }
        }

        public PostPatientBarrierResponse SavePatientBarrier(PostPatientBarrierRequest request)
        {
            try
            {
                PostPatientBarrierResponse response = new PostPatientBarrierResponse();
                response.Barrier = GoalsEndpointUtil.PostUpdateBarrierRequest(request);
                response.Version = request.Version;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:SavePatientBarrier()::" + ex.Message, ex.InnerException);
            }
        } 
        #endregion

        #region Delete
        public PostDeletePatientGoalResponse DeletePatientGoal(PostDeletePatientGoalRequest request)
        {
            try
            {
                PostDeletePatientGoalResponse pgr = new PostDeletePatientGoalResponse();
                bool result = false;

                // we can thread here if we want!
                // save goals
                GoalsEndpointUtil.DeleteGoalRequest(request);

                // save barriers
                GoalsUtil.DeletePatientGoalBarriers(request);

                // save tasks
                GoalsUtil.DeletePatientGoalTasks(request);

                // save interventions
                GoalsUtil.DeletePatientGoalInterventions(request);

                pgr.Result = result;

                return pgr;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:DeletePatientGoal()::" + ex.Message, ex.InnerException);
            }
        } 
        #endregion
    }
}
