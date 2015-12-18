using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Scheduling.DTO;

namespace Phytel.API.DataDomain.Scheduling.Test.Stubs
{
    public class StubToDoDataManager : ISchedulingDataManager
    {
        public ISchedulingRepositoryFactory Factory { get; set; }

        public DTO.GetToDosDataResponse GetToDoList(DTO.GetToDosDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.PutInsertToDoDataResponse InsertToDo(DTO.PutInsertToDoDataRequest request)
        {
            PutInsertToDoDataResponse respone = new PutInsertToDoDataResponse();
            ISchedulingRepository repo = Factory.GetRepository(request, RepositoryType.ToDo);
            string toDoId = (string)repo.Insert(request);
            if (!string.IsNullOrEmpty(toDoId))
            {
                ToDoData data = (ToDoData)repo.FindByID(toDoId);
                respone.ToDoData = data;
            }
            return respone;
        }

        public DTO.PutUpdateToDoDataResponse UpdateToDo(DTO.PutUpdateToDoDataRequest request)
        {
            PutUpdateToDoDataResponse response = new PutUpdateToDoDataResponse();
            ISchedulingRepository repo = Factory.GetRepository(request, RepositoryType.ToDo);
            bool success = (bool)repo.Update(request);
            if (success)
            {
                ToDoData data = (ToDoData)repo.FindByID(request.ToDoData.Id, true);
                response.ToDoData = data;
            }
            return response;
        }

        public DTO.GetToDosDataResponse GetToDos(DTO.GetToDosDataRequest request)
        {
            try
            {
                var result = new GetToDosDataResponse();

                ISchedulingRepository repo = Factory.GetRepository(request, RepositoryType.ToDo);
                repo.UserId = request.UserId;
                result = repo.FindToDos(request);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DTO.GetScheduleDataResponse GetSchedule(DTO.GetScheduleDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.RemoveProgramInToDosDataResponse RemoveProgramInToDos(DTO.RemoveProgramInToDosDataRequest request)
        {
            RemoveProgramInToDosDataResponse response = new RemoveProgramInToDosDataResponse();
            ISchedulingRepository repo = Factory.GetRepository(request, RepositoryType.ToDo);
            if (request.ProgramId != null)
            {
                List<ToDoData> todos = repo.FindToDosWithAProgramId(request.ProgramId) as List<ToDoData>;
                if (todos != null && todos.Count > 0)
                {
                    todos.ForEach(u =>
                    {
                        request.ToDoId = u.Id;
                        if (u.ProgramIds != null && u.ProgramIds.Remove(request.ProgramId))
                        {
                            repo.RemoveProgram(request, u.ProgramIds);
                        }
                    });
                }
            }
            return response;
            
        }


        public DTO.DeleteToDoByPatientIdDataResponse DeleteToDoByPatientId(DTO.DeleteToDoByPatientIdDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.UndoDeletePatientToDosDataResponse UndoDeleteToDos(DTO.UndoDeletePatientToDosDataRequest request)
        {
            throw new NotImplementedException();
        }

        public List<Phytel.API.Common.HttpObjectResponse<ToDoData>> InsertBatchPatientToDos(InsertBatchPatientToDosDataRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
