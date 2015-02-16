using System;
using System.Linq.Expressions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Phytel.Services.Specflow.Assist
{
    public static class TableRowExtend
    {
        public static DateTime GetAsDateTime(this TableRow row, string propertyName)
        {
            DateTime rvalue = new DateTime();
            if (row.HasValueForProperty(propertyName))
            {
                rvalue = row.GetDateTime(propertyName);
            }

            return rvalue;
        }

        public static DateTime GetAsDateTime<T>(this TableRow row, Expression<Func<T, object>> propertySelector)
        {
            var memberExpression = propertySelector.GetMemberInfo();
            return row.GetAsDateTime(memberExpression.Member.Name);
        }

        public static DateTime? GetAsDateTimeNullable(this TableRow row, string propertyName)
        {
            DateTime? rvalue = null;
            if (row.HasValueForProperty(propertyName))
            {
                rvalue = row.GetDateTime(propertyName);
            }

            return rvalue;
        }

        public static DateTime? GetAsDateTimeNullable<T>(this TableRow row, Expression<Func<T, object>> propertySelector)
        {
            var memberExpression = propertySelector.GetMemberInfo();
            return row.GetAsDateTimeNullable(memberExpression.Member.Name);
        }

        public static TEnum? GetAsEnum<TEnum>(this TableRow row, string propertyName) where TEnum : struct
        {
            TEnum? rvalue = null;

            if (row.HasValueForProperty(propertyName))
            {
                string valueAsString = row[propertyName];
                rvalue = (TEnum)Enum.Parse(typeof(TEnum), valueAsString);
            }

            return rvalue;
        }

        public static TEnum? GetAsEnum<T, TEnum>(this TableRow row, Expression<Func<T, object>> propertySelector) where TEnum : struct
        {
            var memberExpression = propertySelector.GetMemberInfo();
            return row.GetAsEnum<TEnum>(memberExpression.Member.Name);
        }

        public static string GetAsString(this TableRow row, string propertyName)
        {
            string rvalue = string.Empty;
            if (row.HasValueForProperty(propertyName))
            {
                rvalue = row[propertyName];
            }

            return rvalue;
        }

        public static string GetAsString<T>(this TableRow row, Expression<Func<T, object>> propertySelector)
        {
            var memberExpression = propertySelector.GetMemberInfo();
            return row.GetAsString(memberExpression.Member.Name);
        }

        public static bool HasValueForProperty(this TableRow row, string propertyName)
        {
            return row.Keys.Contains(propertyName) && !string.IsNullOrEmpty(row[propertyName]);
        }
    }
}