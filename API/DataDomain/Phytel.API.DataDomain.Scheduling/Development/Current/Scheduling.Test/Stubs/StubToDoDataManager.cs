using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Scheduling.Test.Stubs
{
    public class StubToDoDataManager : ISchedulingDataManager
    {

        public DTO.GetToDosDataResponse GetToDoList(DTO.GetToDosDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutInsertToDoDataResponse InsertToDo(DTO.PutInsertToDoDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutUpdateToDoDataResponse UpdateToDo(DTO.PutUpdateToDoDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.GetToDosDataResponse GetToDos(DTO.GetToDosDataRequest request)
        {
            throw new NotImplementedException();
        }


        public DTO.GetScheduleDataResponse GetSchedule(DTO.GetScheduleDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.RemoveProgramInToDosDataResponse RemoveProgramInToDos(DTO.RemoveProgramInToDosDataRequest request)
        {
            throw new NotImplementedException();
        }


        public DTO.DeleteToDoByPatientIdDataResponse DeleteToDoByPatientId(DTO.DeleteToDoByPatientIdDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.UndoDeletePatientToDosDataResponse UndoDeleteToDos(DTO.UndoDeletePatientToDosDataRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
