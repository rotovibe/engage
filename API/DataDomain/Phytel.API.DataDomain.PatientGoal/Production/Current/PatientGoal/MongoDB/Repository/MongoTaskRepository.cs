using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.DataDomain.PatientGoal;
using MB = MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.Common;
using Phytel.API.Common.Data;
using Phytel.API.DataDomain.PatientGoal;
using System.Configuration;
using Phytel.API.DataAudit;
using MongoDB.Bson.Serialization;

namespace Phytel.API.DataDomain.PatientGoal
{
    public class MongoTaskRepository : IGoalRepository
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        private int _initializeDays = Convert.ToInt32(ConfigurationManager.AppSettings["InitializeDays"]);

        static MongoTaskRepository()
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
        public MongoTaskRepository(string contractDBName)
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
                TaskData taskData = null;
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(METask.IdProperty, ObjectId.Parse(entityID)));
                queries.Add(Query.EQ(METask.DeleteFlagProperty, false));
                queries.Add(Query.EQ(METask.TTLDateProperty, BsonNull.Value));
                IMongoQuery mQuery = Query.And(queries);
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    METask t = ctx.Tasks.Collection.Find(mQuery).FirstOrDefault();
                    if (t != null)
                    {
                        taskData = new TaskData
                        {
                            Id = t.Id.ToString(),
                            TargetValue = t.TargetValue,
                            TemplateGoalId = t.TemplateGoalId.ToString(),
                            StatusId = ((int) t.Status),
                            TargetDate = t.TargetDate,
                            BarrierIds = Helper.ConvertToStringList(t.BarrierIds),
                            Description = t.Description,
                            StatusDate = t.StatusDate,
                            StartDateRange = t.StartDateRange,
                            TargetDateRange = t.TargetDateRange,
                            StartDate = t.StartDate,
                            CustomAttributes = DTOUtil.GetCustomAttributeIdAndValues(t.Attributes),
                            CreatedById = t.RecordCreatedBy.ToString(),
                            DeleteFlag = t.DeleteFlag
                        };
                        var mePG = ctx.Goals.Collection.Find(Query.EQ(MEGoal.IdProperty, ObjectId.Parse(taskData.TemplateGoalId))).SetFields(MEGoal.NameProperty).FirstOrDefault();
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
                    
                    // Set the StatusDate to Now if the status is changed.
                    MEPatientTask existingPB = ctx.PatientTasks.Collection.Find(q).SetFields(MEPatientTask.StatusProperty).FirstOrDefault();
                    if (existingPB != null)
                    {
                        if ((int)existingPB.Status != pt.StatusId)
                        {
                            pt.StatusDate = DateTime.UtcNow;
                        }
                    }
                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientTask.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEPatientTask.VersionProperty, ptr.Version));
                    uv.Add(MB.Update.Set(MEPatientTask.LastUpdatedOnProperty, System.DateTime.UtcNow));
                    if (pt.Description != null) uv.Add(MB.Update.Set(MEPatientTask.DescriptionProperty, pt.Description));
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
                    if (pt.PatientGoalId != null) uv.Add(MB.Update.Set(MEPatientTask.PatientGoalIdProperty, ObjectId.Parse(pt.PatientGoalId)));
                    
                    uv.Add(MB.Update.Set(MEPatientTask.ClosedDateProperty, pt.ClosedDate)); 
                    uv.Add(MB.Update.Set(MEPatientTask.DeleteFlagProperty, pt.DeleteFlag));
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
                                GoalName = goalName
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
                                DeleteFlag = t.DeleteFlag
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


        public object FindByTemplateId(string patientId, string entityID)
        {
            throw new NotImplementedException();
        }
    }
}
