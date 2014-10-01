using EntityFramework.BulkInsert.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Phytel.Services.SQLServer.Repository
{
    public class RepositorySql<TContext> : IRepositorySql
        where TContext : DbContext
    {
        public const string SqlFormatIdentityReseed = "DBCC CHECKIDENT ('{0}', RESEED, {1})";
        public const string SqlFormatTruncate = "TRUNCATE TABLE {0}";

        protected readonly TContext _dbContext;
        protected readonly IUnitOfWork<TContext> _uow;

        public RepositorySql(IUnitOfWork<TContext> uow)
        {
            _uow = uow;
            _dbContext = uow.DbContext;
        }

        public virtual void Add<T>(IEnumerable<T> entities) where T : class
        {
            _dbContext.BulkInsert<T>(entities);
        }

        public virtual void Add<T>(T entity) where T : class
        {
            _dbContext.Set<T>().Add(entity);
        }

        public virtual void Delete<T>(IEnumerable<T> entities) where T : class
        {
            _dbContext.Set<T>().RemoveRange(entities);
        }

        public virtual void Delete<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            IEnumerable<T> entitiesToDelete = Query<T>(predicate: predicate);

            Delete<T>(entitiesToDelete);
        }

        public virtual void DeleteAll<T>() where T : class
        {
            IEnumerable<T> entitesToDelete = Query<T>();

            Delete<T>(entitesToDelete);
        }

        public virtual void IdentityReseed(string tableName, int seedValue = 0)
        {
            string sql = string.Format(SqlFormatIdentityReseed, tableName, seedValue.ToString());
            _dbContext.Database.ExecuteSqlCommand(sql);
        }

        public virtual IQueryable<T> Query<T>(params Expression<Func<T, object>>[] includes) where T : class
        {
            return Includes<T>(includes);
        }

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            return Query<T>(includes).Where(predicate);
        }

        public virtual T Single<T>(params object[] keyValues) where T : class
        {
            return _dbContext.Set<T>().Find(keyValues);
        }

        public virtual void Truncate(string tableName)
        {
            string sql = string.Format(SqlFormatTruncate, tableName);
            _dbContext.Database.ExecuteSqlCommand(sql);
        }

        protected virtual IQueryable<T> Includes<T>(Expression<Func<T, object>>[] includes)
            where T : class
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (includes != null)
            {
                foreach (Expression<Func<T, object>> include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query;
        }
    }
}