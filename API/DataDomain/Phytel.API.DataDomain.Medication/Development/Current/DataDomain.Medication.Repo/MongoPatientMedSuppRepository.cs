
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
                MEPatientMedSupp mePMS = new MEPatientMedSupp(this.UserId)
                {
                    PatientId = ObjectId.Parse(data.PatientId),
                    Name = data.Name,
                    CategoryId = (Category)data.CategoryId,
                    TypeId  = ObjectId.Parse(data.TypeId),
                    StatusId = (Status)data.StatusId,
                    Dosage = data.Dosage,
                    Strength = data.Strength,
                    Route = data.Route,
                    Form = data.Form,
                    PharmClasses = data.PharmClasses,
                    NDCs = data.NDCs,
                    FreqQuantity = data.FreqQuantity,
                    FreqHowOftenId = string.IsNullOrEmpty(data.FreqHowOftenId) ? (ObjectId?)null : ObjectId.Parse(data.FreqHowOftenId),
                    FreqWhenId = string.IsNullOrEmpty(data.FreqWhenId) ? (ObjectId?)null : ObjectId.Parse(data.FreqWhenId),
                    SourceId = ObjectId.Parse(data.SourceId),
                    StartDate = data.StartDate == null ? (DateTime?)null : data.StartDate,
                    EndDate = data.EndDate == null ? (DateTime?)null : data.EndDate,
                    Reason = data.Reason,
                    Notes = data.Notes,
                    PrescribedBy = data.PrescribedBy,
                    SystemName = data.SystemName,
                    DeleteFlag = false
                };

                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    ctx.PatientMedSupps.Collection.Insert(mePMS);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientMedSupp.ToString(),
                                            mePMS.Id.ToString(),
                                            DataAuditType.Insert,
                                            request.ContractNumber);
                }
                return AutoMapper.Mapper.Map<PatientMedSuppData>(mePMS);
            }
            catch (Exception) { throw; }
        }

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(object entity)
        {
            DeleteMedSuppsByPatientIdDataRequest request = (DeleteMedSuppsByPatientIdDataRequest)entity;
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
                    uv.Add(MB.Update.Set(MEPatientMedSupp.NameProperty, data.Name));
                    if (data.CategoryId != 0) uv.Add(MB.Update.Set(MEPatientMedSupp.CategoryProperty, data.CategoryId));
                    if (data.TypeId != null) uv.Add(MB.Update.Set(MEPatientMedSupp.TypeIdProperty, ObjectId.Parse(data.TypeId)));
                    if (data.StatusId != 0) uv.Add(MB.Update.Set(MEPatientMedSupp.StatusProperty, data.StatusId));
                    uv.Add(MB.Update.Set(MEPatientMedSupp.DosageProperty, data.Dosage));
                    uv.Add(MB.Update.Set(MEPatientMedSupp.StrengthProperty, data.Strength));
                    uv.Add(MB.Update.Set(MEPatientMedSupp.FormProperty, data.Form));
                    uv.Add(MB.Update.Set(MEPatientMedSupp.RouteProperty, data.Route));
                    uv.Add(MB.Update.SetWrapped<List<string>>(MEPatientMedSupp.PharmClassProperty, data.PharmClasses));
                    uv.Add(MB.Update.SetWrapped<List<string>>(MEPatientMedSupp.NDCProperty, data.NDCs));
                    uv.Add(MB.Update.Set(MEPatientMedSupp.FreqQuantityProperty, data.FreqQuantity));
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
                    uv.Add(MB.Update.Set(MEPatientMedSupp.ReasonProperty, data.Reason));
                    uv.Add(MB.Update.Set(MEPatientMedSupp.NotesProperty, data.Notes));
                    uv.Add(MB.Update.Set(MEPatientMedSupp.PrescribedByProperty, data.PrescribedBy));
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
    }
}
