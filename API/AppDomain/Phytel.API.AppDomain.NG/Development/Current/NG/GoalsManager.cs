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

        public GetInitializeGoalResponse GetInitialGoalRequest(GetInitializeGoalRequest request)
        {
            try
            {
                GetInitializeGoalResponse response = new GetInitializeGoalResponse();
                string id = GoalsEndpointUtil.GetInitialGoalRequest(request);
                response.Id = id;
                response.Version = request.Version;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
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
                throw ex;
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
                throw ex;
            }
        }

        public PostPatientGoalResponse SavePatientGoal(PostPatientGoalRequest request)
        {
            try
            {
                PostPatientGoalResponse pgr = new PostPatientGoalResponse();
                bool result = false;

                // we can thread here if we want!
                // save goals
                result = GoalsEndpointUtil.PostUpdateGoalRequest(request);

                // save barriers
                result = GoalsUtil.SavePatientGoalBarriers(request);

                // save tasks
                result = GoalsUtil.SavePatientGoalTasks(request);

                // save interventions
                result = GoalsUtil.SavePatientGoalInterventions(request);

                pgr.Result = result;

                return pgr;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:SavePatientGoal:" + ex.Message, ex.InnerException);
            }
        }

        public PostDeletePatientGoalResponse DeletePatientGoal(PostDeletePatientGoalRequest request)
        {
            try
            {
                PostDeletePatientGoalResponse pgr = new PostDeletePatientGoalResponse();
                bool result = false;

                // we can thread here if we want!
                // save goals
                result = GoalsEndpointUtil.DeleteGoalRequest(request);

                // save barriers
                result = GoalsUtil.DeletePatientGoalBarriers(request);

                // save tasks
                result = GoalsUtil.DeletePatientGoalTasks(request);

                // save interventions
                result = GoalsUtil.DeletePatientGoalInterventions(request);

                pgr.Result = result;

                return pgr;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:SavePatientGoal:" + ex.Message, ex.InnerException);
            }
        }
    }
}
