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
using Phytel.API.DataDomain.PatientGoal;

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
            DeleteInterventionRequest request = (DeleteInterventionRequest)entity;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientIntervention>.EQ(b => b.Id, ObjectId.Parse(request.InterventionId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientIntervention.TTLDateProperty, System.DateTime.UtcNow.AddDays(7)));
                    uv.Add(MB.Update.Set(MEPatientIntervention.DeleteFlagProperty, true));
                    uv.Add(MB.Update.Set(MEPatientIntervention.UpdatedByProperty, request.UserId));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    WriteConcernResult res = ctx.PatientInterventions.Collection.Update(q, update);
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
            PutUpdateInterventionRequest ir = (PutUpdateInterventionRequest)entity;
            PatientInterventionData pt = ir.Intervention;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientIntervention>.EQ(b => b.Id, ObjectId.Parse(pt.Id));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientIntervention.TTLDateProperty, BsonNull.Value));
                    if (ir.UserId != null) uv.Add(MB.Update.Set(MEPatientIntervention.UpdatedByProperty, ir.UserId));
                    if (pt.Description != null) uv.Add(MB.Update.Set(MEPatientIntervention.DescriptionProperty, pt.Description));
                    if (pt.StartDate != null) uv.Add(MB.Update.Set(MEPatientIntervention.StartDateProperty, pt.StartDate));
                    if (pt.StatusDate != null) uv.Add(MB.Update.Set(MEPatientIntervention.StatusDateProperty, pt.StatusDate));
                    if (pt.StatusId != 0) uv.Add(MB.Update.Set(MEPatientIntervention.StatusProperty, pt.StatusId));
                    if (pt.CategoryId != null) uv.Add(MB.Update.Set(MEPatientIntervention.CategoryProperty, pt.CategoryId));
                    if (pt.AssignedToId != null) uv.Add(MB.Update.Set(MEPatientIntervention.AssignedToProperty, pt.AssignedToId));
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

        public object Initialize(object newEntity)
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
                        interventionsDataList = new List<PatientInterventionData>();
                        foreach (MEPatientIntervention b in meInterventions)
                        {
                            PatientInterventionData interventionData = new PatientInterventionData
                            {
                                Id = b.Id.ToString(),
                                Description = b.Description,
                                PatientGoalId = b.PatientGoalId.ToString(),
                                CategoryId = b.Category == null ? null : b.Category.ToString(),
                                AssignedToId = b.AssignedTo == null ? null : b.AssignedTo.ToString(),
                                Barriers = Helper.ConvertToStringList(b.Barriers),
                                StatusId = ((int)b.Status),
                                StatusDate = b.StatusDate,
                                StartDate = b.StartDate,
                            };
                            interventionsDataList.Add(interventionData);
                        }
                    }
                }
                return interventionsDataList;
            }
            catch (Exception ex) { throw ex; }

        }


        public IEnumerable<object> FindByGoalId(string Id)
        {
            try
            {
                List<PatientInterventionData> interventionsDataList = null;
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEPatientIntervention.PatientGoalIdProperty, ObjectId.Parse(Id)));
                IMongoQuery mQuery = Query.And(queries);

                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    List<MEPatientIntervention> meInterventions = ctx.PatientInterventions.Collection.Find(mQuery).ToList();
                    if (meInterventions != null)
                    {
                        interventionsDataList = new List<PatientInterventionData>();
                        foreach (MEPatientIntervention b in meInterventions)
                        {
                            PatientInterventionData interventionData = new PatientInterventionData
                            {
                                Id = b.Id.ToString(),
                                 AssignedToId = b.AssignedTo != null ? b.AssignedTo.ToString(): null
                                 // need to implement all the values needed!
                            };
                            interventionsDataList.Add(interventionData);
                        }
                    }
                }
                return interventionsDataList;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
