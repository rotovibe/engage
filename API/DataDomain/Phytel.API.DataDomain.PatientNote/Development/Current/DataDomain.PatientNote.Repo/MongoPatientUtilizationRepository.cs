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
                        AdmitDate = data.AdmitDate,
                        DeleteFlag = false,
                        DischargeDate = data.DischargeDate, // not added
                        OtherDisposition = data.OtherDisposition,
                        OtherLocation = data.OtherLocation,
                        OtherType = data.OtherType,
                        ProgramIds = data.ProgramIds != null ? data.ProgramIds.ConvertAll(ObjectId.Parse) : null,
                        PSystem = data.PSystem,
                        Reason = data.Reason,
                        Version = 1,
                        Admitted = data.Admitted
                    };

                    if (!string.IsNullOrEmpty(data.Disposition)) meN.Disposition = ObjectId.Parse(data.Disposition);
                    if (!string.IsNullOrEmpty(data.Location)) meN.Location = ObjectId.Parse(data.Location);
                    if (!string.IsNullOrEmpty(data.SourceId)) meN.SourceId = ObjectId.Parse(data.SourceId);
                    if (!string.IsNullOrEmpty(data.VisitType)) meN.VisitType = ObjectId.Parse(data.VisitType);
                    if (!string.IsNullOrEmpty(data.PatientId)) meN.PatientId = ObjectId.Parse(data.PatientId);
                    if (!string.IsNullOrEmpty(data.NoteType)) meN.NoteType = ObjectId.Parse(data.NoteType);

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
                }
                return noteId;
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
            var utilId = entity as string;
            try
            {
                using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(ContractDBName))
                {
                    var q = MB.Query<MEPatientUtilization>.EQ(b => b.Id, ObjectId.Parse(utilId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientUtilization.TTLDateProperty, DateTime.UtcNow.AddDays(_expireDays)));
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

        private static PatientUtilizationData MapPatientUtilizationData(MEPatientUtilization meN)
        {
            var data = new PatientUtilizationData
            {
                Id = meN.Id.ToString(),
                PatientId = meN.PatientId.ToString(),
                AdmitDate = meN.AdmitDate,
                DischargeDate = meN.DischargeDate,
                Disposition = meN.Disposition.ToString(),
                Location = meN.Location.ToString(),
                OtherDisposition = meN.OtherDisposition,
                OtherLocation = meN.OtherLocation,
                OtherType = meN.OtherType,
                PSystem = meN.PSystem,
                Reason = meN.Reason,
                VisitType = (meN.VisitType == null) ? null : meN.VisitType.ToString(),
                ProgramIds = Helper.ConvertToStringList(meN.ProgramIds),
                SourceId = (meN.SourceId == null) ? null : meN.SourceId.ToString(),
                NoteType = (meN.NoteType == null) ? null : meN.NoteType.ToString(),
                RecordCreatedBy = meN.RecordCreatedBy.ToString(),
                RecordCreatedOn = meN.RecordCreatedOn,
                Admitted = meN.Admitted
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
            bool result = false;
            PatientUtilizationData pn = entity as PatientUtilizationData;
            try
            {
                using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(ContractDBName))
                {
                    var q = MB.Query<MEPatientUtilization>.EQ(b => b.Id, ObjectId.Parse(pn.Id));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientUtilization.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEPatientUtilization.AdmittedProperty, pn.Admitted));
                    uv.Add(MB.Update.Set(MEPatientUtilization.LastUpdatedOnProperty, DateTime.UtcNow));
                    if (pn.OtherType != null) uv.Add(MB.Update.Set(MEPatientUtilization.OtherTypeProperty, pn.OtherType));
                    if (pn.AdmitDate != null) uv.Add(MB.Update.Set(MEPatientUtilization.AdmitDateProperty, pn.AdmitDate));
                    if (pn.DischargeDate != null) uv.Add(MB.Update.Set(MEPatientUtilization.DischargeDateProperty, pn.DischargeDate));
                    if (pn.OtherLocation != null) uv.Add(MB.Update.Set(MEPatientUtilization.OtherLocationProperty, pn.OtherLocation));
                    if (pn.Reason != null) uv.Add(MB.Update.Set(MEPatientUtilization.ReasonProperty, pn.Reason));
                    if (pn.OtherDisposition != null) uv.Add(MB.Update.Set(MEPatientUtilization.OtherDispositionProperty, pn.OtherDisposition));
                    if (pn.PSystem != null) uv.Add(MB.Update.Set(MEPatientUtilization.SystemProperty, pn.PSystem));

                    if (!string.IsNullOrEmpty(pn.PatientId))
                    {
                        uv.Add(MB.Update.Set(MEPatientUtilization.PatientIdProperty, ObjectId.Parse(pn.PatientId)));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientUtilization.PatientIdProperty, BsonNull.Value));
                    }

                    if (!string.IsNullOrEmpty(pn.VisitType))
                    {
                        uv.Add(MB.Update.Set(MEPatientUtilization.VisitTypeProperty, ObjectId.Parse(pn.VisitType)));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientUtilization.VisitTypeProperty, BsonNull.Value));
                    }

                    if (!string.IsNullOrEmpty(pn.Location))
                    {
                        uv.Add(MB.Update.Set(MEPatientUtilization.LocationProperty, ObjectId.Parse(pn.Location)));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientUtilization.LocationProperty, BsonNull.Value));
                    }

                    if (!string.IsNullOrEmpty(pn.Disposition))
                    {
                        uv.Add(MB.Update.Set(MEPatientUtilization.DispositionProperty, ObjectId.Parse(pn.Disposition)));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientUtilization.DispositionProperty, BsonNull.Value));
                    }

                    if (!string.IsNullOrEmpty(pn.SourceId))
                    {
                        uv.Add(MB.Update.Set(MEPatientUtilization.SourceIdProperty, ObjectId.Parse(pn.SourceId)));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientUtilization.SourceIdProperty, BsonNull.Value));
                    }

                    if (!string.IsNullOrEmpty(pn.NoteType))
                    {
                        uv.Add(MB.Update.Set(MEPatientUtilization.NoteTypeProperty, ObjectId.Parse(pn.NoteType)));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientUtilization.NoteTypeProperty, BsonNull.Value));
                    }

                    if (pn.ProgramIds != null && pn.ProgramIds.Count > 0)
                    {
                        uv.Add(MB.Update.SetWrapped<List<ObjectId>>(MEPatientUtilization.ProgramsProperty,
                            Helper.ConvertToObjectIdList(pn.ProgramIds)));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientUtilization.ProgramsProperty, BsonNull.Value));
                    }


                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientUtilizations.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                        MongoCollectionName.PatientUtilization.ToString(),
                        pn.Id,
                        DataAuditType.Update,
                        ContractDBName);

                    result = true;
                }
                return result as object;
            }
            catch (Exception)
            {
                throw;
            }
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
                return utilDataList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UndoDelete(object entity)
        {
            UndoDeletePatientNotesDataRequest request = (UndoDeletePatientNotesDataRequest)entity;
            try
            {
                using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(ContractDBName))
                {
                    var q = MB.Query<MEPatientNote>.EQ(b => b.Id, ObjectId.Parse(request.PatientNoteId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientNote.TTLDateProperty, BsonNull.Value));
                    uv.Add(MB.Update.Set(MEPatientNote.DeleteFlagProperty, false));
                    uv.Add(MB.Update.Set(MEPatientNote.LastUpdatedOnProperty, DateTime.UtcNow));
                    uv.Add(MB.Update.Set(MEPatientNote.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientNotes.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientUtilization.ToString(),
                                            request.PatientNoteId.ToString(),
                                            DataAuditType.UndoDelete,
                                            request.ContractNumber);

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
    }
}

