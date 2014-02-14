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
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
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
                IMongoQuery mQuery = null;
                List<object> PatientGoalItems = new List<object>();

                mQuery = MongoDataUtil.ExpressionQueryBuilder(expression);

                //using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                //{
                //}

                return new Tuple<string, IEnumerable<object>>(expression.ExpressionID, PatientGoalItems);
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
                    if (pb.Status != 0) uv.Add(MB.Update.Set(MEPatientBarrier.StatusProperty, pb.Status));
                    if (pb.StatusDate != null) uv.Add(MB.Update.Set(MEPatientBarrier.StatusDateProperty, pb.StatusDate));
                    if (pb.Category != null) uv.Add(MB.Update.Set(MEPatientBarrier.CategoryProperty, ObjectId.Parse(pb.Category)));

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
    }
}
