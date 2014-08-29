using Phytel.API.DataDomain.Scheduling.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Scheduling;
using System;
using Phytel.API.Common.Format;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Scheduling
{
    public class SchedulingDataManager : ISchedulingDataManager
    {

        public ISchedulingRepositoryFactory Factory { get; set; }

        #region ToDo
        public GetToDosDataResponse GetToDos(GetToDosDataRequest request)
        {
            try
            {
                var result = new GetToDosDataResponse();

                ISchedulingRepository repo = Factory.GetRepository(request, RepositoryType.ToDo);
                repo.UserId = request.UserId;
                result.ToDos = (List<ToDoData>)repo.FindToDos(request);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PutInsertToDoDataResponse InsertToDo(PutInsertToDoDataRequest request)
        {
            PutInsertToDoDataResponse respone = new PutInsertToDoDataResponse();
            try
            {
                ISchedulingRepository repo = Factory.GetRepository(request, RepositoryType.ToDo);
                string toDoId = (string)repo.Insert(request);
                if (!string.IsNullOrEmpty(toDoId))
                {
                    ToDoData data = (ToDoData)repo.FindByID(toDoId);
                    respone.ToDoData = data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return respone;
        }

        public PutUpdateToDoDataResponse UpdateToDo(PutUpdateToDoDataRequest request)
        {
            PutUpdateToDoDataResponse response = new PutUpdateToDoDataResponse();
            try
            {
                ISchedulingRepository repo = Factory.GetRepository(request, RepositoryType.ToDo);
                bool success = (bool)repo.Update(request);
                if (success)
                {
                    ToDoData data = (ToDoData)repo.FindByID(request.ToDoData.Id);
                    response.ToDoData = data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response; ;
        }

        public RemoveProgramInToDosDataResponse RemoveProgramInToDos(RemoveProgramInToDosDataRequest request)
        {
            RemoveProgramInToDosDataResponse response = new RemoveProgramInToDosDataResponse();
            try
            {

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
            catch (Exception ex) { throw ex; }
        }
        #endregion


        #region Schedule
        public GetScheduleDataResponse GetSchedule(GetScheduleDataRequest request)
        {
            try
            {
                var result = new GetScheduleDataResponse();

                ISchedulingRepository repo = Factory.GetRepository(request, RepositoryType.Schedule);
                repo.UserId = request.UserId;
                result.Schedule = (ScheduleData)repo.FindByID(request.Id);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}   
