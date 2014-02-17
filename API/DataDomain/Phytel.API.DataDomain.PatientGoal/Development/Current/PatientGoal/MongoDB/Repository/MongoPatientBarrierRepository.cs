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
using Phytel.API.AppDomain.PatientGoal;
using MB = MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.Common;
using Phytel.API.Common.Data;

namespace Phytel.API.DataDomain.PatientGoal
{
    public class MongoPatientBarrierRepository<T> : IPatientGoalRepository<T>
    {
        private string _dbName = string.Empty;

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
            DeleteBarrierRequest request = (DeleteBarrierRequest)entity;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientBarrier>.EQ(b => b.PatientGoalId, ObjectId.Parse(request.PatientGoalId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientBarrier.TTLDateProperty, System.DateTime.UtcNow.AddDays(7)));
                    uv.Add(MB.Update.Set(MEPatientBarrier.DeleteFlagProperty, true));
                    uv.Add(MB.Update.Set(MEPatientBarrier.UpdatedByProperty, request.UserId));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    WriteConcernResult res = ctx.PatientBarriers.Collection.Update(q, update);
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
            catch (Exception ex) { throw ex; }
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
            PutUpdateBarrierRequest pbr = (PutUpdateBarrierRequest)entity;
            PatientBarrierData pb = pbr.Barrier;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientBarrier>.EQ(b => b.Id, ObjectId.Parse(pb.Id));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientBarrier.TTLDateProperty, BsonNull.Value));
                    uv.Add(MB.Update.Set(MEPatientBarrier.UpdatedByProperty, pbr.UserId));
                    uv.Add(MB.Update.Set(MEPatientBarrier.LastUpdatedOnProperty, System.DateTime.UtcNow));
                    if (pb.Name != null) uv.Add(MB.Update.Set(MEPatientBarrier.NameProperty, pb.Name));
                    if (pb.PatientGoalId != null) uv.Add(MB.Update.Set(MEPatientBarrier.PatientGoalIdProperty, ObjectId.Parse(pb.PatientGoalId)));
                    if (pb.StatusId != 0) uv.Add(MB.Update.Set(MEPatientBarrier.StatusProperty, pb.StatusId));
                    if (pb.StatusDate != null) uv.Add(MB.Update.Set(MEPatientBarrier.StatusDateProperty, pb.StatusDate));
                    if (pb.CategoryId != null) uv.Add(MB.Update.Set(MEPatientBarrier.CategoryProperty, ObjectId.Parse(pb.CategoryId)));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    WriteConcernResult res = ctx.PatientBarriers.Collection.Update(q, update);
                    if (res.Ok)
                        result = true;
                }
                return result as object;
            }
            catch (Exception ex) { throw new Exception("DD:MongoPatientBarrierRepository:Update()" + ex.Message, ex.InnerException); }
        }

        public void CacheByID(List<string> entityIDs)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public string Initialize(object newEntity)
        {
            PutInitializeBarrierDataRequest request = (PutInitializeBarrierDataRequest)newEntity;
            string Id = null;
            try
            {
                MEPatientBarrier mePb = new MEPatientBarrier
                {
                    Id = ObjectId.GenerateNewId(),
                    PatientGoalId = ObjectId.Parse(request.PatientGoalId),
                    Status = BarrierStatus.Open,
                    StatusDate = DateTime.UtcNow,
                    TTLDate = System.DateTime.UtcNow.AddDays(1),
                    UpdatedBy = request.UserId,
                    LastUpdatedOn = DateTime.UtcNow
                };

                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    WriteConcernResult wcr = ctx.PatientBarriers.Collection.Insert(mePb);
                    if (wcr.Ok)
                    {
                        Id = mePb.Id.ToString();
                    }
                }
                return Id;
            }
            catch (Exception ex) { throw ex; }
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
                                CategoryId = b.Category == null ? null : b.Category.ToString(),
                                StatusId = ((int)b.Status),
                                StatusDate = b.StatusDate
                            };
                            barriersDataList.Add(barrierData);
                        }
                    }
                }
                return barriersDataList;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
