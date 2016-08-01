using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientObservation.DTO;

namespace Phytel.Data.ETL.BulkCopy
{
    public class ReadObservationsSeries : ReadSeries
    {
        protected readonly Dictionary<string, int> stepIdList;
        protected readonly Dictionary<string, int> userIdList;
        protected readonly Dictionary<string, int> patientIdList;
        protected readonly Dictionary<string, int> observationIdList;

        public ReadObservationsSeries(string contract)
        {
            stepIdList = Utils.GetStepIdList(contract);
            userIdList = Utils.GetUserIdList(contract);
            patientIdList = Utils.GetPatientIdList(contract);
            observationIdList = Utils.GetObservationIdsList(contract);
        }

        public IEnumerable<EObservationResponse> ReadEObservationSeries(List<MEPatientObservation> list)
        {
            var enumerable = new ConcurrentBag<EObservationResponse>();

            Parallel.ForEach(list, r =>
            {
                var userIds = userIdList;

                enumerable.Add(new EObservationResponse
                {
                    MongoId = r.Id.ToString(),
                    MongoObservationId = r.ObservationId.ToString(),
                    AdministeredBy = r.AdministeredBy != null ? r.AdministeredBy.ToString() : null,
                    Display = r.Display == null ? string.Empty : r.Display.ToString(),
                    EndDate = r.EndDate ?? null,
                    MongoPatientId = r.PatientId == null ? null : r.PatientId.ToString(),
                    PatientId = r.PatientId != null ? GetId(patientIdList, r.PatientId.ToString()) : 0 ,
                    NonNumericValue = r.NonNumericValue ,
                    NumericValue = Convert.ToDecimal(r.NumericValue) ,
                    ObservationId = r.ObservationId != null ? GetId(observationIdList, r.ObservationId.ToString()) : 0,
                    Source = r.Source,
                    StartDate = r.StartDate ?? null,
                    State = r.State == null ? null : r.State.ToString(),
                    Type = r.Type,
                    Units = r.Units,
                    MongoRecordCreatedBy = r.RecordCreatedBy == null ? null : r.RecordCreatedBy.ToString(),
                    RecordCreatedById = r.RecordCreatedBy != null ? GetId(userIds, r.RecordCreatedBy.ToString()) : 0, // int
                    RecordCreatedOn = r.RecordCreatedOn,
                    Delete = r.DeleteFlag.ToString(),
                    LastUpdatedOn = r.LastUpdatedOn,
                    MongoUpdatedBy = r.UpdatedBy == null ? string.Empty : r.UpdatedBy.ToString(),
                    UpdatedById = GetId(userIds, r.UpdatedBy.ToString()), // int
                    TTLDate = r.TTLDate ?? null,
                    Version = Convert.ToInt32(r.Version) // int
                });
            });

            return enumerable;
        }
    }
}
