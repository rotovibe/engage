
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
            try
            {
                object result = null;

                var findcp = Query.And(
                    Query<MEAllergy>.EQ(b => b.Id, ObjectId.Parse(entityID)),
                    Query<MEAllergy>.EQ(b => b.DeleteFlag, false));

                var cp = Context.Allergies.Collection.Find(findcp).FirstOrDefault();

                if (cp == null) return result;

                AutoMapper.Mapper.CreateMap<MEAllergy, DTO.AllergyData>();

                result = AutoMapper.Mapper.Map<DTO.AllergyData>(cp);

                //result = new DTO.DdAllergy
                //{
                //    Id = cp.Id.ToString(),
                //    DeleteFlag = cp.DeleteFlag,
                //    LastUpdatedOn = cp.LastUpdatedOn,
                //    RecordCreatedBy = cp.RecordCreatedBy.ToString(),
                //    RecordCreatedOn = cp.RecordCreatedOn,
                //    TtlDate = cp.TTLDate,
                //    UpdatedBy = cp.UpdatedBy.ToString(),
                //    Version = cp.Version
                //};

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PatientProgramRepository:FindByID()::" + ex.Message, ex.InnerException);
            }
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
            throw new NotImplementedException();
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
                    ctx.PatientAllergies.Collection.Insert(meA);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.Allergy.ToString(),
                                            meA.Id.ToString(),
                                            DataAuditType.Insert,
                                            request.ContractNumber);

                    data = new AllergyData
                    {
                        Id = meA.Id.ToString(),
                        Name = meA.Name
                    };
                }
                return data;
            }
            catch (Exception) { throw; }
        }
    }
}
