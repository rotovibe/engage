using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.Services.SQLServer.Repository.PredicateBuilder;

namespace Phytel.Services.SQLServer.Repository.PredicateBuilder
{
    public static class TypeHelper
    {
        public static FilterArgumentType GetArgumentType(string argument)
        {
            FilterArgumentType argumentType = new FilterArgumentType();

            switch (argument)
            {
                case "=":
                    argumentType = FilterArgumentType.IsEqualTo;
                    break;
                case ">":
                    argumentType = FilterArgumentType.GreaterThan;
                    break;
                case "<":
                    argumentType = FilterArgumentType.LessThan;
                    break;
                case "IN":
                    argumentType = FilterArgumentType.IsEqualTo;
                    break;
                case "LIKE":
                    argumentType = FilterArgumentType.Contains;
                    break;
                case ">=":
                    argumentType = FilterArgumentType.GreaterThanOrEqualTo;
                    break;
                case "<=":
                    argumentType = FilterArgumentType.LessThanOrEqualTo;
                    break;
                default:
                    break;
            }

            return argumentType;
        }

        public static TypeCode GetTypeCode(string typeName)
        {
            TypeCode typeCode = new TypeCode();

            switch(typeName)
            {
                case "int":
                    typeCode = TypeCode.Int32;
                    break;
                case "string":
                    typeCode = TypeCode.String;
                    break;
                case "datetime":
                    typeCode = TypeCode.DateTime;
                    break;
                case "decimal":
                    typeCode = TypeCode.Decimal;
                    break;
                case "bool":
                    typeCode = TypeCode.Boolean;
                    break;
                case "byte":
                    typeCode = TypeCode.Byte;
                    break;
                case "smallint":
                    typeCode = TypeCode.Int16;
                    break;
                case "bigint":
                    typeCode = TypeCode.Int64;
                    break;
                case "char":
                    typeCode = TypeCode.Char;
                    break;
                case "float":
                    typeCode = TypeCode.Double;
                    break;
                default:
                    break;
            }
            return typeCode;
        }
    }
}
