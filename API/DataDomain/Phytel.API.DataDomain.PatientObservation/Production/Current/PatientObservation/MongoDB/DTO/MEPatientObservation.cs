using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Services.Mongo.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Phytel.API.DataDomain.PatientObservation.MongoDB.DTO;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.PatientObservation.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    [MongoIndex(Keys = new string[] { PatientIdProperty,ObservationIdProperty, DeleteFlagProperty, TTLDateProperty })]
    public class MEPatientObservation : IMongoEntity<ObjectId>, IMEEntity,IMEDataSource
    {
        public MEPatientObservation(string userId)
        {
            Id = ObjectId.GenerateNewId();
            Version = 1.0;
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = DateTime.UtcNow;
        }

        public const string RecordCreatedByProperty = "rcby";
        public const string RecordCreatedOnProperty = "rcon";

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

        public const string NumericValueProperty = "nval";
        [BsonElement(NumericValueProperty)]
        [BsonIgnoreIfNull(true)]
        public double? NumericValue { get; set; }

        public const string NonNumericValueProperty = "nnval";
        [BsonElement(NonNumericValueProperty)]
        [BsonIgnoreIfNull(true)]
        public string NonNumericValue { get; set; }

        public const string ObservationStateProperty = "st";
        [BsonElement(ObservationStateProperty)]
        [BsonIgnoreIfNull(true)]
        public ObservationState State { get; set; }

        public const string DisplayProperty = "dis";
        [BsonElement(DisplayProperty)]
        [BsonIgnoreIfNull(true)]
        public ObservationDisplay Display { get; set; }

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

        public const string DataSourceProperty = "dsrc";
        [BsonElement(DataSourceProperty)]
        [BsonIgnoreIfNull(true)]
        public string DataSource { get; set; }

        public const string ExternalRecordIdProperty = "extrid";
        [BsonElement(ExternalRecordIdProperty)]
        [BsonIgnoreIfNull(true)]
        public string ExternalRecordId { get; set; }


        #region Standard IMongoEntity Implementation
        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }

        public const string VersionProperty = "v";
        [BsonElement(VersionProperty)]
        [BsonDefaultValue(1.0)]
        public double Version { get; set; }

        public const string UpdatedByProperty = "uby";
        [BsonElement(UpdatedByProperty)]
        public ObjectId? UpdatedBy { get; set; }

        public const string DeleteFlagProperty = "del";
        [BsonElement(DeleteFlagProperty)]
        [BsonDefaultValue(false)]
        public bool DeleteFlag { get; set; }

        public const string TTLDateProperty = "ttl";
        [BsonElement(TTLDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime? TTLDate { get; set; }

        public const string LastUpdatedOnProperty = "uon";
        [BsonElement(LastUpdatedOnProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime? LastUpdatedOn { get; set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedByProperty)]
        public ObjectId RecordCreatedBy { get; private set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedOnProperty)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime RecordCreatedOn { get; private set; }
        #endregion

        
    }

}
