using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MB = MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.DataDomain.Program;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using Phytel.API.Common;
using Phytel.API.Common.Data;
using Phytel.API.DataAudit;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Builders;
using System.Configuration;

namespace Phytel.API.DataDomain.Program
{
    public class MongoPatientProgramResponseRepository : IProgramRepository
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);

        static MongoPatientProgramResponseRepository()
        {
            #region Register ClassMap
            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(ProgramBase)) == false)
                    BsonClassMap.RegisterClassMap<ProgramBase>();
            }
            catch { }
            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(MEPatientProgramResponse)) == false)
                    BsonClassMap.RegisterClassMap<MEPatientProgramResponse>();
            }
            catch { }
            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(MEResponse)) == false)
                    BsonClassMap.RegisterClassMap<MEResponse>();
            }
            catch { }
            #endregion
        }

        public MongoPatientProgramResponseRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object InsertAsBatch(object newEntity)
        {
            List<MEPatientProgramResponse> mers = newEntity as List<MEPatientProgramResponse>;
            bool res = false;
            try
            {
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    ctx.PatientProgramResponses.Collection.InsertBatch(mers);

                    List<string> ids = new List<string>();
                    mers.ForEach(r =>
                    {
                        ids.Add(r.Id.ToString());
                    });

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientProgramResponse.ToString(),
                                            ids,
                                            Common.DataAuditType.Insert,
                                            _dbName);

                    res = true;
                }
                return res as object;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PatientProgramResponseRepository:InsertAsBatch()::" + ex.Message, ex.InnerException);
            }
        }

        public object Insert(object newEntity)
        {
            MEPatientProgramResponse mer = newEntity as MEPatientProgramResponse;
            bool res = false;
            try
            {
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    ctx.PatientProgramResponses.Collection.Insert(mer);

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.PatientProgramResponse.ToString(), 
                                            mer.Id.ToString(), 
                                            Common.DataAuditType.Insert, 
                                            _dbName);

                    res = true;
                }
                return res as object;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PatientProgramResponseRepository:Insert()::" + ex.Message, ex.InnerException);
            }
        }

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(object entity)
        {
            DeletePatientProgramResponsesDataRequest request = (DeletePatientProgramResponsesDataRequest)entity;
            try
            {
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var query = MB.Query<MEPatientProgramResponse>.EQ(b => b.Id, ObjectId.Parse(request.Id));
                    var builder = new List<MB.UpdateBuilder>();
                    builder.Add(MB.Update.Set(MEPatientProgramResponse.TTLDateProperty, DateTime.UtcNow.AddDays(_expireDays)));
                    builder.Add(MB.Update.Set(MEPatientProgramResponse.DeleteFlagProperty, true));
                    builder.Add(MB.Update.Set(MEPatientProgramResponse.LastUpdatedOnProperty, DateTime.UtcNow));
                    builder.Add(MB.Update.Set(MEPatientProgramResponse.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(builder);
                    ctx.PatientProgramResponses.Collection.Update(query, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientProgramResponse.ToString(),
                                            request.Id.ToString(),
                                            Common.DataAuditType.Delete,
                                            request.ContractNumber);
                }
            }
            catch (Exception) { throw; }
        }

        public IEnumerable<object> FindByStepId(string entityID)
        {
            List<MEPatientProgramResponse> response = null;
            try
            {
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEPatientProgramResponse.StepIdProperty, ObjectId.Parse(entityID)));
                    // Excluding deleteflag query. Night-952
                    //queries.Add(Query.EQ(MEPatientProgramResponse.DeleteFlagProperty, false));
                    IMongoQuery mQuery = Query.And(queries);
                    response = ctx.PatientProgramResponses.Collection.Find(mQuery).ToList();
                }
                return response;
            }
            catch (Exception ex) { throw ex; }
        }

        public void DeleteAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            MEPatientProgramResponse response = null;
            try
            {
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var findcp = MB.Query<MEResponse>.EQ(b => b.Id, ObjectId.Parse(entityID));
                    response = ctx.PatientProgramResponses.Collection.Find(findcp).FirstOrDefault();
                }

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PatientProgramResponseRepository:FindByID()::" + ex.Message, ex.InnerException);
            }
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            try
            {
                IMongoQuery mQuery = null;
                List<object> rps;

                mQuery = MongoDataUtil.ExpressionQueryBuilder(expression);

                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    rps = ctx.PatientProgramResponses.Collection.Find(mQuery).ToList<object>();
                }

                return new Tuple<string, IEnumerable<object>>(expression.ExpressionID, rps);
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PatientProgramResponseRepository:Select()::" + ex.Message, ex.InnerException);
            }
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Update(object entity)
        {
            MEPatientProgramResponse resp = (MEPatientProgramResponse)entity;
            bool result = false;
            try
            {
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var q = MB.Query<MEResponse>.EQ(b => b.Id, resp.Id);

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientProgramResponse.NextStepIdProperty, resp.NextStepId));
                    uv.Add(MB.Update.Set(MEPatientProgramResponse.StepIdProperty, resp.StepId));
                    uv.Add(MB.Update.Set(MEPatientProgramResponse.NominalProperty, resp.Nominal));
                    uv.Add(MB.Update.Set(MEPatientProgramResponse.RequiredProperty, resp.Required));
                    uv.Add(MB.Update.Set(MEPatientProgramResponse.SelectedProperty, resp.Selected));
                    uv.Add(MB.Update.Set(MEPatientProgramResponse.VersionProperty, resp.Version));
                    uv.Add(MB.Update.Set(MEPatientProgramResponse.LastUpdatedOnProperty, System.DateTime.UtcNow));
                    uv.Add(MB.Update.Set(MEPatientProgramResponse.DeleteFlagProperty, resp.DeleteFlag));
                    uv.Add(MB.Update.Set(MEPatientProgramResponse.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    if (resp.Order != 0) uv.Add(MB.Update.Set(MEPatientProgramResponse.OrderProperty, resp.Order));
                    if (resp.Text != null) uv.Add(MB.Update.Set(MEPatientProgramResponse.TextProperty, resp.Text));
                    if (resp.Value != null) uv.Add(MB.Update.Set(MEPatientProgramResponse.ValueProperty, resp.Value));
                    if (resp.Spawn != null) { uv.Add(MB.Update.SetWrapped<List<SpawnElement>>(MEPatientProgramResponse.SpawnElementProperty, resp.Spawn)); }

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientProgramResponses.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientProgramResponse.ToString(), 
                                            resp.Id.ToString(), 
                                            Common.DataAuditType.Update, 
                                            _dbName);

                    result = true;
                }
                return result as object;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PatientProgramResponseRepository:Update()::" + ex.Message, ex.InnerException);
            }
        }

        public List<ProgramInfo> GetActiveProgramsInfoList(GetAllActiveProgramsRequest request)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }
        
        public DTO.Program FindByName(string entityID)
        {
            throw new NotImplementedException();
        }

        public string UserId { get; set; }
        public string ContractNumber { get; set; }

        public IEnumerable<object> Find(string Id)
        {
            throw new NotImplementedException();
        }

        public object FindByPlanElementID(string entityID)
        {
            throw new NotImplementedException();
        }

        public object GetLimitedProgramFields(string objectId)
        {
            throw new NotImplementedException();
        }


        public object FindByEntityExistsID(string patientID, string progId)
        {
            throw new NotImplementedException();
        }

        public static List<BsonValue> ConvertToBsonValueList(List<ObjectId> p)
        {
            List<BsonValue> bsonValues = null;

            if (p.Count() > 0)
            {
                bsonValues = new List<BsonValue>();
                foreach (ObjectId s in p)
                {
                    bsonValues.Add(BsonValue.Create(s));
                }
            }
            return bsonValues;
        }

        public IEnumerable<object> Find(List<ObjectId> Ids)
        {
            try
            {
                List<MEPatientProgramResponse> responses = null;

                IList<IMongoQuery> queries = new List<IMongoQuery>();

                IMongoQuery mQuery = null;
                List<BsonValue> bsonList = ConvertToBsonValueList(Ids);
                if (bsonList != null)
                {
                    mQuery = Query.In(MEPatientProgramResponse.StepIdProperty, bsonList);
                }

                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {

                    responses = ctx.PatientProgramResponses.Collection.Find(mQuery).ToList();

                }
                return responses;
            }
            catch (Exception) { throw; }
        }

        public bool Save(object entity)
        {
            throw new NotImplementedException();
        }

        public List<Module> GetProgramModules(ObjectId progId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> FindByPatientId(string patientId)
        {
            throw new NotImplementedException();
        }


        public void UndoDelete(object entity)
        {
            UndoDeletePatientProgramResponsesDataRequest request = (UndoDeletePatientProgramResponsesDataRequest)entity;
            try
            {
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var query = MB.Query<MEPatientProgramResponse>.EQ(b => b.Id, ObjectId.Parse(request.PatientProgramResponseId));
                    var builder = new List<MB.UpdateBuilder>();
                    builder.Add(MB.Update.Set(MEPatientProgramResponse.TTLDateProperty, BsonNull.Value));
                    builder.Add(MB.Update.Set(MEPatientProgramResponse.DeleteFlagProperty, false));
                    builder.Add(MB.Update.Set(MEPatientProgramResponse.LastUpdatedOnProperty, DateTime.UtcNow));
                    builder.Add(MB.Update.Set(MEPatientProgramResponse.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(builder);
                    ctx.PatientProgramResponses.Collection.Update(query, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientProgramResponse.ToString(),
                                            request.PatientProgramResponseId.ToString(),
                                            Common.DataAuditType.UndoDelete,
                                            request.ContractNumber);
                }
            }
            catch (Exception) { throw; }
        }
    }
}
