using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using Phytel.Framework.ASE.Process;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.Common
{
    public class Helpers : IHelpers
    {
        public string BuildURL(string baseURL, string userId)
        {
            string returnURL = baseURL;
            if (returnURL.Contains("?"))
                return string.Format("{0}&UserId={1}", returnURL, userId);
            else
                return string.Format("{0}?UserId={1}", returnURL, userId);
        }

        /// <summary>
        /// Converts the name of the Enum to a friendly name that can be shown on the UI
        /// </summary>
        /// <param name="s">Status enum object</param>
        /// <returns>friendly string</returns>
        public string ToFriendlyString(Status s)
        {
            switch(s)
            {
                case Status.Active:
                return "Active";
                case Status.Inactive:
                return "Inactive";
                case Status.InReview:
                return "In Review";
                case Status.Met:
                return "Met";
                case Status.NotMet:
                return "Not Met";
            }
            return null;
        }

        /// <summary>
        /// Converts a list of objectIds to list of strings.
        /// </summary>
        /// <param name="objectIds">list of objectIds</param>
        /// <returns>list of strings</returns>
        public List<string> ConvertToStringList(List<ObjectId> objectIds)
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
        public List<ObjectId> ConvertToObjectIdList(List<string> stringList)
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
        public object ConvertToAppropriateType(object p)
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
        public List<BsonValue> ConvertToBsonValueList(object p)
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

        public void LogException(int processId, Exception ex)
        {
            Log.LogError(processId, ex, Framework.ASE.Data.Common.LogErrorCode.Error, Framework.ASE.Data.Common.LogErrorSeverity.High);
        }

    }
}
