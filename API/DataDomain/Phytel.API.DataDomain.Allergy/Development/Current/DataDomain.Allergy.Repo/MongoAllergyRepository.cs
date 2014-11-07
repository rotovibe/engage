
using System.Linq;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using DTO = Phytel.API.DataDomain.Allergy.DTO;
using Phytel.API.DataDomain.Allergy.DTO;
using System.Configuration;
using Phytel.API.DataAudit;
using Phytel.API.Common;
using MB = MongoDB.Driver.Builders;

namespace DataDomain.Allergy.Repo
{
    public class MongoAllergyRepository<TContext> : IMongoAllergyRepository where TContext : AllergyMongoContext
    {
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        private int _initializeDays = Convert.ToInt32(ConfigurationManager.AppSettings["InitializeDays"]);

        protected readonly TContext Context;
        public string ContractDBName { get; set; }
        public string UserId { get; set; }

        public MongoAllergyRepository(IUOWMongo<TContext> uow)
        {
            Context = uow.MongoContext;
        }

        public MongoAllergyRepository(TContext context)
        {
            Context = context;
        }

        public MongoAllergyRepository(string dbName)
        {
            ContractDBName = dbName;
        }

        public object Insert(object newEntity)
        {
            try
            {
                object result = null;
                using (AllergyMongoContext ctx = new AllergyMongoContext(ContractDBName))
                {
                    var allgr = new MEAllergy(UserId){ Name = ((AllergyData)newEntity).Name };
                    ctx.Allergies.Insert(allgr);
                    result = Mapper.Map<AllergyData>(allgr);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("AllergyDD:MongoAllergyRepository:Insert()::" + ex.Message, ex.InnerException); 
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
            AllergyData data = null;
            try
            {
                using (AllergyMongoContext ctx = new AllergyMongoContext(ContractDBName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEAllergy.IdProperty, ObjectId.Parse(entityID)));
                    IMongoQuery mQuery = Query.And(queries);
                    MEAllergy meA = ctx.Allergies.Collection.Find(mQuery).FirstOrDefault();
                    if (meA != null)
                    {
                        data = AutoMapper.Mapper.Map<AllergyData>(meA);
                    }
                }
                return data;
            }
            catch (Exception) { throw; }
        }

        public Tuple<string, IEnumerable<object>> Select(Phytel.API.Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            using (AllergyMongoContext ctx = new AllergyMongoContext(ContractDBName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEAllergy.DeleteFlagProperty, false));
                queries.Add(Query.EQ(MEAllergy.TTLDateProperty, BsonNull.Value));
                IMongoQuery mQuery = Query.And(queries);

                List<MEAllergy> meAllgy = ctx.Allergies.Collection.Find(mQuery).ToList();

                List<AllergyData> allgs = null;
                if (meAllgy != null && meAllgy.Count > 0)
                {
                    allgs = meAllgy.Select(a => Mapper.Map<AllergyData>(a)).ToList();
                }

                return allgs;
            }
        }

        public object Update(object entity)
        {
            bool result = false;
            PutAllergyDataRequest pa = (PutAllergyDataRequest)entity;
            AllergyData pt = pa.AllergyData;
            try
            {
                using (AllergyMongoContext ctx = new AllergyMongoContext(ContractDBName))
                {
                    var q = MB.Query<MEAllergy>.EQ(b => b.Id, ObjectId.Parse(pt.Id));
                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEAllergy.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEAllergy.VersionProperty, pa.Version));
                    uv.Add(MB.Update.Set(MEAllergy.LastUpdatedOnProperty, System.DateTime.UtcNow));
                    if (pt.TypeIds != null && pt.TypeIds.Count > 0)
                    {
                        uv.Add(MB.Update.SetWrapped<List<ObjectId>>(MEAllergy.TypeIdsProperty, Helper.ConvertToObjectIdList(pt.TypeIds)));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEAllergy.TypeIdsProperty, BsonNull.Value));
                    }
                    uv.Add(MB.Update.Set(MEAllergy.DeleteFlagProperty, pt.DeleteFlag));
                    DataAuditType type;
                    if (pt.DeleteFlag)
                    {
                        uv.Add(MB.Update.Set(MEAllergy.TTLDateProperty, System.DateTime.UtcNow.AddDays(_expireDays)));
                        type = DataAuditType.Delete;
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEAllergy.TTLDateProperty, BsonNull.Value));
                        type = DataAuditType.Update;
                    }
                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.Allergies.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.Allergy.ToString(),
                                            pt.Id,
                                            type,
                                            pa.ContractNumber);

                    result = true;
                }
                return result as object;
            }
            catch (Exception) { throw; }
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }


        public void UndoDelete(object entity)
        {
            throw new NotImplementedException();
        }

        public object Initialize(object newEntity)
        {
            PutInitializeAllergyDataRequest request = (PutInitializeAllergyDataRequest)newEntity;
            AllergyData data = null;
            try
            {
                MEAllergy meA = new MEAllergy(this.UserId)
                {
                    Name = request.AllergyName,
                    TTLDate = System.DateTime.UtcNow.AddDays(_initializeDays),
                    DeleteFlag = false
                };

                using (AllergyMongoContext ctx = new AllergyMongoContext(ContractDBName))
                {
                    ctx.Allergies.Collection.Insert(meA);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.Allergy.ToString(),
                                            meA.Id.ToString(),
                                            DataAuditType.Insert,
                                            request.ContractNumber);

                    data = new AllergyData
                    {
                        Id = meA.Id.ToString(),
                        Name = meA.Name.ToUpper()
                    };
                }
                return data;
            }
            catch (Exception) { throw; }
        }


        public object FindByPatientId(object request)
        {
            throw new NotImplementedException();
        }
    }
}
