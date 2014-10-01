using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Phytel.Services.SQLServer.Repository
{
    public interface IRepositorySql
    {
        void Add<T>(T entity) where T : class;

        void Add<T>(IEnumerable<T> entities) where T : class;

        void Delete<T>(IEnumerable<T> entities) where T : class;

        void Delete<T>(Expression<Func<T, bool>> predicate) where T : class;

        void DeleteAll<T>() where T : class;

        void IdentityReseed(string tableName, int seedValue = 0);

        IQueryable<T> Query<T>(params Expression<Func<T, object>>[] includes) where T : class;

        IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class;

        T Single<T>(params object[] keyValues) where T : class;

        void Truncate(string tableName);
    }
}