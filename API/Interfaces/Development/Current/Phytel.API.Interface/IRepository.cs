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
        Tuple<int, IQueryable<T>> Select(APIExpression expression);
        IQueryable<T> SelectAll();
        T Update(T entity);
        void CacheByID(List<string> entityIDs);
    }

    public class APIExpression
    {
        ICollection<SelectExpression> Expressions { get; set; }
        string ExpressionID { get; set; }
        int skip { get; set; }
        int take { get; set; }
    }

    public struct SelectExpression
    {
        public string FieldName { get; set; }
        public SelectExpressionType Type { get; set; }
        public string Value { get; set; }
        public SelectExpressionGroupType NextExpressionType { get; set; }
        public int GroupID { get; set; }
        public int ExpressionOrder { get; set; }
    }

    public enum SelectExpressionType
    {
        EQ,
        NOTEQ,
        LIKE,
        NOTLIKE
    }

    public enum SelectExpressionGroupType
    {
        AND,
        OR
    }
}