
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using DTO = Phytel.API.DataDomain.Template.DTO;
using Phytel.API.DataDomain.Template.DTO;

namespace DataDomain.Template.Repo
{
    public class MongoTemplateRepository<TContext> : IMongoTemplateRepository where TContext : TemplateMongoContext
    {
        protected readonly TContext Context;
        public string UserId { get; set; }

        public MongoTemplateRepository(IUOWMongo<TContext> uow)
        {
            Context = uow.MongoContext;
        }

        public MongoTemplateRepository(TContext context)
        {
            Context = context;
        }

        public object Insert(object newEntity)
        {
            throw new NotImplementedException();
        }

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
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

                var findcp = Query.And(
                    Query<METemplate>.EQ(b => b.Id, ObjectId.Parse(entityID)),
                    Query<METemplate>.EQ(b => b.DeleteFlag, false));

                var cp = Context.Templates.Collection.Find(findcp).FirstOrDefault();

                if (cp == null) return result;

                result = new DTO.Template
                {
                    Id = cp.Id.ToString(),
                    DeleteFlag = cp.DeleteFlag,
                    LastUpdatedOn = cp.LastUpdatedOn,
                    RecordCreatedBy = cp.RecordCreatedBy.ToString(),
                    RecordCreatedOn = cp.RecordCreatedOn,
                    TtlDate = cp.TTLDate,
                    UpdatedBy = cp.UpdatedBy.ToString(),
                    Version = cp.Version
                };
                
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PatientProgramRepository:FindByID()::" + ex.Message, ex.InnerException);
            }
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
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }


        public void UndoDelete(object entity)
        {
            throw new NotImplementedException();
        }
    }
}
