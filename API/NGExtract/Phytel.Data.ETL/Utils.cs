using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MongoDB.Driver;
using Phytel.Services;
using Phytel.Services.SQLServer;

namespace Phytel.Data.ETL
{
    public static class Utils
    {
        public static List<T> GetMongoCollectionList<T>(MongoCollection<T> mogoList, int skip)
        {
            var list = new List<T>();
            int counter = 0;
            try
            {
                var count = mogoList.Count();
                var interval = skip;
                for (var i = 0; i <= count; i = i + interval)
                {
                    list.AddRange(
                        mogoList.FindAllAs<T>()
                            .Skip(i)
                            .Take(interval)
                            .ToList());
                    counter++;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetMongoCollectionList(): patient counter:" + counter + " " + ex.Message + ex.StackTrace);
            }
            return list;
        }

        public static IEnumerable<List<T>> GetMongoCollectionBatch<T>(MongoCollection<T> mongoList, int skip)
        {
            var count = mongoList.Count();
            var interval = skip;
            for (var i = 0; i <= count; i = i + interval)
            {
                yield return mongoList.FindAllAs<T>()
                    .Skip(i)
                    .Take(interval)
                    .ToList();
            }
            yield break;
        }

        internal static Dictionary<string, int> GetStepIdList(string contract)
        {
            try
            {
                var set = SQLDataService.Instance.ExecuteSQL(contract, true, "REPORT",
                    "select StepId, MongoId from RPT_PatientProgramStep");

                if (set == null)
                    throw new ArgumentException("GetStepIdList(): UserId was not found.");

                return set.Tables[0].Rows.Cast<DataRow>()
                    .ToDictionary(dr => dr["MongoId"].ToString(), dr => Convert.ToInt32(dr["StepId"]));
            }
            catch (Exception ex)
            {
                throw new Exception("GetStepIdList()" + ex.Message + ex.StackTrace);
            }
        }

        internal static Dictionary<string, int> GetProgramIdList(string contract)
        {
            try
            {
                var set = SQLDataService.Instance.ExecuteSQL(contract, true, "REPORT",
                    "SELECT PatientProgramId,MongoId FROM RPT_PatientProgram");

                if (set == null)
                    throw new ArgumentException("GetProgramIdList(): ProgramId was not found.");

                return set.Tables[0].Rows.Cast<DataRow>()
                    .ToDictionary(dr => dr["MongoId"].ToString(), dr => Convert.ToInt32(dr["PatientProgramId"]));
            }
            catch (Exception ex)
            {
                throw new Exception("GetProgramIdList()" + ex.Message + ex.StackTrace);
            }
        }

        internal static Dictionary<string, int> GetActionIdList(string contract)
        {
            try { 
            var set = SQLDataService.Instance.ExecuteSQL(contract, true, "REPORT", "SELECT ActionId,MongoId FROM RPT_PatientProgramAction");

            if (set == null)
                throw new ArgumentException("GetActionIdList(): ProgramId was not found.");

            return set.Tables[0].Rows.Cast<DataRow>()
                .ToDictionary(dr => dr["MongoId"].ToString(), dr => Convert.ToInt32(dr["ActionId"]));
            }
            catch (Exception ex)
            {
                throw new Exception("GetActionIdList()" + ex.Message + ex.StackTrace);
            }
        }

        internal static Dictionary<string, int> GetModuleIdList(string contract)
        {
            try { 
            var set = SQLDataService.Instance.ExecuteSQL(contract, true, "REPORT", "SELECT PatientProgramModuleId,MongoId FROM RPT_PatientProgramModule");
            if (set == null)
                throw new ArgumentException("GetModuleIdList(): ModuleId was not found.");

            return set.Tables[0].Rows.Cast<DataRow>()
                .ToDictionary(dr => dr["MongoId"].ToString(), dr => Convert.ToInt32(dr["PatientProgramModuleId"]));
            }
            catch (Exception ex)
            {
                throw new Exception("GetModuleIdList()" + ex.Message + ex.StackTrace);
            }
        }

        internal static Dictionary<string, int> GetModuleIdBySourceIdList(string contract)
        {
            try
            {
                var set = SQLDataService.Instance.ExecuteSQL(contract, true, "REPORT",
                    "SELECT PatientProgramModuleId,SourceId FROM RPT_PatientProgramModule");
                if (set == null)
                    throw new ArgumentException("GetModuleIdBySourceIdList(): ModuleId was not found.");

                return set.Tables[0].Rows.Cast<DataRow>()
                    .GroupBy(p => p["SourceId"].ToString(), StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(dr => dr.Key, dr => Convert.ToInt32(((DataRow) dr.First()).ItemArray.GetValue(0)),
                        StringComparer.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                throw new Exception("GetModuleIdBySourceIdList()" + ex.Message + ex.StackTrace);
            }
        }

        internal static Dictionary<string, int> GetUserIdList(string contract)
        {
            try
            {
                var set = SQLDataService.Instance.ExecuteSQL(contract, true, "REPORT",
                    "select dbo.[RPT_User].UserId, MongoId from dbo.[RPT_User]");

                if (set == null)
                    throw new ArgumentException("GetUserIdList(): UserId was not found.");

                return set.Tables[0].Rows.Cast<DataRow>()
                    .ToDictionary(dr => dr["MongoId"].ToString(), dr => Convert.ToInt32(dr["UserId"]));
            }
            catch (Exception ex)
            {
                throw new Exception("GetUserIdList()" + ex.Message + ex.StackTrace);
            }
        }

        internal static Dictionary<string, int> GetPatientIdList(string contract)
        {
            try { 
            var set = SQLDataService.Instance.ExecuteSQL(contract, true, "REPORT",
                "select dbo.[RPT_Patient].PatientId, MongoId from dbo.[RPT_Patient]");

            if (set == null)
                throw new ArgumentException("GetPatientIdList(): UserId was not found.");

            return set.Tables[0].Rows.Cast<DataRow>()
                .ToDictionary(dr => dr["MongoId"].ToString(), dr => Convert.ToInt32(dr["PatientId"]));
            }
            catch (Exception ex)
            {
                throw new Exception("GetPatientIdList()" + ex.Message + ex.StackTrace);
            }
        }

        internal static Dictionary<string, int> GetObservationIdsList(string contract)
        {
            try { 
            var set = SQLDataService.Instance.ExecuteSQL(contract, true, "REPORT",
                "select dbo.[RPT_Observation].ObservationId, MongoId from dbo.[RPT_Observation]");

            if (set == null)
                throw new ArgumentException("GetObservationIdList(): ObservationId was not found.");

            return set.Tables[0].Rows.Cast<DataRow>()
                .ToDictionary(dr => dr["MongoId"].ToString(), dr => Convert.ToInt32(dr["ObservationId"]));
            }
            catch (Exception ex)
            {
                throw new Exception("GetObservationIdsList()" + ex.Message + ex.StackTrace);
            }
        }

        internal static int GetUserId(string mId, string contract)
        {
            try
            {
                var set = SQLDataService.Instance.ExecuteSQL(contract, true, "REPORT",
                    "select dbo.[RPT_User].UserId from dbo.[RPT_User] where MongoId = '" + mId + "'");

                var row = set.Tables[0].Rows.Cast<DataRow>().FirstOrDefault<DataRow>();
                var id = 0;
                if (row != null)
                    id = Convert.ToInt32(row[0]);

                //if (id == 0)
                //    throw new ArgumentException("GetUserId(): UserId was not found.");

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception("GetUserId()" + ex.Message + ex.StackTrace);
            }
        }
    }
}
