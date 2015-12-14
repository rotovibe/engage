using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using MongoDB.Bson;
using Phytel.API.DataDomain.ASE.Common.Enums;
using Phytel.ASE.Core;

namespace Phytel.API.Common
{
    public static class Helper
    {
        public static string BuildURL(string baseURL, string userId)
        {
            string returnURL = baseURL;
            if (returnURL.Contains("?"))
                return string.Format("{0}&UserId={1}", returnURL, userId);
            else
                return string.Format("{0}?UserId={1}", returnURL, userId);
        }

        /// <summary>
        /// Converts a list of objectIds to list of strings.
        /// </summary>
        /// <param name="objectIds">list of objectIds</param>
        /// <returns>list of strings</returns>
        public static List<string> ConvertToStringList(List<ObjectId> objectIds)
        {
            List<string> stringList = null;
            if (objectIds != null && objectIds.Count != 0)
            {
                stringList = new List<string>();
                foreach (ObjectId o in objectIds)
                {
                    stringList.Add(o.ToString());
                }
            }
            return stringList;
        }

        /// <summary>
        /// Converts a list of strings to list of ObjectIds.
        /// </summary>
        /// <param name="objectIds">list of strings</param>
        /// <returns>list of ObjectIds</returns>
        public static List<ObjectId> ConvertToObjectIdList(List<string> stringList)
        {
            List<ObjectId> objectIdList = null;
            if (stringList != null && stringList.Count > 0)
            {
                objectIdList = new List<ObjectId>();
                stringList.ForEach(l =>
                {
                    ObjectId newObjectId;
                    if (ObjectId.TryParse(l, out newObjectId))
                    {
                        objectIdList.Add(newObjectId);
                    }
                });
            }
            return objectIdList;
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
            if(p != null)
            {
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
            return bsonValues;
        }

        public static void LogException(int processId, Exception ex)
        {
            string aseAPIURL = ConfigurationManager.AppSettings.Get("ASEAPI");
            Log.LogError(aseAPIURL, processId, ex, LogErrorCode.Error, LogErrorSeverity.High);
        }

        public static void LogException(int processId, string errorMessage)
        {
            string aseAPIURL = ConfigurationManager.AppSettings.Get("ASEAPI");
            Log.LogError(aseAPIURL, processId, errorMessage, LogErrorCode.Error, LogErrorSeverity.High, string.Empty);
        }

        public static string TrimAndLimit(string value, int limit)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Trim();
                if (value.Length > limit)
                    value = value.Substring(0, limit);
            }
            return value;
        }

        public static void SerializeObject<T>(T obj, string filePath)
        {
            //serialize
            try
            {
                using (Stream stream = File.Open(filePath, FileMode.Create))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    bformatter.Serialize(stream, obj);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("SerializeObject():" + ex.Message);
            }
        }

        public static object DeserializeObject<T>(string filePath)
        {
            try
            {
                //deserialize
                object obj = null;
                using (Stream stream = File.Open(filePath, FileMode.Open))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    obj = (T)bformatter.Deserialize(stream);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("DeserializeObject():" + ex.Message);
            }
        }
    }
}
