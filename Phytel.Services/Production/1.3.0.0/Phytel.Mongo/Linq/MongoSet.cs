using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using System.Reflection;

namespace Phytel.Mongo.Linq
{
    /// <summary>
    /// MongoCollection<typeparamref name="T"/> wrapper to allow Linq and expression based access to
    /// the collection without the need to switch between MongoQuery and Linq query operators.
    /// </summary>
    /// <typeparam name="T">Class implementing IMongoEntity</typeparam>
    public class MongoSet<TEntity, TKey> : IQueryable<TEntity>, IEnumerable<TEntity> where TEntity : IMongoEntity<TKey>
    {
        private string _collectionName;
        private MongoContext _context;

        public MongoSet(MongoContext context)
        {
            object[] mongoDbAttr = typeof(TEntity).GetCustomAttributes(typeof(MongoCollectionAttribute), false);

            if (mongoDbAttr.HasItems())
            {
                _collectionName = ((MongoCollectionAttribute)mongoDbAttr[0]).CollectionName;
            }
            else
            {
                _collectionName = typeof(TEntity).Name;
            }

            this._context = context;

            if (context.WriteConcern != null)
            {
                WriteConcern = context.WriteConcern;
            }
            else
            {
                WriteConcern = WriteConcern.Acknowledged;
            }

            EnsureIndexes();
        }

        public MongoSet(MongoContext context, WriteConcern writeConcern) : this(context)
        {
            WriteConcern = writeConcern;
        }

        public MongoSet(MongoContext context, string collectionName)
        {
            _collectionName = collectionName;
            _context = context;

            EnsureIndexes();
        }

        public MongoSet(MongoContext context, string collectionName, WriteConcern writeConcern)
            : this(context, collectionName)
        {
            WriteConcern = writeConcern;
        }

        private WriteConcern writeConcern;

        /// <summary>
        /// WriteConcern used for all writes on this MongoSet.
        /// </summary>
        /// <remarks>
        /// Defaults to acknowledged.
        /// </remarks>
        public WriteConcern WriteConcern 
        { 
            get
            {
                return writeConcern;
            }
            
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("WriteConcern cannot be set to null");
                }

                writeConcern = value;
            }
        }

        /// <summary>
        /// Underlying MongoCollection for this MongoSet
        /// </summary>
        public MongoCollection<TEntity> Collection
        {
            get
            {
                return _context.GetDatabase().GetCollection<TEntity>(_collectionName);
            }
        }

        #region IEnumerable methods

        public IEnumerator<TEntity> GetEnumerator()
        {
            return Collection.FindAllAs<TEntity>().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IQueryAble methods

        public Type ElementType
        {
            get
            {
                return typeof(TEntity);
            }
        }

        public Expression Expression
        {
            get 
            {
                return Collection.AsQueryable().Expression;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return Collection.AsQueryable().Provider;
            }
        }

        #endregion

        #region Public method

        /// <summary>
        /// Inserts a new item
        /// </summary>
        /// <param name="item">The item to insert</param>
        /// <exception cref="MongoDB.Drive.MongoException" />
        public void Insert(TEntity item)
        {
            var result = Collection.Insert<TEntity>(item, WriteConcern);

            CheckResult(result);
        }

        /// <summary>
        /// Inserts an IEnumerable of items in a batch
        /// </summary>
        /// <param name="items">The items to insert</param>
        public void InsertBatch(IEnumerable<TEntity> items)
        {
            var results = Collection.InsertBatch<TEntity>(items, WriteConcern);

            foreach (var result in results)
            {
                CheckResult(result);
            }
        }

        /// <summary>
        /// Saves the item 
        /// </summary>
        /// <param name="item">The item to save</param>
        public void Save(TEntity item)
        {
            var result = Collection.Save<TEntity>(item, WriteConcern);

            CheckResult(result);
        }

        /// <summary>
        /// Remove the item
        /// </summary>
        /// <param name="item">The item to remove</param>
        /// <returns>The number of records affected. If WriteConcern is unacknowledged -1 is returned</returns>
        public long Remove(TEntity item)
        {
            WriteConcernResult result = Collection.Remove(Query.EQ("_id", BsonValue.Create(item.Id)), WriteConcern);

            return CheckResult(result);
        }

        /// <summary>
        /// Remove the item/s matching the criteria
        /// </summary>
        /// <param name="criteria">criteria expression</param>
        /// <returns>The number of records affected. If WriteConcern is unacknowledged -1 is returned</returns>
        public long Remove(Expression<Func<TEntity, bool>> criteria)
        {
            var queryable = Collection.AsQueryable().Where(criteria);
            var mongoQuery = ((MongoQueryProvider)Provider).BuildMongoQuery((MongoQueryable<TEntity>)queryable);
            var result = Collection.Remove(mongoQuery, WriteConcern);

            return CheckResult(result);
        }

        /// <summary>
        /// Removes all items
        /// </summary>
        /// <returns>The number of records affected. If WriteConcern is unacknowledged -1 is returned</returns>
        /// <remarks>Careful this deletes everything in the MongoSet/Collection!</remarks>
        public long RemoveAll()
        {
            var result = Collection.RemoveAll(WriteConcern);

            return CheckResult(result);
        }

        /// <summary>
        /// Update one property of an object.
        /// </summary>
        /// <typeparam name="TMember">The type of the property to be updated</typeparam>
        /// <param name="propertySelector">The property selector expression</param>
        /// <param name="value">New value of the property</param>
        /// <param name="criteria">Criteria to update documents based on</param>
        /// <returns>The number of records affected. If WriteConcern is unacknowledged -1 is returned</returns>
        public long Update<TMember>(Expression<Func<TEntity, TMember>> propertySelector, TMember value, Expression<Func<TEntity, bool>> criteria)
        {
            UpdateBuilder<TEntity> updBuilder = new UpdateBuilder<TEntity>();
            updBuilder.Set<TMember>(propertySelector, value);

            return Update(updBuilder, criteria);
        }

        /// <summary>
        /// Update with UpdateBuilder. Use MongoSet<typeparamref name"T"/>.Set().Set()... to build update
        /// </summary>
        /// <param name="update">The update object</param>
        /// <param name="criteria">Criteria to update documents based on</param>
        /// <returns></returns>
        public long Update(UpdateBuilder<TEntity> update, Expression<Func<TEntity, bool>> criteria)
        {
            var queryable = Collection.AsQueryable().Where(criteria);
            var mongoQuery = ((MongoQueryProvider)Provider).BuildMongoQuery((MongoQueryable<TEntity>)queryable);
            
            UpdateFlags flags = UpdateFlags.Multi;
            var result = Collection.Update(mongoQuery, update, flags, WriteConcern);

            return CheckResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMember">The type of the property to update</typeparam>
        /// <param name="propertySelector">Property selector for the property to update</param>
        /// <param name="value">Value to set the property to</param>
        /// <returns>The number of records affected. If WriteConcern is unacknowledged -1 is returned</returns>
        public UpdateBuilder<TEntity> Set<TMember>(Expression<Func<TEntity, TMember>> propertySelector, TMember value)
        {
            return MongoDB.Driver.Builders.Update<TEntity>.Set<TMember>(propertySelector, value);
        }

        public bool Contains(TEntity item)
        {
            var result = Collection.FindOneByIdAs<TEntity>(BsonValue.Create(item.Id));
            return result != null;
        }

        #endregion

        #region private methods

        private long CheckResult(WriteConcernResult result)
        {
            if (result == null)
            {
                return -1;
            }

            if (!result.Ok)
            {
                throw new MongoException(result.ErrorMessage);
            }

            return result.DocumentsAffected;
        }

        private void EnsureIndexes()
        {
            object[] attributes = typeof(TEntity).GetCustomAttributes(typeof(MongoIndexAttribute), false);

            if (attributes.HasItems())
            {
                foreach (object attr in attributes)
                {
                    MongoIndexAttribute indexAttr = (MongoIndexAttribute)attr;
                    IndexKeysBuilder keybuilder = new IndexKeysBuilder();
                    IndexOptionsBuilder optionsBuilder = new IndexOptionsBuilder();

                    if (indexAttr.Unique)
                    {
                        optionsBuilder.SetUnique(indexAttr.Unique);
                    }

                    if (indexAttr.TimeToLive > -1)
                    {
                        TimeSpan ttl = new TimeSpan(0, 0, indexAttr.TimeToLive);
                        optionsBuilder.SetTimeToLive(ttl);
                    }

                    Collection.EnsureIndex(
                            indexAttr.Descending ? keybuilder.Descending(indexAttr.Keys) : keybuilder.Ascending(indexAttr.Keys),
                            optionsBuilder);
                }
            }
        }

        #endregion
    }
}
