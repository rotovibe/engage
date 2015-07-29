using AutoMapper;
using ServiceStack;
using ServiceStack.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Phytel.Services.API.Urn
{
    public static class UrnExtend
    {
        public static MemberExpression GetMemberInfo(this Expression method)
        {
            LambdaExpression lambda = method as LambdaExpression;
            if (lambda == null)
                throw new ArgumentNullException("method");

            MemberExpression memberExpr = null;

            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                memberExpr =
                    ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr = lambda.Body as MemberExpression;
            }

            if (memberExpr == null)
                throw new ArgumentException("method");

            return memberExpr;
        }

        public static string ToUrnFromProperties(this object dto, params string[] propertyNames)
        {
            List<string> parts = new List<string>();
            Type dtoType = dto.GetType();

            foreach (string propertyName in propertyNames)
            {
                PropertyInfo property = dto.GetType().GetProperty(propertyName);
                object value = property.GetValue(dto);
                if (value != null)
                {
                    string valueAsString = string.Empty;
                    valueAsString = Mapper.Map(value, valueAsString, property.PropertyType, typeof(string)) as string;
                    if(!string.IsNullOrEmpty(valueAsString))
                    {
                        string part = string.Join(":", propertyName, valueAsString);
                        parts.Add(part);
                    }
                }
            }

            return dto.ToUrn(parts.ToArray());
        }

        public static string ToUrn(this object dto, params string[] parts)
        {
            return UrnId.CreateWithParts(dto.GetType().Name, parts);
        }

        public static string ToUrnFromAttributes(this object dto)
        {
            List<string> propertyNames = new List<string>();

            Type dtoType = dto.GetType();
            PropertyInfo[] properties = dtoType.GetProperties();
            foreach(PropertyInfo property in properties)
            {
                UrnPartAttribute attribute = property.GetCustomAttribute<UrnPartAttribute>();
                if(attribute != null)
                {
                    propertyNames.Add(property.Name);
                }
            }

            return dto.ToUrnFromProperties(propertyNames.ToArray());
        }

        public static string ToUrnFromProperties<T>(this T dto, params Expression<Func<T, object>>[] selectors)
            where T : class
        {
            List<string> propertyNames = new List<string>();

            foreach (Expression<Func<T, object>> selector in selectors)
            {
                var memberInfo = selector.GetMemberInfo();
                propertyNames.Add(memberInfo.Member.Name);
            }

            return dto.ToUrnFromProperties(propertyNames.ToArray());
        }
    }
}