using MongoDB.Driver;
using Phytel.API.Common.Data;
using System;
using System.Collections.Generic;
using Phytel.API.DataDomain.Scheduling.DTO;
using MongoDB.Bson;
using Phytel.API.Common;
using Phytel.API.DataAudit;
using MB = MongoDB.Driver.Builders;
using MongoDB.Driver.Builders;
using System.Linq;
using System.Configuration;
using System.Reflection;

namespace Phytel.API.DataDomain.Scheduling
{
    public class MongoToDoRepository : ISchedulingRepository
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);

        public MongoToDoRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public string UserId { get; set; }

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
                        Status = (Status)todoData.StatusId,
                        Priority = (Priority)todoData.PriorityId,
                        Description = todoData.Description,
                        Title = todoData.Title,
                        LoweredTitle = todoData.Title != null ? todoData.Title.ToLower() : null,
                        DueDate = todoData.DueDate,
                        StartTime = todoData.StartTime,
                        Duration = todoData.Duration,
                        ProgramIds = Helper.ConvertToObjectIdList(todoData.ProgramIds),
                        DeleteFlag = false,
                        LastUpdatedOn = todoData.UpdatedOn,
                        ExternalRecordId = todoData.ExternalRecordId
                    };
                    
                    if (!string.IsNullOrEmpty(todoData.AssignedToId))
                    {
                        meToDo.AssignedToId = ObjectId.Parse(todoData.AssignedToId);
                    }
                    if (!string.IsNullOrEmpty(todoData.CategoryId))
                    {
                        meToDo.Category = ObjectId.Parse(todoData.CategoryId);
                    }
                    if (!string.IsNullOrEmpty(todoData.PatientId))
                    {
                        meToDo.PatientId = ObjectId.Parse(todoData.PatientId);
                    }
                    if (!string.IsNullOrEmpty(todoData.SourceId))
                    {
                        meToDo.SourceId = ObjectId.Parse(todoData.SourceId);
                    }
                    if (todoData.ClosedDate != null)
                    {
                        meToDo.ClosedDate = todoData.ClosedDate;
                    }

                    using (SchedulingMongoContext ctx = new SchedulingMongoContext(_dbName))
                    {
                        ctx.ToDos.Collection.Insert(meToDo);

                        AuditHelper.LogDataAudit(this.UserId,
                                                MongoCollectionName.ToDo.ToString(),
                                                meToDo.Id.ToString(),
                                                Common.DataAuditType.Insert,
                                                request.ContractNumber);

                        id =  meToDo.Id.ToString();
                    }
                }
                return id;
            }
            catch (Exception) { throw; }
        }

        public object InsertAll(List<object> entities)
        {
            BulkInsertResult result = new BulkInsertResult();
            List<string> insertedIds = new List<string>();
            List<string> errorMessages = new List<string>();
            try
            {
                using (SchedulingMongoContext ctx = new SchedulingMongoContext(_dbName))
                {
                    var bulk = ctx.ToDos.Collection.InitializeUnorderedBulkOperation();
                    foreach (ToDoData t in entities)
                    {
                        METoDo meToDo = new METoDo(this.UserId, t.CreatedOn)
                        {
                            Status = (Status)t.StatusId,
                            Priority = (Priority)t.PriorityId,
                            Description = t.Description,
                            Title = t.Title,
                            LoweredTitle = t.Title != null ? t.Title.ToLower() : null,
                            DueDate = t.DueDate,
                            StartTime = t.StartTime,
                            Duration = t.Duration,
                            ProgramIds = Helper.ConvertToObjectIdList(t.ProgramIds),
                            DeleteFlag = false,
                            LastUpdatedOn = t.UpdatedOn,
                            ExternalRecordId = t.ExternalRecordId
                        };

                        if (!string.IsNullOrEmpty(t.AssignedToId))
                        {
                            meToDo.AssignedToId = ObjectId.Parse(t.AssignedToId);
                        }
                        if (!string.IsNullOrEmpty(t.CategoryId))
                        {
                            meToDo.Category = ObjectId.Parse(t.CategoryId);
                        }
                        if (!string.IsNullOrEmpty(t.PatientId))
                        {
                            meToDo.PatientId = ObjectId.Parse(t.PatientId);
                        }
                        if (!string.IsNullOrEmpty(t.SourceId))
                        {
                            meToDo.SourceId = ObjectId.Parse(t.SourceId);
                        }
                        if (t.ClosedDate != null)
                        {
                            meToDo.ClosedDate = t.ClosedDate;
                        }
                        bulk.Insert(meToDo.ToBsonDocument());
                        insertedIds.Add(meToDo.Id.ToString());
                    }
                    BulkWriteResult bwr = bulk.Execute();
                }
                // TODO: Auditing.
            }
            catch (BulkWriteException bwEx)
            {
                // Get the error messages for the ones that failed.
                foreach (BulkWriteError er in bwEx.WriteErrors)
                {
                    errorMessages.Add(er.Message);
                }
            }
            catch (Exception ex)
            {
                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helper.LogException(int.Parse(aseProcessID), ex);
            }
            result.ProcessedIds = insertedIds;
            result.ErrorMessages = errorMessages;
            return result;
        }

        public void Delete(object entity)
        {
            DeleteToDoByPatientIdDataRequest request = (DeleteToDoByPatientIdDataRequest)entity;
            try
            {
                using (SchedulingMongoContext ctx = new SchedulingMongoContext(_dbName))
                {
                    var q = MB.Query<METoDo>.EQ(b => b.Id, ObjectId.Parse(request.Id));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(METoDo.TTLDateProperty, DateTime.UtcNow.AddDays(_expireDays)));
                    uv.Add(MB.Update.Set(METoDo.LastUpdatedOnProperty, DateTime.UtcNow));
                    uv.Add(MB.Update.Set(METoDo.DeleteFlagProperty, true));
                    uv.Add(MB.Update.Set(METoDo.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.ToDos.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.ToDo.ToString(),
                                            request.Id.ToString(),
                                            Common.DataAuditType.Delete,
                                            request.ContractNumber);

                }
            }
            catch (Exception) { throw; }
        }

        public void DeleteAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            return FindByID(entityID, false);
        }

        public object FindByID(string entityID, bool includeDeletedToDo)
        {
            ToDoData todoData = null;
            try
            {
                using (SchedulingMongoContext ctx = new SchedulingMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    if (!includeDeletedToDo)
                    {
                        queries.Add(Query.EQ(METoDo.DeleteFlagProperty, false));
                    }
                    queries.Add(Query.EQ(METoDo.IdProperty, ObjectId.Parse(entityID)));
                    IMongoQuery mQuery = Query.And(queries);
                    METoDo meToDo = ctx.ToDos.Collection.Find(mQuery).FirstOrDefault();
                    if (meToDo != null)
                    {
                        todoData = new ToDoData
                        {
                            AssignedToId = meToDo.AssignedToId == null ? string.Empty : meToDo.AssignedToId.ToString(),
                            CategoryId = meToDo.Category.ToString(),
                            ClosedDate = meToDo.ClosedDate,
                            CreatedById = meToDo.RecordCreatedBy.ToString(),
                            CreatedOn = meToDo.RecordCreatedOn,
                            Description = meToDo.Description,
                            DueDate = meToDo.DueDate,
                            StartTime = meToDo.StartTime,
                            Duration = meToDo.Duration,
                            Id = meToDo.Id.ToString(),
                            PatientId = meToDo.PatientId == null ? string.Empty : meToDo.PatientId.ToString(),
                            PriorityId = (int)meToDo.Priority,
                            ProgramIds = Helper.ConvertToStringList(meToDo.ProgramIds),
                            StatusId = (int)meToDo.Status,
                            Title = meToDo.Title,
                            UpdatedOn = meToDo.LastUpdatedOn,
                            ExternalRecordId  = meToDo.ExternalRecordId,
                            DeleteFlag = meToDo.DeleteFlag
                        };
                    }
                }
                return todoData;
            }
            catch (Exception) { throw; }
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
            try
            {
                if (todoData != null)
                {
                    using (SchedulingMongoContext ctx = new SchedulingMongoContext(_dbName))
                    {
                        var q = MB.Query<METoDo>.EQ(b => b.Id, ObjectId.Parse(todoData.Id));

                        // Set the ClosedDate if the status is changed.
                        METoDo existingToDo = ctx.ToDos.Collection.Find(q).SetFields(METoDo.StatusProperty).FirstOrDefault();
                        if (existingToDo != null)
                        {
                            if ((todoData.StatusId == (int)Status.Met || todoData.StatusId == (int)Status.Abandoned))
                            {
                                if (existingToDo.Status != (Status)todoData.StatusId)
                                {
                                    todoData.ClosedDate = DateTime.UtcNow;
                                }
                            }
                            else
                            {
                                todoData.ClosedDate = null;
                            }
                        }
                        var uv = new List<MB.UpdateBuilder>();
                        uv.Add(MB.Update.Set(METoDo.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                        uv.Add(MB.Update.Set(METoDo.VersionProperty, request.Version));
                        uv.Add(MB.Update.Set(METoDo.LastUpdatedOnProperty, System.DateTime.UtcNow));
                        uv.Add(MB.Update.Set(METoDo.TitleProperty, todoData.Title));
                        if( !string.IsNullOrEmpty(todoData.Title)){
                            uv.Add(MB.Update.Set(METoDo.LoweredTitleProperty, todoData.Title.ToLower()));
                        }                        
                        uv.Add(MB.Update.Set(METoDo.StatusProperty, (Status)todoData.StatusId));
                        uv.Add(MB.Update.Set(METoDo.PriorityProperty, (Priority)todoData.PriorityId));

                        if (!string.IsNullOrEmpty(todoData.Description))
                        {
                            uv.Add(MB.Update.Set(METoDo.DescriptionProperty, todoData.Description));
                        }
                        else
                        {
                            uv.Add(MB.Update.Set(METoDo.DescriptionProperty, BsonNull.Value));
                        }
                        if (todoData.DueDate != null)
                        {
                            uv.Add(MB.Update.Set(METoDo.DueDateProperty, todoData.DueDate));
                        }
                        else
                        {
                            uv.Add(MB.Update.Set(METoDo.DueDateProperty, BsonNull.Value));
                        }
                        if (todoData.StartTime != null)
                        {
                            uv.Add(MB.Update.Set(METoDo.StartTimeProperty, todoData.StartTime));
                        }
                        else
                        {
                            uv.Add(MB.Update.Set(METoDo.StartTimeProperty, BsonNull.Value));
                        }
                        if (todoData.Duration != null)
                        {
                            uv.Add(MB.Update.Set(METoDo.DurationProperty, todoData.Duration));
                        }
                        else
                        {
                            uv.Add(MB.Update.Set(METoDo.DurationProperty, BsonNull.Value));
                        }
                        if (todoData.ClosedDate != null)
                        {
                            uv.Add(MB.Update.Set(METoDo.ClosedDateProperty, todoData.ClosedDate));
                        }
                        else
                        {
                            uv.Add(MB.Update.Set(METoDo.ClosedDateProperty, BsonNull.Value));
                        }
                        if (todoData.ProgramIds != null)
                        {
                            uv.Add(MB.Update.SetWrapped(METoDo.ProgramProperty, Helper.ConvertToObjectIdList(todoData.ProgramIds)));
                        }
                        else
                        {
                            uv.Add(MB.Update.Set(METoDo.ProgramProperty, BsonNull.Value));
                        }
                        
                        if (!string.IsNullOrEmpty(todoData.PatientId)) 
                        {
                            uv.Add(MB.Update.Set(METoDo.PatientIdProperty, ObjectId.Parse(todoData.PatientId)));
                        }
                        else
                        {
                            uv.Add(MB.Update.Set(METoDo.PatientIdProperty, BsonNull.Value));
                        }
                        if (!string.IsNullOrEmpty(todoData.CategoryId))
                        {
                            uv.Add(MB.Update.Set(METoDo.CatgegoryProperty, ObjectId.Parse(todoData.CategoryId)));
                        }
                        else
                        {
                            uv.Add(MB.Update.Set(METoDo.CatgegoryProperty, BsonNull.Value));
                        }
                        if (!string.IsNullOrEmpty(todoData.AssignedToId))
                        {
                            uv.Add(MB.Update.Set(METoDo.AssignedToProperty, ObjectId.Parse(todoData.AssignedToId)));
                        }
                        else
                        {
                            uv.Add(MB.Update.Set(METoDo.AssignedToProperty, BsonNull.Value));
                        }
                        uv.Add(MB.Update.Set(METoDo.DeleteFlagProperty, todoData.DeleteFlag));
                        DataAuditType type;
                        if (todoData.DeleteFlag)
                        {
                            uv.Add(MB.Update.Set(METoDo.TTLDateProperty, System.DateTime.UtcNow.AddDays(_expireDays)));
                            type = Common.DataAuditType.Delete;
                        }
                        else
                        {
                            uv.Add(MB.Update.Set(METoDo.TTLDateProperty, BsonNull.Value));
                            type = Common.DataAuditType.Update;
                        }
                        IMongoUpdate update = MB.Update.Combine(uv);
                        WriteConcernResult res = ctx.ToDos.Collection.Update(q, update);
                        if (res.Ok == false)
                            throw new Exception("Failed to update a ToDo: " + res.ErrorMessage);
                        else
                            AuditHelper.LogDataAudit(this.UserId,
                                                    MongoCollectionName.ToDo.ToString(),
                                                    todoData.Id,
                                                    type,
                                                    request.ContractNumber);

                        result = true;
                    }
                }
                return result;
            }
            catch (Exception) { throw; }
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public void UndoDelete(object entity)
        {
            UndoDeletePatientToDosDataRequest request = (UndoDeletePatientToDosDataRequest)entity;
            try
            {
                using (SchedulingMongoContext ctx = new SchedulingMongoContext(_dbName))
                {
                    var q = MB.Query<METoDo>.EQ(b => b.Id, ObjectId.Parse(request.ToDoId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(METoDo.TTLDateProperty, BsonNull.Value));
                    uv.Add(MB.Update.Set(METoDo.DeleteFlagProperty, false));
                    uv.Add(MB.Update.Set(METoDo.LastUpdatedOnProperty, DateTime.UtcNow));
                    uv.Add(MB.Update.Set(METoDo.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.ToDos.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.ToDo.ToString(),
                                            request.ToDoId.ToString(),
                                            Common.DataAuditType.UndoDelete,
                                            request.ContractNumber);

                }
            }
            catch (Exception) { throw; }
        }

        public GetToDosDataResponse FindToDos(object request)
        {
            GetToDosDataResponse response = new GetToDosDataResponse();
            List<ToDoData> todoList = null;
            GetToDosDataRequest dataRequest = (GetToDosDataRequest)request;
            try
            {
                using (SchedulingMongoContext ctx = new SchedulingMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(METoDo.DeleteFlagProperty, false));
                    if (!string.IsNullOrEmpty(dataRequest.AssignedToId))
                    {
                        ObjectId ato;
                        if (ObjectId.TryParse(dataRequest.AssignedToId, out ato))
                        {
                            queries.Add(Query.EQ(METoDo.AssignedToProperty, ato));
                        }
                        else
                        {
                            // Fix for bug  - ENG-1068, UI sends AssignedToId = -1 to query on all unassigned ToDos.
                            if (string.Compare(dataRequest.AssignedToId, "-1", true) == 0)
                            {                                
                                //queries.Add(Query.Or(Query.EQ(METoDo.AssignedToProperty, BsonNull.Value), Query.EQ(METoDo.AssignedToProperty, BsonString.Empty)));
                                queries.Add(Query.In(METoDo.AssignedToProperty, new BsonArray { BsonNull.Value, BsonString.Empty }));
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(dataRequest.NotAssignedToId))
                    {
                        ObjectId nto;
                        if (ObjectId.TryParse(dataRequest.NotAssignedToId, out nto))
                        {                            
                            queries.Add(Query.NotIn(METoDo.AssignedToProperty, new BsonArray { nto, BsonNull.Value, BsonString.Empty }));                         
                        }
                    }
                    if (!string.IsNullOrEmpty(dataRequest.CreatedById))
                    {
                        queries.Add(Query.EQ(METoDo.RecordCreatedByProperty, ObjectId.Parse(dataRequest.CreatedById)));
                    }
                    if (!string.IsNullOrEmpty(dataRequest.PatientId))
                    {
                        queries.Add(Query.EQ(METoDo.PatientIdProperty, ObjectId.Parse(dataRequest.PatientId)));
                    }
                    if (dataRequest.StatusIds != null && dataRequest.StatusIds.Count > 0)
                    {
                        queries.Add(Query.In(METoDo.StatusProperty, new BsonArray(dataRequest.StatusIds)));
                    }
                    if (dataRequest.PriorityIds != null && dataRequest.PriorityIds.Count > 0)
                    {
                        queries.Add(Query.In(METoDo.PriorityProperty, new BsonArray(dataRequest.PriorityIds))); //integer values (not object ids)
                    }
                    if (dataRequest.CategoryIds != null && dataRequest.CategoryIds.Count > 0)
                    {
                        List<BsonValue> categories = new List<BsonValue>();
                        foreach( string categoryId in dataRequest.CategoryIds ){
                            if (categoryId.Length > 0)
                            {
                                categories.Add(BsonValue.Create(ObjectId.Parse(categoryId)));
                            }
                            else
                            {
                                categories.Add(BsonNull.Value); //empty string => include null categories ( not set )
                            }
                        }
                        queries.Add(Query.In(METoDo.CatgegoryProperty, categories));
                    }
                    if (dataRequest.FromDate != null)
                    {
                        queries.Add(Query.GTE(METoDo.ClosedDateProperty, dataRequest.FromDate));
                    }
                    
                    IMongoQuery mQuery = Query.And(queries);
                    MongoCursor<METoDo> cToDos = ctx.ToDos.Collection.Find(mQuery);               
                    List<METoDo> meToDos = null;                   
                    SortByBuilder sortBy = MongoSortingUtils.GetSortByBuilder( dataRequest.Sort, typeof(METoDo) );                             
                    cToDos = cToDos.SetSortOrder(sortBy);                                        
                    cToDos = (MongoCursor<METoDo>) MongoSortingUtils.ApplySkipTake(cToDos, dataRequest);
                    response.TotalCount = cToDos.Count();
                    meToDos = cToDos.ToList();  //query now                    
                                                            
                    if (meToDos != null && meToDos.Count > 0)
                    {
                        todoList = new List<ToDoData>();
                        foreach (METoDo t in meToDos)
                        {
                            todoList.Add(new ToDoData
                            {
                                AssignedToId  = t.AssignedToId == null ? string.Empty : t.AssignedToId.ToString(),
                                CategoryId = t.Category == null ? string.Empty : t.Category.ToString(),
                                ClosedDate = t.ClosedDate,
                                CreatedById = t.RecordCreatedBy.ToString(),
                                CreatedOn = t.RecordCreatedOn,
                                Description = t.Description,
                                DueDate = t.DueDate,
                                StartTime = t.StartTime,
                                Duration = t.Duration,
                                Id = t.Id.ToString(),
                                PatientId = t.PatientId == null ? string.Empty : t.PatientId.ToString(),
                                PriorityId = (int)t.Priority,
                                ProgramIds = Helper.ConvertToStringList(t.ProgramIds),
                                StatusId = (int)t.Status,
                                Title = t.Title,
                                UpdatedOn = t.LastUpdatedOn, 
                                DeleteFlag = t.DeleteFlag,
                                ExternalRecordId = t.ExternalRecordId,
                                SourceId = t.SourceId.ToString()
                            });
                        }
                    }
                }
                response.ToDos = todoList;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:MongoToDoRepository:FindToDos()::" + ex.Message, ex.InnerException);
            }
        }

        public void RemoveProgram(object entity, List<string> updatedProgramIds)
        {
            RemoveProgramInToDosDataRequest request = (RemoveProgramInToDosDataRequest)entity;
            try
            {
                List<ObjectId> ids = updatedProgramIds.ConvertAll(r => ObjectId.Parse(r));
                using (SchedulingMongoContext ctx = new SchedulingMongoContext(_dbName))
                {
                    var q = MB.Query<METoDo>.EQ(b => b.Id, ObjectId.Parse(request.ToDoId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.SetWrapped<List<ObjectId>>(METoDo.ProgramProperty, ids));
                    uv.Add(MB.Update.Set(METoDo.LastUpdatedOnProperty, DateTime.UtcNow));
                    uv.Add(MB.Update.Set(METoDo.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.ToDos.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.ToDo.ToString(),
                                            request.ToDoId.ToString(),
                                            Common.DataAuditType.Update,
                                            request.ContractNumber);

                }
            }
            catch (Exception) { throw; }
        }

        public IEnumerable<object> FindToDosWithAProgramId(string entityId)
        {
            List<ToDoData> dataList = null;
            try
            {
                using (SchedulingMongoContext ctx = new SchedulingMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.In(METoDo.ProgramProperty, new List<BsonValue> { BsonValue.Create(ObjectId.Parse(entityId)) }));
                    queries.Add(Query.EQ(METoDo.DeleteFlagProperty, false));
                    IMongoQuery mQuery = Query.And(queries);
                    List<METoDo> meToDos = ctx.ToDos.Collection.Find(mQuery).ToList();
                    if (meToDos != null && meToDos.Count > 0)
                    {
                        dataList = new List<ToDoData>();
                        foreach (METoDo t in meToDos)
                        {
                            ToDoData data = new ToDoData
                            {
                                Id = t.Id.ToString(),
                                Title = t.Title,
                                ProgramIds = Helper.ConvertToStringList(t.ProgramIds),
                                CreatedOn = t.RecordCreatedOn,
                                CreatedById = t.RecordCreatedBy.ToString()
                            };
                            dataList.Add(data);
                        }

                    }
                }
                return dataList;
            }
            catch (Exception ex) { throw ex; }
        }

        public object FindByExternalRecordId(string externalRecordId)
        {
            ToDoData data = null;
            try
            {
                using (SchedulingMongoContext ctx = new SchedulingMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(METoDo.ExternalRecordIdProperty, externalRecordId));
                    queries.Add(Query.EQ(METoDo.DeleteFlagProperty, false));
                    queries.Add(Query.EQ(METoDo.TTLDateProperty, BsonNull.Value));
                    IMongoQuery mQuery = Query.And(queries);
                    METoDo mePN = ctx.ToDos.Collection.Find(mQuery).FirstOrDefault();
                    if (mePN != null)
                    {
                        data = new ToDoData
                        {
                            Id = mePN.Id.ToString(),
                        };
                    }
                }
                return data;
            }
            catch (Exception) { throw; }
        }

        public IEnumerable<object> Select(List<string> ids)
        {
            List<ToDoData> dataList = null;
            try
            {
                using (SchedulingMongoContext ctx = new SchedulingMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.In(METoDo.IdProperty, new BsonArray(Helper.ConvertToObjectIdList(ids))));
                    queries.Add(Query.EQ(METoDo.DeleteFlagProperty, false));
                    queries.Add(Query.EQ(METoDo.TTLDateProperty, BsonNull.Value));
                    IMongoQuery mQuery = Query.And(queries);
                    List<METoDo> meToDos = ctx.ToDos.Collection.Find(mQuery).ToList();
                    if (meToDos != null && meToDos.Count > 0)
                    {
                        dataList = new List<ToDoData>();
                        foreach (METoDo t in meToDos)
                        {
                            dataList.Add(new ToDoData
                            {
                                Id = t.Id.ToString(),
                                PatientId = t.PatientId == null ? string.Empty : t.PatientId.ToString(),
                                PriorityId = (int)t.Priority,
                                ProgramIds = Helper.ConvertToStringList(t.ProgramIds),
                                StatusId = (int)t.Status,
                                Title = t.Title,
                                UpdatedOn = t.LastUpdatedOn,
                                ExternalRecordId = t.ExternalRecordId,
                                DeleteFlag = t.DeleteFlag,
                                AssignedToId = t.AssignedToId == null ? string.Empty : t.AssignedToId.ToString(),
                                CategoryId = t.Category.ToString(),
                                ClosedDate = t.ClosedDate,
                                CreatedById = t.RecordCreatedBy.ToString(),
                                CreatedOn = t.RecordCreatedOn,
                                Description = t.Description,
                                DueDate = t.DueDate,
                                StartTime = t.StartTime,
                                Duration = t.Duration
                            });
                        }
                    }
                }
                return dataList as IEnumerable<object>;
            }
            catch (Exception) { throw; }
        }
    }
}
