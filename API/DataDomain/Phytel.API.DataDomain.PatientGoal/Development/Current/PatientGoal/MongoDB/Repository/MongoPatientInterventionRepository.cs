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

namespace Phytel.API.DataDomain.PatientGoal
{
    public class MongoPatientInterventionRepository<T> : IPatientGoalRepository<T>
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        private int _initializeDays = Convert.ToInt32(ConfigurationManager.AppSettings["InitializeDays"]);

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
            DeleteInterventionRequest request = (DeleteInterventionRequest)entity;
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
                throw new NotImplementedException();
                // code here //
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
            PatientInterventionData pt = ir.Intervention;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientIntervention>.EQ(b => b.Id, ObjectId.Parse(pt.Id));

                    // Set the StatusDate to Now if the status is changed.
                    MEPatientIntervention existingPI = ctx.PatientInterventions.Collection.Find(q).SetFields(MEPatientIntervention.StatusProperty).FirstOrDefault();
                    if (existingPI != null)
                    {
                        if ((int)existingPI.Status != pt.StatusId)
                        {
                            pt.StatusDate = DateTime.UtcNow;
                        }
                    }

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientIntervention.TTLDateProperty, BsonNull.Value));
                    uv.Add(MB.Update.Set(MEPatientIntervention.DeleteFlagProperty, false));
                    uv.Add(MB.Update.Set(MEPatientIntervention.VersionProperty, ir.Version));
                    uv.Add(MB.Update.Set(MEPatientIntervention.LastUpdatedOnProperty, System.DateTime.UtcNow));
                    uv.Add(MB.Update.Set(MEPatientIntervention.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    if (pt.Description != null) uv.Add(MB.Update.Set(MEPatientIntervention.DescriptionProperty, pt.Description));
                    if (pt.StartDate != null) uv.Add(MB.Update.Set(MEPatientIntervention.StartDateProperty, pt.StartDate));
                    if (pt.StatusDate != null) uv.Add(MB.Update.Set(MEPatientIntervention.StatusDateProperty, pt.StatusDate));
                    if (pt.StatusId != 0) uv.Add(MB.Update.Set(MEPatientIntervention.StatusProperty, pt.StatusId));
                    if (pt.CategoryId != null) uv.Add(MB.Update.Set(MEPatientIntervention.CategoryProperty, ObjectId.Parse(pt.CategoryId)));
                    if (pt.AssignedToId != null) uv.Add(MB.Update.Set(MEPatientIntervention.AssignedToProperty, ObjectId.Parse(pt.AssignedToId)));
                    if (pt.BarrierIds != null) { uv.Add(MB.Update.SetWrapped<List<ObjectId>>(MEPatientIntervention.BarriersProperty, DTOUtil.ConvertObjectId(pt.BarrierIds))); }

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientInterventions.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.PatientIntervention.ToString(), 
                                            pt.Id.ToString(), 
                                            Common.DataAuditType.Update, 
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
                pi = new MEPatientIntervention
                {
                    Id = ObjectId.GenerateNewId(),
                    PatientGoalId = ObjectId.Parse(ptr.PatientGoalId),
                    TTLDate = System.DateTime.UtcNow.AddDays(_initializeDays),
                    StatusDate = DateTime.UtcNow,
                    LastUpdatedOn = DateTime.UtcNow,
                    UpdatedBy = ObjectId.Parse(this.UserId),
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
                            };
                            interventionsDataList.Add(interventionData);
                        }
                    }
                }
                return interventionsDataList;
            }
            catch (Exception) { throw; }

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
                                 AssignedToId = b.AssignedToId != null ? b.AssignedToId.ToString(): null
                                 // need to implement all the values needed!
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
    }
}
