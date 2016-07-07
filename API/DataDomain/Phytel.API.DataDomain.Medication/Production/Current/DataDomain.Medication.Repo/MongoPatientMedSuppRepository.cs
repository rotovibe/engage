
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
    public class MongoPatientMedSuppRepository<TContext> : IMongoPatientMedSuppRepository, IMongoMedicationRepository where TContext : MedicationMongoContext
    {
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        private int _initializeDays = Convert.ToInt32(ConfigurationManager.AppSettings["InitializeDays"]);
        
        protected readonly TContext Context;
        public string ContractDBName { get; set; }
        public string UserId { get; set; }

        public MongoPatientMedSuppRepository(IUOWMongo<TContext> uow)
        {
            Context = uow.MongoContext;
        }

        public MongoPatientMedSuppRepository(TContext context)
        {
            Context = context;
        }


        public MongoPatientMedSuppRepository(string dbName)
        {
            ContractDBName = dbName;
        }

        public object Insert(object newEntity)
        {
            PutPatientMedSuppDataRequest request = (PutPatientMedSuppDataRequest)newEntity;
            PatientMedSuppData data  = request.PatientMedSuppData;
            try
            {
                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    MEPatientMedSupp mePMS = new MEPatientMedSupp(this.UserId)
                    {
                        PatientId = ObjectId.Parse(data.PatientId),
                        //FamilyId = string.IsNullOrEmpty(data.FamilyId) ? (ObjectId?)null : ObjectId.Parse(data.FamilyId),
                        Name = string.IsNullOrEmpty(data.Name) ? null : data.Name.ToUpper(),
                        CategoryId = (Category)data.CategoryId,
                        TypeId  = ObjectId.Parse(data.TypeId),
                        StatusId = (Status)data.StatusId,
                        Dosage = data.Dosage,
                        Strength = data.Strength,
                        Route = string.IsNullOrEmpty(data.Route) ? null : data.Route.ToUpper(),
                        Form = string.IsNullOrEmpty(data.Form) ? null : data.Form.ToUpper(),
                        PharmClasses = getPharmClassses(ctx, data.Name),
                        NDCs = (data.NDCs != null && data.NDCs.Count > 0) ? data.NDCs : null,
                        FreqQuantity = data.FreqQuantity,
                        FreqHowOftenId = string.IsNullOrEmpty(data.FreqHowOftenId) ? (ObjectId?)null : ObjectId.Parse(data.FreqHowOftenId),
                        FreqWhenId = string.IsNullOrEmpty(data.FreqWhenId) ? (ObjectId?)null : ObjectId.Parse(data.FreqWhenId),
                        FrequencyId = string.IsNullOrEmpty(data.FrequencyId) ? (ObjectId?)null : ObjectId.Parse(data.FrequencyId),
                        SourceId = ObjectId.Parse(data.SourceId),
                        StartDate = data.StartDate == null ? (DateTime?)null : data.StartDate,
                        EndDate = data.EndDate == null ? (DateTime?)null : data.EndDate,
                        Reason = data.Reason,
                        Notes = data.Notes,
                        SigCode = data.SigCode,
                        PrescribedBy = data.PrescribedBy,
                        SystemName = data.SystemName,
                        DeleteFlag = false,
                        DataSource = data.DataSource,
                        ExternalRecordId = data.ExternalRecordId,
                        OriginalDataSource = data.OriginalDataSource,
                        Duration = data.Duration,
                        DurationUnitId = string.IsNullOrEmpty(data.DurationUnitId) ? (ObjectId?)null : ObjectId.Parse(data.DurationUnitId),
                        OtherDuration = data.OtherDuration,
                        ReviewId = string.IsNullOrEmpty(data.ReviewId) ? (ObjectId?)null : ObjectId.Parse(data.ReviewId),
                        RefusalReasonId = string.IsNullOrEmpty(data.RefusalReasonId) ? (ObjectId?)null : ObjectId.Parse(data.RefusalReasonId),
                        OtherRefusalReason = data.OtherRefusalReason,
                        OrderedBy = data.OrderedBy,
                        OrderedDate = data.OrderedDate,
                        PrescribedDate = data.PrescribedDate,
                        RxNumber = Helper.TrimAndLimit(data.RxNumber, 50),
                        RxDate = data.RxDate,
                        Pharmacy = Helper.TrimAndLimit(data.Pharmacy, 500)
                    };
                   ctx.PatientMedSupps.Collection.Insert(mePMS);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientMedSupp.ToString(),
                                            mePMS.Id.ToString(),
                                            DataAuditType.Insert,
                                            request.ContractNumber);
                    return mePMS.Id.ToString();
                }
            }
            catch (Exception) { throw; }
        }

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(object entity)
        {
            DeletePatientMedSuppDataRequest request = (DeletePatientMedSuppDataRequest)entity;
            try
            {
                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    var q = MB.Query<MEPatientMedSupp>.EQ(b => b.Id, ObjectId.Parse(request.Id));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientMedSupp.TTLDateProperty, DateTime.UtcNow.AddDays(_expireDays)));
                    uv.Add(MB.Update.Set(MEPatientMedSupp.LastUpdatedOnProperty, DateTime.UtcNow));
                    uv.Add(MB.Update.Set(MEPatientMedSupp.DeleteFlagProperty, true));
                    uv.Add(MB.Update.Set(MEPatientMedSupp.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientMedSupps.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientMedSupp.ToString(),
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
            PatientMedSuppData data = null;
            try
            {
                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEPatientMedSupp.IdProperty, ObjectId.Parse(entityID)));
                    IMongoQuery mQuery = Query.And(queries);
                    MEPatientMedSupp mePMS = ctx.PatientMedSupps.Collection.Find(mQuery).FirstOrDefault();
                    if (mePMS != null)
                    {
                        data = AutoMapper.Mapper.Map<PatientMedSuppData>(mePMS);
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
            throw new NotImplementedException();
        }

        public object Update(object entity)
        {
            bool result = false;
            PutPatientMedSuppDataRequest request = (PutPatientMedSuppDataRequest)entity;
            PatientMedSuppData data = request.PatientMedSuppData;
            try
            {
                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    var q = MB.Query<MEPatientMedSupp>.EQ(b => b.Id, ObjectId.Parse(data.Id));
                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientMedSupp.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEPatientMedSupp.VersionProperty, request.Version));
                    uv.Add(MB.Update.Set(MEPatientMedSupp.LastUpdatedOnProperty, System.DateTime.UtcNow));
                    if (data.PatientId != null) uv.Add(MB.Update.Set(MEPatientMedSupp.PatientIdProperty, ObjectId.Parse(data.PatientId)));
                    if (!string.IsNullOrEmpty(data.Name))
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.NameProperty, data.Name.ToUpper()));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.NameProperty, BsonNull.Value));
                    }
                    //if (data.FamilyId != null)
                    //{
                    //    uv.Add(MB.Update.Set(MEPatientMedSupp.FamilyIdProperty, ObjectId.Parse(data.FamilyId)));
                    //}
                    //else
                    //{
                    //    uv.Add(MB.Update.Set(MEPatientMedSupp.FamilyIdProperty, BsonNull.Value));
                    //}
                    if (data.CategoryId != 0) uv.Add(MB.Update.Set(MEPatientMedSupp.CategoryProperty, data.CategoryId));
                    if (data.TypeId != null) uv.Add(MB.Update.Set(MEPatientMedSupp.TypeIdProperty, ObjectId.Parse(data.TypeId)));
                    if (data.StatusId != 0) uv.Add(MB.Update.Set(MEPatientMedSupp.StatusProperty, data.StatusId));
                    if (!string.IsNullOrEmpty(data.Dosage))
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.DosageProperty, data.Dosage));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.DosageProperty, BsonNull.Value));
                    }
                    if (!string.IsNullOrEmpty(data.Strength))
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.StrengthProperty, data.Strength));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.StrengthProperty, BsonNull.Value));
                    }
                    if (!string.IsNullOrEmpty(data.Form))
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.FormProperty, data.Form.ToUpper()));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.FormProperty, BsonNull.Value));
                    }
                    if (!string.IsNullOrEmpty(data.Route))
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.RouteProperty, data.Route.ToUpper()));
                    }
                    else 
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.RouteProperty, BsonNull.Value));
                    }
                    uv.Add(MB.Update.SetWrapped<List<string>>(MEPatientMedSupp.PharmClassProperty, (data.PharmClasses != null && data.PharmClasses.Count > 0) ? data.PharmClasses : null));
                    uv.Add(MB.Update.SetWrapped<List<string>>(MEPatientMedSupp.NDCProperty, (data.NDCs != null && data.NDCs.Count > 0) ? data.NDCs : null));
                    if (!string.IsNullOrEmpty(data.FreqQuantity))
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.FreqQuantityProperty, data.FreqQuantity));
                    }
                    else 
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.FreqQuantityProperty, BsonNull.Value));
                    }
                    if (data.FreqHowOftenId != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.FreqHowOftenIdProperty, ObjectId.Parse(data.FreqHowOftenId)));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.FreqHowOftenIdProperty, BsonNull.Value));
                    }
                    if (data.FreqWhenId != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.FreqWhenIdProperty, ObjectId.Parse(data.FreqWhenId)));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.FreqWhenIdProperty, BsonNull.Value));
                    }
                    if (data.FrequencyId != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.FrequencyIdProperty, ObjectId.Parse(data.FrequencyId)));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.FrequencyIdProperty, BsonNull.Value));
                    }
                    if (data.SourceId != null) { uv.Add(MB.Update.Set(MEPatientMedSupp.SourceIdProperty, ObjectId.Parse(data.SourceId))); }
                    if (data.StartDate != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.StartDateProperty, data.StartDate));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.StartDateProperty, BsonNull.Value));
                    }
                    if (data.EndDate != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.EndDateProperty, data.EndDate));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.EndDateProperty, BsonNull.Value));
                    }
                    if (!string.IsNullOrEmpty(data.Reason))
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.ReasonProperty, data.Reason));
                    }
                    else 
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.ReasonProperty, BsonNull.Value));
                    }
                    if (!string.IsNullOrEmpty(data.SigCode))
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.SigProperty, data.SigCode));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.SigProperty, BsonNull.Value));
                    }
                    if (!string.IsNullOrEmpty(data.Notes))
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.NotesProperty, data.Notes));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.NotesProperty, BsonNull.Value));
                    }
                    if (!string.IsNullOrEmpty(data.PrescribedBy))
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.PrescribedByProperty, data.PrescribedBy));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.PrescribedByProperty, BsonNull.Value));
                    }

                    if (!string.IsNullOrEmpty(data.DataSource))
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.DataSourceProperty, data.DataSource));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.DataSourceProperty, BsonNull.Value));
                    }

                    if (!string.IsNullOrEmpty(data.ExternalRecordId))
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.ExternalRecordIdProperty, data.ExternalRecordId));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.ExternalRecordIdProperty, BsonNull.Value));
                    }

                    if (string.IsNullOrEmpty(data.OriginalDataSource))
                        uv.Add(MB.Update.Set(MEPatientMedSupp.OriginalDataSourceProperty, BsonNull.Value));
                    else
                        uv.Add(MB.Update.Set(MEPatientMedSupp.OriginalDataSourceProperty, data.OriginalDataSource));
                    uv.Add(MB.Update.Set(MEPatientMedSupp.DurationProperty, data.Duration));
                    if (string.IsNullOrEmpty(data.DurationUnitId))
                        uv.Add(MB.Update.Set(MEPatientMedSupp.DurationUnitProperty, BsonNull.Value));
                    else
                        uv.Add(MB.Update.Set(MEPatientMedSupp.DurationUnitProperty, ObjectId.Parse(data.DurationUnitId)));
                    if (string.IsNullOrEmpty(data.OtherDuration))
                        uv.Add(MB.Update.Set(MEPatientMedSupp.OtherDurationProperty, BsonNull.Value));
                    else
                        uv.Add(MB.Update.Set(MEPatientMedSupp.OtherDurationProperty, data.OtherDuration));
                    if (string.IsNullOrEmpty(data.ReviewId))
                        uv.Add(MB.Update.Set(MEPatientMedSupp.ReviewProperty, BsonNull.Value));
                    else
                        uv.Add(MB.Update.Set(MEPatientMedSupp.ReviewProperty, ObjectId.Parse(data.ReviewId)));
                    if (string.IsNullOrEmpty(data.RefusalReasonId))
                        uv.Add(MB.Update.Set(MEPatientMedSupp.RefusalReasonProperty, BsonNull.Value));
                    else
                        uv.Add(MB.Update.Set(MEPatientMedSupp.RefusalReasonProperty, ObjectId.Parse(data.RefusalReasonId)));
                    if (string.IsNullOrEmpty(data.OtherRefusalReason))
                        uv.Add(MB.Update.Set(MEPatientMedSupp.OtherRefusalReasonProperty, BsonNull.Value));
                    else
                        uv.Add(MB.Update.Set(MEPatientMedSupp.OtherRefusalReasonProperty, data.OtherRefusalReason));
                    if (string.IsNullOrEmpty(data.OrderedBy))
                        uv.Add(MB.Update.Set(MEPatientMedSupp.OrderedByProperty, BsonNull.Value));
                    else
                        uv.Add(MB.Update.Set(MEPatientMedSupp.OrderedByProperty, data.OrderedBy));
                    if (data.OrderedDate == null)
                        uv.Add(MB.Update.Set(MEPatientMedSupp.OrderedDateProperty, BsonNull.Value));
                    else
                        uv.Add(MB.Update.Set(MEPatientMedSupp.OrderedDateProperty, data.OrderedDate));
                    if (data.PrescribedDate == null)
                        uv.Add(MB.Update.Set(MEPatientMedSupp.PrescribedDateProperty, BsonNull.Value));
                    else
                        uv.Add(MB.Update.Set(MEPatientMedSupp.PrescribedDateProperty, data.PrescribedDate));
                    if (data.RxDate == null)
                        uv.Add(MB.Update.Set(MEPatientMedSupp.RxDateProperty, BsonNull.Value));
                    else
                        uv.Add(MB.Update.Set(MEPatientMedSupp.RxDateProperty, data.RxDate));
                    if (string.IsNullOrEmpty(data.Pharmacy))
                        uv.Add(MB.Update.Set(MEPatientMedSupp.PharmacyProperty, BsonNull.Value));
                    else
                        uv.Add(MB.Update.Set(MEPatientMedSupp.PharmacyProperty, Helper.TrimAndLimit(data.Pharmacy, 500)));
                    if (string.IsNullOrEmpty(data.RxNumber))
                        uv.Add(MB.Update.Set(MEPatientMedSupp.RxNumberProperty, BsonNull.Value));
                    else
                        uv.Add(MB.Update.Set(MEPatientMedSupp.RxNumberProperty, Helper.TrimAndLimit(data.RxNumber, 50)));
                    uv.Add(MB.Update.Set(MEPatientMedSupp.SystemProperty, data.SystemName));
                    uv.Add(MB.Update.Set(MEPatientMedSupp.DeleteFlagProperty, data.DeleteFlag));
                    DataAuditType type;
                    if (data.DeleteFlag)
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.TTLDateProperty, System.DateTime.UtcNow.AddDays(_expireDays)));
                        type = DataAuditType.Delete;
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientMedSupp.TTLDateProperty, BsonNull.Value));
                        type = DataAuditType.Update;
                    }
                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientMedSupps.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientMedSupp.ToString(),
                                            data.Id,
                                            type,
                                            request.ContractNumber);

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
            UndoDeletePatientMedSuppsDataRequest request = (UndoDeletePatientMedSuppsDataRequest)entity;
            try
            {
                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    var q = MB.Query<MEPatientMedSupp>.EQ(b => b.Id, ObjectId.Parse(request.PatientMedSuppId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientMedSupp.TTLDateProperty, BsonNull.Value));
                    uv.Add(MB.Update.Set(MEPatientMedSupp.DeleteFlagProperty, false));
                    uv.Add(MB.Update.Set(MEPatientMedSupp.LastUpdatedOnProperty, DateTime.UtcNow));
                    uv.Add(MB.Update.Set(MEPatientMedSupp.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientMedSupps.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientMedSupp.ToString(),
                                            request.PatientMedSuppId.ToString(),
                                            DataAuditType.UndoDelete,
                                            request.ContractNumber);

                }
            }
            catch (Exception) { throw; }
        }

        public object FindByPatientId(object request)
        {
            List<PatientMedSuppData> list = null;
            GetPatientMedSuppsDataRequest dataRequest = (GetPatientMedSuppsDataRequest)request;
            try
            {
                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEPatientMedSupp.PatientIdProperty, ObjectId.Parse(dataRequest.PatientId)));
                    queries.Add(Query.EQ(MEPatientMedSupp.DeleteFlagProperty, false));
                    queries.Add(Query.EQ(MEPatientMedSupp.TTLDateProperty, BsonNull.Value));
                    if (dataRequest.StatusIds != null && dataRequest.StatusIds.Count > 0)
                    {
                        queries.Add(Query.In(MEPatientMedSupp.StatusProperty, new BsonArray(dataRequest.StatusIds)));
                    }
                    if (dataRequest.CategoryIds != null && dataRequest.CategoryIds.Count > 0)
                    {
                        queries.Add(Query.In(MEPatientMedSupp.CategoryProperty, new BsonArray(dataRequest.CategoryIds)));
                    }
                    IMongoQuery mQuery = Query.And(queries);
                    List<MEPatientMedSupp> mePMSs = ctx.PatientMedSupps.Collection.Find(mQuery).ToList();
                    if (mePMSs != null && mePMSs.Count > 0)
                    {
                        list = new List<PatientMedSuppData>();
                        mePMSs.ForEach(p =>
                        {
                            PatientMedSuppData data = AutoMapper.Mapper.Map<PatientMedSuppData>(p);
                            list.Add(data);
                        });
                    }
                }
                return list;
            }
            catch (Exception) { throw; }
        }

        private List<string> getPharmClassses(MedicationMongoContext ctx, string name)
        {
            List<string> result = null;
            List<MEMedication> meMs = ctx.Medications.Collection.Find(Query.EQ(MEMedication.FullNameProperty, name)).SetFields(MEMedication.PharmClassProperty).ToList();
            if (meMs != null && meMs.Count > 0)
            {
                List<string> list = new List<string>();
                meMs.ForEach(m =>
                {
                    if (m.PharmClass != null && m.PharmClass.Count > 0)
                    {
                        m.PharmClass.ForEach(p =>
                        {
                            // Add only unique pharm classes codes & do not add if it is string empty.
                            if (!list.Contains(p) && !string.IsNullOrEmpty(p))
                            {
                                list.Add(p);
                            }
                        });
                    }
                });
                if (list.Count > 0) result = list;
            }
            return result;
        }

        public object FindNDCCodes(object request)
        {
            throw new NotImplementedException();
        }

        public object Initialize(object newEntity)
        {
            throw new NotImplementedException();
        }

        public object Search(object request)
        {
            int result = 0;
            GetPatientMedSuppsCountDataRequest dataRequest = (GetPatientMedSuppsCountDataRequest)request;
            try
            {
                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    if (string.IsNullOrEmpty(dataRequest.Name))
                    {
                        queries.Add(Query.EQ(MEPatientMedSupp.NameProperty, BsonNull.Value));
                    }
                    else
                    {
                        queries.Add(Query.EQ(MEPatientMedSupp.NameProperty, dataRequest.Name));
                    }
                    if (string.IsNullOrEmpty(dataRequest.Route))
                    {
                        queries.Add(Query.EQ(MEPatientMedSupp.RouteProperty, BsonNull.Value));
                    }
                    else
                    {
                        queries.Add(Query.EQ(MEPatientMedSupp.RouteProperty, dataRequest.Route));
                    }
                    if (string.IsNullOrEmpty(dataRequest.Form))
                    {
                        queries.Add(Query.EQ(MEPatientMedSupp.FormProperty, BsonNull.Value));
                    }
                    else
                    {
                        queries.Add(Query.EQ(MEPatientMedSupp.FormProperty, dataRequest.Form));
                    }
                    if (string.IsNullOrEmpty(dataRequest.Strength))
                    {
                        queries.Add(Query.EQ(MEPatientMedSupp.StrengthProperty, BsonNull.Value));
                    }
                    else
                    {
                        queries.Add(Query.EQ(MEPatientMedSupp.StrengthProperty, dataRequest.Strength));
                    }
                    queries.Add(Query.EQ(MEPatientMedSupp.DeleteFlagProperty, false));
                    queries.Add(Query.EQ(MEPatientMedSupp.TTLDateProperty, BsonNull.Value));
                    IMongoQuery mQuery = Query.And(queries);
                    List<MEPatientMedSupp> list = ctx.PatientMedSupps.Collection.Find(mQuery).SetFields(MEPatientMedSupp.PatientIdProperty).ToList();
                    result = list.Select(s => s.PatientId).Distinct().Count();
                }
                return result;
            }
            catch (Exception) { throw; }
        }


        public object Find(object request)
        {
            throw new NotImplementedException();
        }


        public object FindByName(object request)
        {
            throw new NotImplementedException();
        }
    }
}
