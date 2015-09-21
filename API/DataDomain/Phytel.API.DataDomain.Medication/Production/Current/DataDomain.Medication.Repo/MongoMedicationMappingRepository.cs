
using System.Linq;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using DTO = Phytel.API.DataDomain.Medication.DTO;
using Phytel.API.DataDomain.Medication.DTO;
using System.Configuration;
using MB = MongoDB.Driver.Builders;
using Phytel.API.DataAudit;
using Phytel.API.Common;
using ServiceStack.Common.Extensions;

namespace DataDomain.Medication.Repo
{
    public class MongoMedicationMappingRepository<TContext> :  IMongoMedicationRepository, IMongoMedicationMappingRepository where TContext : MedicationMongoContext
    {
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        private int _initializeDays = Convert.ToInt32(ConfigurationManager.AppSettings["InitializeDays"]);
        
        protected readonly TContext Context;
        public string ContractDBName { get; set; }
        public string UserId { get; set; }

        public MongoMedicationMappingRepository(IUOWMongo<TContext> uow)
        {
            Context = uow.MongoContext;
        }

        public MongoMedicationMappingRepository(TContext context)
        {
            Context = context;
        }


        public MongoMedicationMappingRepository(string dbName)
        {
            ContractDBName = dbName;
        }

        public object Insert(object newEntity)
        {
            var request = newEntity as PostMedicationMapDataRequest;
            var mm = request.MedicationMapData;
            try
            {
                var MEMedMap = new MEMedicationMapping(this.UserId)
                {
                    FullName = string.IsNullOrEmpty(mm.FullName) ? null : mm.FullName.ToUpper(),
                    SubstanceName = mm.SubstanceName,
                    Strength = mm.Strength,
                    Route = string.IsNullOrEmpty(mm.Route) ? null : mm.Route.ToUpper(),
                    Form = string.IsNullOrEmpty(mm.Form) ? null : mm.Form.ToUpper(),
                    Verified = mm.Verified,
                    Custom = mm.Custom
                };

                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    ctx.MedicationMaps.Collection.Insert(MEMedMap);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.MedicationMap.ToString(),
                                            MEMedMap.Id.ToString(),
                                            DataAuditType.Insert,
                                            request.ContractNumber);
                    return AutoMapper.Mapper.Map<MedicationMapData>(MEMedMap);
                }
            }
            catch (Exception) { throw; }
        }

        public object InsertAll(List<object> entities)
        {
            try
            {
                var list = entities.Cast<DTO.MedicationMapData>();
                var mColl = new List<MEMedicationMapping>();

                list.ToList().ForEach(mm =>
                {
                    var MEMedMap = new MEMedicationMapping("5368ff2ad4332316288f3e3e")
                    {
                        Custom = mm.Custom,
                        FullName = mm.FullName,
                        LastUpdatedOn = mm.LastUpdatedOn,
                        SubstanceName = mm.SubstanceName,
                        TTLDate = mm.TTLDate,
                        Verified = mm.Verified,
                        Version = mm.Version,
                        //UpdatedBy = mm.UpdatedBy,
                        Strength = mm.Strength,
                        Route = mm.Route,
                        Form = mm.Form,
                        DeleteFlag = false
                    };
                    mColl.Add(MEMedMap);
                });

                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    object result = null;
                    ctx.MedicationMaps.Collection.InsertBatch(mColl);

                    var mMapsData = new List<MedicationMapData>();
                    var mMaps = ctx.MedicationMaps.Collection.FindAll().ToList();

                    mMaps.ForEach(mm => mMapsData.Add(Mapper.Map<MedicationMapData>(mm)));

                    return mMapsData;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PatientProgramRepository:FindByID()::" + ex.Message, ex.InnerException);
            }
        }

        public void Delete(object entity)
        {
            DeleteMedicationMapsDataRequest request = (DeleteMedicationMapsDataRequest)entity;
            try
            {
                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    var query = MB.Query<MEMedicationMapping>.EQ(b => b.Id, ObjectId.Parse(request.Id));
                    var builder = new List<MB.UpdateBuilder>();
                    builder.Add(MB.Update.Set(MEMedicationMapping.TTLDateProperty, DateTime.UtcNow.AddDays(_expireDays)));
                    builder.Add(MB.Update.Set(MEMedicationMapping.DeleteFlagProperty, true));
                    builder.Add(MB.Update.Set(MEMedicationMapping.LastUpdatedOnProperty, DateTime.UtcNow));
                    builder.Add(MB.Update.Set(MEMedicationMapping.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(builder);
                    ctx.MedicationMaps.Collection.Update(query, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.MedicationMap.ToString(),
                                            request.Id.ToString(),
                                            DataAuditType.Delete,
                                            request.ContractNumber);
                }
            }
            catch (Exception) { throw; }
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

                var query = Query.And(
                    Query<MEMedicationMapping>.EQ(b => b.Id, ObjectId.Parse(entityID)),
                    Query<MEMedicationMapping>.EQ(b => b.DeleteFlag, false));

                var meMMP = Context.MedicationMaps.Collection.Find(query).FirstOrDefault();

                if (meMMP == null) return result;

                result = Mapper.Map<DTO.MedicationMapData>(meMMP);
                return result;
            }
            catch (Exception ex) { throw ex;  }
        }

        public object Search(object request)
        {
            List<MedicationMapData> list = null;
            GetMedicationMapDataRequest dataRequest = (GetMedicationMapDataRequest)request;
            try
            {
                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    if (!string.IsNullOrEmpty(dataRequest.Name))
                    {
                        queries.Add(Query.EQ(MEMedicationMapping.FullNameProperty, dataRequest.Name.ToUpper()));
                    }
                    if (!string.IsNullOrEmpty(dataRequest.Route))
                    {
                        queries.Add(Query.EQ(MEMedicationMapping.RouteProperty, dataRequest.Route));
                    }
                    if (!string.IsNullOrEmpty(dataRequest.Form))
                    {
                        queries.Add(Query.EQ(MEMedicationMapping.FormProperty, dataRequest.Form));
                    }
                    if (!string.IsNullOrEmpty(dataRequest.Strength))
                    {
                        queries.Add(Query.EQ(MEMedicationMapping.StrengthProperty, dataRequest.Strength));
                    }
                    if (dataRequest.Type  != 0)
                    {
                        if (dataRequest.Type == 1)
                        {
                            queries.Add(Query.EQ(MEMedicationMapping.CustomProperty, false));
                        }
                        else if(dataRequest.Type == 2)
                        {
                            queries.Add(Query.EQ(MEMedicationMapping.CustomProperty, true));
                        }
                    }
                    queries.Add(Query.EQ(MEMedicationMapping.DeleteFlagProperty, false));
                    queries.Add(Query.EQ(MEMedicationMapping.TTLDateProperty, BsonNull.Value));
                    IMongoQuery mQuery = Query.And(queries);
                    string[] fields = { MEMedicationMapping.IdProperty, MEMedicationMapping.FullNameProperty, MEMedicationMapping.RouteProperty,MEMedicationMapping.StrengthProperty,MEMedicationMapping.FormProperty,MEMedicationMapping.CustomProperty };
                    List<MEMedicationMapping> meMMs = null;
                    if (dataRequest.Skip == 0 && dataRequest.Take == 0)
                    {
                        meMMs = ctx.MedicationMaps.Collection.Find(mQuery).SetFields(fields).ToList();
                    }
                    else
                    {
                        meMMs = ctx.MedicationMaps.Collection.Find(mQuery).SetFields(fields).SetSkip(dataRequest.Skip).SetLimit(dataRequest.Take).ToList();
                    }
                    
                    if (meMMs != null && meMMs.Count > 0)
                    {
                        list = new List<MedicationMapData>();
                        meMMs.ForEach(p =>
                        {
                            MedicationMapData data = AutoMapper.Mapper.Map<MedicationMapData>(p);
                            list.Add(data);
                        });
                    }
                }
                return list;
            }
            catch (Exception) { throw; }
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
                queries.Add(Query.EQ(MEMedicationMapping.DeleteFlagProperty, false));
                queries.Add(Query.EQ(MEMedicationMapping.TTLDateProperty, BsonNull.Value));
                IMongoQuery mQuery = Query.And(queries);

                List<MEMedicationMapping> meMeds = ctx.MedicationMaps.Collection.Find(mQuery).ToList();

                List<MedicationMapData> medsData = null;
                if (meMeds != null && meMeds.Count > 0)
                {
                    medsData = meMeds.Select(a => Mapper.Map<MedicationMapData>(a)).ToList();
                }

                return medsData;
            }
        }

        public object Update(object entity)
        {
            bool result = false;
            PutMedicationMapDataRequest pa = (PutMedicationMapDataRequest)entity;
            MedicationMapData data = pa.MedicationMapData;
            try
            {
                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    var q = MB.Query<MEMedicationMapping>.EQ(b => b.Id, ObjectId.Parse(data.Id));
                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEMedicationMapping.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEMedicationMapping.VersionProperty, pa.Version));
                    uv.Add(MB.Update.Set(MEMedicationMapping.LastUpdatedOnProperty, System.DateTime.UtcNow));
                    if (!string.IsNullOrEmpty(data.FullName))
                    {
                        uv.Add(MB.Update.Set(MEMedicationMapping.FullNameProperty, data.FullName.ToUpper()));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEMedicationMapping.FullNameProperty, BsonNull.Value));
                    }
                    if (!string.IsNullOrEmpty(data.SubstanceName))
                    {
                        uv.Add(MB.Update.Set(MEMedicationMapping.SubstanceNameProperty, data.SubstanceName));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEMedicationMapping.SubstanceNameProperty, BsonNull.Value));
                    }
                    if (!string.IsNullOrEmpty(data.Strength))
                    {
                        uv.Add(MB.Update.Set(MEMedicationMapping.StrengthProperty, data.Strength));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEMedicationMapping.StrengthProperty, BsonNull.Value));
                    }
                    if (!string.IsNullOrEmpty(data.Form))
                    {
                        uv.Add(MB.Update.Set(MEMedicationMapping.FormProperty, data.Form.ToUpper()));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEMedicationMapping.FormProperty, BsonNull.Value));
                    }
                    if (!string.IsNullOrEmpty(data.Route))
                    {
                        uv.Add(MB.Update.Set(MEMedicationMapping.RouteProperty, data.Route.ToUpper()));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEMedicationMapping.RouteProperty, BsonNull.Value));
                    }
                    uv.Add(MB.Update.Set(MEMedicationMapping.CustomProperty, data.Custom));
                    uv.Add(MB.Update.Set(MEMedicationMapping.VerifiedProperty, data.Verified));
                    uv.Add(MB.Update.Set(MEMedicationMapping.DeleteFlagProperty, data.DeleteFlag));
                    DataAuditType type;
                    if (data.DeleteFlag)
                    {
                        uv.Add(MB.Update.Set(MEMedicationMapping.TTLDateProperty, System.DateTime.UtcNow.AddDays(_expireDays)));
                        type = DataAuditType.Delete;
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEMedicationMapping.TTLDateProperty, BsonNull.Value));
                        type = DataAuditType.Update;
                    }
                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.MedicationMaps.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.MedicationMap.ToString(),
                                            data.Id,
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

        public object FindNDCCodes(object request)
        {
            throw new NotImplementedException();
        }

        public object Initialize(object newEntity)
        {
            PutInitializeMedicationMapDataRequest request = (PutInitializeMedicationMapDataRequest)newEntity;
            MedicationMapData data = null;
            try
            {
                MEMedicationMapping meMM = new MEMedicationMapping(this.UserId)
                {
                    FullName = string.IsNullOrEmpty(request.MedicationMapData.FullName) ? null : request.MedicationMapData.FullName.ToUpper(),
                    Strength = request.MedicationMapData.Strength,
                    Form = string.IsNullOrEmpty(request.MedicationMapData.Form) ? null : request.MedicationMapData.Form.ToUpper(),
                    Route = string.IsNullOrEmpty(request.MedicationMapData.Route) ? null : request.MedicationMapData.Route.ToUpper(),
                    TTLDate = System.DateTime.UtcNow.AddDays(_initializeDays),
                    DeleteFlag = false
                };

                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    ctx.MedicationMaps.Collection.Insert(meMM);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.MedicationMap.ToString(),
                                            meMM.Id.ToString(),
                                            DataAuditType.Insert,
                                            request.ContractNumber);

                    data = new MedicationMapData
                    {
                        Id = meMM.Id.ToString(),
                        FullName = meMM.FullName
                    };
                }
                return data;
            }
            catch (Exception) { throw; }
        }

        public object Find(object entity)
        {
            string result = null;
            try
            {
                MedicationMapData e = (MedicationMapData)entity;
                List<IMongoQuery> qList = new List<IMongoQuery>();
                if (e.Form.IsNullOrEmpty())
                {
                    qList.Add(Query.EQ(MEMedicationMapping.FormProperty, BsonNull.Value));
                }
                else
                {
                    qList.Add(Query<MEMedicationMapping>.EQ(b => b.Form, e.Form));
                }

                if (e.Strength.IsNullOrEmpty())
                {
                    qList.Add(Query.EQ(MEMedicationMapping.StrengthProperty, BsonNull.Value));
                }
                else
                {
                    qList.Add(Query<MEMedicationMapping>.EQ(b => b.Strength, e.Strength));
                }

                if (e.Route.IsNullOrEmpty())
                {
                    qList.Add(Query.EQ(MEMedicationMapping.RouteProperty, BsonNull.Value));
                }
                else
                {
                    qList.Add(Query<MEMedicationMapping>.EQ(b => b.Route, e.Route));
                }

                if (e.FullName.IsNullOrEmpty())
                {
                    qList.Add(Query.EQ(MEMedicationMapping.FullNameProperty, BsonNull.Value));
                }
                else
                {
                    qList.Add(Query<MEMedicationMapping>.EQ(b => b.FullName, e.FullName));
                }

                qList.Add(Query<MEMedicationMapping>.EQ(b => b.Custom, true));

                var query = Query.And(qList);
                MEMedicationMapping meMM = Context.MedicationMaps.Collection.Find(query).FirstOrDefault();
                if(meMM != null)
                {
                    result = meMM.Id.ToString();
                }
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        public object FindByName(object request)
        {
            throw new NotImplementedException();
        }
    }
}
