using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MongoDB.Driver;
using Phytel.Services;

namespace Phytel.Data.ETL
{
    public static class Utils
    {
        public static List<T> GetMongoCollectionList<T>(MongoCollection<T> mogoList, int skip)
        {
            var list = new List<T>();
            var count = mogoList.Count();
            var interval = skip;
            for (var i = 0; i <= count; i = i + interval)
            {
                list.AddRange(
                    mogoList.FindAllAs<T>()
                        .Skip(i)
                        .Take(interval)
                        .ToList());
            }
            return list;
        }

        internal static Dictionary<string, int> GetStepIdList()
        {
            var set = SQLDataService.Instance.ExecuteSQL("InHealth001", true, "REPORT", "select StepId, _id from RPT_PatientProgramStep");

            if (set == null)
                throw new ArgumentException("GetStepIdList(): UserId was not found.");

            return set.Tables[0].Rows.Cast<DataRow>()
                .ToDictionary(dr => dr["_id"].ToString(), dr => Convert.ToInt32(dr["StepId"]));
        }

        internal static int GetUserId(string mId)
        {
            var set = SQLDataService.Instance.ExecuteSQL("InHealth001", true, "REPORT",
                "select dbo.[RPT_User].UserId from dbo.[RPT_User] where _id = '" + mId + "'");

            var row = set.Tables[0].Rows.Cast<DataRow>().FirstOrDefault<DataRow>();
            var id = 0;
            if (row != null)
                id = Convert.ToInt32(row[0]);

            //if (id == 0)
            //    throw new ArgumentException("GetUserId(): UserId was not found.");

            return id;
        }
    }
}
