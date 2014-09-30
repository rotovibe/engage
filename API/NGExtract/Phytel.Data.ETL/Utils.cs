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
            var set = SQLDataService.Instance.ExecuteSQL("InHealth001", true, "REPORT", "select StepId, MongoId from RPT_PatientProgramStep");

            if (set == null)
                throw new ArgumentException("GetStepIdList(): UserId was not found.");

            return set.Tables[0].Rows.Cast<DataRow>()
                .ToDictionary(dr => dr["MongoId"].ToString(), dr => Convert.ToInt32(dr["StepId"]));
        }

        internal static Dictionary<string, int> GetUserIdList()
        {
            var set = SQLDataService.Instance.ExecuteSQL("InHealth001", true, "REPORT",
                "select dbo.[RPT_User].UserId, MongoId from dbo.[RPT_User]");

            if (set == null)
                throw new ArgumentException("GetUserIdList(): UserId was not found.");

            return set.Tables[0].Rows.Cast<DataRow>()
                .ToDictionary(dr => dr["MongoId"].ToString(), dr => Convert.ToInt32(dr["UserId"]));
        }

        internal static Dictionary<string, int> GetPatientIdList()
        {
            var set = SQLDataService.Instance.ExecuteSQL("InHealth001", true, "REPORT",
                "select dbo.[RPT_Patient].PatientId, MongoId from dbo.[RPT_Patient]");

            if (set == null)
                throw new ArgumentException("GetPatientIdList(): UserId was not found.");

            return set.Tables[0].Rows.Cast<DataRow>()
                .ToDictionary(dr => dr["MongoId"].ToString(), dr => Convert.ToInt32(dr["PatientId"]));
        }

        internal static Dictionary<string, int> GetObservationIdsList()
        {
            var set = SQLDataService.Instance.ExecuteSQL("InHealth001", true, "REPORT",
                "select dbo.[RPT_Observation].ObservationId, MongoId from dbo.[RPT_Observation]");

            if (set == null)
                throw new ArgumentException("GetObservationIdList(): ObservationId was not found.");

            return set.Tables[0].Rows.Cast<DataRow>()
                .ToDictionary(dr => dr["MongoId"].ToString(), dr => Convert.ToInt32(dr["ObservationId"]));
        }

        internal static int GetUserId(string mId)
        {
            var set = SQLDataService.Instance.ExecuteSQL("InHealth001", true, "REPORT",
                "select dbo.[RPT_User].UserId from dbo.[RPT_User] where MongoId = '" + mId + "'");

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
