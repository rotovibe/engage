using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.Services.Mongo.Linq;
using Phytel.Services.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Phytel.Services.Mongo.Repository
{
    public class RepositoryMongo<TContext> : IRepositoryMongo
        where TContext : MongoContext
    {
        protected readonly TContext _context;
        protected readonly MongoDatabase _db;

        public RepositoryMongo(IUnitOfWorkMongo<TContext> uow)
        {
            _context = uow.MongoContext;
        }

        public bool Exists<T, TKey>(Expression<Func<T, bool>> predicate) where T : IMongoEntity<TKey>
        {
            return _context.Set<T, TKey>().Any(predicate);
        }

        public T First<T, TKey>(Expression<Func<T, bool>> predicate) where T : class, IMongoEntity<TKey>
        {
            return _context.Set<T, TKey>().FirstOrDefault(predicate);
        }

        public void Insert<T, TKey>(T entity) where T : IMongoEntity<TKey>
        {
            _context.Set<T, TKey>().Insert(entity);
        }

        public void InsertBatch<T, TKey>(IEnumerable<T> entities) where T : IMongoEntity<TKey>
        {
            RetryHelper.DoWithRetry(() => 
            {
                _context.Set<T, TKey>().InsertBatch(entities);
            }, 
                RetryHelper.RETRIES, 
                RetryHelper.RETRYDELAY
            );
        }

        public IQueryable<T> Query<T, TKey>() where T : IMongoEntity<TKey>
        {
            return _context.Set<T, TKey>();
        }

        public IQueryable<T> Query<T, TKey>(Expression<Func<T, bool>> predicate) where T : IMongoEntity<TKey>
        {
            return _context.Set<T, TKey>().Where(predicate);
        }

        public void Save<T, TKey>(T entity) where T : IMongoEntity<TKey>
        {
            RetryHelper.DoWithRetry(() =>
            {
                _context.Set<T, TKey>().Save(entity);
            },
                RetryHelper.RETRIES,
                RetryHelper.RETRYDELAY
            );
        }

        public long Update<T, TKey>(UpdateBuilder<T> update, Expression<Func<T, bool>> criteria) where T : IMongoEntity<TKey>
        {
            return _context.Set<T, TKey>().Update(update, criteria);
        }

        public long Update<T, TKey, TMember>(Expression<Func<T, TMember>> propertySelector, TMember value, Expression<Func<T, bool>> criteria) where T : IMongoEntity<TKey>
        {
            return _context.Set<T, TKey>().Update(propertySelector, value, criteria);
        }

        public BsonValue Eval(string code, params object[] args)
        {
            return _db.Eval(code, args);
        }

        public long Remove<T, TKey>(Expression<Func<T, bool>> predicate) where T : IMongoEntity<TKey>
        {
            return _context.Set<T, TKey>().Remove(predicate);
        }

        public AggregateResult Aggregate<T, TKey>(params BsonDocument[] pipeline) where T : IMongoEntity<TKey>
        {
            return _context.Set<T, TKey>().Collection.Aggregate(pipeline);
        }
    }
}