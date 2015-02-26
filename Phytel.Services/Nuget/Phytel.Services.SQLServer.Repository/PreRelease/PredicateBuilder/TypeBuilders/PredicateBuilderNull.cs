using System;
using System.Linq.Expressions;

namespace Phytel.Services.SQLServer.Repository.PredicateBuilder
{
    class PredicateBuilderNull : IPredicateBuilder
    {
        public Expression<Func<T, bool>> BuildLambdaExpression<T>(PredicateFilter filterRow)
        {
            Expression<Func<T, bool>> lambda = null;

            return lambda;
        }
    }
}
