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
                    meN = new MEPatientNote(this.UserId)
                    {
                        PatientId = ObjectId.Parse(noteData.PatientId),
                        Text = noteData.Text,
                        ProgramIds = Helper.ConvertToObjectIdList(noteData.ProgramIds),
                        ValidatedIdentity = noteData.ValidatedIdentity,
                        ContactedOn = noteData.ContactedOn,
                        Type = ObjectId.Parse(noteData.TypeId),
                        DeleteFlag = false
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
                    if(!string.IsNullOrEmpty(noteData.DurationId))
                    {
                        meN.DurationId = ObjectId.Parse(noteData.DurationId);
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
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception) { throw; }
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
                    uv.Add(MB.Update.Set(MEPatientNote.TTLDateProperty, DateTime.UtcNow.AddDays(_expireDays)));
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
                            DurationId = (meN.DurationId == null) ? null : meN.DurationId.ToString(),
                            ValidatedIdentity = meN.ValidatedIdentity,
                            ContactedOn = meN.ContactedOn,
                            UpdatedById = (meN.UpdatedBy == null) ? null : meN.UpdatedBy.ToString(),
                            UpdatedOn = meN.LastUpdatedOn
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
                    if (pn.DurationId != null)
                    {
                        uv.Add(MB.Update.Set(MEPatientNote.DurationIdProperty, ObjectId.Parse(pn.DurationId)));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientNote.DurationIdProperty, BsonNull.Value));
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
                                DurationId = (meN.DurationId == null) ? null : meN.DurationId.ToString(),
                                ValidatedIdentity = meN.ValidatedIdentity,
                                ContactedOn = meN.ContactedOn,
                                UpdatedById = (meN.UpdatedBy == null) ? null : meN.UpdatedBy.ToString(),
                                UpdatedOn = meN.LastUpdatedOn
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
    }
}
