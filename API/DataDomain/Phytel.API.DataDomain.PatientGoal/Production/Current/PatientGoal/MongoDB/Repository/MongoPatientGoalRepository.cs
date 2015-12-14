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
    public class MongoPatientGoalRepository : IGoalRepository
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        private int _initializeDays = Convert.ToInt32(ConfigurationManager.AppSettings["InitializeDays"]);

        static MongoPatientGoalRepository()
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
                if (BsonClassMap.IsClassMapRegistered(typeof(MEPatientGoal)) == false)
                    BsonClassMap.RegisterClassMap<MEPatientGoal>();
            }
            catch { }
            #endregion
        }

        public MongoPatientGoalRepository(string contractDBName)
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
            DeletePatientGoalDataRequest request = (DeletePatientGoalDataRequest)entity;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientGoal>.EQ(b => b.Id, ObjectId.Parse(request.PatientGoalId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientGoal.TTLDateProperty, System.DateTime.UtcNow.AddDays(_expireDays)));
                    uv.Add(MB.Update.Set(MEPatientGoal.DeleteFlagProperty, true));
                    uv.Add(MB.Update.Set(MEPatientGoal.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEPatientGoal.LastUpdatedOnProperty, DateTime.UtcNow));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientGoals.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.PatientGoal.ToString(), 
                                            request.PatientGoalId.ToString(), 
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

        public object FindByTemplateId(string patientId, string entityID)
        {
            PatientGoalData goalData = null;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>
                    {
                        Query.EQ(MEPatientGoal.PatientIdProperty, ObjectId.Parse(patientId)),
                        Query.EQ(MEPatientGoal.TemplateIdProperty, ObjectId.Parse(entityID)),
                        Query.In(MEPatientGoal.StatusProperty, new BsonArray{1, 3}),
                        Query.EQ(MEPatientGoal.DeleteFlagProperty, false),
                        Query.EQ(MEPatientGoal.TTLDateProperty, BsonNull.Value)
                    };

                    var mQuery = Query.And(queries);
                    var mePG = ctx.PatientGoals.Collection.Find(mQuery).FirstOrDefault();

                    if (mePG != null)
                    {
                        goalData = new PatientGoalData
                        {
                            Id = mePG.Id.ToString(),
                            PatientId = mePG.PatientId.ToString(),
                            FocusAreaIds = Helper.ConvertToStringList(mePG.FocusAreaIds),
                            Name = mePG.Name,
                            SourceId = (mePG.SourceId == null) ? null : mePG.SourceId.ToString(),
                            ProgramIds = Helper.ConvertToStringList(mePG.ProgramIds),
                            TypeId = ((int) mePG.Type),
                            StatusId = ((int) mePG.Status),
                            StartDate = mePG.StartDate,
                            EndDate = mePG.EndDate,
                            TargetDate = mePG.TargetDate,
                            TargetValue = mePG.TargetValue,
                            CustomAttributes = DTOUtil.GetCustomAttributeIdAndValues(mePG.Attributes),
                            Details = mePG.Details
                        };
                    }
                }
                return goalData;
            }
            catch (Exception) { throw; }
        }

        public object FindByID(string entityID)
        {
            PatientGoalData goalData = null;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEPatientGoal.IdProperty, ObjectId.Parse(entityID)));
                    queries.Add(Query.EQ(MEPatientGoal.DeleteFlagProperty, false));
                    queries.Add(Query.EQ(MEPatientGoal.TTLDateProperty, BsonNull.Value));
                    IMongoQuery mQuery = Query.And(queries);
                    MEPatientGoal mePG = ctx.PatientGoals.Collection.Find(mQuery).FirstOrDefault();
                    if (mePG != null)
                    {
                        goalData = new PatientGoalData
                        {
                            Id = mePG.Id.ToString(),
                            PatientId = mePG.PatientId.ToString(),
                            FocusAreaIds = Helper.ConvertToStringList(mePG.FocusAreaIds),
                            Name = mePG.Name,
                            SourceId = (mePG.SourceId == null) ? null : mePG.SourceId.ToString(),
                            ProgramIds = Helper.ConvertToStringList(mePG.ProgramIds),
                            TypeId =((int)mePG.Type),
                            StatusId = ((int)mePG.Status),
                            StartDate = mePG.StartDate,
                            EndDate = mePG.EndDate,
                            TargetDate = mePG.TargetDate,
                            TargetValue = mePG.TargetValue,
                            CustomAttributes = DTOUtil.GetCustomAttributeIdAndValues(mePG.Attributes),
                            Details = mePG.Details
                        };
                    }
                }
                return goalData;
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
            PutUpdateGoalDataRequest pgr = (PutUpdateGoalDataRequest)entity;
            PatientGoalData pt = pgr.Goal;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientGoal>.EQ(b => b.Id, ObjectId.Parse(pt.Id));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientGoal.TTLDateProperty, BsonNull.Value));
                    uv.Add(MB.Update.Set(MEPatientGoal.DeleteFlagProperty, false));
                    uv.Add(MB.Update.Set(MEPatientGoal.VersionProperty, pgr.Version));
                    uv.Add(MB.Update.Set(MEPatientGoal.LastUpdatedOnProperty, DateTime.UtcNow));
                    uv.Add(MB.Update.Set(MEPatientGoal.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    if (pt.PatientId != null) uv.Add(MB.Update.Set(MEPatientGoal.PatientIdProperty, ObjectId.Parse(pt.PatientId)));
                    if (pt.FocusAreaIds != null) { uv.Add(MB.Update.SetWrapped<List<ObjectId>>(MEPatientGoal.FocusAreaProperty, DTOUtil.ConvertObjectId(pt.FocusAreaIds))); }
                    if (pt.Name != null) uv.Add(MB.Update.Set(MEPatientGoal.NameProperty, pt.Name));
                    if (pt.SourceId != null) uv.Add(MB.Update.Set(MEPatientGoal.SourceProperty, ObjectId.Parse(pt.SourceId)));
                    if (pt.ProgramIds != null) { uv.Add(MB.Update.SetWrapped<List<ObjectId>>(MEPatientGoal.ProgramProperty, DTOUtil.ConvertObjectId(pt.ProgramIds))); }
                    if (pt.TypeId != null) uv.Add(MB.Update.Set(MEPatientGoal.TypeProperty, pt.TypeId)); 
                    if (pt.StatusId != 0) uv.Add(MB.Update.Set(MEPatientGoal.StatusProperty, pt.StatusId ));
                    if (pt.TargetValue != null) uv.Add(MB.Update.Set(MEPatientGoal.TargetValueProperty, pt.TargetValue));
                    if (pt.TemplateId != null) uv.Add(MB.Update.Set(MEPatientGoal.TemplateIdProperty, ObjectId.Parse(pt.TemplateId)));

                    if (pt.StartDate != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientGoal.StartDateProperty, pt.StartDate));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientGoal.StartDateProperty, BsonNull.Value));
                    }
                    if (pt.EndDate != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientGoal.EndDateProperty, pt.EndDate));
                    }
                    else 
                    {
                        uv.Add(MB.Update.Set(MEPatientGoal.EndDateProperty, BsonNull.Value));
                    }
                    if (pt.TargetDate != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientGoal.TargetDateProperty, pt.TargetDate));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientGoal.TargetDateProperty, BsonNull.Value));
                    }
                    if (pt.CustomAttributes != null) { uv.Add(MB.Update.SetWrapped<List<MAttribute>>(MEPatientGoal.AttributesProperty, DTOUtil.GetAttributes(pt.CustomAttributes))); }
                    if (pt.Details != null) uv.Add(MB.Update.Set(MEPatientGoal.DetailProperty, pt.Details));
                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientGoals.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.PatientGoal.ToString(),
                                            pt.Id.ToString(), 
                                            Common.DataAuditType.Update, 
                                            pgr.ContractNumber);
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
            PutInitializeGoalDataRequest request = (PutInitializeGoalDataRequest)newEntity;
            PatientGoalData patientGoalData = null;
            MEPatientGoal mePg = null;
            try
            {
                mePg = new MEPatientGoal(this.UserId)
                {
                    Id = ObjectId.GenerateNewId(),
                    PatientId = ObjectId.Parse(request.PatientId),
                    TTLDate = System.DateTime.UtcNow.AddDays(_initializeDays)
                    //,
                    //LastUpdatedOn = DateTime.UtcNow,
                    //UpdatedBy = ObjectId.Parse(this.UserId)
                };
                
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    ctx.PatientGoals.Collection.Insert(mePg);

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.PatientGoal.ToString(), 
                                            mePg.Id.ToString(), 
                                            Common.DataAuditType.Insert, 
                                            request.ContractNumber);

                    patientGoalData = new PatientGoalData
                    {
                        Id = mePg.Id.ToString()
                    };
                }
                return patientGoalData;
            }
            catch (Exception) { throw; }
        }

        public IEnumerable<object> Find(string Id)
        {
            try
            {
                List<PatientGoalViewData> goalsViewDataList = null;
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEPatientGoal.PatientIdProperty, ObjectId.Parse(Id)));
                queries.Add(Query.EQ(MEPatientGoal.DeleteFlagProperty, false));
                queries.Add(Query.EQ(MEPatientGoal.TTLDateProperty, BsonNull.Value));
                IMongoQuery mQuery = Query.And(queries);
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    
                    List<MEPatientGoal> meGoals = ctx.PatientGoals.Collection.Find(mQuery).ToList();
                    if (meGoals != null)
                    {
                        goalsViewDataList = new List<PatientGoalViewData>();
                        foreach (MEPatientGoal b in meGoals)
                        {
                            PatientGoalViewData goalViewData = new PatientGoalViewData
                            {
                                Id = b.Id.ToString(),
                                PatientId = b.PatientId.ToString(),
                                FocusAreaIds = Helper.ConvertToStringList(b.FocusAreaIds),
                                Name = b.Name,
                                StatusId = ((int)b.Status)
                            };
                            goalsViewDataList.Add(goalViewData);
                        }
                        goalsViewDataList = goalsViewDataList.OrderByDescending(o => o.Id).ToList();
                    }
                }
                return goalsViewDataList;
            }
            catch (Exception) { throw; }
        }

        public string UserId { get; set; }


        public void UndoDelete(object entity)
        {
            UndoDeletePatientGoalDataRequest request = (UndoDeletePatientGoalDataRequest)entity;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientGoal>.EQ(b => b.Id, ObjectId.Parse(request.PatientGoalId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientGoal.TTLDateProperty, BsonNull.Value));
                    uv.Add(MB.Update.Set(MEPatientGoal.DeleteFlagProperty, false));
                    uv.Add(MB.Update.Set(MEPatientGoal.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEPatientGoal.LastUpdatedOnProperty, DateTime.UtcNow));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientGoals.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientGoal.ToString(),
                                            request.PatientGoalId.ToString(),
                                            Common.DataAuditType.UndoDelete,
                                            request.ContractNumber);
                }
            }
            catch (Exception) { throw; }
        }

        public void RemoveProgram(object entity, List<string> updatedProgramIds)
        {
            RemoveProgramInPatientGoalsDataRequest request = (RemoveProgramInPatientGoalsDataRequest)entity;
            try
            {
                List<ObjectId> ids = updatedProgramIds.ConvertAll(r => ObjectId.Parse(r));
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientGoal>.EQ(b => b.Id, ObjectId.Parse(request.PatientGoalId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.SetWrapped<List<ObjectId>>(MEPatientGoal.ProgramProperty, ids));
                    uv.Add(MB.Update.Set(MEPatientGoal.LastUpdatedOnProperty, DateTime.UtcNow));
                    uv.Add(MB.Update.Set(MEPatientGoal.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientGoals.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientGoal.ToString(),
                                            request.PatientGoalId.ToString(),
                                            Common.DataAuditType.Update,
                                            request.ContractNumber);

                }
            }
            catch (Exception) { throw; }
        }

        public IEnumerable<object> FindGoalsWithAProgramId(string entityId)
        {
            List<PatientGoalData> goals = null;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.In(MEPatientGoal.ProgramProperty, new List<BsonValue> { BsonValue.Create(ObjectId.Parse(entityId)) }));
                    queries.Add(Query.EQ(MEPatientGoal.DeleteFlagProperty, false));
                    queries.Add(Query.EQ(MEPatientGoal.TTLDateProperty, BsonNull.Value));
                    IMongoQuery mQuery = Query.And(queries);
                    List<MEPatientGoal> meGoals = ctx.PatientGoals.Collection.Find(mQuery).ToList();
                    if (meGoals != null && meGoals.Count > 0)
                    {
                        goals = new List<PatientGoalData>();
                        foreach (MEPatientGoal mePG in meGoals)
                        {
                            PatientGoalData data = new PatientGoalData
                            {
                                Id = mePG.Id.ToString(),
                                PatientId = mePG.PatientId.ToString(),
                                Name = mePG.Name,
                                ProgramIds = Helper.ConvertToStringList(mePG.ProgramIds)
                            };
                            goals.Add(data);
                        }
                    }
                }
                return goals;
            }
            catch (Exception ex) { throw ex; }
        }


        public IEnumerable<object> Search(object request, List<string> patientGoalIds)
        {
            throw new NotImplementedException();
        }
    }
}
