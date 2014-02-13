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
    }
}
