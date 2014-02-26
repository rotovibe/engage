using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Phytel.API.DataDomain.PatientObservation.MongoDB.DTO;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.PatientObservation.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    public class MEPatientObservation : IMongoEntity<ObjectId>, IMEEntity
    {
        public MEPatientObservation() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }

        public const string ObservationIdProperty = "oid";
        [BsonElement(ObservationIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId ObservationId { get; set; }

        public const string PatientIdProperty = "pid";
        [BsonElement(PatientIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId PatientId { get; set; }

        public const string ReferenceCodingIdProperty = "rcid";
        [BsonElement(ReferenceCodingIdProperty)]
        [BsonIgnoreIfNull(true)]
        public string ReferenceCodingId { get; set; }

        public const string NumericValueProperty = "nval";
        [BsonElement(NumericValueProperty)]
        [BsonIgnoreIfNull(true)]
        public float? NumericValue { get; set; }

        public const string NonNumericValueProperty = "nnval";
        [BsonElement(NonNumericValueProperty)]
        [BsonIgnoreIfNull(true)]
        public string NonNumericValue { get; set; }

        public const string ObservationStateProperty = "st";
        [BsonElement(ObservationStateProperty)]
        [BsonIgnoreIfNull(true)]
        public ObservationState State { get; set; }

        public const string StartDateProperty = "sd";
        [BsonElement(StartDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? StartDate { get; set; }

        public const string EndDateProperty = "ed";
        [BsonElement(EndDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? EndDate { get; set; }

        public const string TypeProperty = "type";
        [BsonElement(TypeProperty)]
        [BsonIgnoreIfNull(true)]
        public string Type { get; set; }

        public const string UnitsProperty = "u";
        [BsonElement(UnitsProperty)]
        [BsonIgnoreIfNull(true)]
        public string Units { get; set; }

        public const string AdministeredByProperty = "adminby";
        [BsonElement(AdministeredByProperty)]
        [BsonIgnoreIfNull(true)]
        public string AdministeredBy { get; set; }

        public const string SourceProperty = "src";
        [BsonElement(SourceProperty)]
        [BsonIgnoreIfNull(true)]
        public string Source { get; set; }

        #region Standard IMongoEntity Implementation
        public const string ExtraElementsProperty = "ex";
        [BsonElement(ExtraElementsProperty)]
        [BsonExtraElements()]
        [BsonIgnoreIfNull(true)]
        public Dictionary<string, object> ExtraElements { get; set; }

        public const string VersionProperty = "v";
        [BsonElement(VersionProperty)]
        [BsonDefaultValue("v1")]
        public string Version { get; set; }

        public const string UpdatedByProperty = "uby";
        [BsonElement(UpdatedByProperty)]
        [BsonDefaultValue("-100")]
        public string UpdatedBy { get; set; }

        public const string DeleteFlagProperty = "del";
        [BsonElement(DeleteFlagProperty)]
        [BsonDefaultValue(false)]
        public bool DeleteFlag { get; set; }

        public const string TTLDateProperty = "ttl";
        [BsonElement(TTLDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public System.DateTime? TTLDate { get; set; }

        public const string LastUpdatedOnProperty = "uon";
        [BsonElement(LastUpdatedOnProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public System.DateTime? LastUpdatedOn { get; set; }
        #endregion
    }
}
