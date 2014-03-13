using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.DataDomain.PatientNote;
using MB = MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.Common;
using Phytel.API.Common.Data;
using System.Configuration;
using Phytel.API.DataAudit;

namespace Phytel.API.DataDomain.PatientNote
{
    public class MongoPatientNoteRepository<T> : IPatientNoteRepository<T>
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);

        public MongoPatientNoteRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            PutPatientNoteDataRequest request = (PutPatientNoteDataRequest)newEntity;
            PatientNoteData noteData = request.PatientNote;
            string noteId = string.Empty;
            MEPatientNote meN = null;
            try
            {
                if(noteData != null)
                {
                    meN = new MEPatientNote(this.UserId)
                    {
                        Id = ObjectId.GenerateNewId(),
                        PatientId = ObjectId.Parse(noteData.PatientId),
                        Text = noteData.Text,
                        ProgramIds = Helper.ConvertToObjectIdList(noteData.ProgramIds),
                        Version = request.Version,
                        UpdatedBy = ObjectId.Parse(this.UserId),
                        LastUpdatedOn = DateTime.UtcNow
                    };

                    using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(_dbName))
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
                using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(_dbName))
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
                using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(_dbName))
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
                            CreatedById = meN.RecordCreatedBy.ToString()
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
                IMongoQuery mQuery = null;
                List<object> PatientNoteItems = new List<object>();

                mQuery = MongoDataUtil.ExpressionQueryBuilder(expression);

                //using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(_dbName))
                //{
                //}

                return new Tuple<string, IEnumerable<object>>(expression.ExpressionID, PatientNoteItems);
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
            try
            {
                throw new NotImplementedException();
                // code here //
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
                using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEPatientNote.PatientIdProperty, ObjectId.Parse(dataRequest.PatientId)));
                    queries.Add(Query.EQ(MEPatientNote.DeleteFlagProperty, false));
                    IMongoQuery mQuery = Query.And(queries);
                    List<MEPatientNote> meNotes = ctx.PatientNotes.Collection.Find(mQuery).OrderByDescending(o => o.RecordCreatedOn).Take(dataRequest.Count).ToList();
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
                                CreatedById = meN.RecordCreatedBy.ToString()
                            });
                        }
                    }
                }
                return noteDataList;
            }
            catch (Exception) { throw; }
        }

        public string UserId { get; set; }
    }
}
