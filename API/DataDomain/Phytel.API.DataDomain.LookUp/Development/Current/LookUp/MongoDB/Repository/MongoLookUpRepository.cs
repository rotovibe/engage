using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.AppDomain.LookUp;

namespace Phytel.API.DataDomain.LookUp
{
    public class MongoConditionRepository<T> : ILookUpRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoConditionRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public T Insert(T newEntity)
        {
            throw new NotImplementedException();
        }

        public T InsertAll(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            MECondition condition = null;
            using (ConditionMongoContext ctx = new ConditionMongoContext(_dbName))
            {
                condition = ctx.Conditions.Collection.FindOneById(ObjectId.Parse(entityID));
            }
            return condition;
        }

        public Tuple<int, IQueryable<T>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> SelectAll()
        {
            List<MECondition> conditions = new List<MECondition>();
            using (ConditionMongoContext ctx = new ConditionMongoContext(_dbName))
            {
                conditions = ctx.Conditions.Collection.FindAll().ToList();
            }
            return conditions as IQueryable<T>;
        }

        public T Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }
    }
}
