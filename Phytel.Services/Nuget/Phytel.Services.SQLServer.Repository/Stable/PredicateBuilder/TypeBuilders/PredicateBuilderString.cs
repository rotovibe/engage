using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Phytel.Services.SQLServer.Repository.PredicateBuilder
{
    class PredicateBuilderString : IPredicateBuilder
    {
        public Expression<Func<T, bool>> BuildLambdaExpression<T>(PredicateFilter filterRow)
        {
            List<FilterValue> filterValues = filterRow.FilterValues;

            string value = string.Empty;

            if (filterValues != null)
            {
                if (filterValues.Count > 0)
                {
                    if (filterValues[0].Value != null && filterValues[0].Value.Trim().Length != 0)
                    {
                        value = filterValues[0].Value.Replace(";", string.Empty).Trim();
                    }
                }
            }

            if (value == string.Empty && filterRow.FilterArgument != FilterArgumentType.IsNotEmpty)
            {
                filterRow.FilterArgument = FilterArgumentType.IsEmpty;
            }

            Expression<Func<T, bool>> lambda = null;
            PropertyInfo propertyInfo = typeof(T).GetProperty(filterRow.FilterField);

            if (propertyInfo != null && (typeof(string) == propertyInfo.PropertyType))
            {
                ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
                MemberExpression member = Expression.MakeMemberAccess(parameter, propertyInfo);
                ConstantExpression constant = Expression.Constant(value, typeof(string));
                ConstantExpression constantEmpty = Expression.Constant(string.Empty, typeof(string));
                ConstantExpression constantNull = Expression.Constant(null, typeof(string));

                var lambdaProperty = Expression.Property(parameter, filterRow.FilterField);

                Expression lambdaEqual = Expression.Equal(lambdaProperty, constant);
                Expression lambdaNotEqual = Expression.NotEqual(lambdaProperty, constant);

                switch (filterRow.FilterArgument)
                {
                    case FilterArgumentType.IsEqualTo:
                        lambda = Expression.Lambda<Func<T, bool>>(lambdaEqual, parameter);
                        break;

                    case FilterArgumentType.IsNotEqualTo:
                        Expression<Func<T, bool>> e1IsNotEqualTo = Expression.Lambda<Func<T, bool>>(lambdaNotEqual, parameter);

                        Expression<Func<T, bool>> isNotEqualToIsEmpty = Expression.Lambda<Func<T, bool>>(Expression.Equal(lambdaProperty, constantEmpty), parameter);
                        Expression<Func<T, bool>> isNotEqualToIsNull = Expression.Lambda<Func<T, bool>>(Expression.Equal(lambdaProperty, constantNull), parameter);
                        Expression<Func<T, bool>> e2IsNotEqualTo = Expression.Lambda<Func<T, bool>>(Expression.Or(isNotEqualToIsEmpty.Body, isNotEqualToIsNull.Body), isNotEqualToIsEmpty.Parameters.Single());

                        lambda = Expression.Lambda<Func<T, bool>>(Expression.Or(e1IsNotEqualTo.Body, e2IsNotEqualTo.Body), e1IsNotEqualTo.Parameters.Single());
                        break;

                    case FilterArgumentType.Contains:
                        MethodInfo methodInfoContains = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
                        Expression callContains = Expression.Call(member, methodInfoContains, constant);
                        lambda = Expression.Lambda<Func<T, bool>>(callContains, parameter);
                        break;

                    case FilterArgumentType.DoesNotContain:
                        MethodInfo methodInfoDoesNotContain = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
                        Expression callDoesNotContain = Expression.Call(member, methodInfoDoesNotContain, constant);
                        Expression<Func<T, bool>> e1DoesNotContain = Expression.Lambda<Func<T, bool>>(Expression.Not(callDoesNotContain), parameter);

                        Expression<Func<T, bool>> doesNotContainIsEmpty = Expression.Lambda<Func<T, bool>>(Expression.Equal(lambdaProperty, constantEmpty), parameter);
                        Expression<Func<T, bool>> doesNotContainIsNull = Expression.Lambda<Func<T, bool>>(Expression.Equal(lambdaProperty, constantNull), parameter);
                        Expression<Func<T, bool>> e2DoesNotContain = Expression.Lambda<Func<T, bool>>(Expression.Or(doesNotContainIsEmpty.Body, doesNotContainIsNull.Body), doesNotContainIsEmpty.Parameters.Single());

                        lambda = Expression.Lambda<Func<T, bool>>(Expression.Or(e1DoesNotContain.Body, e2DoesNotContain.Body), e1DoesNotContain.Parameters.Single());
                        break;

                    case FilterArgumentType.StartsWith:
                        MethodInfo methodInfoStartsWith = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
                        Expression callStartsWith = Expression.Call(member, methodInfoStartsWith, constant);
                        lambda = Expression.Lambda<Func<T, bool>>(callStartsWith, parameter);
                        break;

                    case FilterArgumentType.EndsWith:
                        MethodInfo methodInfoEndsWith = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });
                        Expression callEndsWith = Expression.Call(member, methodInfoEndsWith, constant);
                        lambda = Expression.Lambda<Func<T, bool>>(callEndsWith, parameter);
                        break;

                    case FilterArgumentType.IsEmpty:
                        Expression<Func<T, bool>> e1IsEmpty = Expression.Lambda<Func<T, bool>>(Expression.Equal(lambdaProperty, constantEmpty), parameter);
                        Expression<Func<T, bool>> e2IsEmpty = Expression.Lambda<Func<T, bool>>(Expression.Equal(lambdaProperty, constantNull), parameter);
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.Or(e1IsEmpty.Body, e2IsEmpty.Body), e1IsEmpty.Parameters.Single());
                        break;

                    case FilterArgumentType.IsNotEmpty:
                        MethodInfo methodIsNotEmpty = typeof(string).GetMethod("Equals", new Type[] { typeof(string) });
                        Expression callIsNotEmpty = Expression.Call(member, methodIsNotEmpty, constantEmpty);
                        Expression<Func<T, bool>> lambdaIsNotEmpty = Expression.Lambda<Func<T, bool>>(Expression.Not(callIsNotEmpty), parameter);

                        MethodInfo methodIsNotNull = typeof(string).GetMethod("Equals", new Type[] { typeof(string) });
                        Expression callIsNotNull = Expression.Call(member, methodIsNotNull, constantNull);
                        Expression<Func<T, bool>> lambdaIsNotNull = Expression.Lambda<Func<T, bool>>(Expression.Not(callIsNotNull), parameter);

                        lambda = Expression.Lambda<Func<T, bool>>(Expression.And(lambdaIsNotEmpty.Body, lambdaIsNotNull.Body), lambdaIsNotEmpty.Parameters.Single());
                        break;

                    default:
                        lambda = Expression.Lambda<Func<T, bool>>(lambdaEqual, parameter);
                        break;
                }
            }

            return lambda;
        }
    }
}
