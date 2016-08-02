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
    public class MongoPatientBarrierRepository : IGoalRepository
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        private int _initializeDays = Convert.ToInt32(ConfigurationManager.AppSettings["InitializeDays"]);

        static MongoPatientBarrierRepository()
        {
            #region Register ClassMap
            try 
            {
            if (BsonClassMap.IsClassMapRegistered(typeof(GoalBase)) == false)
                BsonClassMap.RegisterClassMap<GoalBase>();
            }
            catch {}

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(MEPatientBarrier)) == false)
                    BsonClassMap.RegisterClassMap<MEPatientBarrier>();
            }
            catch {}
            #endregion
        }
        public MongoPatientBarrierRepository(string contractDBName)
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
            DeleteBarrierDataRequest request = (DeleteBarrierDataRequest)entity;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientBarrier>.EQ(b => b.Id, ObjectId.Parse(request.BarrierId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientBarrier.TTLDateProperty, System.DateTime.UtcNow.AddDays(_expireDays)));
                    uv.Add(MB.Update.Set(MEPatientBarrier.DeleteFlagProperty, true));
                    uv.Add(MB.Update.Set(MEPatientBarrier.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEPatientBarrier.LastUpdatedOnProperty, DateTime.UtcNow));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    WriteConcernResult res = ctx.PatientBarriers.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.PatientBarrier.ToString(), 
                                            request.BarrierId.ToString(), 
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
                PatientBarrierData barrierData = null;
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEPatientBarrier.IdProperty, ObjectId.Parse(entityID)));
                queries.Add(Query.EQ(MEPatientBarrier.DeleteFlagProperty, false));
                queries.Add(Query.EQ(MEPatientBarrier.TTLDateProperty, BsonNull.Value));
                IMongoQuery mQuery = Query.And(queries);
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    MEPatientBarrier b = ctx.PatientBarriers.Collection.Find(mQuery).FirstOrDefault();
                    if (b != null)
                    {
                        barrierData = new PatientBarrierData { 
                            Id = b.Id.ToString(),
                            Name = b.Name,
                            PatientGoalId = b.PatientGoalId.ToString(),
                            CategoryId = b.CategoryId == null ? null : b.CategoryId.ToString(),
                            StatusId = ((int)b.Status),
                            StatusDate = b.StatusDate,
                            DeleteFlag = b.DeleteFlag,
                            Details = b.Details
                        };
                    }
                }
                return barrierData;
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
            PutUpdateBarrierRequest pbr = (PutUpdateBarrierRequest)entity;
            PatientBarrierData pb = pbr.Barrier;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientBarrier>.EQ(b => b.Id, ObjectId.Parse(pb.Id));

                    // Set the StatusDate to Now if the status is changed.
                    MEPatientBarrier existingPB = ctx.PatientBarriers.Collection.Find(q).SetFields(MEPatientBarrier.StatusProperty).FirstOrDefault();
                    if (existingPB != null)
                    {
                        if ((int)existingPB.Status != pb.StatusId)
                        {
                            pb.StatusDate = DateTime.UtcNow;
                        }
                    }

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientBarrier.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEPatientBarrier.VersionProperty, pbr.Version));
                    uv.Add(MB.Update.Set(MEPatientBarrier.LastUpdatedOnProperty, System.DateTime.UtcNow));
                    if (pb.Name != null) uv.Add(MB.Update.Set(MEPatientBarrier.NameProperty, pb.Name));
                    if (pb.Details != null) uv.Add(MB.Update.Set(MEPatientBarrier.DetailProperty, pb.Details));
                    if (pb.PatientGoalId != null) uv.Add(MB.Update.Set(MEPatientBarrier.PatientGoalIdProperty, ObjectId.Parse(pb.PatientGoalId)));
                    if (pb.StatusId != 0) uv.Add(MB.Update.Set(MEPatientBarrier.StatusProperty, pb.StatusId));
                    if (pb.StatusDate != null) uv.Add(MB.Update.Set(MEPatientBarrier.StatusDateProperty, pb.StatusDate));
                    if (pb.CategoryId != null) uv.Add(MB.Update.Set(MEPatientBarrier.CategoryProperty, ObjectId.Parse(pb.CategoryId)));
                    uv.Add(MB.Update.Set(MEPatientBarrier.DeleteFlagProperty, pb.DeleteFlag));
                    DataAuditType type;
                    if (pb.DeleteFlag)
                    {
                        uv.Add(MB.Update.Set(MEPatientBarrier.TTLDateProperty, System.DateTime.UtcNow.AddDays(_expireDays)));
                        type = Common.DataAuditType.Delete;
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientBarrier.TTLDateProperty, BsonNull.Value));
                        type = Common.DataAuditType.Update;
                    }
                    IMongoUpdate update = MB.Update.Combine(uv);
                    
                    ctx.PatientBarriers.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(pbr.UserId, 
                                            MongoCollectionName.PatientBarrier.ToString(), 
                                            pb.Id.ToString(),
                                            type, 
                                            pbr.ContractNumber);
                    result = true;
                }
                return result as object;
            }
            catch (Exception ex) { throw new Exception("PatientGoalDD:MongoPatientBarrierRepository:Update()" + ex.Message, ex.InnerException); }
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
            PutInitializeBarrierDataRequest request = (PutInitializeBarrierDataRequest)newEntity;
            MEPatientBarrier mePb = null;
            try
            {
                mePb = new MEPatientBarrier(this.UserId)
                {
                    Id = ObjectId.GenerateNewId(),
                    PatientGoalId = ObjectId.Parse(request.PatientGoalId),
                    TTLDate = System.DateTime.UtcNow.AddDays(_initializeDays),
                    StatusDate = DateTime.UtcNow,
                    DeleteFlag = false
                    //,
                    //LastUpdatedOn = DateTime.UtcNow,
                    //UpdatedBy = ObjectId.Parse(this.UserId),
                    //Version = request.Version
                };

                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    ctx.PatientBarriers.Collection.Insert(mePb);

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.PatientBarrier.ToString(),
                                            mePb.Id.ToString(), 
                                            Common.DataAuditType.Insert, 
                                            request.ContractNumber);
                }
                return mePb.Id.ToString();
            }
            catch (Exception) { throw; }
        }

        public IEnumerable<object> Find(string Id)
        {
            try
            {
                List<PatientBarrierData> barriersDataList = null;
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEPatientBarrier.PatientGoalIdProperty, ObjectId.Parse(Id)));
                queries.Add(Query.EQ(MEPatientBarrier.DeleteFlagProperty, false));
                queries.Add(Query.EQ(MEPatientBarrier.TTLDateProperty, BsonNull.Value));
                IMongoQuery mQuery = Query.And(queries);
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    List<MEPatientBarrier> meBarriers = ctx.PatientBarriers.Collection.Find(mQuery).ToList();
                    if (meBarriers != null)
                    {
                        barriersDataList = new List<PatientBarrierData>();
                        foreach (MEPatientBarrier b in meBarriers)
                        {
                            PatientBarrierData barrierData = new PatientBarrierData
                            {
                                Id = b.Id.ToString(),
                                Name = b.Name,
                                PatientGoalId = b.PatientGoalId.ToString(),
                                CategoryId = b.CategoryId == null ? null : b.CategoryId.ToString(),
                                StatusId = ((int)b.Status),
                                StatusDate = b.StatusDate,
                                DeleteFlag = b.DeleteFlag,
                                Details = b.Details
                            };
                            barriersDataList.Add(barrierData);
                        }
                    }
                }
                return barriersDataList;
            }
            catch (Exception) { throw; }
        }
      
        public string UserId { get; set; }


        public void UndoDelete(object entity)
        {
            UndoDeleteBarrierDataRequest request = (UndoDeleteBarrierDataRequest)entity;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientBarrier>.EQ(b => b.Id, ObjectId.Parse(request.BarrierId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientBarrier.TTLDateProperty, BsonNull.Value));
                    uv.Add(MB.Update.Set(MEPatientBarrier.DeleteFlagProperty, false));
                    uv.Add(MB.Update.Set(MEPatientBarrier.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEPatientBarrier.LastUpdatedOnProperty, DateTime.UtcNow));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    WriteConcernResult res = ctx.PatientBarriers.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientBarrier.ToString(),
                                            request.BarrierId.ToString(),
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
            throw new NotImplementedException();
        }


        public object FindByTemplateId(string patientId, string entityID)
        {
            throw new NotImplementedException();
        }
    }
}
