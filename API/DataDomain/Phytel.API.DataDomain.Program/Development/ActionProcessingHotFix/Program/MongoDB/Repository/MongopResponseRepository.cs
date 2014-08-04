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

namespace Phytel.API.DataDomain.Program
{
    public class MongoResponseRepository : IProgramRepository
    {
        private string _dbName = string.Empty;

        static MongoResponseRepository()
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
                if (BsonClassMap.IsClassMapRegistered(typeof(MEResponse)) == false)
                    BsonClassMap.RegisterClassMap<MEResponse>();
            }
            catch { }
            #endregion
        }

        public MongoResponseRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            try
            {
            ResponseDetail rs = (ResponseDetail)newEntity;
            MEResponse mer = new MEResponse(this.UserId)
            {
                Id = ObjectId.Parse(rs.Id),
                NextStepId = ObjectId.Parse(rs.NextStepId),
                Nominal = rs.Nominal,
                Order = rs.Order,
                Required = rs.Required,
                Spawn = DTOUtils.GetSpawnElements(rs.SpawnElement),
                StepId = ObjectId.Parse(rs.StepId),
                Text = rs.Text,
                Value = rs.Value,
                DeleteFlag = true,
                LastUpdatedOn = DateTime.UtcNow,
                Version = 1.0,
                UpdatedBy = ObjectId.Parse(this.UserId)
            };

            bool res = false;

                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    ctx.Responses.Collection.Insert(mer);
                    
                    AuditHelper.LogDataAudit(this.UserId,
                        MongoCollectionName.Response.ToString(),
                        mer.Id.ToString(),
                        Common.DataAuditType.Insert,
                        "");

                    res = true;
                }
                return res as object;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:ResponseRepository:Insert()::" + ex.Message, ex.InnerException);
            }
        }

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(object entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            MEResponse response = null;
            try
            {
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var findcp = MB.Query<MEResponse>.EQ(b => b.Id, ObjectId.Parse(entityID));
                    response = ctx.Responses.Collection.Find(findcp).FirstOrDefault();
                }

                return response;
            }
            catch(Exception ex)
            {
                throw new Exception("DD:ResponseRepository:FindById()::" + ex.Message, ex.InnerException);
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
                rps = ctx.Responses.Collection.Find(mQuery).ToList<object>();
            }

            return new Tuple<string, IEnumerable<object>>(expression.ExpressionID, rps);
        }
            catch (Exception ex)
            {
                throw new Exception("DD:ResponseRepository:Select()::" + ex.Message, ex.InnerException);
            }
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Update(object entity)
        {
            MEResponse resp = (MEResponse)entity;
            bool result = false;
            try
            {
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var q = MB.Query<MEResponse>.EQ(b => b.Id, resp.Id);

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEResponse.NextStepIdProperty, resp.NextStepId));
                    uv.Add(MB.Update.Set(MEResponse.StepIdProperty, resp.StepId));
                    uv.Add(MB.Update.Set(MEResponse.NominalProperty, resp.Nominal));
                    uv.Add(MB.Update.Set(MEResponse.RequiredProperty, resp.Required));
                    uv.Add(MB.Update.Set(MEResponse.DeleteFlagProperty, false));
                    uv.Add(MB.Update.Set(MEResponse.SelectedProperty, resp.Selected));

                    uv.Add(MB.Update.Set(MEResponse.LastUpdatedOnProperty, DateTime.UtcNow));
                    uv.Add(MB.Update.Set(MEResponse.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    if (resp.Order != 0) uv.Add(MB.Update.Set(MEResponse.OrderProperty, resp.Order));
                    if (resp.Text != null) uv.Add(MB.Update.Set(MEResponse.TextProperty, resp.Text));
                    if (resp.Value != null) uv.Add(MB.Update.Set(MEResponse.ValueProperty, resp.Value));
                    if (resp.Spawn != null) { uv.Add(MB.Update.SetWrapped<List<SpawnElement>>(MEResponse.SpawnElementProperty, resp.Spawn)); }

                    IMongoUpdate update = MB.Update.Combine(uv);
                    WriteConcernResult res = ctx.Responses.Collection.Update(q, update);
                    if (res.Ok)
                    {
                        result = true;
                        AuditHelper.LogDataAudit(this.UserId,
                       MongoCollectionName.Response.ToString(),
                       resp.Id.ToString(),
                       Common.DataAuditType.Update,
                       "");
                    }
                }
                return result as object;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:ResponseRepository:Update()::" + ex.Message, ex.InnerException);
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
            try
            {
                List<MEResponse> responses = null;
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEResponse.StepIdProperty, ObjectId.Parse(Id)));
                IMongoQuery mQuery = Query.And(queries);
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {

                    responses = ctx.Responses.Collection.Find(mQuery).ToList();

                }
                return responses;
            }
            catch (Exception) { throw; }
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
                List<MEResponse> responses = null;

                IList<IMongoQuery> queries = new List<IMongoQuery>();

                IMongoQuery mQuery = null;
                List<BsonValue> bsonList = ConvertToBsonValueList(Ids);
                if (bsonList != null)
                {
                    mQuery = Query.In(MEResponse.StepIdProperty, bsonList);
                }
    
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {

                    responses = ctx.Responses.Collection.Find(mQuery).ToList();

                }
                return responses;
            }
            catch (Exception) { throw; }
        }


        public object FindByPlanElementID(string entityID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> FindByStepId(string entityID)
        {
            throw new NotImplementedException();
        }

        public object GetLimitedProgramFields(string objectId)
        {
            throw new NotImplementedException();
        }


        public object InsertAsBatch(object newEntity)
        {
            throw new NotImplementedException();
        }


        public object FindByEntityExistsID(string patientID, string progId)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
