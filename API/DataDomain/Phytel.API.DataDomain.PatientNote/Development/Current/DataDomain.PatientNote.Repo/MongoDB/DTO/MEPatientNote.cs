using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Services.Mongo.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Phytel.API.DataDomain.PatientNote.Repo
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    [MongoIndex(Keys = new string[] { PatientIdProperty, DeleteFlagProperty }, Unique = false)]
    public class MEPatientNote : IMongoEntity<ObjectId>, IMEEntity
    {
        public MEPatientNote(string userId, DateTime? createdOn)
        {
            Id = ObjectId.GenerateNewId();
            Version = 1.0;
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = createdOn == null || createdOn.Equals(new DateTime()) ? DateTime.UtcNow : (DateTime)createdOn;
        }

        public const string IdProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }

        public const string PatientIdProperty = "pid";
        [BsonElement(PatientIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId PatientId { get; set; }

        public const string ExternalRecordIdProperty = "extrid";
        [BsonElement(ExternalRecordIdProperty)]
        [BsonIgnoreIfNull(true)]
        public string ExternalRecordId { get; set; }

        public const string TextProperty = "txt";
        [BsonElement(TextProperty)]
        [BsonIgnoreIfNull(true)]
        public string Text { get; set; }

        public const string ProgramProperty = "prog";
        [BsonElement(ProgramProperty)]
        [BsonIgnoreIfNull(true)]
        public List<ObjectId> ProgramIds { get; set; }

        public const string NoteTypeProperty = "type";
        [BsonElement(NoteTypeProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId Type { get; set; }

        public const string MethodIdProperty = "mid";
        [BsonElement(MethodIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? MethodId { get; set; }

        public const string WhoIdProperty = "wid";
        [BsonElement(WhoIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? WhoId { get; set; }

        public const string SourceIdProperty = "srcid";
        [BsonElement(SourceIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? SourceId { get; set; }

        public const string OutcomeIdProperty = "oid";
        [BsonElement(OutcomeIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? OutcomeId { get; set; }

        public const string DurationIdProperty = "did";
        [BsonElement(DurationIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? DurationId { get; set; }

        public const string DurationProperty = "dur";
        [BsonElement(DurationProperty)]
        [BsonIgnoreIfNull(true)]
        public int? Duration { get; set; }

        public const string ContactedOnProperty = "con";
        [BsonElement(ContactedOnProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? ContactedOn { get; set; }

        public const string ValidatedIdentityProperty = "vi";
        [BsonElement(ValidatedIdentityProperty)]
        [BsonIgnoreIfNull(true)]
        public bool ValidatedIdentity { get; set; }

        public const string DataSourceProperty = "dsrc";
        [BsonElement(DataSourceProperty)]
        [BsonIgnoreIfNull(true)]
        public string DataSource { get; set; }

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

        public const string RecordCreatedByProperty = "rcby";
        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedByProperty)]
        public ObjectId RecordCreatedBy { get; private set; }

        public const string RecordCreatedOnProperty = "rcon";
        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedOnProperty)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime RecordCreatedOn { get; private set; }
        #endregion
    }
}
