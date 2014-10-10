using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientGoal.DTO;

namespace Phytel.API.DataDomain.PatientGoal
{
    public interface IPatientGoalDataManager
    {
        PutInitializeGoalDataResponse InitializeGoal(PutInitializeGoalDataRequest request);
        PutInitializeBarrierDataResponse InitializeBarrier(PutInitializeBarrierDataRequest request);
        GetPatientGoalDataResponse GetPatientGoal(GetPatientGoalDataRequest request);
        GetAllPatientGoalsDataResponse GetPatientGoalList(GetAllPatientGoalsDataRequest request);
        PutPatientGoalDataResponse PutPatientGoal(PutPatientGoalDataRequest request);
        PutInitializeTaskResponse InsertNewPatientTask(PutInitializeTaskRequest request);
        PutUpdateTaskResponse UpdatePatientTask(PutUpdateTaskRequest request);
        PutUpdateInterventionResponse UpdatePatientIntervention(PutUpdateInterventionRequest request);
        PutUpdateBarrierResponse UpdatePatientBarrier(PutUpdateBarrierRequest request);
        PutInitializeInterventionResponse InsertNewPatientIntervention(PutInitializeInterventionRequest request);
        DeletePatientGoalDataResponse DeletePatientGoal(DeletePatientGoalDataRequest request);
        DeleteTaskDataResponse DeleteTask(DeleteTaskDataRequest request);
        DeleteInterventionDataResponse DeleteIntervention(DeleteInterventionDataRequest request);
        DeleteBarrierDataResponse DeleteBarrier(DeleteBarrierDataRequest request);
        DeletePatientGoalByPatientIdDataResponse DeletePatientGoalByPatientId(DeletePatientGoalByPatientIdDataRequest request);
        UndoDeletePatientGoalDataResponse UndoDeletePatientGoals(UndoDeletePatientGoalDataRequest request);
        GetCustomAttributesDataResponse GetCustomAttributesByType(GetCustomAttributesDataRequest request);
        RemoveProgramInPatientGoalsDataResponse RemoveProgramInPatientGoals(RemoveProgramInPatientGoalsDataRequest request);
        GetInterventionsDataResponse GetInterventions(GetInterventionsDataRequest request);
        GetTasksDataResponse GetTasks(GetTasksDataRequest request);
    }
}
