using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.DataDomain.PatientNote.DTO;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using MB = MongoDB.Driver.Builders;
using System.Configuration;
using MongoDB.Bson.Serialization;
using Phytel.API.Common;
using Phytel.API.Common.Audit;
using Phytel.API.DataAudit;

namespace Phytel.API.DataDomain.PatientNote.Repo
{
    public class MongoPatientNoteRepository<TContext> : IMongoPatientNoteRepository where TContext : PatientNoteMongoContext
    {
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);

        protected readonly TContext Context;
        public string ContractDBName { get; set; }
        public string UserId { get; set; }
        
        public MongoPatientNoteRepository(IUOWMongo<TContext> uow)
        {
            Context = uow.MongoContext;
        }

        public MongoPatientNoteRepository(TContext context)
        {
            Context = context;
        }

        public MongoPatientNoteRepository(string dbName)
        {
            ContractDBName = dbName;
        }

        public object Insert(object newEntity)
        {
            InsertPatientNoteDataRequest request = (InsertPatientNoteDataRequest)newEntity;
            PatientNoteData noteData = request.PatientNote;
            string noteId = string.Empty;
            MEPatientNote meN = null;
            try
            {
                if(noteData != null)
                {
                    meN = new MEPatientNote(this.UserId, noteData.CreatedOn)
                    {
                        PatientId = ObjectId.Parse(noteData.PatientId),
                        Text = noteData.Text,
                        ProgramIds = Helper.ConvertToObjectIdList(noteData.ProgramIds),
                        ValidatedIdentity = noteData.ValidatedIdentity,
                        ContactedOn = noteData.ContactedOn,
                        Type = ObjectId.Parse(noteData.TypeId),
                        DeleteFlag = false,
                        DataSource = Helper.TrimAndLimit(noteData.DataSource, 50),
                        LastUpdatedOn = noteData.UpdatedOn,
                        ExternalRecordId = noteData.ExternalRecordId,
                        Duration = noteData.Duration,
                    };

                    if(!string.IsNullOrEmpty(noteData.MethodId))
                    {
                        meN.MethodId = ObjectId.Parse(noteData.MethodId);
                    }
                    if(!string.IsNullOrEmpty(noteData.OutcomeId))
                    {
                        meN.OutcomeId = ObjectId.Parse(noteData.OutcomeId);
                    }
                    if(!string.IsNullOrEmpty(noteData.WhoId))
                    {
                        meN.WhoId = ObjectId.Parse(noteData.WhoId);
                    }
                    if(!string.IsNullOrEmpty(noteData.SourceId))
                    {
                        meN.SourceId = ObjectId.Parse(noteData.SourceId);
                    }                    

                    using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(ContractDBName))
                    {
                        ctx.PatientNotes.Collection.Insert(meN);

                        AuditHelper.LogDataAudit(this.UserId, 
                                                MongoCollectionName.PatientNote.ToString(), 
                                                meN.Id.ToString(), 
                                                Common.DataAuditType.Insert, 
                                                request.ContractNumber);

                        noteId = meN.Id.ToString();
                    }
                }
                return noteId;
            }
            catch (Exception) { throw; }
        }

        public object InsertAll(List<object> entities)
        {
            BulkInsertResult result = new BulkInsertResult();
            List<string> insertedIds = new List<string>();
            List<string> errorMessages = new List<string>();
            try
            {
                using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(ContractDBName))
                {
                    var bulk = ctx.PatientNotes.Collection.InitializeUnorderedBulkOperation();
                    foreach (PatientNoteData data in entities)
                    {
                        MEPatientNote meN = new MEPatientNote(this.UserId, data.CreatedOn)
                        {
                            PatientId = ObjectId.Parse(data.PatientId),
                            Text = data.Text,
                            ProgramIds = Helper.ConvertToObjectIdList(data.ProgramIds),
                            ValidatedIdentity = data.ValidatedIdentity,
                            ContactedOn = data.ContactedOn,
                            Type = ObjectId.Parse(data.TypeId),
                            DeleteFlag = false,
                            DataSource = Helper.TrimAndLimit(data.DataSource, 50),
                            LastUpdatedOn = data.UpdatedOn,
                            ExternalRecordId = data.ExternalRecordId,
                            Duration = data.Duration
                        };

                        if (!string.IsNullOrEmpty(data.MethodId))
                        {
                            meN.MethodId = ObjectId.Parse(data.MethodId);
                        }
                        if (!string.IsNullOrEmpty(data.OutcomeId))
                        {
                            meN.OutcomeId = ObjectId.Parse(data.OutcomeId);
                        }
                        if (!string.IsNullOrEmpty(data.WhoId))
                        {
                            meN.WhoId = ObjectId.Parse(data.WhoId);
                        }
                        if (!string.IsNullOrEmpty(data.SourceId))
                        {
                            meN.SourceId = ObjectId.Parse(data.SourceId);
                        }                        
                        bulk.Insert(meN.ToBsonDocument());
                        insertedIds.Add(meN.Id.ToString());
                    }
                    BulkWriteResult bwr = bulk.Execute();
                }
                // TODO: Auditing.
            }
            catch (BulkWriteException bwEx)
            {
                // Get the error messages for the ones that failed.
                foreach (BulkWriteError er in bwEx.WriteErrors)
                {
                    errorMessages.Add(er.Message);
                }
            }
            catch (Exception ex)
            {
                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Helper.LogException(int.Parse(aseProcessID), ex);
            }
            result.ProcessedIds = insertedIds;
            result.ErrorMessages = errorMessages;
            return result;
        }

        public void Delete(object entity)
        {
            DeletePatientNoteDataRequest request = (DeletePatientNoteDataRequest)entity;
            try
            {
                using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(ContractDBName))
                {
                    var q = MB.Query<MEPatientNote>.EQ(b => b.Id, ObjectId.Parse(request.Id));

                    var uv = new List<MB.UpdateBuilder>();
                    // eng-1408
                    //uv.Add(MB.Update.Set(MEPatientNote.TTLDateProperty, DateTime.UtcNow.AddDays(_expireDays)));
                    uv.Add(MB.Update.Set(MEPatientNote.LastUpdatedOnProperty, DateTime.UtcNow));
                    uv.Add(MB.Update.Set(MEPatientNote.DeleteFlagProperty, true));
                    uv.Add(MB.Update.Set(MEPatientNote.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientNotes.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.PatientNote.ToString(), 
                                            request.Id.ToString(), 
                                            Common.DataAuditType.Delete, 
                                            request.ContractNumber);
                
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
            PatientNoteData noteData = null;
            try
            {
                using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(ContractDBName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEPatientNote.IdProperty, ObjectId.Parse(entityID)));
                    queries.Add(Query.EQ(MEPatientNote.DeleteFlagProperty, false));
                    IMongoQuery mQuery = Query.And(queries);
                    MEPatientNote meN = ctx.PatientNotes.Collection.Find(mQuery).FirstOrDefault();
                    if (meN != null)
                    {
                        noteData = new PatientNoteData
                        {
                            Id = meN.Id.ToString(),
                            PatientId = meN.PatientId.ToString(),
                            Text = meN.Text,
                            ProgramIds = Helper.ConvertToStringList(meN.ProgramIds),
                            CreatedOn = meN.RecordCreatedOn,
                            CreatedById = meN.RecordCreatedBy.ToString(),
                            TypeId = meN.Type.ToString(),
                            MethodId = (meN.MethodId == null) ? null :  meN.MethodId.ToString(),
                            OutcomeId = (meN.OutcomeId == null) ? null : meN.OutcomeId.ToString(),
                            WhoId = (meN.WhoId == null) ? null : meN.WhoId.ToString(),
                            SourceId = (meN.SourceId == null) ? null : meN.SourceId.ToString(),
                            Duration = meN.Duration,
                            ValidatedIdentity = meN.ValidatedIdentity,
                            ContactedOn = meN.ContactedOn,
                            UpdatedById = (meN.UpdatedBy == null) ? null : meN.UpdatedBy.ToString(),
                            UpdatedOn = meN.LastUpdatedOn,
                            DataSource = meN.DataSource,
                            ExternalRecordId = meN.ExternalRecordId
                        };
                    }
                }
                return noteData;
            }
            catch (Exception) { throw; }
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
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
            UpdatePatientNoteDataRequest request = (UpdatePatientNoteDataRequest)entity;
            PatientNoteData pn = request.PatientNoteData;
            try
            {
                using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(ContractDBName))
                {
                    var q = MB.Query<MEPatientNote>.EQ(b => b.Id, ObjectId.Parse(pn.Id));
                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientNote.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEPatientNote.VersionProperty, request.Version));
                    uv.Add(MB.Update.Set(MEPatientNote.LastUpdatedOnProperty, System.DateTime.UtcNow));
                    if (pn.PatientId != null) uv.Add(MB.Update.Set(MEPatientNote.PatientIdProperty, ObjectId.Parse(pn.PatientId)));
                    if (pn.Text != null) uv.Add(MB.Update.Set(MEPatientNote.TextProperty, pn.Text));
                    if (pn.TypeId != null) uv.Add(MB.Update.Set(MEPatientNote.NoteTypeProperty, ObjectId.Parse(pn.TypeId)));
                    if (pn.ProgramIds != null && pn.ProgramIds.Count > 0)
                    {
                        uv.Add(MB.Update.SetWrapped<List<ObjectId>>(MEPatientNote.ProgramProperty, Helper.ConvertToObjectIdList(pn.ProgramIds)));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientNote.ProgramProperty, BsonNull.Value));
                    }
                    if (pn.MethodId != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientNote.MethodIdProperty, ObjectId.Parse(pn.MethodId)));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientNote.MethodIdProperty, BsonNull.Value));
                    }
                    if (pn.WhoId != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientNote.WhoIdProperty, pn.WhoId));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientNote.WhoIdProperty, BsonNull.Value));
                    }
                    if (pn.SourceId != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientNote.SourceIdProperty, pn.SourceId));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientNote.SourceIdProperty, BsonNull.Value));
                    }
                    if (pn.OutcomeId != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientNote.OutcomeIdProperty, pn.OutcomeId));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientNote.OutcomeIdProperty, BsonNull.Value));
                    }
                    if (pn.Duration != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientNote.DurationProperty, pn.Duration));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientNote.DurationProperty, BsonNull.Value));
                    }
                    uv.Add(MB.Update.Set(MEPatientNote.ValidatedIdentityProperty, pn.ValidatedIdentity));
                    if (pn.ContactedOn != null && !pn.ContactedOn.Equals(new DateTime()))
                    {
                        uv.Add(MB.Update.Set(MEPatientNote.ContactedOnProperty, pn.ContactedOn));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientNote.ContactedOnProperty, BsonNull.Value));
                    }

                    if (pn.DataSource != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientNote.DataSourceProperty, Helper.TrimAndLimit(pn.DataSource, 50)));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientNote.DataSourceProperty, BsonNull.Value));
                    }

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientNotes.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientNote.ToString(),
                                            pn.Id,
                                            DataAuditType.Update,
                                            request.ContractNumber);

                    result = true;
                }
                return result as object;
            }
            catch (Exception) { throw; }
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
            List<PatientNoteData> noteDataList = null;
            GetAllPatientNotesDataRequest dataRequest = (GetAllPatientNotesDataRequest)request;
            try
            {
                using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(ContractDBName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEPatientNote.PatientIdProperty, ObjectId.Parse(dataRequest.PatientId)));
                    queries.Add(Query.EQ(MEPatientNote.DeleteFlagProperty, false));
                    IMongoQuery mQuery = Query.And(queries);
                    List<MEPatientNote> meNotes = null;
                    //if (dataRequest.Count > 0)
                    //{
                    //    meNotes = ctx.PatientNotes.Collection.Find(mQuery).OrderByDescending(o => o.RecordCreatedOn).Take(dataRequest.Count).ToList();
                    //}
                    //else
                    //{
                        meNotes = ctx.PatientNotes.Collection.Find(mQuery).ToList();
                    //}
                    if (meNotes != null && meNotes.Count > 0)
                    {
                        noteDataList = new List<PatientNoteData>();
                        foreach(MEPatientNote meN in meNotes)
                        {
                            noteDataList.Add(new PatientNoteData {
                                Id = meN.Id.ToString(),
                                PatientId = meN.PatientId.ToString(),
                                Text = meN.Text,
                                ProgramIds = Helper.ConvertToStringList(meN.ProgramIds),
                                CreatedOn = meN.RecordCreatedOn,
                                CreatedById = meN.RecordCreatedBy.ToString(),
                                TypeId = meN.Type.ToString(),
                                MethodId = (meN.MethodId == null) ? null : meN.MethodId.ToString(),
                                OutcomeId = (meN.OutcomeId == null) ? null : meN.OutcomeId.ToString(),
                                WhoId = (meN.WhoId == null) ? null : meN.WhoId.ToString(),
                                SourceId = (meN.SourceId == null) ? null : meN.SourceId.ToString(),
                                Duration = meN.Duration,
                                ValidatedIdentity = meN.ValidatedIdentity,
                                ContactedOn = meN.ContactedOn,
                                UpdatedById = (meN.UpdatedBy == null) ? null : meN.UpdatedBy.ToString(),
                                UpdatedOn = meN.LastUpdatedOn,
                                DataSource = meN.DataSource,
                                ExternalRecordId = meN.ExternalRecordId
                            });
                        }
                    }
                }
                return noteDataList;
            }
            catch (Exception) { throw; }
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
                                            MongoCollectionName.PatientNote.ToString(),
                                            request.PatientNoteId.ToString(),
                                            Common.DataAuditType.UndoDelete,
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
                                            MongoCollectionName.PatientNote.ToString(),
                                            request.NoteId.ToString(),
                                            Common.DataAuditType.Update,
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
                    queries.Add(Query.In(MEPatientNote.ProgramProperty, new List<BsonValue> { BsonValue.Create(ObjectId.Parse(entityId)) }));
                    queries.Add(Query.EQ(MEPatientNote.DeleteFlagProperty, false));
                    IMongoQuery mQuery = Query.And(queries);
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
            PatientNoteData data = null;
            try
            {
                using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(ContractDBName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEPatientNote.ExternalRecordIdProperty, externalRecordId));
                    queries.Add(Query.EQ(MEPatientNote.DeleteFlagProperty, false));
                    queries.Add(Query.EQ(MEPatientNote.TTLDateProperty, BsonNull.Value));
                    IMongoQuery mQuery = Query.And(queries);
                    MEPatientNote mePN = ctx.PatientNotes.Collection.Find(mQuery).FirstOrDefault();
                    if (mePN != null)
                    {
                        data = new PatientNoteData
                        {
                            Id = mePN.Id.ToString(),
                        };
                    }
                }
                return data;
            }
            catch (Exception) { throw; }
        }

        public IEnumerable<object> Select(List<string> ids)
        {
            List<PatientNoteData> dataList = null;
            try
            {
                using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(ContractDBName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.In(MEPatientNote.IdProperty, new BsonArray(Helper.ConvertToObjectIdList(ids))));
                    queries.Add(Query.EQ(MEPatientNote.DeleteFlagProperty, false));
                    queries.Add(Query.EQ(MEPatientNote.TTLDateProperty, BsonNull.Value));
                    IMongoQuery mQuery = Query.And(queries);
                    List<MEPatientNote> meNotes = ctx.PatientNotes.Collection.Find(mQuery).ToList();
                    if (meNotes != null && meNotes.Count > 0)
                    {
                        dataList = new List<PatientNoteData>();
                        foreach (MEPatientNote meN in meNotes)
                        {
                            dataList.Add(new PatientNoteData
                            {
                                Id = meN.Id.ToString(),
                                PatientId = meN.PatientId.ToString(),
                                Text = meN.Text,
                                ProgramIds = Helper.ConvertToStringList(meN.ProgramIds),
                                CreatedOn = meN.RecordCreatedOn,
                                CreatedById = meN.RecordCreatedBy.ToString(),
                                TypeId = meN.Type.ToString(),
                                MethodId = (meN.MethodId == null) ? null : meN.MethodId.ToString(),
                                OutcomeId = (meN.OutcomeId == null) ? null : meN.OutcomeId.ToString(),
                                WhoId = (meN.WhoId == null) ? null : meN.WhoId.ToString(),
                                SourceId = (meN.SourceId == null) ? null : meN.SourceId.ToString(),
                                Duration = meN.Duration,
                                ValidatedIdentity = meN.ValidatedIdentity,
                                ContactedOn = meN.ContactedOn,
                                UpdatedById = (meN.UpdatedBy == null) ? null : meN.UpdatedBy.ToString(),
                                UpdatedOn = meN.LastUpdatedOn,
                                DataSource = meN.DataSource,
                                ExternalRecordId = meN.ExternalRecordId
                            });
                        }
                    }
                }
                return dataList as IEnumerable<object>;
            }
            catch (Exception) { throw; }
        }
    }
}
