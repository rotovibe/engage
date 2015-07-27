using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Phytel.Services.SQLServer.Repository.PredicateBuilder
{
	public static class QueryBuilder
	{
        public static IQueryable<T> BuildQuery<T>(List<PredicateFilter> filterList, IQueryable<T> objects)
		{
			foreach (PredicateFilter filterRow in filterList)
			{
				// Fetching the proper LambdaBuilder based on the current filter's data type via a factory method
                Expression<Func<T, bool>> lambda = null;
				PredicateBuilderTypeFactory lambdaBuilderFactory = new PredicateBuilderTypeFactory();
				IPredicateBuilder controlBuilder = lambdaBuilderFactory.GetPredicateBuilderForDataType(filterRow.DataType);

				// Splitting filter values (primary) when semi-colon delimited values are present
				string[] filterValues = null;

				if (filterRow.FilterArgument != FilterArgumentType.Between && filterRow.FilterValues != null)
				{
					if (filterRow.FilterValues.Count > 0 && filterRow.FilterValues.Any())
					{
						if (filterRow.FilterValues[0].Value.Contains("|"))
						{
							filterValues = filterRow.FilterValues[0].Value.Split('|');
							filterRow.PrefixBoolean = FilterBooleanType.AND;
						}
						else
						{
							filterValues = filterRow.FilterValues[0].Value.Split(';');
							filterRow.PrefixBoolean = FilterBooleanType.OR;
						}
					}
				}

				if (filterValues != null && filterValues.Count() > 1)
				{
					foreach (string value in filterValues)
					{
						filterRow.FilterValues[0].Value = value.Trim();
                        Expression<Func<T, bool>> lambdaInner = null;
                        lambdaInner = controlBuilder.BuildLambdaExpression<T>(filterRow);

						if (lambda == null)
						{
							lambda = lambdaInner;
						}
						else
						{
							if (filterRow.PrefixBoolean == FilterBooleanType.OR)
							{
                                lambda = PredicateUtility.Or<T>(lambda, lambdaInner);
							}
							else
							{
                                lambda = PredicateUtility.And<T>(lambda, lambdaInner);
							}
						}
					}
				}
				else
				{
                    lambda = controlBuilder.BuildLambdaExpression<T>(filterRow);
				}

                objects = (lambda != null) ? objects.Where(lambda) : objects;
			}
            return objects;
		}
    }
}
