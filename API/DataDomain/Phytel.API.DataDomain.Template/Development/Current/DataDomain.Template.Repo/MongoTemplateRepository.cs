
using Phytel.API.Common.Data;
using System;
using System.Collections.Generic;
using Phytel.API.DataDomain.Template;
using Phytel.Repository;

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
            throw new NotImplementedException();
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
