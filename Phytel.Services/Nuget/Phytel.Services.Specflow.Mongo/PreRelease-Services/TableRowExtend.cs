using MongoDB.Bson;
using Phytel.Services.Specflow.Assist;
using System;
using System.Linq.Expressions;
using TechTalk.SpecFlow;

namespace Phytel.Services.Specflow.Mongo
{
    public static class TableRowExtend
    {
        public static ObjectId GetAsObjectId(this TableRow row, string propertyName)
        {
            ObjectId rvalue = ObjectId.Empty;

            if (row.HasValueForProperty(propertyName))
            {
                rvalue = new ObjectId(row[propertyName]);
            }

            return rvalue;
        }

        public static ObjectId GetAsObjectId<T>(this TableRow row, Expression<Func<T, object>> propertySelector)
        {
            var memberExpression = propertySelector.GetMemberInfo();
            return row.GetAsObjectId(memberExpression.Member.Name);
        }

        public static string GetAsObjectIdString(this TableRow row, string propertyName)
        {
            return row.GetAsObjectId(propertyName).ToString();
        }

        public static string GetAsObjectIdString<T>(this TableRow row, Expression<Func<T, object>> propertySelector)
        {
            var memberExpression = propertySelector.GetMemberInfo();
            return row.GetAsObjectIdString(memberExpression.Member.Name);
        }
    }
}