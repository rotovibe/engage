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
    public class MongoPatientGoalRepository<T> : IPatientGoalRepository<T>
    {
        private string _dbName = string.Empty;

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
            PatientGoalData goalData = null;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEPatientGoal.IdProperty, ObjectId.Parse(entityID)));
                    queries.Add(Query.EQ(MEPatientGoal.DeleteFlagProperty, false));
                    queries.Add(Query.NE(MEPatientGoal.TTLDateProperty, null));
                    IMongoQuery mQuery = Query.And(queries);
                    MEPatientGoal mePG = ctx.PatientGoals.Collection.Find(mQuery).FirstOrDefault();
                    if (mePG != null)
                    {
                        goalData = new PatientGoalData
                        {
                            

                        };
                    }
                }
                return goalData;
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
            PatientGoalData pt = (PatientGoalData)entity;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientGoal>.EQ(b => b.Id, ObjectId.Parse(pt.Id));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientGoal.TTLDateProperty, BsonNull.Value));
                    if (pt.PatientId != null) uv.Add(MB.Update.Set(MEPatientGoal.PatientIdProperty, ObjectId.Parse(pt.PatientId)));
                    if (pt.FocusAreas != null) { uv.Add(MB.Update.SetWrapped<List<ObjectId>>(MEPatientGoal.FocusAreaProperty, DTOUtil.ConvertObjectId(pt.FocusAreas))); }
                    if (pt.Name != null) uv.Add(MB.Update.Set(MEPatientGoal.NameProperty, pt.Name));
                    if (pt.Source != null) uv.Add(MB.Update.Set(MEPatientGoal.SourceProperty, pt.Source));
                    if (pt.Programs != null) { uv.Add(MB.Update.SetWrapped<List<ObjectId>>(MEPatientGoal.ProgramProperty, DTOUtil.ConvertObjectId(pt.Programs))); }
                    if (pt.Type != null) uv.Add(MB.Update.Set(MEPatientGoal.TypeProperty, ObjectId.Parse(pt.Type))); // why is this an objectid?
                    if (pt.Status != 0) uv.Add(MB.Update.Set(MEPatientGoal.StatusProperty, pt.Status ));
                    if (pt.StartDate != null) uv.Add(MB.Update.Set(MEPatientGoal.StartDateProperty, pt.StartDate));
                    if (pt.EndDate != null) uv.Add(MB.Update.Set(MEPatientGoal.EndDateProperty, pt.EndDate));
                    if (pt.TargetValue != null) uv.Add(MB.Update.Set(MEPatientGoal.TargetValueProperty, pt.TargetValue));
                    if (pt.TargetDate != null) uv.Add(MB.Update.Set(MEPatientGoal.TargetDateProperty, pt.TargetDate));
                    if (pt.Attributes != null) { uv.Add(MB.Update.SetWrapped<List<MAttribute>>(MEPatientGoal.AttributesProperty, DTOUtil.GetAttributes(pt.Attributes))); }
                    

                    IMongoUpdate update = MB.Update.Combine(uv);
                    WriteConcernResult res = ctx.PatientGoals.Collection.Update(q, update);
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
            catch (Exception ex) { throw ex; }
        }

        public string Initialize(object newEntity)
        {
            PutInitializeGoalDataRequest request = (PutInitializeGoalDataRequest)newEntity;
            string patientGoalId = null;
            try
            {
                MEPatientGoal mePg = new MEPatientGoal
                {
                    Id = ObjectId.GenerateNewId(),
                    PatientId = ObjectId.Parse(request.PatientId),
                    TTLDate = System.DateTime.UtcNow.AddDays(1),
                    UpdatedBy = request.UserId,
                    LastUpdatedOn = DateTime.UtcNow
                };

                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    WriteConcernResult wcr = ctx.PatientGoals.Collection.Insert(mePg);
                    if (wcr.Ok)
                    {
                        patientGoalId = mePg.Id.ToString();
                    }
                }
                return patientGoalId;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
