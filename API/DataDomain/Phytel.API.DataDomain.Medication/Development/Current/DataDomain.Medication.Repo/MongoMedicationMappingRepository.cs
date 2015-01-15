
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
            try
            {
                var req = newEntity as PutInsertMedicationMappingRequest;
                var mm = req.MedicationMapping;
                // create endpoints for insert so you can use the request object.
                var MEMedMap = new MEMedicationMapping(req.UserId)
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

                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    ctx.MedicationMaps.Collection.Insert(MEMedMap);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.MedicationMap.ToString(),
                                            MEMedMap.Id.ToString(),
                                            DataAuditType.Insert,
                                            ContractDBName);
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

        public Tuple<string, IEnumerable<object>> Select(Phytel.API.Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Update(object entity)
        {
            bool result = false;
            PutMedicationMapDataRequest pa = (PutMedicationMapDataRequest)entity;
            MedicationMapData data = pa.MedicationMappingData;
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
                        uv.Add(MB.Update.Set(MEMedicationMapping.FullNameProperty, data.FullName));
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
                        uv.Add(MB.Update.Set(MEMedicationMapping.FormProperty, data.Form));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEMedicationMapping.FormProperty, BsonNull.Value));
                    }
                    if (!string.IsNullOrEmpty(data.Route))
                    {
                        uv.Add(MB.Update.Set(MEMedicationMapping.RouteProperty, data.Route));
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
                    FullName = request.MedicationMapData.FullName,
                    Custom = true,
                    Verified = false,
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
                        FullName = meMM.FullName.ToUpper()
                    };
                }
                return data;
            }
            catch (Exception) { throw; }
        }

    }
}
