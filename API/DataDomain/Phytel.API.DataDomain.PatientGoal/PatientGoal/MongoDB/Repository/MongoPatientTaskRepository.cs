using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.API.Common;
using Phytel.API.DataAudit;
using Phytel.API.DataDomain.PatientGoal.DTO;
using MB = MongoDB.Driver.Builders;

namespace Phytel.API.DataDomain.PatientGoal
{
    public class MongoPatientTaskRepository : IGoalRepository
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        private int _initializeDays = Convert.ToInt32(ConfigurationManager.AppSettings["InitializeDays"]);

        static MongoPatientTaskRepository()
        {
            #region Register ClassMap
            try { 
            if (BsonClassMap.IsClassMapRegistered(typeof(GoalBase)) == false)
                BsonClassMap.RegisterClassMap<GoalBase>();
            }
            catch { }

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(MEPatientTask)) == false)
                    BsonClassMap.RegisterClassMap<MEPatientTask>();
            }
            catch { }
            #endregion

        }
        public MongoPatientTaskRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
        }

        public object InsertAll(List<object> entities)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
        }

        public void Delete(object entity)
        {
            DeleteTaskDataRequest request = (DeleteTaskDataRequest)entity;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientTask>.EQ(b => b.Id, ObjectId.Parse(request.TaskId));
                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientTask.TTLDateProperty, System.DateTime.UtcNow.AddDays(_expireDays)));
                    uv.Add(MB.Update.Set(MEPatientTask.DeleteFlagProperty, true));
                    uv.Add(MB.Update.Set(MEPatientTask.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEPatientTask.LastUpdatedOnProperty, DateTime.UtcNow));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientTasks.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.PatientTask.ToString(), 
                                            request.TaskId.ToString(), 
                                            Common.DataAuditType.Delete, 
                                            request.ContractNumber);
                }
            }
            catch (Exception) { throw; }
        }

        public void DeleteAll(List<object> entities)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
        }

        public object FindByID(string entityID)
        {
            try
            {
                PatientTaskData taskData = null;
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEPatientTask.IdProperty, ObjectId.Parse(entityID)));
                queries.Add(Query.EQ(MEPatientTask.DeleteFlagProperty, false));
                queries.Add(Query.EQ(MEPatientTask.TTLDateProperty, BsonNull.Value));
                IMongoQuery mQuery = Query.And(queries);
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    MEPatientTask t = ctx.PatientTasks.Collection.Find(mQuery).FirstOrDefault();
                    if (t != null)
                    {
                        taskData = new PatientTaskData
                        {
                            Id = t.Id.ToString(),
                            TargetValue = t.TargetValue,
                            PatientGoalId = t.PatientGoalId.ToString(),
                            StatusId = ((int)t.Status),
                            TargetDate = t.TargetDate,
                            BarrierIds = Helper.ConvertToStringList(t.BarrierIds),
                            Description = t.Description,
                            StatusDate = t.StatusDate,
                            StartDate = t.StartDate,
                            CustomAttributes = DTOUtil.GetCustomAttributeIdAndValues(t.Attributes),
                            ClosedDate = t.ClosedDate,
                            CreatedById = t.RecordCreatedBy.ToString(),
                            DeleteFlag = t.DeleteFlag,
                            Details = t.Details
                        };
                        var mePG = ctx.PatientGoals.Collection.Find(Query.EQ(MEPatientGoal.IdProperty, ObjectId.Parse(taskData.PatientGoalId))).SetFields(MEPatientGoal.NameProperty).FirstOrDefault();
                        if (mePG != null)
                        {
                            taskData.GoalName = mePG.Name;
                        }
                    }
                }
                return taskData;
            }
            catch (Exception) { throw; }
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw; }
        }

        public IEnumerable<object> SelectAll()
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
        }

        public object Update(object entity)
        {
            bool result = false;
            PutUpdateTaskRequest ptr = (PutUpdateTaskRequest)entity;
            PatientTaskData pt = ptr.Task;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientTask>.EQ(b => b.Id, ObjectId.Parse(pt.Id));
                    
                    // Set the StatusDate to Now if the status is changed. Set the ClosedDate depending on the Status.
                    MEPatientTask existingPB = ctx.PatientTasks.Collection.Find(q).SetFields(MEPatientTask.StatusProperty).FirstOrDefault();
                    if (existingPB != null)
                    {
                        if ((int)existingPB.Status != pt.StatusId)
                        {
                            pt.StatusDate = DateTime.UtcNow;
                        }
                        if ((pt.StatusId == (int)GoalTaskStatus.Met || pt.StatusId == (int)GoalTaskStatus.Abandoned))
                        {
                            if (existingPB.Status != (GoalTaskStatus)pt.StatusId)
                            {
                                pt.ClosedDate = DateTime.UtcNow;
                            }
                        }
                        else
                        {
                            pt.ClosedDate = null;
                        }
                    }
                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientTask.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEPatientTask.VersionProperty, ptr.Version));
                    uv.Add(MB.Update.Set(MEPatientTask.LastUpdatedOnProperty, System.DateTime.UtcNow));
                    if (pt.Description != null) uv.Add(MB.Update.Set(MEPatientTask.DescriptionProperty, pt.Description));
                    if (pt.Details != null) uv.Add(MB.Update.Set(MEPatientTask.DetailProperty, pt.Details));
                    if (pt.StartDate != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientTask.StartDateProperty, pt.StartDate));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientTask.StartDateProperty, BsonNull.Value));
                    }
                    if (pt.StatusDate != null) uv.Add(MB.Update.Set(MEPatientTask.StatusDateProperty, pt.StatusDate));
                    if (pt.StatusId != 0) uv.Add(MB.Update.Set(MEPatientTask.StatusProperty, pt.StatusId));
                    if (pt.TargetDate != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientTask.TargetDateProperty, pt.TargetDate));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientTask.TargetDateProperty, BsonNull.Value));
                    }
                    if (pt.TargetValue != null) uv.Add(MB.Update.Set(MEPatientTask.TargetValueProperty, pt.TargetValue));
                    if (pt.CustomAttributes != null) { uv.Add(MB.Update.SetWrapped<List<MAttribute>>(MEPatientTask.AttributesProperty, DTOUtil.GetAttributes(pt.CustomAttributes))); }
                    if (pt.BarrierIds != null) { uv.Add(MB.Update.SetWrapped<List<ObjectId>>(MEPatientTask.BarriersProperty, DTOUtil.ConvertObjectId(pt.BarrierIds))); }
                    if (pt.ClosedDate != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientTask.ClosedDateProperty, pt.ClosedDate));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientTask.ClosedDateProperty, BsonNull.Value));
                    }
                    uv.Add(MB.Update.Set(MEPatientTask.DeleteFlagProperty, pt.DeleteFlag));
                    if (pt.TemplateId != null) uv.Add(MB.Update.Set(MEPatientTask.TemplateIdProperty, ObjectId.Parse(pt.TemplateId)));

                    DataAuditType type;
                    if (pt.DeleteFlag)
                    {
                        uv.Add(MB.Update.Set(MEPatientTask.TTLDateProperty, System.DateTime.UtcNow.AddDays(_expireDays)));
                        type = Common.DataAuditType.Delete;
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientTask.TTLDateProperty, BsonNull.Value));
                        type = Common.DataAuditType.Update;
                    }
                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientTasks.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.PatientTask.ToString(), 
                                            pt.Id, 
                                            type, 
                                            ptr.ContractNumber);

                    result = true;
                }
                return result as object;
            }
            catch (Exception) { throw; }
        }

        public void CacheByID(List<string> entityIDs)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
        }

        public object Initialize(object newEntity)
        {
            PutInitializeTaskRequest ptr = (PutInitializeTaskRequest)newEntity;
            PatientTaskData task = null;
            MEPatientTask pat = null;
            try
            {
                pat = new MEPatientTask(this.UserId)
                {
                    Id = ObjectId.GenerateNewId(),
                    PatientGoalId = ObjectId.Parse(ptr.PatientGoalId),
                    TTLDate = System.DateTime.UtcNow.AddDays(_initializeDays),
                    StatusDate = DateTime.UtcNow,
                    DeleteFlag = false
                    //,
                    //LastUpdatedOn = DateTime.UtcNow,
                    //UpdatedBy = ObjectId.Parse(this.UserId)
                };

                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    ctx.PatientTasks.Collection.Insert(pat);

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.PatientTask.ToString(), 
                                            pat.Id.ToString(), 
                                            Common.DataAuditType.Insert, 
                                            ptr.ContractNumber);

                    task = new PatientTaskData
                    {
                        Id = pat.Id.ToString()
                    };
                }
                return task;
            }
            catch (Exception) { throw; }
        }

        public IEnumerable<object> Find(string Id)
        {
            try
            {
                List<PatientTaskData> tasksDataList = null;
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEPatientTask.PatientGoalIdProperty, ObjectId.Parse(Id)));
                queries.Add(Query.EQ(MEPatientTask.DeleteFlagProperty, false));
                queries.Add(Query.EQ(MEPatientTask.TTLDateProperty, BsonNull.Value));
                IMongoQuery mQuery = Query.And(queries);
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    List<MEPatientTask> meTasks = ctx.PatientTasks.Collection.Find(mQuery).ToList();
                    if (meTasks != null)
                    {
                        string goalName = string.Empty;
                        var mePG = ctx.PatientGoals.Collection.Find(Query.EQ(MEPatientGoal.IdProperty, ObjectId.Parse(Id))).SetFields(MEPatientGoal.NameProperty).FirstOrDefault();
                        if (mePG != null)
                        {
                            goalName = mePG.Name;
                        }
                        tasksDataList = new List<PatientTaskData>();
                        foreach (MEPatientTask t in meTasks)
                        {
                            PatientTaskData taskData = new PatientTaskData
                            {
                                Id = t.Id.ToString(),
                                TargetValue = t.TargetValue,
                                PatientGoalId = t.PatientGoalId.ToString(),
                                StatusId = ((int)t.Status),
                                TargetDate = t.TargetDate,
                                BarrierIds = Helper.ConvertToStringList(t.BarrierIds),
                                Description = t.Description,
                                StatusDate = t.StatusDate,
                                StartDate = t.StartDate,
                                CustomAttributes = DTOUtil.GetCustomAttributeIdAndValues(t.Attributes),
                                ClosedDate = t.ClosedDate,
                                CreatedById = t.RecordCreatedBy.ToString(),
                                DeleteFlag = t.DeleteFlag,
                                GoalName = goalName,
                                Details = t.Details
                            };
                            tasksDataList.Add(taskData);
                        }
                    }
                }
                return tasksDataList;
            }
            catch (Exception) { throw; }
        }

        public string UserId { get; set; }


        public void UndoDelete(object entity)
        {
            UndoDeleteTaskDataRequest request = (UndoDeleteTaskDataRequest)entity;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientTask>.EQ(b => b.Id, ObjectId.Parse(request.TaskId));
                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientTask.TTLDateProperty, BsonNull.Value));
                    uv.Add(MB.Update.Set(MEPatientTask.DeleteFlagProperty, false));
                    uv.Add(MB.Update.Set(MEPatientTask.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEPatientTask.LastUpdatedOnProperty, DateTime.UtcNow));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientTasks.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientTask.ToString(),
                                            request.TaskId.ToString(),
                                            Common.DataAuditType.UndoDelete,
                                            request.ContractNumber);
                }
            }
            catch (Exception) { throw; }
        }


        public IEnumerable<object> FindGoalsWithAProgramId(string entityId)
        {
            throw new NotImplementedException();
        }

        public void RemoveProgram(object entity, List<string> updatedProgramIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> Search(object request, List<string> patientGoalIds)
        {
            List<PatientTaskData> list = null;
            GetPatientTasksDataRequest dataRequest = (GetPatientTasksDataRequest)request;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEPatientTask.DeleteFlagProperty, false));
                    if (dataRequest.StatusIds != null && dataRequest.StatusIds.Count > 0)
                    {
                        queries.Add(Query.In(MEPatientTask.StatusProperty, new BsonArray(dataRequest.StatusIds)));
                    }
                    if (patientGoalIds != null && patientGoalIds.Count > 0)
                    {
                        List<BsonValue> bsonList = Helper.ConvertToBsonValueList(patientGoalIds);
                        queries.Add(Query.In(MEPatientTask.PatientGoalIdProperty, bsonList));
                    }
                    IMongoQuery mQuery = Query.And(queries);
                    List<MEPatientTask> meTasks = null;
                    meTasks = ctx.PatientTasks.Collection.Find(mQuery).ToList();
                    if (meTasks != null && meTasks.Count > 0)
                    {
                        list = new List<PatientTaskData>();
                        foreach (MEPatientTask t in meTasks)
                        {
                            PatientTaskData taskData = new PatientTaskData
                            {
                                Id = t.Id.ToString(),
                                TargetValue = t.TargetValue,
                                PatientGoalId = t.PatientGoalId.ToString(),
                                StatusId = ((int)t.Status),
                                TargetDate = t.TargetDate,
                                BarrierIds = Helper.ConvertToStringList(t.BarrierIds),
                                Description = t.Description,
                                StatusDate = t.StatusDate,
                                StartDate = t.StartDate,
                                CustomAttributes = DTOUtil.GetCustomAttributeIdAndValues(t.Attributes),
                                ClosedDate = t.ClosedDate,
                                CreatedById = t.RecordCreatedBy.ToString(),
                                DeleteFlag = t.DeleteFlag,
                                Details = t.Details
                            };
                            var mePG = ctx.PatientGoals.Collection.Find(Query.EQ(MEPatientGoal.IdProperty, ObjectId.Parse(taskData.PatientGoalId))).SetFields(MEPatientGoal.NameProperty).FirstOrDefault();
                            if (mePG != null)
                            {
                                taskData.GoalName = mePG.Name;
                            }
                            list.Add(taskData);
                        }
                    }
                }
                return list;
            }
            catch (Exception) { throw; }
        }


        public object FindByTemplateId(string patientGoalId, string entityID)
        {
            PatientTaskData taskData = null;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>
                    {
                        Query.EQ(MEPatientTask.PatientGoalIdProperty, ObjectId.Parse(patientGoalId)),
                        Query.EQ(MEPatientTask.TemplateIdProperty, ObjectId.Parse(entityID)),
                        Query.In(MEPatientTask.StatusProperty, new BsonArray{1, 3}),
                        Query.EQ(MEPatientTask.DeleteFlagProperty, false),
                        Query.EQ(MEPatientTask.TTLDateProperty, BsonNull.Value)
                    };

                    var mQuery = Query.And(queries);
                    var b = ctx.PatientTasks.Collection.Find(mQuery).FirstOrDefault();

                    if (b != null)
                    {
                        taskData = new PatientTaskData
                        {
                            Id = b.Id.ToString(),
                            Description = b.Description,
                            PatientGoalId = b.PatientGoalId.ToString(),
                            BarrierIds = Helper.ConvertToStringList(b.BarrierIds),
                            StatusId = ((int) b.Status),
                            StatusDate = b.StatusDate,
                            StartDate = b.StartDate,
                            TargetValue = b.TargetValue,
                            CustomAttributes = DTOUtil.GetCustomAttributeIdAndValues(b.Attributes),
                            TargetDate = b.TargetDate,
                            ClosedDate = b.ClosedDate,
                            CreatedById = b.RecordCreatedBy.ToString(),
                            DeleteFlag = b.DeleteFlag,
                            Details = b.Details
                        };

                        var mePG = ctx.PatientGoals.Collection.Find(Query.EQ(MEPatientGoal.IdProperty, ObjectId.Parse(taskData.PatientGoalId))).SetFields(MEPatientGoal.PatientIdProperty, MEPatientGoal.NameProperty).FirstOrDefault();
                        if (mePG != null)
                        {
                            taskData.GoalName = mePG.Name;
                        }
                    }
                }
                return taskData;
            }
            catch (Exception) { throw; }
        }
    }
}
