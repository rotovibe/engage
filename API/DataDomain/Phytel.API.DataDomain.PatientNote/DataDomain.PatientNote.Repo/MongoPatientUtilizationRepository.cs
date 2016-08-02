using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using Phytel.API.Common;
using Phytel.API.DataAudit;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.Interface;
using MB = MongoDB.Driver.Builders;

namespace Phytel.API.DataDomain.PatientNote.Repo
{
    public class MongoPatientUtilizationRepository<TContext> : IMongoPatientNoteRepository where TContext : PatientNoteMongoContext
    {
        private readonly int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        private readonly string _noteTypeId = "5595a957fe7a5904989f229e";

        protected readonly TContext Context;
        public string ContractDBName { get; set; }
        public string UserId { get; set; }
        
        public MongoPatientUtilizationRepository(IUOWMongo<TContext> uow){Context = uow.MongoContext;}

        public MongoPatientUtilizationRepository(TContext context){Context = context;}

        public MongoPatientUtilizationRepository(string dbName){ContractDBName = dbName;}

        public object Insert(object newEntity)
        {
            var data = newEntity as PatientUtilizationData;
            string noteId = string.Empty;
            try
            {
                if (data != null)
                {
                    var meN = new MEPatientUtilization(this.UserId)
                    {
                        DeleteFlag = false,
                        OtherDisposition = Trim(data.OtherDisposition,100),
                        OtherLocation = Trim(data.OtherLocation, 100),
                        OtherType = Trim(data.OtherType, 100),
                        ProgramIds = data.ProgramIds != null ? data.ProgramIds.ConvertAll(ObjectId.Parse) : null,
                        DataSource = data.DataSource = Helper.TrimAndLimit(data.DataSource, 50),
                        Reason = data.Reason = Helper.TrimAndLimit(data.Reason, 5000),
                        Version = 1,
                        Admitted = data.Admitted
                    };

                    if (data.AdmitDate != null && !data.AdmitDate.Equals(new DateTime())) 
                        meN.AdmitDate = data.AdmitDate = data.AdmitDate.GetValueOrDefault().ToUniversalTime();
                    else
                        data.AdmitDate = null;
                    
                    if (data.DischargeDate != null && !data.DischargeDate.Equals(new DateTime()))
                        meN.DischargeDate = data.DischargeDate = data.DischargeDate.GetValueOrDefault().ToUniversalTime();
                    else
                        data.DischargeDate = null;

                    if (!string.IsNullOrEmpty(data.DispositionId)) meN.Disposition = ObjectId.Parse(data.DispositionId);
                    if (!string.IsNullOrEmpty(data.LocationId)) meN.Location = ObjectId.Parse(data.LocationId);
                    if (!string.IsNullOrEmpty(data.SourceId)) meN.SourceId = ObjectId.Parse(data.SourceId);
                    if (!string.IsNullOrEmpty(data.VisitTypeId)) meN.VisitType = ObjectId.Parse(data.VisitTypeId);
                    if (!string.IsNullOrEmpty(data.PatientId)) meN.PatientId = ObjectId.Parse(data.PatientId);
                    if (!string.IsNullOrEmpty(data.TypeId)) meN.NoteType = ObjectId.Parse(data.TypeId);

                    using (Context)
                    {
                        Context.PatientUtilizations.Collection.Insert(meN);

                        AuditHelper.LogDataAudit(this.UserId,
                            MongoCollectionName.PatientUtilization.ToString(),
                            meN.Id.ToString(),
                            DataAuditType.Insert,
                            ContractDBName);

                        noteId = meN.Id.ToString();
                    }


                    data.CreatedById = meN.RecordCreatedBy.ToString();
                    data.CreatedOn = meN.RecordCreatedOn;
                    data.Id = meN.Id.ToString();
                }

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object InsertAll(List<object> entities)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
        }

        public void Delete(object entity)
        {
            try
            {
                if (entity != null)
                {
                    using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(ContractDBName))
                    {
                        string utilId = entity.ToString();
                        var q = MB.Query<MEPatientUtilization>.EQ(b => b.Id, ObjectId.Parse(utilId));
                        var uv = new List<MB.UpdateBuilder>();
                        uv.Add(MB.Update.Set(MEPatientUtilization.LastUpdatedOnProperty, DateTime.UtcNow));
                        uv.Add(MB.Update.Set(MEPatientUtilization.DeleteFlagProperty, true));
                        uv.Add(MB.Update.Set(MEPatientUtilization.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                        IMongoUpdate update = MB.Update.Combine(uv);
                        ctx.PatientUtilizations.Collection.Update(q, update);
                        AuditHelper.LogDataAudit(this.UserId,
                                                MongoCollectionName.PatientUtilization.ToString(),
                                                utilId,
                                                DataAuditType.Delete,
                                                ContractDBName);
                    }
                }
            }
            catch (Exception) { throw; }
        }

        public void DeleteAll(List<object> entities)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
        }

        public object FindByID(string entityID)
        {
            PatientUtilizationData utilData = null;
            try
            {
                using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(ContractDBName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(MB.Query.EQ(MEPatientUtilization.IdProperty, ObjectId.Parse(entityID)));
                    queries.Add(MB.Query.EQ(MEPatientUtilization.DeleteFlagProperty, false));
                    IMongoQuery mQuery = MB.Query.And(queries);
                    MEPatientUtilization meN = ctx.PatientUtilizations.Collection.Find(mQuery).FirstOrDefault();
                    if (meN != null)
                    {
                        utilData = MapPatientUtilizationData(meN);
                    }
                }
                return utilData;
            }
            catch (Exception) { throw; }
        }

        private PatientUtilizationData MapPatientUtilizationData(MEPatientUtilization meN)
        {
            var data = new PatientUtilizationData
            {
                Id = meN.Id.ToString(),
                PatientId = meN.PatientId.ToString(),
                AdmitDate = meN.AdmitDate,
                DischargeDate = meN.DischargeDate,
                DispositionId = meN.Disposition.ToString(),
                LocationId = meN.Location.ToString(),
                OtherDisposition = Helper.TrimAndLimit(meN.OtherDisposition, 100),
                OtherLocation = Helper.TrimAndLimit(meN.OtherLocation, 100),
                OtherType = Helper.TrimAndLimit(meN.OtherType, 100),
                DataSource = Helper.TrimAndLimit(meN.DataSource, 50),
                Reason = Helper.TrimAndLimit(meN.Reason, 5000),
                VisitTypeId = (meN.VisitType == null) ? null : meN.VisitType.ToString(),
                ProgramIds = Helper.ConvertToStringList(meN.ProgramIds),
                SourceId = (meN.SourceId == null) ? null : meN.SourceId.ToString(),
                TypeId = (meN.NoteType == null) ? null : meN.NoteType.ToString(),
                CreatedById = meN.RecordCreatedBy.ToString(),
                CreatedOn = meN.RecordCreatedOn,
                Admitted = meN.Admitted,
                UpdatedById = (meN.UpdatedBy == null) ? null : meN.UpdatedBy.ToString(),
                UpdatedOn = meN.LastUpdatedOn
            };

            return data;
        }

        public Tuple<string, IEnumerable<object>> Select(APIExpression expression)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
        }

        public IEnumerable<object> SelectAll()
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
        }

        public object Update(object entity)
        {
            PatientUtilizationData pn = entity as PatientUtilizationData;
            try
            {
                PatientUtilizationData result;
                using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(ContractDBName))
                {
                    var q = MB.Query<MEPatientUtilization>.EQ(b => b.Id, ObjectId.Parse(pn.Id));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientUtilization.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEPatientUtilization.LastUpdatedOnProperty, DateTime.UtcNow));
                    
                    uv.Add(MB.Update.Set(MEPatientUtilization.AdmittedProperty, pn.Admitted));
                    if (pn.OtherType != null) uv.Add(MB.Update.Set(MEPatientUtilization.OtherTypeProperty, Trim(pn.OtherType, 100)));
                    else uv.Add(MB.Update.Set(MEPatientUtilization.OtherTypeProperty, BsonNull.Value));

                    if (pn.OtherLocation != null) uv.Add(MB.Update.Set(MEPatientUtilization.OtherLocationProperty, Trim(pn.OtherLocation, 100)));
                    else uv.Add(MB.Update.Set(MEPatientUtilization.OtherLocationProperty, BsonNull.Value));

                    if (pn.Reason != null) uv.Add(MB.Update.Set(MEPatientUtilization.ReasonProperty, Trim(pn.Reason, 5000)));
                    else uv.Add(MB.Update.Set(MEPatientUtilization.ReasonProperty, BsonNull.Value));

                    if (pn.OtherDisposition != null) uv.Add(MB.Update.Set(MEPatientUtilization.OtherDispositionProperty, Trim(pn.OtherDisposition, 100)));
                    else uv.Add(MB.Update.Set(MEPatientUtilization.OtherDispositionProperty, BsonNull.Value));

                    if (pn.DataSource != null) uv.Add(MB.Update.Set(MEPatientUtilization.DataSourceProperty, Trim(pn.DataSource, 50)));
                    else uv.Add(MB.Update.Set(MEPatientUtilization.DataSourceProperty, BsonNull.Value));

                    if (pn.AdmitDate != null && !pn.AdmitDate.Equals(new DateTime())) uv.Add(MB.Update.Set(MEPatientUtilization.AdmitDateProperty, pn.AdmitDate));
                    else uv.Add(MB.Update.Set(MEPatientUtilization.AdmitDateProperty, BsonNull.Value));

                    if (pn.DischargeDate != null && !pn.DischargeDate.Equals(new DateTime())) uv.Add(MB.Update.Set(MEPatientUtilization.DischargeDateProperty, pn.DischargeDate));
                    else uv.Add(MB.Update.Set(MEPatientUtilization.DischargeDateProperty, BsonNull.Value));

                    if (!string.IsNullOrEmpty(pn.PatientId)) uv.Add(MB.Update.Set(MEPatientUtilization.PatientIdProperty, ObjectId.Parse(pn.PatientId)));
                    else uv.Add(MB.Update.Set(MEPatientUtilization.PatientIdProperty, BsonNull.Value));

                    if (!string.IsNullOrEmpty(pn.VisitTypeId))
                        uv.Add(MB.Update.Set(MEPatientUtilization.VisitTypeProperty, ObjectId.Parse(pn.VisitTypeId)));
                    else
                        uv.Add(MB.Update.Set(MEPatientUtilization.VisitTypeProperty, BsonNull.Value));

                    if (!string.IsNullOrEmpty(pn.LocationId))
                        uv.Add(MB.Update.Set(MEPatientUtilization.LocationProperty, ObjectId.Parse(pn.LocationId)));
                    else
                        uv.Add(MB.Update.Set(MEPatientUtilization.LocationProperty, BsonNull.Value));

                    if (!string.IsNullOrEmpty(pn.DispositionId))
                        uv.Add(MB.Update.Set(MEPatientUtilization.DispositionProperty, ObjectId.Parse(pn.DispositionId)));
                    else
                        uv.Add(MB.Update.Set(MEPatientUtilization.DispositionProperty, BsonNull.Value));

                    if (!string.IsNullOrEmpty(pn.SourceId))
                        uv.Add(MB.Update.Set(MEPatientUtilization.SourceIdProperty, ObjectId.Parse(pn.SourceId)));
                    else
                        uv.Add(MB.Update.Set(MEPatientUtilization.SourceIdProperty, BsonNull.Value));

                    if (!string.IsNullOrEmpty(pn.TypeId))
                        uv.Add(MB.Update.Set(MEPatientUtilization.NoteTypeProperty, ObjectId.Parse(pn.TypeId)));
                    else
                        uv.Add(MB.Update.Set(MEPatientUtilization.NoteTypeProperty, BsonNull.Value));

                    if (pn.ProgramIds != null && pn.ProgramIds.Count > 0)
                        uv.Add(MB.Update.SetWrapped<List<ObjectId>>(MEPatientUtilization.ProgramsProperty,
                            Helper.ConvertToObjectIdList(pn.ProgramIds)));
                    else
                        uv.Add(MB.Update.Set(MEPatientUtilization.ProgramsProperty, BsonNull.Value));


                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientUtilizations.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                        MongoCollectionName.PatientUtilization.ToString(),
                        pn.Id,
                        DataAuditType.Update,
                        ContractDBName);

                    result = FindByID(pn.Id) as PatientUtilizationData;
                }
                return result as object;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Trim(string p, int limit)
        {
            var val = p;
            if (!string.IsNullOrEmpty(p) && p.Length > limit)
            {
                val = p.Substring(0, limit);
            }
            return val;
        }

        public void CacheByID(List<string> entityIDs)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
        }

        public IEnumerable<object> FindByPatientId(object request)
        {
            List<PatientUtilizationData> utilDataList = null;
            try
            {
                if (request != null)
                {
                    using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(ContractDBName))
                    {
                        List<IMongoQuery> queries = new List<IMongoQuery>();
                        queries.Add(MB.Query.EQ(MEPatientUtilization.PatientIdProperty, ObjectId.Parse(request.ToString())));
                        queries.Add(MB.Query.EQ(MEPatientUtilization.DeleteFlagProperty, false));

                        IMongoQuery mQuery = MB.Query.And(queries);
                        List<MEPatientUtilization> meUtils = null;

                        meUtils = ctx.PatientUtilizations.Collection.Find(mQuery).ToList();

                        if (meUtils != null && meUtils.Count > 0)
                        {
                            utilDataList = meUtils.Select(MapPatientUtilizationData).ToList();
                        }
                    }
                }
                return utilDataList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UndoDelete(object entity)
        {
            try
            {
                if (entity != null)
                {
                    using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(ContractDBName))
                    {
                        string id = entity.ToString();
                        var q = MB.Query<MEPatientUtilization>.EQ(b => b.Id, ObjectId.Parse(id));
                        var uv = new List<MB.UpdateBuilder>();
                        uv.Add(MB.Update.Set(MEPatientUtilization.TTLDateProperty, BsonNull.Value));
                        uv.Add(MB.Update.Set(MEPatientUtilization.DeleteFlagProperty, false));
                        uv.Add(MB.Update.Set(MEPatientUtilization.LastUpdatedOnProperty, DateTime.UtcNow));
                        uv.Add(MB.Update.Set(MEPatientUtilization.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                        IMongoUpdate update = MB.Update.Combine(uv);
                        ctx.PatientUtilizations.Collection.Update(q, update);

                        AuditHelper.LogDataAudit(this.UserId,
                                                MongoCollectionName.PatientUtilization.ToString(),
                                                id,
                                                DataAuditType.UndoDelete,
                                                ContractDBName);

                    }
                }
            }
            catch (Exception) { throw; }
        }

        public void RemoveProgram(object entity, List<string> updatedProgramIds)
        {
            RemoveProgramInPatientNotesDataRequest request = (RemoveProgramInPatientNotesDataRequest)entity;
            try
            {
                List<ObjectId> ids = updatedProgramIds.ConvertAll(r => ObjectId.Parse(r));
                using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(ContractDBName))
                {
                    var q = MB.Query<MEPatientNote>.EQ(b => b.Id, ObjectId.Parse(request.NoteId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.SetWrapped<List<ObjectId>>(MEPatientNote.ProgramProperty, ids));
                    uv.Add(MB.Update.Set(MEPatientNote.LastUpdatedOnProperty, DateTime.UtcNow));
                    uv.Add(MB.Update.Set(MEPatientNote.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientNotes.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientUtilization.ToString(),
                                            request.NoteId.ToString(),
                                            DataAuditType.Update,
                                            request.ContractNumber);

                }
            }
            catch (Exception) { throw; }
        }

        public IEnumerable<object> FindNotesWithAProgramId(string entityId)
        {
            List<PatientNoteData> noteDataList = null;
            try
            {
                using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(ContractDBName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(MB.Query.In(MEPatientNote.ProgramProperty, new List<BsonValue> { BsonValue.Create(ObjectId.Parse(entityId)) }));
                    queries.Add(MB.Query.EQ(MEPatientNote.DeleteFlagProperty, false));
                    IMongoQuery mQuery = MB.Query.And(queries);
                    List<MEPatientNote> meNotes = ctx.PatientNotes.Collection.Find(mQuery).ToList();
                    if (meNotes != null && meNotes.Count > 0)
                    {
                        noteDataList = new List<PatientNoteData>();
                        foreach (MEPatientNote meN in meNotes)
                        {
                            PatientNoteData data = new PatientNoteData
                            {
                                Id = meN.Id.ToString(),
                                PatientId = meN.PatientId.ToString(),
                                Text = meN.Text,
                                ProgramIds = Helper.ConvertToStringList(meN.ProgramIds),
                                CreatedOn = meN.RecordCreatedOn,
                                CreatedById = meN.RecordCreatedBy.ToString()
                            };
                            noteDataList.Add(data);
                        }

                    }
                }
                return noteDataList;
            }
            catch (Exception ex) { throw ex; }
        }


        public object FindByExternalRecordId(string externalRecordId)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<object> Select(List<string> ids)
        {
            throw new NotImplementedException();
        }
    }
}

