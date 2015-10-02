using Phytel.API.DataDomain.Scheduling.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Scheduling;
using System;
using Phytel.API.Common.Format;
using System.Collections.Generic;
using Phytel.API.Common;
using System.Net;

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
                    ToDoData data = (ToDoData)repo.FindByID(request.ToDoData.Id, true);
                    response.ToDoData = data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response; ;
        }

        public UpsertBatchPatientToDosDataResponse UpsertBatchPatientToDos(UpsertBatchPatientToDosDataRequest request)
        {
            UpsertBatchPatientToDosDataResponse response = new UpsertBatchPatientToDosDataResponse();
            if (request.PatientToDosData != null && request.PatientToDosData.Count > 0)
            {
                List<HttpObjectResponse<ToDoData>> list = new List<HttpObjectResponse<ToDoData>>();
                ISchedulingRepository repo = Factory.GetRepository(request, RepositoryType.ToDo);
                request.PatientToDosData.ForEach(p =>
                {
                    HttpStatusCode code = HttpStatusCode.OK;
                    ToDoData todoData = null;
                    string message = string.Empty;
                    try
                    {
                        if (string.IsNullOrEmpty(p.ExternalRecordId))
                        {
                            code = HttpStatusCode.BadRequest;
                            message = string.Format("ExternalRecordId is missing for PatientId : {0}", p.PatientId);
                        }
                        else
                        {
                            ToDoData data = (ToDoData)repo.FindByExternalRecordId(p.ExternalRecordId);
                            if (data == null)
                            {
                                PutInsertToDoDataRequest insertReq = new PutInsertToDoDataRequest
                                {
                                    Context = request.Context,
                                    ContractNumber = request.ContractNumber,
                                    ToDoData = p,
                                    UserId = request.UserId,
                                    Version = request.Version
                                };
                                string id = (string)repo.Insert(insertReq);
                                if (!string.IsNullOrEmpty(id))
                                {
                                    code = HttpStatusCode.Created;
                                    todoData = new ToDoData { Id = id, ExternalRecordId = p.ExternalRecordId, PatientId = p.PatientId };
                                }
                            }
                            else
                            {
                                p.Id = data.Id;
                                PutUpdateToDoDataRequest updateReq = new PutUpdateToDoDataRequest
                                {
                                    ToDoData = p,
                                    Context = request.Context,
                                    ContractNumber = request.ContractNumber,
                                    UserId = request.UserId,
                                    Version = request.Version
                                };
                                bool status = (bool)repo.Update(updateReq);
                                if (status)
                                {
                                    code = HttpStatusCode.NoContent;
                                    todoData = new ToDoData { Id = p.Id, ExternalRecordId = p.ExternalRecordId, PatientId = p.PatientId };
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        code = HttpStatusCode.InternalServerError;
                        message = string.Format("ExternalRecordId: {0}, Message: {1}, StackTrace: {2}", p.ExternalRecordId, ex.Message, ex.StackTrace);
                    }
                    list.Add(new HttpObjectResponse<ToDoData> { Code = code, Body = (ToDoData)todoData, Message = message });
                });
                response.Responses = list;
            }
            return response;
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

        public DeleteToDoByPatientIdDataResponse DeleteToDoByPatientId(DeleteToDoByPatientIdDataRequest request)
        {
            DeleteToDoByPatientIdDataResponse response = new DeleteToDoByPatientIdDataResponse();
            try
            {
                ISchedulingRepository repo = Factory.GetRepository(request, RepositoryType.ToDo);
                GetToDosDataRequest getToDosDataRequest = new GetToDosDataRequest
                {
                    PatientId = request.PatientId,
                    Context = request.Context,
                    ContractNumber = request.ContractNumber,
                    UserId = request.UserId,
                    Version = request.Version
                };
                List<ToDoData> patientToDos = (List<ToDoData>)repo.FindToDos(getToDosDataRequest); ;
                List<string> deletedIds = null;
                if (patientToDos != null)
                {
                    deletedIds = new List<string>();
                    patientToDos.ForEach(u =>
                    {
                        request.Id = u.Id;
                        repo.Delete(request);
                        deletedIds.Add(request.Id);
                    });
                    response.DeletedIds = deletedIds;
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }

        public UndoDeletePatientToDosDataResponse UndoDeleteToDos(UndoDeletePatientToDosDataRequest request)
        {
            UndoDeletePatientToDosDataResponse response = new UndoDeletePatientToDosDataResponse();
            try
            {
                ISchedulingRepository repo = Factory.GetRepository(request, RepositoryType.ToDo);
                if (request.Ids != null && request.Ids.Count > 0)
                {
                    request.Ids.ForEach(u =>
                    {
                        request.ToDoId = u;
                        repo.UndoDelete(request);
                    });
                }
                response.Success = true;
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

                var repo = Factory.GetRepository(request, RepositoryType.Schedule);
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
