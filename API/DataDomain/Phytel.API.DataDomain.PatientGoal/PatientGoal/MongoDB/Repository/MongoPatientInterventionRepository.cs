using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.API.Common;
using Phytel.API.DataAudit;
using Phytel.API.DataDomain.PatientGoal.DTO;
using MB = MongoDB.Driver.Builders;

namespace Phytel.API.DataDomain.PatientGoal
{
    public class MongoPatientInterventionRepository : IGoalRepository
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        private int _initializeDays = Convert.ToInt32(ConfigurationManager.AppSettings["InitializeDays"]);

        static MongoPatientInterventionRepository()
        {
            #region Register ClassMap
            try
            { 
            if (BsonClassMap.IsClassMapRegistered(typeof(GoalBase)) == false)
                BsonClassMap.RegisterClassMap<GoalBase>();
            }
            catch { }

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(MEPatientIntervention)) == false)
                    BsonClassMap.RegisterClassMap<MEPatientIntervention>();
            }
            catch { }
            #endregion
        }

        public MongoPatientInterventionRepository(string contractDBName)
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
            DeleteInterventionDataRequest request = (DeleteInterventionDataRequest)entity;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientIntervention>.EQ(b => b.Id, ObjectId.Parse(request.InterventionId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientIntervention.TTLDateProperty, System.DateTime.UtcNow.AddDays(_expireDays)));
                    uv.Add(MB.Update.Set(MEPatientIntervention.DeleteFlagProperty, true));
                    uv.Add(MB.Update.Set(MEPatientIntervention.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEPatientIntervention.LastUpdatedOnProperty, DateTime.UtcNow));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientInterventions.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.PatientIntervention.ToString(), 
                                            request.InterventionId.ToString(), 
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
                PatientInterventionData interventionData = null;
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEPatientIntervention.IdProperty, ObjectId.Parse(entityID)));
                queries.Add(Query.EQ(MEPatientIntervention.DeleteFlagProperty, false));
                queries.Add(Query.EQ(MEPatientIntervention.TTLDateProperty, BsonNull.Value));
                IMongoQuery mQuery = Query.And(queries);
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    MEPatientIntervention b = ctx.PatientInterventions.Collection.Find(mQuery).FirstOrDefault();
                    if (b != null)
                    {
                        interventionData = new PatientInterventionData
                        {
                            Id = b.Id.ToString(),
                            Description = b.Description,
                            PatientGoalId = b.PatientGoalId.ToString(),
                            CategoryId = b.CategoryId == null ? null : b.CategoryId.ToString(),
                            AssignedToId = b.AssignedToId == null ? null : b.AssignedToId.ToString(),
                            BarrierIds = Helper.ConvertToStringList(b.BarrierIds),
                            StatusId = ((int)b.Status),
                            StatusDate = b.StatusDate,
                            StartDate = b.StartDate,
                            DueDate = b.DueDate,
                            ClosedDate = b.ClosedDate,
                            CreatedById = b.RecordCreatedBy.ToString(),
                            DeleteFlag = b.DeleteFlag,
                            Details = b.Details
                        };
                        var mePG = ctx.PatientGoals.Collection.Find(Query.EQ(MEPatientGoal.IdProperty, ObjectId.Parse(interventionData.PatientGoalId))).SetFields(MEPatientGoal.PatientIdProperty, MEPatientGoal.NameProperty).FirstOrDefault();
                        if (mePG != null)
                        {
                            interventionData.PatientId = mePG.PatientId.ToString();
                            interventionData.GoalName = mePG.Name;
                        }
                    }
                }
                return interventionData;
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
            catch (Exception) { throw; }
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
            PutUpdateInterventionRequest ir = (PutUpdateInterventionRequest)entity;
            PatientInterventionData pi = ir.Intervention;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientIntervention>.EQ(b => b.Id, ObjectId.Parse(pi.Id));

                    // Set the StatusDate to Now if the status is changed.
                    MEPatientIntervention existingPI = ctx.PatientInterventions.Collection.Find(q).SetFields(MEPatientIntervention.StatusProperty).FirstOrDefault();
                    if (existingPI != null)
                    {
                        if ((int)existingPI.Status != pi.StatusId)
                        {
                            pi.StatusDate = DateTime.UtcNow;
                        }
                        if ((pi.StatusId == (int)InterventionStatus.Removed || pi.StatusId == (int)InterventionStatus.Completed))
                        {
                            if (existingPI.Status != (InterventionStatus)pi.StatusId)
                            {
                                pi.ClosedDate = DateTime.UtcNow;
                            }
                        }
                        else
                        {
                            pi.ClosedDate = null;
                        }
                    }

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientIntervention.VersionProperty, ir.Version));
                    uv.Add(MB.Update.Set(MEPatientIntervention.LastUpdatedOnProperty, System.DateTime.UtcNow));
                    uv.Add(MB.Update.Set(MEPatientIntervention.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    if(pi.TemplateId != null)
                        uv.Add(MB.Update.Set(MEPatientIntervention.TemplateIdProperty, ObjectId.Parse(pi.TemplateId)));
                    
                    if (pi.Description != null) uv.Add(MB.Update.Set(MEPatientIntervention.DescriptionProperty, pi.Description));
                    if (pi.Details != null) uv.Add(MB.Update.Set(MEPatientIntervention.DetailProperty, pi.Details));
                    if (pi.StartDate != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientIntervention.StartDateProperty, pi.StartDate));
                    }
                    else 
                    {
                        uv.Add(MB.Update.Set(MEPatientIntervention.StartDateProperty, BsonNull.Value));
                    }
                    if (pi.DueDate != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientIntervention.DueDateProperty, pi.DueDate));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientIntervention.DueDateProperty, BsonNull.Value));
                    }
                    if (pi.StatusDate != null) uv.Add(MB.Update.Set(MEPatientIntervention.StatusDateProperty, pi.StatusDate));
                    if (pi.StatusId != 0) uv.Add(MB.Update.Set(MEPatientIntervention.StatusProperty, pi.StatusId));
                    if (pi.CategoryId != null) uv.Add(MB.Update.Set(MEPatientIntervention.CategoryProperty, ObjectId.Parse(pi.CategoryId)));
                    if (pi.AssignedToId != null) uv.Add(MB.Update.Set(MEPatientIntervention.AssignedToProperty, ObjectId.Parse(pi.AssignedToId)));
                    if (pi.BarrierIds != null) { uv.Add(MB.Update.SetWrapped<List<ObjectId>>(MEPatientIntervention.BarriersProperty, DTOUtil.ConvertObjectId(pi.BarrierIds))); }
                    if (pi.PatientGoalId != null) uv.Add(MB.Update.Set(MEPatientIntervention.PatientGoalIdProperty, ObjectId.Parse(pi.PatientGoalId)));
                    if (pi.ClosedDate != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientIntervention.ClosedDateProperty, pi.ClosedDate));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientIntervention.ClosedDateProperty, BsonNull.Value));
                    } 
                    uv.Add(MB.Update.Set(MEPatientIntervention.DeleteFlagProperty, pi.DeleteFlag));
                    DataAuditType type;
                    if (pi.DeleteFlag)
                    {
                        uv.Add(MB.Update.Set(MEPatientIntervention.TTLDateProperty, System.DateTime.UtcNow.AddDays(_expireDays)));
                        type = Common.DataAuditType.Delete;
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientIntervention.TTLDateProperty, BsonNull.Value));
                        type = Common.DataAuditType.Update;
                    }
                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientInterventions.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.PatientIntervention.ToString(), 
                                            pi.Id.ToString(), 
                                            type, 
                                            ir.ContractNumber);

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
            PutInitializeInterventionRequest ptr = (PutInitializeInterventionRequest)newEntity;
            MEPatientIntervention pi = null;
            try
            {
                pi = new MEPatientIntervention(this.UserId)
                {
                    Id = ObjectId.GenerateNewId(),
                    PatientGoalId = ObjectId.Parse(ptr.PatientGoalId),
                    TTLDate = System.DateTime.UtcNow.AddDays(_initializeDays),
                    StatusDate = DateTime.UtcNow,
                    //LastUpdatedOn = DateTime.UtcNow,
                    //UpdatedBy = ObjectId.Parse(this.UserId),
                    Version = ptr.Version
                };
                
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    ctx.PatientInterventions.Collection.Insert(pi);

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.PatientIntervention.ToString(), 
                                            pi.Id.ToString(), 
                                            Common.DataAuditType.Insert, 
                                            ptr.ContractNumber);

                }
                return pi.Id.ToString();
            }
            catch (Exception) { throw; }
        }

        public IEnumerable<object> Find(string Id)
        {
            try
            {
                List<PatientInterventionData> interventionsDataList = null;
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEPatientIntervention.PatientGoalIdProperty, ObjectId.Parse(Id)));
                queries.Add(Query.EQ(MEPatientIntervention.DeleteFlagProperty, false));
                queries.Add(Query.EQ(MEPatientIntervention.TTLDateProperty, BsonNull.Value));
                IMongoQuery mQuery = Query.And(queries);
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    List<MEPatientIntervention> meInterventions = ctx.PatientInterventions.Collection.Find(mQuery).ToList();
                    if (meInterventions != null)
                    {
                        string goalName = string.Empty;
                        string patientId = string.Empty;
                        var mePG = ctx.PatientGoals.Collection.Find(Query.EQ(MEPatientGoal.IdProperty, ObjectId.Parse(Id))).SetFields(MEPatientGoal.NameProperty).FirstOrDefault();
                        if (mePG != null)
                        {
                            goalName = mePG.Name;
                            patientId = mePG.PatientId.ToString();
                        }
                        interventionsDataList = new List<PatientInterventionData>();
                        foreach (MEPatientIntervention b in meInterventions)
                        {
                            PatientInterventionData interventionData = new PatientInterventionData
                            {
                                Id = b.Id.ToString(),
                                Description = b.Description,
                                PatientGoalId = b.PatientGoalId.ToString(),
                                CategoryId = b.CategoryId == null ? null : b.CategoryId.ToString(),
                                AssignedToId = b.AssignedToId == null ? null : b.AssignedToId.ToString(),
                                BarrierIds = Helper.ConvertToStringList(b.BarrierIds),
                                StatusId = ((int)b.Status),
                                StatusDate = b.StatusDate,
                                StartDate = b.StartDate,
                                DueDate = b.DueDate,
                                ClosedDate = b.ClosedDate,
                                CreatedById = b.RecordCreatedBy.ToString(),
                                DeleteFlag = b.DeleteFlag,
                                GoalName = goalName,
                                PatientId = patientId,
                                Details = b.Details
                            };
                            interventionsDataList.Add(interventionData);
                        }
                    }
                }
                return interventionsDataList;
            }
            catch (Exception) { throw; }

        }
       
        public string UserId { get; set; }


        public void UndoDelete(object entity)
        {
            UndoDeleteInterventionDataRequest request = (UndoDeleteInterventionDataRequest)entity;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientIntervention>.EQ(b => b.Id, ObjectId.Parse(request.InterventionId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientIntervention.TTLDateProperty, BsonNull.Value));
                    uv.Add(MB.Update.Set(MEPatientIntervention.DeleteFlagProperty, false));
                    uv.Add(MB.Update.Set(MEPatientIntervention.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEPatientIntervention.LastUpdatedOnProperty, DateTime.UtcNow));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientInterventions.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientIntervention.ToString(),
                                            request.InterventionId.ToString(),
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
            List<PatientInterventionData> list = null;
            GetPatientInterventionsDataRequest dataRequest = (GetPatientInterventionsDataRequest)request;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEPatientIntervention.DeleteFlagProperty, false));
                    if (!string.IsNullOrEmpty(dataRequest.AssignedToId))
                    {
                        queries.Add(Query.EQ(MEPatientIntervention.AssignedToProperty, ObjectId.Parse(dataRequest.AssignedToId)));
                    }
                    if (!string.IsNullOrEmpty(dataRequest.CreatedById))
                    {
                        queries.Add(Query.EQ(MEPatientIntervention.RecordCreatedByProperty, ObjectId.Parse(dataRequest.CreatedById)));
                    }
                    if (dataRequest.StatusIds != null && dataRequest.StatusIds.Count > 0)
                    {
                        queries.Add(Query.In(MEPatientIntervention.StatusProperty, new BsonArray(dataRequest.StatusIds)));
                    }
                    if (patientGoalIds != null && patientGoalIds.Count > 0)
                    {
                        List<BsonValue> bsonList = Helper.ConvertToBsonValueList(patientGoalIds);
                        queries.Add(Query.In(MEPatientIntervention.PatientGoalIdProperty, bsonList));
                    }
                    IMongoQuery mQuery = Query.And(queries);
                    List<MEPatientIntervention> meInterventions = null;
                    meInterventions = ctx.PatientInterventions.Collection.Find(mQuery).ToList();
                  
                    if (meInterventions != null && meInterventions.Count > 0)
                    {
                        //We get the Patient Goals for the found interventions
                        List<MEPatientGoal> mePatientGoals = null;                                           
                        mQuery = Query.In(MEPatientGoal.IdProperty, meInterventions.Select(x => BsonValue.Create(x.PatientGoalId)).ToList());
                        mePatientGoals = ctx.PatientGoals.Collection.Find(mQuery).ToList();

                        list = new List<PatientInterventionData>();
                        foreach (MEPatientIntervention b in meInterventions)
                        {
                            PatientInterventionData interventionData = new PatientInterventionData
                            {
                                Id = b.Id.ToString(),
                                Description = b.Description,
                                PatientGoalId = b.PatientGoalId.ToString(),
                                CategoryId = b.CategoryId == null ? null : b.CategoryId.ToString(),
                                AssignedToId = b.AssignedToId == null ? null : b.AssignedToId.ToString(),
                                BarrierIds = Helper.ConvertToStringList(b.BarrierIds),
                                StatusId = ((int)b.Status),
                                StatusDate = b.StatusDate,
                                StartDate = b.StartDate,                                
                                DueDate = b.DueDate,
                                ClosedDate = b.ClosedDate,
                                CreatedById = b.RecordCreatedBy.ToString(),
                                DeleteFlag = b.DeleteFlag,
                                Details = b.Details
                            };
                            //var mePG = ctx.PatientGoals.Collection.Find(Query.EQ(MEPatientGoal.IdProperty, ObjectId.Parse(interventionData.PatientGoalId))).SetFields(MEPatientGoal.PatientIdProperty, MEPatientGoal.NameProperty).FirstOrDefault();
                            var mePG = mePatientGoals.FirstOrDefault(x => x.Id == b.PatientGoalId);
                            if (mePG != null)
                            {
                                interventionData.PatientId = mePG.PatientId.ToString();
                                interventionData.GoalName = mePG.Name;
                            }
                            list.Add(interventionData);
                        }         
                    }
                }
                return list;
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Finds existing patient interventions that are in an open state. This is specifically used for spawn logic.
        /// </summary>
        /// <param name="patientGoalId">string</param>
        /// <param name="entityID">string</param>
        /// <returns>PatientInterventionData</returns>
        public object FindByTemplateId(string patientGoalId, string entityID)
        {
            PatientInterventionData interventionData = null;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>
                    {
                        Query.EQ(MEPatientIntervention.PatientGoalIdProperty, ObjectId.Parse(patientGoalId)),
                        Query.EQ(MEPatientIntervention.TemplateIdProperty, ObjectId.Parse(entityID)),
                        // Open = 1, Completed = 2, Removed =3
                        Query.EQ(MEPatientIntervention.StatusProperty, 1),
                        Query.EQ(MEPatientIntervention.DeleteFlagProperty, false),
                        Query.EQ(MEPatientIntervention.TTLDateProperty, BsonNull.Value)
                    };

                    var mQuery = Query.And(queries);
                    var b = ctx.PatientInterventions.Collection.Find(mQuery).FirstOrDefault();

                    if (b != null)
                    {
                        interventionData = new PatientInterventionData
                        {
                            Id = b.Id.ToString(),
                            Description = b.Description,
                            PatientGoalId = b.PatientGoalId.ToString(),
                            CategoryId = b.CategoryId == null ? null : b.CategoryId.ToString(),
                            AssignedToId = b.AssignedToId == null ? null : b.AssignedToId.ToString(),
                            BarrierIds = Helper.ConvertToStringList(b.BarrierIds),
                            StatusId = ((int)b.Status),
                            StatusDate = b.StatusDate,
                            StartDate = b.StartDate,
                            DueDate = b.DueDate,
                            ClosedDate = b.ClosedDate,
                            CreatedById = b.RecordCreatedBy.ToString(),
                            DeleteFlag = b.DeleteFlag,
                            Details = b.Details
                        };
                        var mePG = ctx.PatientGoals.Collection.Find(Query.EQ(MEPatientGoal.IdProperty, ObjectId.Parse(interventionData.PatientGoalId))).SetFields(MEPatientGoal.PatientIdProperty, MEPatientGoal.NameProperty).FirstOrDefault();
                        if (mePG != null)
                        {
                            interventionData.PatientId = mePG.PatientId.ToString();
                            interventionData.GoalName = mePG.Name;
                        }
                    }
                }
                return interventionData;
            }
            catch (Exception) { throw; }
        }
    }
}
