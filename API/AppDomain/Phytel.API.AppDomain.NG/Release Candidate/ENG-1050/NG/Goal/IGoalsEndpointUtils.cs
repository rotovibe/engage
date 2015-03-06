using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.Interface;
using ServiceStack.Service;

namespace Phytel.API.AppDomain.NG
{
    public interface IGoalsEndpointUtils
    {
        PatientGoalData GetInitialGoalRequest(GetInitializeGoalRequest request);
        string GetInitialBarrierRequest(GetInitializeBarrierRequest request);
        PatientTaskData GetInitialTaskRequest(GetInitializeTaskRequest request);
        PatientGoal GetPatientGoal(GetPatientGoalRequest request);
        List<PatientGoalView> GetAllPatientGoals(GetAllPatientGoalsRequest request);
        List<PatientIntervention> GetInterventions(GetInterventionsRequest request);
        List<PatientTask> GetTasks(GetTasksRequest request);
        bool DeleteGoalRequest(PostDeletePatientGoalRequest request);
        bool DeleteBarrierRequest(PostDeletePatientGoalRequest request, string id);
        bool DeleteTaskRequest(PostDeletePatientGoalRequest request, string id);
        bool DeleteInterventionRequest(PostDeletePatientGoalRequest request, string id);
        List<CustomAttribute> GetAttributesLibraryByType(IAppDomainRequest request, int typeId);
        bool PostUpdateInterventionRequest(PostPatientGoalRequest request, PatientInterventionData pi);
        bool PostUpdateBarrierRequest(PostPatientGoalRequest request, PatientBarrierData bd);
        bool PostUpdateTaskRequest(PostPatientGoalRequest request, PatientTaskData td);
        string GetInitialInterventionRequest(GetInitializeInterventionRequest request);
        PatientGoal PostUpdateGoalRequest(PostPatientGoalRequest request);
        PatientIntervention PostUpdateInterventionRequest(PostPatientInterventionRequest request);
        PatientTask PostUpdateTaskRequest(PostPatientTaskRequest request);
        PatientBarrier PostUpdateBarrierRequest(PostPatientBarrierRequest request);
        PatientGoalData convertToPatientGoalData(PatientGoal pg);
        List<CustomAttributeData> GetPatientGoalAttributes(List<DTO.CustomAttribute> list);
        List<string> GetTaskIdsForRequest(List<PatientTask> list);
        List<string> GetInterventionIdsForRequest(List<PatientIntervention> list);
        List<string> GetBarrierIdsForRequest(List<PatientBarrier> list);
        PatientDetails GetPatientDetails(double version, string contractNumber, string userId, IRestClient client, string patientId);
    }
}