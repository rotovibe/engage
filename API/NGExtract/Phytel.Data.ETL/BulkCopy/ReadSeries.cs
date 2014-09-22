using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Program.MongoDB.DTO;

namespace Phytel.Data.ETL.BulkCopy
{
    public static class ReadSeries
    {
        public static IEnumerable<EStepResponse> ReadEStepResponseSeries(List<MEPatientProgramResponse> list)
        {
            var enumerable = new ConcurrentBag<EStepResponse>();
            var stepIdList = Utils.GetStepIdList();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Parallel.ForEach(list, r =>
            {
                var stepIds = stepIdList;
                //lock (enumerable)
                //{
                    //list.ForEach(r => 
                    enumerable.Add(new EStepResponse
                    {
                        _id = r.Id.ToString(),
                        _stepSourceId = r.StepSourceId.ToString(),
                        _actionId = r.ActionId.ToString(),             
                        //ActionId = get action id,
                        Delete = r.DeleteFlag.ToString(),
                        LastUpdatedOn = r.LastUpdatedOn,
                        _nextStepId = r.NextStepId == null ? string.Empty : r.NextStepId.ToString(),
                        NextStepId = GetStepId(stepIds, r.NextStepId.ToString()),
                        Nominal = r.Nominal.ToString(),
                        Order = r.Order.ToString(),
                        Text = r.Text ?? string.Empty,
                        Value = r.Value ?? string.Empty,
                        Required = r.Required.ToString(),
                        Selected = r.Selected.ToString(),
                        _recordCreatedBy = r.RecordCreatedBy.ToString(),
                        RecordCreatedBy = Utils.GetUserId(r.RecordCreatedBy.ToString()),
                        RecordCreatedOn = r.RecordCreatedOn,
                        _StepId = r.StepId == null ? string.Empty : r.StepId.ToString(),
                        StepId = GetStepId(stepIds, r.StepId.ToString()),
                        StepSourceId = 000000000000000000000000,
                        _updatedBy = r.UpdatedBy == null ? string.Empty : r.UpdatedBy.ToString(),
                        UpdatedBy = Utils.GetUserId(r.UpdatedBy.ToString()),
                        TTLDate = r.TTLDate ?? null,
                        Version = Convert.ToInt32(r.Version)

                    });
                //}
            });
            stopwatch.Stop();

            var mins = stopwatch.Elapsed.TotalMinutes;

            return enumerable;
        }

        //public static IEnumerable<EStepResponse> ReadEStepResponseSeries(List<MEPatientProgramResponse> list)
        //{
        //    var stepIdList = Utils.GetStepIdList();

        //    var lt = list.AsParallel().Select(r =>
        //        new EStepResponse
        //        {
        //            _id = r.Id.ToString(),
        //            _stepSourceId = r.StepSourceId.ToString(),
        //            _actionId = r.ActionId.ToString(),
        //            //ActionId = get action id,
        //            Delete = r.DeleteFlag.ToString(),
        //            LastUpdatedOn = r.LastUpdatedOn,
        //            _nextStepId = r.NextStepId == null ? string.Empty : r.NextStepId.ToString(),
        //            NextStepId = GetStepId(stepIdList, r.NextStepId.ToString()),
        //            Nominal = r.Nominal.ToString(),
        //            Order = r.Order.ToString(),
        //            Text = r.Text ?? string.Empty,
        //            Value = r.Value ?? string.Empty,
        //            Required = r.Required.ToString(),
        //            Selected = r.Selected.ToString(),
        //            _recordCreatedBy = r.RecordCreatedBy.ToString(),
        //            RecordCreatedBy = Utils.GetUserId(r.RecordCreatedBy.ToString()),
        //            RecordCreatedOn = r.RecordCreatedOn,
        //            _StepId = r.StepId == null ? string.Empty : r.StepId.ToString(),
        //            StepId = GetStepId(stepIdList, r.StepId.ToString()),
        //            StepSourceId = 000000000000000000000000,
        //            _updatedBy = r.UpdatedBy == null ? string.Empty : r.UpdatedBy.ToString(),
        //            UpdatedBy = Utils.GetUserId(r.UpdatedBy.ToString()),
        //            TTLDate = r.TTLDate ?? null,
        //            Version = Convert.ToInt32(r.Version)

        //        }).ToList();

        //    return lt;
        //}

        private static int GetStepId(Dictionary<string, int> dic, string p)
        {
            var stepid = 0;
            if (dic.ContainsKey(p))
            {
                stepid = dic[p];
            }
            return stepid;
        }
    }
}
