using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Phytel.API.Interface
{
    public interface IRepository<T>
    {
        T Insert(T newEntity);
        T InsertAll(List<T> entities);
        void Delete(T entity);
        void DeleteAll(List<T> entities);
        object FindByID(string entityID);
        IQueryable<T> Select(Expression<Func<T, bool>> predicate);
        IQueryable<T> SelectAll();
        T Update(T entity);
        void CacheByID(List<string> entityIDs);
    }
}
