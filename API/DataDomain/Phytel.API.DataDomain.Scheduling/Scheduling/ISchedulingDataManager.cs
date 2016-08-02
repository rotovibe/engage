using System.Collections.Generic;
using Phytel.API.Common;
using Phytel.API.DataDomain.Scheduling.DTO;

namespace Phytel.API.DataDomain.Scheduling
{
    public interface ISchedulingDataManager
    {
        GetToDosDataResponse GetToDos(GetToDosDataRequest request);
        PutInsertToDoDataResponse InsertToDo(PutInsertToDoDataRequest request);
        PutUpdateToDoDataResponse UpdateToDo(PutUpdateToDoDataRequest request);
        GetScheduleDataResponse GetSchedule(GetScheduleDataRequest request);
        RemoveProgramInToDosDataResponse RemoveProgramInToDos(RemoveProgramInToDosDataRequest request);
        DeleteToDoByPatientIdDataResponse DeleteToDoByPatientId(DeleteToDoByPatientIdDataRequest request);
        UndoDeletePatientToDosDataResponse UndoDeleteToDos(UndoDeletePatientToDosDataRequest request);
        List<HttpObjectResponse<ToDoData>> InsertBatchPatientToDos(InsertBatchPatientToDosDataRequest request);
    }
}