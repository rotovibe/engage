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

        public PostPatientGoalResponse SavePatientGoal(PostPatientGoalRequest request)
        {
            try
            {
                PostPatientGoalResponse pgr = new PostPatientGoalResponse();
                
                // save goals
                bool result = GoalsEndpointUtil.PostUpdateGoalRequest(request);

                // save barriers

                // save tasks
                GoalsUtil.SavePatientGoalTasks(request);

                // save interventions
                GoalsUtil.SavePatientGoalInterventions(request);

                pgr.Result = result;

                return pgr;
            }
            catch
            {
                throw;
            }
        }
    }
}
