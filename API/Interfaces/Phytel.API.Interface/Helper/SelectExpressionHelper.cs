using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Phytel.API.Interface
{
    public static class SelectExpressionHelper
    {

        /// <summary>
        /// Applies appropriate query operators on fied name and value.
        /// </summary>
        /// <param name="type">Type can be EQ, NOTEQ, LIKE, NOTLIKE, STARTWITH</param>
        /// <param name="fieldName">field name</param>
        /// <param name="value">value of field</param>
        /// <returns>IMongoQuery object</returns>
        public static IMongoQuery ApplyQueryOperators(SelectExpressionType type, string fieldName, object value)
        {
            IMongoQuery query = null;
            switch (type)
            {
                case SelectExpressionType.EQ:
                    query = Query.EQ(fieldName, BsonValue.Create(ConvertToAppropriateType(value)));
                    break;
                case SelectExpressionType.NOTEQ:
                    query = Query.NE(fieldName, BsonValue.Create(ConvertToAppropriateType(value)));
                    break;
                case SelectExpressionType.IN:
                    List<BsonValue> bsonList = ConvertToBsonValueList(value);
                    if(bsonList != null)
                    {
                        query = Query.In(fieldName, bsonList);
                    }
                    break;
            }
            return query;
        }

        /// <summary>
        /// Builds a single mongo query from list of queries depending on AND or OR clause.
        /// </summary>
        /// <param name="groupType">AND or OR</param>
        /// <param name="queries">List of queries</param>
        /// <returns>A single mongo query</returns>
        public static IMongoQuery BuildQuery(SelectExpressionGroupType groupType, IList<IMongoQuery> queries)
        {
            IMongoQuery query = null;
            if (groupType.Equals(SelectExpressionGroupType.AND))
            {
                query = Query.And(queries);
            }
            else if (groupType.Equals(SelectExpressionGroupType.OR))
            {
                query = Query.Or(queries);
            }
            return query;
        }

        /// <summary>
        /// Converts the object to an appropriate type.
        /// </summary>
        /// <param name="p">object to be converted</param>
        /// <returns>converted object.</returns>
        public static object ConvertToAppropriateType(object p)
        {
            string type = p.GetType().ToString();
            switch (type)
            {
                case "System.Int32":
                    return Convert.ToInt32(p);
                case "System.String":
                    ObjectId result;
                    if (ObjectId.TryParse(p.ToString(), out result))
                    {
                        return result;
                    }
                    else
                    {
                        return p.ToString();
                    }
                case "System.Int16":
                    return Convert.ToInt16(p);
                case "System.Int64":
                    return Convert.ToInt64(p);
                case "System.Boolean":
                    return Convert.ToBoolean(p);
                default:
                    return p.ToString();
            }
        }

        /// <summary>
        /// Converts the object to List of BsonValue.
        /// </summary>
        /// <param name="p">object to be converted.</param>
        /// <returns>List of BsonValue</returns>
        public static List<BsonValue> ConvertToBsonValueList(object p)
        {
            List<BsonValue> bsonValues = null;
            string type = p.GetType().ToString();
            switch (type)
            {
                case "System.Collections.Generic.List`1[System.Int32]":
                    List<int> intList = (List<int>)p;
                    if (intList.Count() > 0)
                    {
                        bsonValues = new List<BsonValue>();
                        foreach (int i in intList)
                        {
                            bsonValues.Add(BsonValue.Create(ConvertToAppropriateType(i)));
                        }
                    }
                    return bsonValues;
                case "System.Collections.Generic.List`1[System.String]":
                    List<string> stringList = (List<string>)p;
                    if (stringList.Count() > 0)
                    {
                        bsonValues = new List<BsonValue>();
                        foreach (string s in stringList)
                        {
                            bsonValues.Add(BsonValue.Create(ConvertToAppropriateType(s)));
                        }
                    }
                    return bsonValues;
                default:
                    return bsonValues;
            }
        }
    }
}
