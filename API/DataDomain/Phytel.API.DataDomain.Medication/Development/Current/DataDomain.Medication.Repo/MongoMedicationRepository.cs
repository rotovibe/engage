
using System.Linq;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using DTO = Phytel.API.DataDomain.Medication.DTO;
using Phytel.API.DataDomain.Medication.DTO;

namespace DataDomain.Medication.Repo
{
    public class MongoMedicationRepository<TContext> : IMongoMedicationRepository where TContext : MedicationMongoContext
    {
        protected readonly TContext Context;
        public string ContractDBName { get; set; }
        public string UserId { get; set; }

        public MongoMedicationRepository(IUOWMongo<TContext> uow)
        {
            Context = uow.MongoContext;
        }

        public MongoMedicationRepository(TContext context)
        {
            Context = context;
        }


        public MongoMedicationRepository(string dbName)
        {
            ContractDBName = dbName;
        }

        public object Insert(object newEntity)
        {
            throw new NotImplementedException();
        }

        public object InsertAll(List<object> entities)
        {
            try
            {
                var list = entities.Cast<DTO.MedicationData>();
                var mColl = new List<MEMedication>();

                list.ToList().ForEach(m =>
                {
                    var med = new MEMedication("5368ff2ad4332316288f3e3e")
                    {
                        DeleteFlag = m.DeleteFlag,
                        EndDate = m.EndDate,
                        Form = m.Form,
                        Version = m.Version,
                        Unit = m.Unit,
                        Strength = m.Strength,
                        Route = m.Route,
                        PharmClass = m.PharmClass,
                        SubstanceName = m.SubstanceName,
                        NDC = m.NDC,
                        StartDate = m.StartDate,
                        ProductId = m.ProductId,
                        FullName = m.FullName,
                        ProprietaryName = m.ProprietaryName,
                        ProprietaryNameSuffix = m.ProprietaryNameSuffix
                    };
                    mColl.Add(med);
                });

                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    object result = null;
                    ctx.Medications.Collection.InsertBatch(mColl);

                    return result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PatientProgramRepository:FindByID()::" + ex.Message, ex.InnerException);
            }
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
                    Query<MEMedication>.EQ(b => b.Id, ObjectId.Parse(entityID)),
                    Query<MEMedication>.EQ(b => b.DeleteFlag, false));

                var cp = Context.Medications.Collection.Find(findcp).FirstOrDefault();

                if (cp == null) return result;

                result = Mapper.Map<DTO.MedicationData>(cp);                
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
            using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEMedication.DeleteFlagProperty, false));
                queries.Add(Query.EQ(MEMedication.TTLDateProperty, BsonNull.Value));
                IMongoQuery mQuery = Query.And(queries);

                List<MEMedication> meMeds = ctx.Medications.Collection.Find(mQuery).ToList();

                List<MedicationData> medsData = null;
                if (meMeds != null && meMeds.Count > 0)
                {
                    medsData = meMeds.Select(a => Mapper.Map<MedicationData>(a)).ToList();
                }

                return medsData;
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

        public object FindByPatientId(object request)
        {
            throw new NotImplementedException();
        }

        public object SearchMedications(object request)
        {
            List<MEMedication> list = null;
            GetMedicationDetailsDataRequest dataRequest = (GetMedicationDetailsDataRequest)request;
            try
            {
                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEMedication.DeleteFlagProperty, false));
                    if (!string.IsNullOrEmpty(dataRequest.Name))
                    {
                        queries.Add(Query.EQ(MEMedication.FullNameProperty, dataRequest.Name));
                    }
                    if (!string.IsNullOrEmpty(dataRequest.Strength))
                    {
                        queries.Add(Query.In(MEMedication.StrengthProperty, new BsonArray(dataRequest.Strength)));
                    }
                    if (!string.IsNullOrEmpty(dataRequest.Route))
                    {
                        queries.Add(Query.In(MEMedication.RouteProperty, new BsonArray(dataRequest.Route)));
                    }
                    if (!string.IsNullOrEmpty(dataRequest.Form))
                    {
                        queries.Add(Query.EQ(MEMedication.FormProperty, dataRequest.Form));
                    } 
                    if (!string.IsNullOrEmpty(dataRequest.Form))
                    {
                        queries.Add(Query.In(MEMedication.UnitProperty, new BsonArray(dataRequest.Unit)));
                    }
                    IMongoQuery mQuery = Query.And(queries);
                    list = ctx.Medications.Collection.Find(mQuery).ToList();
                }
                return list;
            }
            catch (Exception) { throw; }
        }
    }
}
