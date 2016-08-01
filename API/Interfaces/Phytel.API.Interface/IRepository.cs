using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Phytel.API.Interface
{
    public interface IRepository<T>
    {
        string UserId { get; set; }

        object Insert(object newEntity);
        object InsertAll(List<object> entities);
        void Delete(object entity);
        void DeleteAll(List<object> entities);
        object FindByID(string entityID);
        Tuple<string, IEnumerable<object>> Select(APIExpression expression);
        IEnumerable<object> SelectAll();
        object Update(object entity);
        void CacheByID(List<string> entityIDs);
        void UndoDelete(object entity);
    }

    public interface IRepository
    {
        string UserId { get; set; }
        object Insert(object newEntity);
        object InsertAll(List<object> entities);
        void Delete(object entity);
        void DeleteAll(List<object> entities);
        object FindByID(string entityID);
        Tuple<string, IEnumerable<object>> Select(APIExpression expression);
        IEnumerable<object> SelectAll();
        object Update(object entity);
        void CacheByID(List<string> entityIDs);
        void UndoDelete(object entity);
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
        OR,
        EQ
    }
}