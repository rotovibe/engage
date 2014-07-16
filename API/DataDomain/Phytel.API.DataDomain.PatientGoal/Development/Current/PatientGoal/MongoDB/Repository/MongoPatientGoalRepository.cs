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
    public class MongoPatientGoalRepository<T> : IPatientGoalRepository<T>
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
                            CustomAttributes = DTOUtil.GetCustomAttributeIdAndValues(mePG.Attributes)
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
            PutPatientGoalDataRequest pgr = (PutPatientGoalDataRequest)entity;
            PatientGoalData pt = pgr.GoalData;
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
                    if (pt.StartDate != null) uv.Add(MB.Update.Set(MEPatientGoal.StartDateProperty, pt.StartDate));
                    if (pt.EndDate != null) uv.Add(MB.Update.Set(MEPatientGoal.EndDateProperty, pt.EndDate));
                    if (pt.TargetValue != null) uv.Add(MB.Update.Set(MEPatientGoal.TargetValueProperty, pt.TargetValue));
                    if (pt.TargetDate != null) uv.Add(MB.Update.Set(MEPatientGoal.TargetDateProperty, pt.TargetDate));
                    if (pt.CustomAttributes != null) { uv.Add(MB.Update.SetWrapped<List<MAttribute>>(MEPatientGoal.AttributesProperty, DTOUtil.GetAttributes(pt.CustomAttributes))); }
                    
                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientGoals.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.PatientGoal.ToString(),
                                            pt.Id.ToString(), 
                                            Common.DataAuditType.Update, 
                                            pgr.ContractNumber);
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
        
        public IEnumerable<object> FindByGoalId(string Id)
        {
            throw new NotImplementedException();
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
    }
}
