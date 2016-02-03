using Phytel.API.DataDomain.Scheduling.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Scheduling;
using System;
using Phytel.API.Common.Format;
using System.Collections.Generic;
using Phytel.API.Common;
using System.Net;
using System.Linq;
using Phytel.API.DataAudit;

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
                result = (GetToDosDataResponse)repo.FindToDos(request);                
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

        public List<HttpObjectResponse<ToDoData>> InsertBatchPatientToDos(InsertBatchPatientToDosDataRequest request)
        {
            List<HttpObjectResponse<ToDoData>> list = null;
            try
            {
                if (request.PatientToDosData != null && request.PatientToDosData.Count > 0)
                {
                    list = new List<HttpObjectResponse<ToDoData>>();
                    ISchedulingRepository repo = Factory.GetRepository(request, RepositoryType.ToDo);
                    BulkInsertResult result = (BulkInsertResult)repo.InsertAll(request.PatientToDosData.Cast<object>().ToList());
                    if (result != null)
                    {
                        if (result.ProcessedIds != null && result.ProcessedIds.Count > 0)
                        {
                            // Get the ToDos that were newly inserted. 
                            List<ToDoData> insertedToDos = repo.Select(result.ProcessedIds) as List<ToDoData>;
                            if (insertedToDos != null && insertedToDos.Count > 0)
                            {
                                #region DataAudit
                                List<string> insertedPatientToDoIds = insertedToDos.Select(p => p.Id).ToList();
                                AuditHelper.LogDataAudit(request.UserId, MongoCollectionName.ToDo.ToString(), insertedPatientToDoIds, Common.DataAuditType.Insert, request.ContractNumber);
                                #endregion

                                insertedToDos.ForEach(r =>
                                {
                                    list.Add(new HttpObjectResponse<ToDoData> { Code = HttpStatusCode.Created, Body = (ToDoData)new ToDoData { Id = r.Id, ExternalRecordId = r.ExternalRecordId, PatientId = r.PatientId } });
                                });
                            }
                        }
                        result.ErrorMessages.ForEach(e =>
                        {
                            list.Add(new HttpObjectResponse<ToDoData> { Code = HttpStatusCode.InternalServerError, Message = e });
                        });
                    }
                }
            }
            catch (Exception ex) { throw ex; }
            return list;
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
                GetToDosDataResponse toDosResponse = repo.FindToDos(getToDosDataRequest);
                List<ToDoData> patientToDos = (List<ToDoData>)toDosResponse.ToDos; //repo.FindToDos(getToDosDataRequest);
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
