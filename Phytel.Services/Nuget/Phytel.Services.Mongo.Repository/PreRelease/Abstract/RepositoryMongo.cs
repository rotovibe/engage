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

        public AggregateResult Aggregate<T, TKey>(params BsonDocument[] pipeline) where T : IMongoEntity<TKey>
        {
            return
                RetryHelper.DoWithRetry<AggregateResult>(() =>
                {
                    return _context.Set<T, TKey>().Collection.Aggregate(pipeline);
                },
                    RetryHelper.RETRIES,
                    RetryHelper.RETRYDELAY
                );
        }

        public BsonValue Eval(string code, params object[] args)
        {
            return _db.Eval(code, args);
        }

        public bool Exists<T, TKey>(Expression<Func<T, bool>> predicate) where T : IMongoEntity<TKey>
        {
            return
                RetryHelper.DoWithRetry<bool>(() =>
                {
                    return _context.Set<T, TKey>().Any(predicate);
                },
                    RetryHelper.RETRIES,
                    RetryHelper.RETRYDELAY
                );
        }

        public MongoCursor<T> Find<T, TKey>(IMongoQuery query) where T : class, IMongoEntity<TKey>
        {
            return
                RetryHelper.DoWithRetry<MongoCursor<T>>(() =>
                {
                    return _context.Set<T, TKey>().Collection.Find(query);
                },
                    RetryHelper.RETRIES,
                    RetryHelper.RETRYDELAY
                );
        }

        public T FindAndModify<T, TKey>(IMongoQuery query, IMongoSortBy sort, IMongoUpdate update) where T : class, IMongoEntity<TKey>
        {
            return
                RetryHelper.DoWithRetry<T>(() =>
                {
                    return _context.Set<T, TKey>().Collection.FindAndModify(query, sort, update).GetModifiedDocumentAs<T>();
                },
                    RetryHelper.RETRIES,
                    RetryHelper.RETRYDELAY
                );
        }

        public T First<T, TKey>(Expression<Func<T, bool>> predicate) where T : class, IMongoEntity<TKey>
        {
            return
                RetryHelper.DoWithRetry<T>(() =>
                {
                    return _context.Set<T, TKey>().FirstOrDefault(predicate);
                },
                    RetryHelper.RETRIES,
                    RetryHelper.RETRYDELAY
                );
        }

        public void Insert<T, TKey>(T entity) where T : IMongoEntity<TKey>
        {
            RetryHelper.DoWithRetry(() =>
            {
                _context.Set<T, TKey>().Insert(entity);
            },
                RetryHelper.RETRIES,
                RetryHelper.RETRYDELAY
            );
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

        public long Remove<T, TKey>(Expression<Func<T, bool>> predicate) where T : IMongoEntity<TKey>
        {
            long rvalue = default(long);

            RetryHelper.DoWithRetry(() =>
            {
                rvalue = _context.Set<T, TKey>().Remove(predicate);
            },
                RetryHelper.RETRIES,
                RetryHelper.RETRYDELAY
            );

            return rvalue;
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
            long rvalue = default(long);

            RetryHelper.DoWithRetry(() =>
            {
                rvalue = _context.Set<T, TKey>().Update(update, criteria);
            },
                RetryHelper.RETRIES,
                RetryHelper.RETRYDELAY
            );

            return rvalue;
        }

        public long Update<T, TKey, TMember>(Expression<Func<T, TMember>> propertySelector, TMember value, Expression<Func<T, bool>> criteria) where T : IMongoEntity<TKey>
        {
            long rvalue = default(long);

            RetryHelper.DoWithRetry(() =>
            {
                rvalue = _context.Set<T, TKey>().Update(propertySelector, value, criteria);
            },
                RetryHelper.RETRIES,
                RetryHelper.RETRYDELAY
            );

            return rvalue;
        }
    }
}