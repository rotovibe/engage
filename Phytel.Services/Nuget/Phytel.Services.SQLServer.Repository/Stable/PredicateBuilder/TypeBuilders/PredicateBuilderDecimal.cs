using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Phytel.Services.SQLServer.Repository.PredicateBuilder
{
    class PredicateBuilderDecimal : IPredicateBuilder
    {
        public Expression<Func<T, bool>> BuildLambdaExpression<T>(PredicateFilter filterRow)
        {
            List<FilterValue> filterValues = filterRow.FilterValues;

            decimal primaryValue = 0;
            decimal secondaryValue = 0;
            bool validSecondaryValue = false;

            if (filterValues != null)
            {
                if (filterValues.Count > 0)
                {
                    decimal.TryParse(filterValues[0].Value.Trim(), out primaryValue);
                }

                if (filterValues.Count > 1)
                {
                    if (decimal.TryParse(filterValues[1].Value.Trim(), out secondaryValue))
                    {
                        validSecondaryValue = true;
                    }
                }
            }

            Expression<Func<T, bool>> lambda = null;
            PropertyInfo propertyInfo = typeof(T).GetProperty(filterRow.FilterField);

            if (propertyInfo != null && (typeof(decimal) == propertyInfo.PropertyType || typeof(decimal?) == propertyInfo.PropertyType))
            {
                ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
                MemberExpression member = Expression.MakeMemberAccess(parameter, propertyInfo);
                Expression lambdaProperty = Expression.Property(parameter, filterRow.FilterField);

                switch (filterRow.FilterArgument)
                {
                    case FilterArgumentType.IsEqualTo:
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.Equal(lambdaProperty, GetConstantExpression(propertyInfo, primaryValue)), parameter);
                        break;

                    case FilterArgumentType.IsNotEqualTo:
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.NotEqual(lambdaProperty, GetConstantExpression(propertyInfo, primaryValue)), parameter);
                        break;

                    case FilterArgumentType.Between:
                        if (validSecondaryValue)
                        {
                            Expression<Func<T, bool>> e1 = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(lambdaProperty, GetConstantExpression(propertyInfo, primaryValue)), parameter);
                            Expression<Func<T, bool>> e2 = Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(lambdaProperty, GetConstantExpression(propertyInfo, secondaryValue)), parameter);
                            lambda = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(e1.Body, e2.Body), e1.Parameters.Single());
                        }
                        break;

                    case FilterArgumentType.GreaterThan:
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(lambdaProperty, GetConstantExpression(propertyInfo, primaryValue)), parameter);
                        break;

                    case FilterArgumentType.GreaterThanOrEqualTo:
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(lambdaProperty, GetConstantExpression(propertyInfo, primaryValue)), parameter);
                        break;

                    case FilterArgumentType.LessThan:
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.LessThan(lambdaProperty, GetConstantExpression(propertyInfo, primaryValue)), parameter);
                        break;

                    case FilterArgumentType.LessThanOrEqualTo:
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(lambdaProperty, GetConstantExpression(propertyInfo, primaryValue)), parameter);
                        break;

                    case FilterArgumentType.IsNull:
                        if (propertyInfo.PropertyType.Name.Contains("Nullable"))
                        {
                            Expression leftNull = Expression.Property(parameter, typeof(T).GetProperty(filterRow.FilterField));
                            Expression rightNull = Expression.Constant(null, typeof(decimal?));
                            Expression InnerLambdaNull = Expression.Equal(leftNull, rightNull);
                            lambda = Expression.Lambda<Func<T, bool>>(InnerLambdaNull, parameter);
                        }
                        else
                        {
                            // Need a valid lambda expression (not null) for a non-nullable decimal that will always return 0 records - chose decimal.MinValue but may need refactoring 
                            lambda = Expression.Lambda<Func<T, bool>>(Expression.Equal(lambdaProperty, GetConstantExpression(propertyInfo, decimal.MinValue)), parameter);
                        }
                        break;

                    case FilterArgumentType.IsNotNull:
                        if (propertyInfo.PropertyType.Name.Contains("Nullable"))
                        {
                            Expression leftNotNull = Expression.Property(parameter, typeof(T).GetProperty(filterRow.FilterField));
                            Expression rightNotNull = Expression.Constant(null, typeof(decimal?));
                            Expression InnerLambdaNotNull = Expression.NotEqual(leftNotNull, rightNotNull);
                            lambda = Expression.Lambda<Func<T, bool>>(InnerLambdaNotNull, parameter);
                        }
                        else
                        {
                            // Need a valid lambda expression (not null) for a non-nullable decimal that will always return ALL records - chose !decimal.MinValue but may need refactoring 
                            lambda = Expression.Lambda<Func<T, bool>>(Expression.NotEqual(lambdaProperty, GetConstantExpression(propertyInfo, decimal.MinValue)), parameter);
                        }
                        break;

                    case FilterArgumentType.IsEmpty:
                        if (propertyInfo.PropertyType.Name.Contains("Nullable"))
                        {
                            Expression leftEmpty = Expression.Property(parameter, typeof(T).GetProperty(filterRow.FilterField));
                            Expression rightEmpty = Expression.Constant(null, typeof(decimal?));
                            Expression InnerLambdaEmpty = Expression.Equal(leftEmpty, rightEmpty);
                            lambda = Expression.Lambda<Func<T, bool>>(InnerLambdaEmpty, parameter);
                        }
                        else
                        {
                            // Need a valid lambda expression (not null) for a non-nullable decimal that will always return 0 records - chose decimal.MinValue but may need refactoring 
                            lambda = Expression.Lambda<Func<T, bool>>(Expression.Equal(lambdaProperty, GetConstantExpression(propertyInfo, decimal.MinValue)), parameter);
                        }
                        break;

                    case FilterArgumentType.IsNotEmpty:
                        if (propertyInfo.PropertyType.Name.Contains("Nullable"))
                        {
                            Expression leftNotEmpty = Expression.Property(parameter, typeof(T).GetProperty(filterRow.FilterField));
                            Expression rightNotEmpty = Expression.Constant(null, typeof(decimal?));
                            Expression InnerLambdaNotEmpty = Expression.NotEqual(leftNotEmpty, rightNotEmpty);
                            lambda = Expression.Lambda<Func<T, bool>>(InnerLambdaNotEmpty, parameter);
                        }
                        else
                        {
                            // Need a valid lambda expression (not null) for a non-nullable decimal that will always return ALL records - chose !decimal.MinValue but may need refactoring 
                            lambda = Expression.Lambda<Func<T, bool>>(Expression.NotEqual(lambdaProperty, GetConstantExpression(propertyInfo, decimal.MinValue)), parameter);
                        }
                        break;

                    default:
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.Equal(lambdaProperty, GetConstantExpression(propertyInfo, primaryValue)), parameter);
                        break;
                }
            }

            return lambda;
        }

        private Expression GetConstantExpression(PropertyInfo propertyInfo, decimal? value)
        {
            ConstantExpression constant;
            if (propertyInfo.PropertyType.Name.Contains("Nullable"))
            {
                constant = Expression.Constant(value, typeof(decimal?));
            }
            else
            {
                constant = Expression.Constant(value, typeof(decimal));
            }
            return constant;
        }
    }
}
