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

namespace Phytel.API.DataDomain.PatientGoal
{
    public class MongoPatientTaskRepository<T> : IPatientGoalRepository<T>
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        private int _initializeDays = Convert.ToInt32(ConfigurationManager.AppSettings["InitializeDays"]);

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
            catch (Exception ex) { throw ex; }
        }

        public object InsertAll(List<object> entities)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public void Delete(object entity)
        {
            DeleteTaskRequest request = (DeleteTaskRequest)entity;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientTask>.EQ(b => b.Id, ObjectId.Parse(request.TaskId));

                    var f = ctx.PatientTasks.Collection.FindAll();
                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientTask.TTLDateProperty, System.DateTime.UtcNow.AddDays(_expireDays)));
                    uv.Add(MB.Update.Set(MEPatientTask.DeleteFlagProperty, true));
                    uv.Add(MB.Update.Set(MEPatientTask.UpdatedByProperty, request.UserId));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    WriteConcernResult res = ctx.PatientTasks.Collection.Update(q, update);
                }
            }
            catch (Exception ex) { throw; }
        }

        public void DeleteAll(List<object> entities)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public object FindByID(string entityID)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
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
            catch (Exception ex) { throw ex; }
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
                    uv.Add(MB.Update.Set(MEPatientTask.TTLDateProperty, BsonNull.Value));
                    uv.Add(MB.Update.Set(MEPatientTask.DeleteFlagProperty, false));
                    uv.Add(MB.Update.Set(MEPatientTask.UpdatedByProperty, ptr.UserId));
                    if (pt.Description != null) uv.Add(MB.Update.Set(MEPatientTask.DescriptionProperty, pt.Description));
                    if (pt.StartDate != null) uv.Add(MB.Update.Set(MEPatientTask.StartDateProperty, pt.StartDate));
                    if (pt.StatusDate != null) uv.Add(MB.Update.Set(MEPatientTask.StatusDateProperty, pt.StatusDate));
                    if (pt.StatusId != 0) uv.Add(MB.Update.Set(MEPatientTask.StatusProperty, pt.StatusId));
                    if (pt.TargetDate != null) uv.Add(MB.Update.Set(MEPatientTask.TargetDateProperty, pt.TargetDate));
                    if (pt.TargetValue != null) uv.Add(MB.Update.Set(MEPatientTask.TargetValueProperty, pt.TargetValue));
                    if (pt.CustomAttributes != null) { uv.Add(MB.Update.SetWrapped<List<MAttribute>>(MEPatientTask.AttributesProperty, DTOUtil.GetAttributes(pt.CustomAttributes))); }
                    if (pt.Barriers != null) { uv.Add(MB.Update.SetWrapped<List<ObjectId>>(MEPatientTask.BarriersProperty, DTOUtil.ConvertObjectId(pt.Barriers))); }

                    IMongoUpdate update = MB.Update.Combine(uv);
                    WriteConcernResult res = ctx.PatientTasks.Collection.Update(q, update);
                    if (res.Ok)
                        result = true;
                }
                return result as object;
            }
            catch (Exception ex) { throw; }
        }

        public void CacheByID(List<string> entityIDs)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw; }
        }

        public object Initialize(object newEntity)
        {
            PutInitializeTaskRequest ptr = (PutInitializeTaskRequest)newEntity;
            PatientTaskData task = null;
            try
            {
                MEPatientTask pat = new MEPatientTask
                {
                    Id = ObjectId.GenerateNewId(),
                    PatientGoalId = ObjectId.Parse(ptr.PatientGoalId),
                    TTLDate = System.DateTime.UtcNow.AddDays(_initializeDays),
                    StatusDate = DateTime.UtcNow,
                    UpdatedBy = ptr.UserId,
                    LastUpdatedOn = DateTime.UtcNow
                };

                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    WriteConcernResult wcr = ctx.PatientTasks.Collection.Insert(pat);
                    if (wcr.Ok)
                    {
                        task = new PatientTaskData
                        {
                            Id = pat.Id.ToString()
                        };
                    }
                }
                return task;
            }
            catch (Exception ex) { throw; }
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
                        tasksDataList = new List<PatientTaskData>();
                        foreach (MEPatientTask b in meTasks)
                        {
                            PatientTaskData taskData = new PatientTaskData
                            {
                                Id = b.Id.ToString(),
                                TargetValue = b.TargetValue,
                                PatientGoalId = b.PatientGoalId.ToString(),
                                StatusId = ((int)b.Status),
                                TargetDate = b.TargetDate,
                                Barriers = Helper.ConvertToStringList(b.Barriers),
                                Description = b.Description,
                                StatusDate = b.StatusDate,
                                StartDate = b.StartDate,
                                CustomAttributes = DTOUtil.GetCustomAttributeIdAndValues(b.Attributes)
                            };
                            tasksDataList.Add(taskData);
                        }
                    }
                }
                return tasksDataList;
            }
            catch (Exception ex) { throw ex; }
        }

        public IEnumerable<object> FindByGoalId(string Id)
        {
            try
            {
                List<PatientTaskData> tasksDataList = null;
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEPatientTask.PatientGoalIdProperty, ObjectId.Parse(Id)));
                IMongoQuery mQuery = Query.And(queries);

                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    List<MEPatientTask> meTasks = ctx.PatientTasks.Collection.Find(mQuery).ToList();
                    if (meTasks != null)
                    {
                        tasksDataList = new List<PatientTaskData>();
                        foreach (MEPatientTask b in meTasks)
                        {
                            PatientTaskData taskData = new PatientTaskData
                            {
                                Id = b.Id.ToString(),
                                TargetValue = b.TargetValue,
                                PatientGoalId = b.PatientGoalId.ToString(),
                                StatusId = ((int)b.Status),
                                TargetDate = b.TargetDate,
                                //Attributes = DTOUtil.ConvertToAttributeDataList(b.Attributes),
                                Barriers = Helper.ConvertToStringList(b.Barriers),
                                Description = b.Description,
                                StatusDate = b.StatusDate,
                                StartDate = b.StartDate
                            };
                            tasksDataList.Add(taskData);
                        }
                    }
                }
                return tasksDataList;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
