using System;
using System.Linq.Expressions;

namespace Phytel.Services.SQLServer.Repository.PredicateBuilder
{
    public interface IPredicateBuilder
    {
        Expression<Func<T, bool>> BuildLambdaExpression<T>(PredicateFilter filterRow);
    }
}
