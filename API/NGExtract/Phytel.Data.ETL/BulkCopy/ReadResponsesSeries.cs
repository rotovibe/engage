using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program.MongoDB.DTO;

namespace Phytel.Data.ETL.BulkCopy
{
    public class ReadResponsesSeries : ReadSeries
    {
        public IEnumerable<EStepResponse> ReadEStepResponseSeries(List<MEPatientProgramResponse> list)
        {
            var enumerable = new ConcurrentBag<EStepResponse>();

            Parallel.ForEach(list, r =>
            {
                var stepIds = stepIdList;
                var userIds = userIdList;

                enumerable.Add(new EStepResponse
                {
                    MongoId = r.Id.ToString(),
                    MongoStepSourceId = r.StepSourceId.ToString(),
                    MongoActionId = r.ActionId.ToString(),
                    ActionId = 0, //get action id, // int
                    Delete = r.DeleteFlag.ToString(),
                    LastUpdatedOn = r.LastUpdatedOn,
                    MongoNextStepId = r.NextStepId == null ? string.Empty : r.NextStepId.ToString(),
                    NextStepId = GetId(stepIds, r.NextStepId.ToString()), // int
                    Nominal = r.Nominal.ToString(),
                    Order = r.Order.ToString(),
                    Text = r.Text ?? string.Empty,
                    Value = r.Value ?? string.Empty,
                    Required = r.Required.ToString(),
                    Selected = r.Selected.ToString(),
                    MongoRecordCreatedBy = r.RecordCreatedBy.ToString(),
                    RecordCreatedBy = GetId(userIds, r.RecordCreatedBy.ToString()), // int
                    RecordCreatedOn = r.RecordCreatedOn,
                    MongoStepId = r.StepId == null ? string.Empty : r.StepId.ToString(),
                    StepId = GetId(stepIds, r.StepId.ToString()), // int 
                    StepSourceId = 000000000000000000000000, //int
                    MongoUpdatedBy = r.UpdatedBy == null ? string.Empty : r.UpdatedBy.ToString(),
                    UpdatedBy = GetId(userIds, r.UpdatedBy.ToString()), // int
                    TTLDate = r.TTLDate ?? null,
                    Version = Convert.ToInt32(r.Version) // int
                });
            });

            return enumerable;
        }
    }
}
