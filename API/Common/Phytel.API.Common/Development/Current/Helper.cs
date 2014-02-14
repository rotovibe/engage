using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Phytel.API.Common
{
    public static class Helper
    {

        /// <summary>
        /// Converts the name of the Enum to a friendly name that can be shown on the UI
        /// </summary>
        /// <param name="s">Status enum object</param>
        /// <returns>friendly string</returns>
        public static string ToFriendlyString(Status s)
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
    }
}
