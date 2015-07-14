using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using MongoDB.Bson;
using Phytel.API.DataDomain.PatientNote.Repo;

namespace Phytel.Data.ETL.BulkCopy
{
    public class ReadPatientNoteProgramSeries : ReadSeries
    {
        public IEnumerable<EPatientNoteProgram> ReadEPatientNoteProgramSeries(Dictionary<ObjectId, MEPatientNote> dic)
        {
            var enumerable = new ConcurrentBag<EPatientNoteProgram>();

            foreach (var n in dic)
            {
                enumerable.Add(new EPatientNoteProgram
                {
                    MongoId = n.Key.ToString(),
                    PatientNoteMongoId = n.Value.Id.ToString(),
                    RecordCreatedBy = n.Value.RecordCreatedBy.ToString(),
                    UpdatedBy = n.Value.UpdatedBy.ToString(),
                    RecordCreatedOn = n.Value.RecordCreatedOn,
                    LastUpdatedOn = n.Value.LastUpdatedOn,
                    Version = Convert.ToInt32(n.Value.Version)
                });
            }

            return enumerable;
        }
    }
}
