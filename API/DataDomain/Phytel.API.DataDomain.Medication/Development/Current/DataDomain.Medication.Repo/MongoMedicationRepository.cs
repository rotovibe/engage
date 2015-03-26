
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
                    
                    ObjectId objId;
                    if (ObjectId.TryParse(m.FamilyId, out objId))
                        med.FamilyId = objId;

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
            var mlist = entity as List<MedicationData>;
            try
            {
                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    var bulk = ctx.Medications.Collection.InitializeUnorderedBulkOperation();

                    foreach (MEMedication fooDoc in ctx.Medications)
                    {
                        var update = new UpdateDocument { { fooDoc.ToBsonDocument() } };
                        update.Set("fmid", ObjectId.Parse(GetMedFamilyId(mlist, fooDoc.Id.ToString())));
                        bulk.Find(Query.EQ("_id", fooDoc.Id)).Upsert().UpdateOne(update);
                    }
                    BulkWriteResult bwr = bulk.Execute();

                    result = true;
                }
                return result as object;
            }
            catch (Exception) { throw; }
        }

        private string GetMedFamilyId(List<MedicationData> mlist, string p)
        {
            var med = mlist.Find(m => m.Id == p);
            var famid = med.FamilyId;
            return famid;
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
            throw new NotImplementedException();
        }

        public object Search(object request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Find the exact match on name, strength, route and form 
        /// If it does not yield any result, find a match on name, route and form.
        /// If it does not yield any result, find a match on name and route.
        /// If it does not yield any result, find a match on name.
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
                        query1.Add(Query.EQ(MEMedication.StrengthProperty, dataRequest.Strength));
                    }
                    if (!string.IsNullOrEmpty(dataRequest.Route))
                    {
                        query1.Add(Query.EQ(MEMedication.RouteProperty, dataRequest.Route));
                    }
                    if (!string.IsNullOrEmpty(dataRequest.Form))
                    {
                        query1.Add(Query.EQ(MEMedication.FormProperty, dataRequest.Form));
                    }
                    IMongoQuery mQuery1 = Query.And(query1);
                    list = ctx.Medications.Collection.Find(mQuery1).SetFields(MEMedication.NDCProperty).ToList();
                    if (list.Count ==  0)
                    {
                        // find a match on name, route and form.
                        List<IMongoQuery> query2 = new List<IMongoQuery>();
                        query2.Add(Query.EQ(MEMedication.DeleteFlagProperty, false));
                        if (!string.IsNullOrEmpty(dataRequest.Name))
                        {
                            query2.Add(Query.EQ(MEMedication.FullNameProperty, dataRequest.Name));
                        }
                        if (!string.IsNullOrEmpty(dataRequest.Form))
                        {
                            query2.Add(Query.EQ(MEMedication.FormProperty, dataRequest.Form));
                        }
                        if (!string.IsNullOrEmpty(dataRequest.Route))
                        {
                            query2.Add(Query.EQ(MEMedication.RouteProperty, dataRequest.Route));
                        }
                        IMongoQuery mQuery2 = Query.And(query2);
                        list = ctx.Medications.Collection.Find(mQuery2).SetFields(MEMedication.NDCProperty).ToList();
                        if (list.Count == 0)
                        {
                            // find a match on name and route.
                            List<IMongoQuery> query3 = new List<IMongoQuery>();
                            query3.Add(Query.EQ(MEMedication.DeleteFlagProperty, false));
                            if (!string.IsNullOrEmpty(dataRequest.Name))
                            {
                                query3.Add(Query.EQ(MEMedication.FullNameProperty, dataRequest.Name));
                            }
                            if (!string.IsNullOrEmpty(dataRequest.Route))
                            {
                                query3.Add(Query.EQ(MEMedication.RouteProperty, dataRequest.Route));
                            }
                            IMongoQuery mQuery3 = Query.And(query3);
                            list = ctx.Medications.Collection.Find(mQuery3).SetFields(MEMedication.NDCProperty).ToList();
                            if (list.Count == 0)
                            {
                                // find a match on name.
                                List<IMongoQuery> query4 = new List<IMongoQuery>();
                                query4.Add(Query.EQ(MEMedication.DeleteFlagProperty, false));
                                if (!string.IsNullOrEmpty(dataRequest.Name))
                                {
                                    query4.Add(Query.EQ(MEMedication.FullNameProperty, dataRequest.Name));
                                }
                                IMongoQuery mQuery4 = Query.And(query4);
                                list = ctx.Medications.Collection.Find(mQuery4).SetFields(MEMedication.NDCProperty).ToList();
                            }
                        }
                    }
                }
                return list;
            }
            catch (Exception) { throw; }
        }


        public object FindByName(object request)
        {
            throw new NotImplementedException();
        }
    }
}
