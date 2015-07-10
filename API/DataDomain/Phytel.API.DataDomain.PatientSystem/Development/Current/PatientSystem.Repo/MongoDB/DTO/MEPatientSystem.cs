using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Services.Mongo.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace Phytel.API.DataDomain.PatientSystem.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { PatientIdProperty }, Unique = false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    public class MEPatientSystem : IMongoEntity<ObjectId>, IMEEntity
    {
        public MEPatientSystem(string userId)
        {
            Id = ObjectId.GenerateNewId();
            Version = 1.0;
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = System.DateTime.UtcNow;
        }

        public const string IdProperty = "_id";
        public const string PatientIdProperty = "pid";
        public const string SystemSourceIdProperty = "ssid";
        public const string ValueProperty = "val";
        public const string StatusProperty = "sts";
        public const string PrimaryProperty = "pri";

        public const string VersionProperty = "v";
        public const string UpdatedByProperty = "uby";
        public const string DeleteFlagProperty = "del";
        public const string TTLDateProperty = "ttl";
        public const string LastUpdatedOnProperty = "uon";
        public const string RecordCreatedByProperty = "rcby";
        public const string RecordCreatedOnProperty = "rcon";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(PatientIdProperty)]
        [BsonIgnoreIfNull(false)]
        public ObjectId PatientId { get; set; }

        [BsonElement(SystemSourceIdProperty)]
        [BsonIgnoreIfNull(false)]
        public ObjectId SystemSourceId { get; set; }

        [BsonElement(ValueProperty)]
        [BsonIgnoreIfNull(false)]
        public string Value { get; set; }

        [BsonElement(StatusProperty)]
        [BsonIgnoreIfNull(false)]
        public Status Status { get; set; }

        [BsonElement(PrimaryProperty)]
        [BsonIgnoreIfNull(false)]
        public bool Primary { get; set; }

        #region BaseElements
        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }

        [BsonElement(VersionProperty)]
        [BsonDefaultValue(1.0)]
        public double Version { get; set; }

        [BsonElement(UpdatedByProperty)]
        public ObjectId? UpdatedBy { get; set; }

        [BsonElement(DeleteFlagProperty)]
        [BsonDefaultValue(false)]
        public bool DeleteFlag { get; set; }

        [BsonElement(TTLDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime? TTLDate { get; set; }

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
