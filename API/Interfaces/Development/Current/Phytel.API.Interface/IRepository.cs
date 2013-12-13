using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Phytel.API.Interface
{
    public interface IRepository<T>
    {
        object Insert(T newEntity);
        object InsertAll(List<T> entities);
        void Delete(T entity);
        void DeleteAll(List<T> entities);
        object FindByID(string entityID);
        Tuple<string, IQueryable<object>> Select(APIExpression expression);
        IQueryable<object> SelectAll();
        object Update(T entity);
        void CacheByID(List<string> entityIDs);
    }

    public class APIExpression
    {
        public ICollection<SelectExpression> Expressions { get; set; }
        public string ExpressionID { get; set; }
        public int skip { get; set; }
        public int take { get; set; }
    }

    public struct SelectExpression
    {
        public string FieldName { get; set; }
        public SelectExpressionType Type { get; set; }
        public object Value { get; set; }
        public SelectExpressionGroupType NextExpressionType { get; set; }
        public int GroupID { get; set; }
        public int ExpressionOrder { get; set; }
    }

    public enum SelectExpressionType
    {
        EQ,
        NOTEQ,
        LIKE,
        NOTLIKE,
        STARTWITH,
        IN
    }

    public enum SelectExpressionGroupType
    {
        AND,
        OR
    }
}