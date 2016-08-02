using MongoDB.Driver;
using Phytel.API.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Phytel.API.Common.Data
{
    public static class MongoDataUtil
    {
        public static IMongoQuery ExpressionQueryBuilder(Interface.APIExpression expression)
        {
            IMongoQuery mQuery = null;
            List<SelectExpression> selectExpressions = expression.Expressions.ToList();
            selectExpressions.Where(s => s.GroupID == 1).OrderBy(o => o.ExpressionOrder).ToList();

            SelectExpressionGroupType groupType = SelectExpressionGroupType.AND;

            if (selectExpressions.Count > 0)
            {
                IList<IMongoQuery> queries = new List<IMongoQuery>();
                for (int i = 0; i < selectExpressions.Count; i++)
                {
                    groupType = selectExpressions[0].NextExpressionType;

                    IMongoQuery query = SelectExpressionHelper.ApplyQueryOperators(selectExpressions[i].Type, selectExpressions[i].FieldName, selectExpressions[i].Value);
                    if (query != null)
                    {
                        queries.Add(query);
                    }
                }

                mQuery = SelectExpressionHelper.BuildQuery(groupType, queries);
            }
            return mQuery;
        }
    }
}
