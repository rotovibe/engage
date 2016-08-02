using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using Phytel.API.DataDomain.Scheduling;
using Phytel.API.DataDomain.Scheduling.DTO;

namespace Phytel.API.DataDomain.ToDo.Test.Stubs
{
    class StubMongoToDoRepository : ISchedulingRepository
    {
        string userId = "testuser";

        public StubMongoToDoRepository(string contract)
        {
        }
        
        public GetToDosDataResponse FindToDos(object request)
        {
            GetToDosDataResponse response = new GetToDosDataResponse();
            List<ToDoData> todoList = null;
            GetToDosDataRequest dataRequest = (GetToDosDataRequest)request;
            todoList = new List<ToDoData>();
            todoList.Add(new ToDoData
            {
                AssignedToId = "535ad70ad2d8e912e857525c",
                CategoryId = "532b64b1a381168abe000a7d",
                ClosedDate = DateTime.UtcNow,
                CreatedById = "532b678fa381168abe000ce8",
                CreatedOn = DateTime.UtcNow,
                Description = "test description",
                DueDate = DateTime.UtcNow,
                StartTime = DateTime.UtcNow,
                Duration = 20,
                Id = "532b6320a381168abe000877",
                PatientId = "532b645fa381168abe0009e9",
                PriorityId = (int)Priority.High,
                StatusId = (int)Status.Met,
                Title = "test title",
                UpdatedOn = DateTime.UtcNow,
                DeleteFlag = true
            });
            response.ToDos = todoList;
            response.TotalCount = 1;
            return response;
        }


        public IEnumerable<object> FindToDosWithAProgramId(string entityId)
        {
            List<ToDoData> todoList = new List<ToDoData>();
            todoList.Add(new ToDoData
            {
                AssignedToId = "535ad70ad2d8e912e857525c",
                CategoryId = "532b64b1a381168abe000a7d",
                ClosedDate = DateTime.UtcNow,
                CreatedById = "532b678fa381168abe000ce8",
                CreatedOn = DateTime.UtcNow,
                Description = "test description",
                DueDate = DateTime.UtcNow,
                StartTime = DateTime.UtcNow,
                Duration = 20,
                Id = "532b6320a381168abe000877",
                PatientId = "532b645fa381168abe0009e9",
                PriorityId = (int)Priority.High,
                StatusId = (int)Status.Met,
                Title = "test title",
                UpdatedOn = DateTime.UtcNow,
                DeleteFlag = true,
                ProgramIds = new List<string>() {"535ad70ad2d8e912e857525c", "532b64b1a381168abe000a7d"}
            });
            return todoList;
        }

        public void RemoveProgram(object entity, List<string> updatedProgramIds)
        {
            RemoveProgramInToDosDataRequest request = (RemoveProgramInToDosDataRequest)entity;
        }

        public object FindByID(string entityID, bool includeDeletedToDo)
        {
            ToDoData todoData = null;
            todoData = new ToDoData
            {
                AssignedToId = "535ad70ad2d8e912e857525c",
                CategoryId = "532b64b1a381168abe000a7d",
                ClosedDate = DateTime.UtcNow,
                CreatedById = "532b678fa381168abe000ce8",
                CreatedOn = DateTime.UtcNow,
                Description = "test description",
                DueDate = DateTime.UtcNow,
                StartTime = DateTime.UtcNow,
                Duration = 20,
                Id = "532b6320a381168abe000877",
                PatientId = "532b645fa381168abe0009e9",
                PriorityId = (int)Priority.High,
                StatusId = (int)Status.Met,
                Title = "test title",
                UpdatedOn = DateTime.UtcNow,
                DeleteFlag = true
            };
            return todoData;
        }

        public string UserId
        {
            get
            {
                return this.userId;
            }
            set
            {
                this.userId = value;
            }
        }

        public object Insert(object newEntity)
        {
            PutInsertToDoDataRequest request = (PutInsertToDoDataRequest)newEntity;
            ToDoData todoData = request.ToDoData;
            string id = null;
            METoDo meToDo = null;
            try
            {
                if (todoData != null)
                {
                    meToDo = new METoDo(this.UserId, todoData.CreatedOn)
                    {
                        Id = ObjectId.Parse("532b6320a381168abe000877"),
                        Status = (Status)todoData.StatusId,
                        Priority = (Priority)todoData.PriorityId,
                        Description = todoData.Description,
                        Title = todoData.Title,
                        LoweredTitle = todoData.Title != null ? todoData.Title.ToLower() : null,
                        DueDate = todoData.DueDate,
                        StartTime = meToDo.StartTime,
                        Duration = meToDo.Duration,
                        DeleteFlag = false,
                        AssignedToId = ObjectId.Parse(todoData.AssignedToId),
                        SourceId = ObjectId.Parse(todoData.SourceId),
                        ClosedDate = todoData.ClosedDate,
                        LastUpdatedOn = todoData.UpdatedOn
                    };
                }
                return meToDo.Id.ToString();
            }
            catch (Exception) { throw; }
        }

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(object entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            ToDoData todoData = null;
            todoData =  new ToDoData
            {
                AssignedToId = "535ad70ad2d8e912e857525c",
                CategoryId = "532b64b1a381168abe000a7d",
                ClosedDate = DateTime.UtcNow,
                CreatedById = "532b678fa381168abe000ce8",
                CreatedOn = DateTime.UtcNow,
                Description = "test description",
                DueDate = DateTime.UtcNow,
                StartTime = DateTime.UtcNow,
                Duration = 20,
                Id = "532b6320a381168abe000877",
                PatientId = "532b645fa381168abe0009e9",
                PriorityId = (int)Priority.High,
                StatusId = (int)Status.Met,
                Title = "test title",
                UpdatedOn = DateTime.UtcNow,
                DeleteFlag = true
            };
            return todoData;
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Update(object entity)
        {
            PutUpdateToDoDataRequest request = (PutUpdateToDoDataRequest)entity;
            ToDoData todoData = request.ToDoData;
            bool result = false;
            if (todoData != null)
            {
                METoDo meToDo = new METoDo(this.UserId, todoData.CreatedOn)
                {
                    Status = (Status)todoData.StatusId,
                    Priority = (Priority)todoData.PriorityId,
                    Description = todoData.Description,
                    Title = todoData.Title,
                    LoweredTitle = todoData.Title != null ? todoData.Title.ToLower() : null,
                    DueDate = todoData.DueDate,
                    StartTime = todoData.StartTime,
                    Duration = todoData.Duration,
                    ClosedDate = todoData.ClosedDate
                };
            }
            result = true;

            return result;
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public void UndoDelete(object entity)
        {
            throw new NotImplementedException();
        }


        public object FindByExternalRecordId(string externalRecordId)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<object> Select(List<string> ids)
        {
            throw new NotImplementedException();
        }
    }
}
