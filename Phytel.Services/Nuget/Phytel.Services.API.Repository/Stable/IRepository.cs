using System;
using System.Collections.Generic;

namespace Phytel.Services.API.Repository
{
    public interface IRepository<T>
    {
        string UserId { get; set; }

        void CacheByID(List<string> entityIDs);

        void Delete(object entity);

        void DeleteAll(List<object> entities);

        object FindByID(string entityID);

        object Insert(object newEntity);

        object InsertAll(List<object> entities);

        Tuple<string, IEnumerable<object>> Select(APIExpression expression);

        IEnumerable<object> SelectAll();

        void UndoDelete(object entity);

        object Update(object entity);
    }

    public interface IRepository
    {
        string UserId { get; set; }

        void CacheByID(List<string> entityIDs);

        void Delete(object entity);

        void DeleteAll(List<object> entities);

        object FindByID(string entityID);

        object Insert(object newEntity);

        object InsertAll(List<object> entities);

        Tuple<string, IEnumerable<object>> Select(APIExpression expression);

        IEnumerable<object> SelectAll();

        void UndoDelete(object entity);

        object Update(object entity);
    }
}