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
            bool isInserted = false;
            try
            {
                MEPatientNote meN = new MEPatientNote
                {
                    Id = ObjectId.GenerateNewId(),
                    PatientId = ObjectId.Parse(noteData.PatientId),
                    Text = noteData.Text,
                    Programs = Helper.ConvertToObjectIdList(noteData.ProgramIds),
                    CreatedBy = ObjectId.Parse(noteData.CreatedBy),
                    CreatedOn = DateTime.UtcNow,
                    Version = request.Version,
                    UpdatedBy = request.UserId,
                    LastUpdatedOn = DateTime.UtcNow
                };

                using (PatientNoteMongoContext ctx = new PatientNoteMongoContext(_dbName))
                {
                    WriteConcernResult wcr = ctx.PatientNotes.Collection.Insert(meN);
                    if (wcr.Ok)
                    {
                        isInserted = true;
                    }
                }
                return isInserted;
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
            try
            {
                throw new NotImplementedException();
                // code here //
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
            try
            {
                throw new NotImplementedException();
                // code here //
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
    }
}
