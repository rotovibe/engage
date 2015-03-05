using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Phytel.Services.SQLServer.Repository.PredicateBuilder
{
    class PredicateBuilderBoolean : IPredicateBuilder
    {
        public Expression<Func<T, bool>> BuildLambdaExpression<T>(PredicateFilter filterRow)
        {
            Expression<Func<T, bool>> lambda = null;
            PropertyInfo propertyInfo = typeof(T).GetProperty(filterRow.FilterField);

            if (propertyInfo != null && (typeof(bool) == propertyInfo.PropertyType || typeof(bool?) == propertyInfo.PropertyType))
            {

                ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
                var lambdaProperty = Expression.Property(parameter, filterRow.FilterField);

                switch (filterRow.FilterArgument)
                {
                    case FilterArgumentType.True:
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.Equal(lambdaProperty, GetConstantExpression(propertyInfo, true)), parameter);
                        break;

                    case FilterArgumentType.False:
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.Equal(lambdaProperty, GetConstantExpression(propertyInfo, false)), parameter);
                        break;

                    default:
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.Equal(lambdaProperty, GetConstantExpression(propertyInfo, true)), parameter);
                        break;
                }
            }

            return lambda;
        }

        private Expression GetConstantExpression(PropertyInfo propertyInfo, bool? value)
        {
            ConstantExpression constant;
            if (propertyInfo.PropertyType.Name.Contains("Nullable"))
            {
                constant = Expression.Constant(value, typeof(bool?));
            }
            else
            {
                constant = Expression.Constant(value, typeof(bool));
            }
            return constant;
        }
    }
}
