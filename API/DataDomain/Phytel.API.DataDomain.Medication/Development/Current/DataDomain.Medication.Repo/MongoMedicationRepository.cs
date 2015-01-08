
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.API.Common;
using Phytel.API.DataAudit;
using Phytel.API.DataDomain.Medication.DTO;
using DTO = Phytel.API.DataDomain.Medication.DTO;
using MB = MongoDB.Driver.Builders;

namespace DataDomain.Medication.Repo
{
    public class MongoMedicationRepository<TContext> : IMongoMedicationRepository where TContext : MedicationMongoContext
    {
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        private int _initializeDays = Convert.ToInt32(ConfigurationManager.AppSettings["InitializeDays"]);
        
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
            bool result = false;
            PutMedicationDataRequest pa = (PutMedicationDataRequest)entity;
            MedicationData pt = pa.MedicationData;
            try
            {
                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    var q = MB.Query<MEMedication>.EQ(b => b.Id, ObjectId.Parse(pt.Id));
                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEMedication.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEMedication.VersionProperty, pa.Version));
                    uv.Add(MB.Update.Set(MEMedication.LastUpdatedOnProperty, System.DateTime.UtcNow));
                    uv.Add(MB.Update.Set(MEMedication.DeleteFlagProperty, pt.DeleteFlag));
                    uv.Add(MB.Update.Set(MEMedication.NDCProperty, pt.NDC));
                    uv.Add(MB.Update.Set(MEMedication.ProductIdProperty, pt.ProductId));
                    uv.Add(MB.Update.Set(MEMedication.ProprietaryNameProperty, pt.ProprietaryName));
                    uv.Add(MB.Update.Set(MEMedication.ProprietaryNameSuffixProperty, pt.ProprietaryNameSuffix));
                    uv.Add(MB.Update.Set(MEMedication.SubstanceNameProperty, pt.SubstanceName));
                    uv.Add(MB.Update.Set(MEMedication.RouteProperty, pt.Route));
                    uv.Add(MB.Update.Set(MEMedication.FormProperty, pt.Form));
                    uv.Add(MB.Update.Set(MEMedication.StrengthProperty, pt.Strength));
                    DataAuditType type;
                    if (pt.DeleteFlag)
                    {
                        uv.Add(MB.Update.Set(MEMedication.TTLDateProperty, System.DateTime.UtcNow.AddDays(_expireDays)));
                        type = DataAuditType.Delete;
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEMedication.TTLDateProperty, BsonNull.Value));
                        type = DataAuditType.Update;
                    }
                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.Medications.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.Medication.ToString(),
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

        public object FindByPatientId(object request)
        {
            throw new NotImplementedException();
        }

        public object Initialize(object newEntity)
        {
            PutInitializeMedSuppDataRequest request = (PutInitializeMedSuppDataRequest)newEntity;
            MedicationData data = null;
            try
            {
                MEMedication meM = new MEMedication(this.UserId)
                {
                    ProprietaryName = request.MedSuppName,
                    FullName = request.MedSuppName,
                    TTLDate = System.DateTime.UtcNow.AddDays(_initializeDays),
                    DeleteFlag = false
                };

                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    ctx.Medications.Collection.Insert(meM);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.Medication.ToString(),
                                            meM.Id.ToString(),
                                            DataAuditType.Insert,
                                            request.ContractNumber);

                    data = new MedicationData
                    {
                        Id = meM.Id.ToString(),
                        FullName = meM.FullName.ToUpper(),
                        ProprietaryName = meM.ProprietaryName.ToUpper()
                    };
                }
                return data;
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Find the exact match on name, strength, route, form and unit. 
        /// If it does not yield any result, find a match for name alone.
        /// If it yields results more than 1, then find a match on name, form. 
        /// If it yields results more than 1, then find a match on name, form, strength. 
        /// If it yields results more than 1, then find a match on name, form, strength and route. 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public object FindNDCCodes(object request)
        {
            List<MEMedication> list = null;
            GetMedicationNDCsDataRequest dataRequest = (GetMedicationNDCsDataRequest)request;
            try
            {
                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    List<IMongoQuery> query1 = new List<IMongoQuery>();
                    query1.Add(Query.EQ(MEMedication.DeleteFlagProperty, false));
                    if (!string.IsNullOrEmpty(dataRequest.Name))
                    {
                        query1.Add(Query.EQ(MEMedication.FullNameProperty, dataRequest.Name));
                    }
                    if (!string.IsNullOrEmpty(dataRequest.Strength))
                    {
                        query1.Add(Query.In(MEMedication.StrengthProperty, new List<BsonValue> { BsonValue.Create(dataRequest.Strength) }));
                    }
                    if (!string.IsNullOrEmpty(dataRequest.Route))
                    {
                        query1.Add(Query.In(MEMedication.RouteProperty, new List<BsonValue> { BsonValue.Create(dataRequest.Route) }));
                    }
                    if (!string.IsNullOrEmpty(dataRequest.Form))
                    {
                        query1.Add(Query.EQ(MEMedication.FormProperty, dataRequest.Form));
                    }
                    if (!string.IsNullOrEmpty(dataRequest.Unit))
                    {
                        query1.Add(Query.In(MEMedication.UnitProperty, new List<BsonValue> { BsonValue.Create(dataRequest.Unit) }));
                    }
                    IMongoQuery mQuery1 = Query.And(query1);
                    list = ctx.Medications.Collection.Find(mQuery1).SetFields(MEMedication.NDCProperty).ToList();
                    if (list.Count ==  0)
                    {
                        // Run the query again on the name alone. 
                        List<IMongoQuery> query2 = new List<IMongoQuery>();
                        query2.Add(Query.EQ(MEMedication.DeleteFlagProperty, false));
                        if (!string.IsNullOrEmpty(dataRequest.Name))
                        {
                            query2.Add(Query.EQ(MEMedication.FullNameProperty, dataRequest.Name));
                        }
                        IMongoQuery mQuery2 = Query.And(query2);
                        list = ctx.Medications.Collection.Find(mQuery2).SetFields(MEMedication.NDCProperty).ToList();
                        if (list.Count > 1)
                        {
                            // Run the query again on the name and form.
                            List<IMongoQuery> query3 = new List<IMongoQuery>();
                            query3.Add(Query.EQ(MEMedication.DeleteFlagProperty, false));
                            if (!string.IsNullOrEmpty(dataRequest.Name))
                            {
                                query3.Add(Query.EQ(MEMedication.FullNameProperty, dataRequest.Name));
                            }
                            if (!string.IsNullOrEmpty(dataRequest.Form))
                            {
                                query3.Add(Query.EQ(MEMedication.FormProperty, dataRequest.Form));
                            }
                            IMongoQuery mQuery3 = Query.And(query3);
                            list = ctx.Medications.Collection.Find(mQuery3).SetFields(MEMedication.NDCProperty).ToList();
                            if (list.Count > 1)
                            {
                                // Run the query again on the name, form and strength.
                                List<IMongoQuery> query4 = new List<IMongoQuery>();
                                query4.Add(Query.EQ(MEMedication.DeleteFlagProperty, false));
                                if (!string.IsNullOrEmpty(dataRequest.Name))
                                {
                                    query4.Add(Query.EQ(MEMedication.FullNameProperty, dataRequest.Name));
                                }
                                if (!string.IsNullOrEmpty(dataRequest.Form))
                                {
                                    query4.Add(Query.EQ(MEMedication.FormProperty, dataRequest.Form));
                                }
                                if (!string.IsNullOrEmpty(dataRequest.Strength))
                                {
                                    query4.Add(Query.In(MEMedication.StrengthProperty, new List<BsonValue> { BsonValue.Create(dataRequest.Strength) }));
                                }
                                IMongoQuery mQuery4 = Query.And(query4);
                                list = ctx.Medications.Collection.Find(mQuery4).SetFields(MEMedication.NDCProperty).ToList();
                                if(list.Count > 1)
                                {
                                    // Run the query again on the name, form, strength & route.
                                    List<IMongoQuery> query5 = new List<IMongoQuery>();
                                    query5.Add(Query.EQ(MEMedication.DeleteFlagProperty, false));
                                    if (!string.IsNullOrEmpty(dataRequest.Name))
                                    {
                                        query5.Add(Query.EQ(MEMedication.FullNameProperty, dataRequest.Name));
                                    }
                                    if (!string.IsNullOrEmpty(dataRequest.Form))
                                    {
                                        query5.Add(Query.EQ(MEMedication.FormProperty, dataRequest.Form));
                                    }
                                    if (!string.IsNullOrEmpty(dataRequest.Strength))
                                    {
                                        query5.Add(Query.In(MEMedication.StrengthProperty, new List<BsonValue> { BsonValue.Create(dataRequest.Strength) }));
                                    }
                                    if (!string.IsNullOrEmpty(dataRequest.Route))
                                    {
                                        query5.Add(Query.In(MEMedication.RouteProperty, new List<BsonValue> { BsonValue.Create(dataRequest.Route) }));
                                    }
                                    IMongoQuery mQuery5 = Query.And(query5);
                                    list = ctx.Medications.Collection.Find(mQuery5).SetFields(MEMedication.NDCProperty).ToList();
                                }
                            }
                        }
                    }
                }
                return list;
            }
            catch (Exception) { throw; }
        }
    }
}
