using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Phytel.Services.SQLServer.Repository.PredicateBuilder
{
    class PredicateBuilderDate : IPredicateBuilder
    {
		ParameterExpression parameter;

		public Expression<Func<T, bool>> BuildLambdaExpression<T>(PredicateFilter filterRow)
		{
			List<FilterValue> filterValues = filterRow.FilterValues;

            DateTime primaryValue = DateTime.MinValue;
            DateTime secondaryValue = DateTime.MinValue;
            bool validSecondaryValue = false;

			if (filterValues != null && filterValues.Count > 0 && filterValues[0] != null)
			{
				if (filterValues.Count > 0)
				{
                    DateTime.TryParse(filterValues[0].Value.Trim(), out primaryValue);
				}

				if (filterValues.Count > 1)
				{
					if (DateTime.TryParse(filterValues[1].Value.Trim(), out secondaryValue))
					{
                        validSecondaryValue = true;
					}
				}
			}

			Expression<Func<T, bool>> lambda = null;
			PropertyInfo propertyInfo = typeof(T).GetProperty(filterRow.FilterField);

            if (propertyInfo != null && (typeof(DateTime) == propertyInfo.PropertyType || typeof(DateTime?) == propertyInfo.PropertyType))
            {
                parameter = Expression.Parameter(typeof(T), "p");
                MemberExpression member = Expression.MakeMemberAccess(parameter, propertyInfo);
                Expression lambdaProperty = Expression.Property(parameter, filterRow.FilterField);

                switch (filterRow.FilterArgument)
                {
                    case FilterArgumentType.IsEqualTo:
                        Expression<Func<T, bool>> e1EqualTo = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(lambdaProperty,
                            GetConstantExpression(propertyInfo, primaryValue.Date)), parameter);
                        Expression<Func<T, bool>> e2EqualTo = Expression.Lambda<Func<T, bool>>(Expression.LessThan(lambdaProperty,
                            GetConstantExpression(propertyInfo, primaryValue.AddDays(1).Date)), parameter);
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(e1EqualTo.Body, e2EqualTo.Body), e1EqualTo.Parameters.Single());
                        break;

                    // *** ONLY used for overrides ***
                    case FilterArgumentType.IsNotEqualTo:
                        Expression<Func<T, bool>> e1NotEqualTo = Expression.Lambda<Func<T, bool>>(Expression.LessThan(lambdaProperty,
                            GetConstantExpression(propertyInfo, primaryValue.Date)), parameter);
                        Expression<Func<T, bool>> e2NotEqualTo = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(lambdaProperty,
                            GetConstantExpression(propertyInfo, primaryValue.AddDays(1).Date)), parameter);
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.Or(e1NotEqualTo.Body, e2NotEqualTo.Body), e1NotEqualTo.Parameters.Single());
                        lambda = NullCheck(lambda, filterRow);
                        break;

                    case FilterArgumentType.Between:
                        if (validSecondaryValue)
                        {
                            Expression<Func<T, bool>> e1Between = (secondaryValue > primaryValue)
                                ? Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(lambdaProperty, GetConstantExpression(propertyInfo, primaryValue)), parameter)
                                : Expression.Lambda<Func<T, bool>>(Expression.LessThan(lambdaProperty, GetConstantExpression(propertyInfo, primaryValue.AddDays(1).Date)), parameter);

                            Expression<Func<T, bool>> e2Between = (secondaryValue > primaryValue)
                                ? Expression.Lambda<Func<T, bool>>(Expression.LessThan(lambdaProperty, GetConstantExpression(propertyInfo, secondaryValue.AddDays(1).Date)), parameter)
                                : Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(lambdaProperty, GetConstantExpression(propertyInfo, secondaryValue)), parameter);

                            lambda = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(e1Between.Body, e2Between.Body), e1Between.Parameters.Single());
                        }
                        break;

                    case FilterArgumentType.NotBetween:
                        if (validSecondaryValue)
                        {
                            Expression<Func<T, bool>> e1NotBetween = (secondaryValue > primaryValue)
                                ? Expression.Lambda<Func<T, bool>>(Expression.LessThan(lambdaProperty, GetConstantExpression(propertyInfo, primaryValue)), parameter)
                                : Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(lambdaProperty, GetConstantExpression(propertyInfo, primaryValue.AddDays(1).Date)), parameter);

                            Expression<Func<T, bool>> e2NotBetween = (secondaryValue > primaryValue)
                                ? Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(lambdaProperty, GetConstantExpression(propertyInfo, secondaryValue.AddDays(1).Date)), parameter)
                                : Expression.Lambda<Func<T, bool>>(Expression.LessThan(lambdaProperty, GetConstantExpression(propertyInfo, secondaryValue)), parameter);

                            lambda = Expression.Lambda<Func<T, bool>>(Expression.Or(e1NotBetween.Body, e2NotBetween.Body), e1NotBetween.Parameters.Single());
                            lambda = NullCheck(lambda, filterRow);
                        }
                        break;

                    case FilterArgumentType.GreaterThan:
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(lambdaProperty, GetConstantExpression(propertyInfo, primaryValue.AddDays(1).Date)), parameter);
                        break;

                    case FilterArgumentType.NotGreaterThan:
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.LessThan(lambdaProperty, GetConstantExpression(propertyInfo, primaryValue.AddDays(1).Date)), parameter);
                        lambda = NullCheck(lambda, filterRow);
                        break;

                    case FilterArgumentType.GreaterThanOrEqualTo:
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(lambdaProperty, GetConstantExpression(propertyInfo, primaryValue)), parameter);
                        break;

                    case FilterArgumentType.NotGreaterThanOrEqualTo:
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.LessThan(lambdaProperty, GetConstantExpression(propertyInfo, primaryValue)), parameter);
                        lambda = NullCheck(lambda, filterRow);
                        break;

                    case FilterArgumentType.LessThan:
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.LessThan(lambdaProperty, GetConstantExpression(propertyInfo, primaryValue)), parameter);
                        break;

                    case FilterArgumentType.NotLessThan:
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(lambdaProperty, GetConstantExpression(propertyInfo, primaryValue)), parameter);
                        lambda = NullCheck(lambda, filterRow);
                        break;

                    case FilterArgumentType.LessThanOrEqualTo:
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.LessThan(lambdaProperty, GetConstantExpression(propertyInfo, primaryValue.AddDays(1).Date)), parameter);
                        break;

                    case FilterArgumentType.NotLessThanOrEqualTo:
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(lambdaProperty, GetConstantExpression(propertyInfo, primaryValue.AddDays(1).Date)), parameter);
                        lambda = NullCheck(lambda, filterRow);
                        break;

                    case FilterArgumentType.IsEmpty:
                        Expression leftEmpty = Expression.Property(parameter, typeof(T).GetProperty(filterRow.FilterField));
                        Expression rightEmpty = Expression.Constant(null);
                        Expression InnerLambdaEmpty = Expression.Equal(leftEmpty, rightEmpty);
                        lambda = Expression.Lambda<Func<T, bool>>(InnerLambdaEmpty, parameter);
                        break;

                    case FilterArgumentType.IsNotEmpty:
                        Expression leftNotEmpty = Expression.Property(parameter, typeof(T).GetProperty(filterRow.FilterField));
                        Expression rightNotEmpty = Expression.Constant(null);
                        Expression InnerLambdaNotEmpty = Expression.NotEqual(leftNotEmpty, rightNotEmpty);
                        lambda = Expression.Lambda<Func<T, bool>>(InnerLambdaNotEmpty, parameter);
                        break;

                    case FilterArgumentType.IsNull:
                        Expression leftNull = Expression.Property(parameter, typeof(T).GetProperty(filterRow.FilterField));
                        Expression rightNull = Expression.Constant(null);
                        Expression InnerLambdaNull = Expression.Equal(leftNull, rightNull);
                        lambda = Expression.Lambda<Func<T, bool>>(InnerLambdaNull, parameter);
                        break;

                    case FilterArgumentType.IsNotNull:
                        Expression leftNotNull = Expression.Property(parameter, typeof(T).GetProperty(filterRow.FilterField));
                        Expression rightNotNull = Expression.Constant(null);
                        Expression InnerLambdaNotNull = Expression.NotEqual(leftNotNull, rightNotNull);
                        lambda = Expression.Lambda<Func<T, bool>>(InnerLambdaNotNull, parameter);
                        break;

                    case FilterArgumentType.IsToday:
                        Expression<Func<T, bool>> e1IsToday = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(lambdaProperty,
                            GetConstantExpression(propertyInfo, DateTime.Now.Date)), parameter);
                        Expression<Func<T, bool>> e2IsToday = Expression.Lambda<Func<T, bool>>(Expression.LessThan(lambdaProperty,
                            GetConstantExpression(propertyInfo, DateTime.Now.AddDays(1).Date)), parameter);
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(e1IsToday.Body, e2IsToday.Body), e1IsToday.Parameters.Single());
                        break;

                    case FilterArgumentType.IsNotToday:
                        Expression<Func<T, bool>> e1IsNotToday = Expression.Lambda<Func<T, bool>>(Expression.LessThan(lambdaProperty,
                            GetConstantExpression(propertyInfo, DateTime.Now.Date)), parameter);
                        Expression<Func<T, bool>> e2IsNotToday = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(lambdaProperty,
                            GetConstantExpression(propertyInfo, DateTime.Now.AddDays(1).Date)), parameter);
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.Or(e1IsNotToday.Body, e2IsNotToday.Body), e1IsNotToday.Parameters.Single());
                        lambda = NullCheck(lambda, filterRow);
                        break;

                    case FilterArgumentType.IsTomorrow:
                        Expression<Func<T, bool>> e1IsTomorrow = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(lambdaProperty,
                            GetConstantExpression(propertyInfo, DateTime.Now.AddDays(1).Date)), parameter);
                        Expression<Func<T, bool>> e2IsTomorrow = Expression.Lambda<Func<T, bool>>(Expression.LessThan(lambdaProperty,
                            GetConstantExpression(propertyInfo, DateTime.Now.AddDays(2).Date)), parameter);
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(e1IsTomorrow.Body, e2IsTomorrow.Body), e1IsTomorrow.Parameters.Single());
                        break;

                    case FilterArgumentType.IsNotTomorrow:
                        Expression<Func<T, bool>> e1IsNotTomorrow = Expression.Lambda<Func<T, bool>>(Expression.LessThan(lambdaProperty,
                            GetConstantExpression(propertyInfo, DateTime.Now.AddDays(1).Date)), parameter);
                        Expression<Func<T, bool>> e2IsNotTomorrow = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(lambdaProperty,
                            GetConstantExpression(propertyInfo, DateTime.Now.AddDays(2).Date)), parameter);
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.Or(e1IsNotTomorrow.Body, e2IsNotTomorrow.Body), e1IsNotTomorrow.Parameters.Single());
                        lambda = NullCheck(lambda, filterRow);
                        break;

                    case FilterArgumentType.InOneWeek:
                        Expression<Func<T, bool>> e1OneWeek = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(lambdaProperty,
                            GetConstantExpression(propertyInfo, DateTime.Now.Date)), parameter);
                        Expression<Func<T, bool>> e2OneWeek = Expression.Lambda<Func<T, bool>>(Expression.LessThan(lambdaProperty,
                            GetConstantExpression(propertyInfo, DateTime.Now.AddDays(7).Date)), parameter);
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(e1OneWeek.Body, e2OneWeek.Body), e1OneWeek.Parameters.Single());
                        break;

                    case FilterArgumentType.NotInOneWeek:
                        Expression<Func<T, bool>> e1NotOneWeek = Expression.Lambda<Func<T, bool>>(Expression.LessThan(lambdaProperty,
                            GetConstantExpression(propertyInfo, DateTime.Now.Date)), parameter);
                        Expression<Func<T, bool>> e2NotOneWeek = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(lambdaProperty,
                            GetConstantExpression(propertyInfo, DateTime.Now.AddDays(7).Date)), parameter);
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.Or(e1NotOneWeek.Body, e2NotOneWeek.Body), e1NotOneWeek.Parameters.Single());
                        lambda = NullCheck(lambda, filterRow);
                        break;

                    case FilterArgumentType.InTwoWeeks:
                        Expression<Func<T, bool>> e1TwoWeeks = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(lambdaProperty,
                            GetConstantExpression(propertyInfo, DateTime.Now.Date)), parameter);
                        Expression<Func<T, bool>> e2TwoWeeks = Expression.Lambda<Func<T, bool>>(Expression.LessThan(lambdaProperty,
                            GetConstantExpression(propertyInfo, DateTime.Now.AddDays(14).Date)), parameter);
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(e1TwoWeeks.Body, e2TwoWeeks.Body), e1TwoWeeks.Parameters.Single());
                        break;

                    case FilterArgumentType.NotInTwoWeeks:
                        Expression<Func<T, bool>> e1NotTwoWeeks = Expression.Lambda<Func<T, bool>>(Expression.LessThan(lambdaProperty,
                            GetConstantExpression(propertyInfo, DateTime.Now.Date)), parameter);
                        Expression<Func<T, bool>> e2NotTwoWeeks = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(lambdaProperty,
                            GetConstantExpression(propertyInfo, DateTime.Now.AddDays(14).Date)), parameter);
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.Or(e1NotTwoWeeks.Body, e2NotTwoWeeks.Body), e1NotTwoWeeks.Parameters.Single());
                        lambda = NullCheck(lambda, filterRow);
                        break;

                    case FilterArgumentType.InFourWeeks:
                        Expression<Func<T, bool>> e1FourWeeks = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(lambdaProperty,
                            GetConstantExpression(propertyInfo, DateTime.Now.Date)), parameter);
                        Expression<Func<T, bool>> e2FourWeeks = Expression.Lambda<Func<T, bool>>(Expression.LessThan(lambdaProperty,
                            GetConstantExpression(propertyInfo, DateTime.Now.AddDays(30).Date)), parameter);
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(e1FourWeeks.Body, e2FourWeeks.Body), e1FourWeeks.Parameters.Single());
                        break;

                    case FilterArgumentType.NotInFourWeeks:
                        Expression<Func<T, bool>> e1NotFourWeeks = Expression.Lambda<Func<T, bool>>(Expression.LessThan(lambdaProperty,
                            GetConstantExpression(propertyInfo, DateTime.Now.Date)), parameter);
                        Expression<Func<T, bool>> e2NotFourWeeks = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(lambdaProperty,
                            GetConstantExpression(propertyInfo, DateTime.Now.AddDays(30).Date)), parameter);
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.Or(e1NotFourWeeks.Body, e2NotFourWeeks.Body), e1NotFourWeeks.Parameters.Single());
                        lambda = NullCheck(lambda, filterRow);
                        break;

                    default:
                        Expression<Func<T, bool>> e1DefaultEqualTo = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(lambdaProperty,
                            GetConstantExpression(propertyInfo, primaryValue.Date)), parameter);
                        Expression<Func<T, bool>> e2DefaultEqualTo = Expression.Lambda<Func<T, bool>>(Expression.LessThan(lambdaProperty,
                            GetConstantExpression(propertyInfo, primaryValue.AddDays(1).Date)), parameter);
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(e1DefaultEqualTo.Body, e2DefaultEqualTo.Body), e1DefaultEqualTo.Parameters.Single());
                        break;
                }
            }

			return lambda;
		}

		private Expression<Func<T, bool>> NullCheck<T>(Expression<Func<T, bool>> lambda, PredicateFilter filterRow)
		{
			return Expression.Lambda<Func<T, bool>>(Expression.Or(lambda.Body, Expression.Equal(Expression.Property(parameter, typeof(T).GetProperty(filterRow.FilterField)), Expression.Constant(null))), parameter);
		}

        private Expression GetConstantExpression(PropertyInfo propertyInfo, DateTime? value)
        {
            ConstantExpression constant;
            if (propertyInfo.PropertyType.Name.Contains("Nullable"))
            {
                constant = Expression.Constant(value, typeof(DateTime?));
            }
            else
            {
                constant = Expression.Constant(value, typeof(DateTime));
            }
            return constant;
        }
    }
}
