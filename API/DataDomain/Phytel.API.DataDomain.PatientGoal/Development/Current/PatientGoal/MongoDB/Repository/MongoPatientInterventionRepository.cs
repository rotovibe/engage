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
using Phytel.API.DataDomain.PatientGoal.MongoDB;

namespace Phytel.API.DataDomain.PatientGoal
{
    public class MongoPatientInterventionRepository<T> : IPatientGoalRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoPatientInterventionRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            PatientInterventionData pt = (PatientInterventionData)newEntity;
            try
            {
                MEPatientIntervention pa = new MEPatientIntervention
                {
                    Id = ObjectId.GenerateNewId(),
                    TTLDate = pt.TTLDate,
                    PatientGoalId = ObjectId.Parse(pt.PatientGoalId)
                };

                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    WriteConcernResult wcr = ctx.PatientInterventions.Collection.Insert(pa);
                    if (wcr.Ok)
                    {
                        pt.Id = pa.Id.ToString();
                    }
                }
                return pt as object;
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
            PatientInterventionData pt = (PatientInterventionData)entity;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientIntervention>.EQ(b => b.Id, ObjectId.Parse(pt.Id));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientIntervention.UpdatedByProperty, pt.PatientGoalId));
                    uv.Add(MB.Update.Set(MEPatientIntervention.TTLDateProperty, BsonNull.Value));
                    if (pt.Description != null) uv.Add(MB.Update.Set(MEPatientIntervention.DescriptionProperty, pt.Description));
                    if (pt.StartDate != null) uv.Add(MB.Update.Set(MEPatientIntervention.StartDateProperty, pt.StartDate));
                    if (pt.StatusDate != null) uv.Add(MB.Update.Set(MEPatientIntervention.StatusDateProperty, pt.StatusDate));
                    if (pt.Status != 0) uv.Add(MB.Update.Set(MEPatientIntervention.StatusProperty, pt.Status));
                    if (pt.Order != 0) uv.Add(MB.Update.Set(MEPatientIntervention.OrderProperty, pt.Order));
                    if (pt.Category != 0) uv.Add(MB.Update.Set(MEPatientIntervention.CategoryProperty, pt.Category));
                    if (pt.Attributes != null) { uv.Add(MB.Update.SetWrapped<List<MAttribute>>(MEPatientIntervention.AttributesProperty, DTOUtil.GetInterventionAttributes(pt.Attributes))); }
                    if (pt.Barriers != null) { uv.Add(MB.Update.SetWrapped<List<ObjectId>>(MEPatientIntervention.BarriersProperty, DTOUtil.ConvertObjectId(pt.Barriers))); }

                    IMongoUpdate update = MB.Update.Combine(uv);
                    WriteConcernResult res = ctx.PatientInterventions.Collection.Update(q, update);
                    if (res.Ok)
                        result = true;
                }
                return result as object;
            }
            catch (Exception ex) { throw ex; }
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
            PutInitializeInterventionRequest ptr = (PutInitializeInterventionRequest)newEntity;
            string intrvId = null;
            try
            {
                MEPatientIntervention pi = new MEPatientIntervention
                {
                    Id = ObjectId.GenerateNewId(),
                    PatientGoalId = ObjectId.Parse(ptr.PatientGoalId),
                    TTLDate = System.DateTime.UtcNow.AddDays(1),
                    UpdatedBy = ptr.UserId,
                    LastUpdatedOn = DateTime.UtcNow
                };

                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    WriteConcernResult wcr = ctx.PatientInterventions.Collection.Insert(pi);
                    if (wcr.Ok)
                    {
                        intrvId = pi.Id.ToString();
                    }
                }
                return intrvId;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
