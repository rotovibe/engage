using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.Services.Mongo.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Phytel.Services.Mongo.Repository
{
    public interface IRepositoryMongo
    {
        AggregateResult Aggregate<T, TKey>(params BsonDocument[] pipeline) where T : IMongoEntity<TKey>;

        BsonValue Eval(string code, params object[] args);

        bool Exists<T, TKey>(Expression<Func<T, bool>> predicate) where T : IMongoEntity<TKey>;

        MongoCursor<T> Find<T, TKey>(IMongoQuery query) where T : class, IMongoEntity<TKey>;

        T FindAndModify<T, TKey>(IMongoQuery query, IMongoSortBy sort, IMongoUpdate update) where T : class, IMongoEntity<TKey>;

        T FindAndModify<T, TKey>(FindAndModifyArgs args) where T : class, IMongoEntity<TKey>;

        List<T> FindAsList<T, TKey>(IMongoQuery query) where T : class, IMongoEntity<TKey>;

        T First<T, TKey>(Expression<Func<T, bool>> predicate) where T : class, IMongoEntity<TKey>;

        void Insert<T, TKey>(T entity) where T : IMongoEntity<TKey>;

        void InsertBatch<T, TKey>(IEnumerable<T> entities) where T : IMongoEntity<TKey>;

        IQueryable<T> Query<T, TKey>() where T : IMongoEntity<TKey>;

        IQueryable<T> Query<T, TKey>(Expression<Func<T, bool>> predicate) where T : IMongoEntity<TKey>;

        long Remove<T, TKey>(Expression<Func<T, bool>> predicate) where T : IMongoEntity<TKey>;

        WriteConcernResult Remove<T, TKey>(IMongoQuery query) where T : IMongoEntity<TKey>;

        void Save<T, TKey>(T entity) where T : IMongoEntity<TKey>;

        long Update<T, TKey>(UpdateBuilder<T> update, Expression<Func<T, bool>> criteria) where T : IMongoEntity<TKey>;

        long Update<T, TKey, TMember>(Expression<Func<T, TMember>> propertySelector, TMember value, Expression<Func<T, bool>> criteria) where T : IMongoEntity<TKey>;
    }
}