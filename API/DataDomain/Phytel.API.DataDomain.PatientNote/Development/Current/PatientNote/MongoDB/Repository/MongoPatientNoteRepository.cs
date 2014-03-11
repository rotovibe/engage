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
            try
            {
                if(noteData != null)
                {
                    MEPatientNote meN = new MEPatientNote
                    {
                        Id = ObjectId.GenerateNewId(),
                        PatientId = ObjectId.Parse(noteData.PatientId),
                        Text = noteData.Text,
                        Programs = Helper.ConvertToObjectIdList(noteData.ProgramIds),
                        CreatedOn = DateTime.UtcNow,
                        Version = request.Version,
                        UpdatedBy = ObjectId.Parse(request.UserId),
                        LastUpdatedOn = DateTime.UtcNow
                    };
                    if (!string.IsNullOrEmpty(noteData.CreatedById))
                    {
                        meN.CreatedBy = ObjectId.Parse(noteData.CreatedById);
                    }
                    using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(_dbName))
                    {
                        WriteConcernResult wcr = ctx.PatientNotes.Collection.Insert(meN);
                        if (wcr.Ok)
                        {
                            noteId = meN.Id.ToString();
                        }
                    }
                }
                return noteId;
            }
            catch (Exception ex) { throw ex; }
        }

        public object InsertAll(List<object> entities)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
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
                    uv.Add(MB.Update.Set(MEPatientNote.UpdatedByProperty, request.UserId));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientNotes.Collection.Update(q, update);
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public void DeleteAll(List<object> entities)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
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
                            ProgramIds = Helper.ConvertToStringList(meN.Programs),
                            CreatedOn = meN.CreatedOn,
                            CreatedById = (meN.CreatedBy == null) ? string.Empty : meN.CreatedBy.ToString()
                        };
                    }
                }
                return noteData;
            }
            catch (Exception ex) { throw ex; }
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
            catch (Exception ex) { throw ex; }
        }

        public IEnumerable<object> SelectAll()
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public object Update(object entity)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public void CacheByID(List<string> entityIDs)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
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
                    List<MEPatientNote> meNotes = ctx.PatientNotes.Collection.Find(mQuery).OrderByDescending(o => o.CreatedOn).Take(dataRequest.Count).ToList();
                    if (meNotes != null && meNotes.Count > 0)
                    {
                        noteDataList = new List<PatientNoteData>();
                        foreach(MEPatientNote meN in meNotes)
                        {
                            noteDataList.Add(new PatientNoteData {
                                Id = meN.Id.ToString(),
                                PatientId = meN.PatientId.ToString(),
                                Text = meN.Text,
                                ProgramIds = Helper.ConvertToStringList(meN.Programs),
                                CreatedOn = meN.CreatedOn,
                                CreatedById = (meN.CreatedBy == null) ? string.Empty : meN.CreatedBy.ToString()
                            });
                        }
                    }
                }
                return noteDataList;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
