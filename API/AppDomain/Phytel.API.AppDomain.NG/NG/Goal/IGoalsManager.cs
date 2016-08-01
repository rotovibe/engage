using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG
{
    public interface IGoalsManager
    {
        GetInitializeGoalResponse GetInitialGoalRequest(GetInitializeGoalRequest request);
        GetInitializeBarrierResponse GetInitialBarrierRequest(GetInitializeBarrierRequest request);
        GetInitializeTaskResponse GetInitialTask(GetInitializeTaskRequest request);
        GetInitializeInterventionResponse GetInitialIntervention(GetInitializeInterventionRequest request);
        GetPatientGoalResponse GetPatientGoal(GetPatientGoalRequest request);
        GetAllPatientGoalsResponse GetAllPatientGoals(GetAllPatientGoalsRequest request);
        GetInterventionsResponse GetInterventions(GetInterventionsRequest request);
        GetTasksResponse GetTasks(GetTasksRequest request);
        PostPatientGoalResponse SavePatientGoal(PostPatientGoalRequest request);
        PostPatientInterventionResponse SavePatientIntervention(PostPatientInterventionRequest request);
        PostPatientTaskResponse SavePatientTask(PostPatientTaskRequest request);
        PostPatientBarrierResponse SavePatientBarrier(PostPatientBarrierRequest request);
        PostDeletePatientGoalResponse DeletePatientGoal(PostDeletePatientGoalRequest request);
    }
}
