using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Services.Mongo.Linq;
using Phytel.API.Interface;
using System.Collections.Generic;
using System;

namespace Phytel.API.DataDomain.Cohort.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    [MongoIndex(Keys = new string[] { DeleteFlagProperty }, Unique = false)]
    public class MEPatientReferral : IMongoEntity<ObjectId>, IMEEntity
    {
        public MEPatientReferral(string userId)
        { 
            Id = ObjectId.GenerateNewId();
            Version = 1.0;
            RecordCreated = userId;
            RecordCreatedOn = DateTime.UtcNow;
        }

        public const string IdProperty = "_id";
        public const string ReferralIdProperty = "rid";
        public const string PatientIdProperty = "pid";
        public const string VersionProperty = "v";
        public const string UpdatedByProperty = "uby";
        public const string UpdatedProperty = "u";
        public const string DeleteFlagProperty = "del";
        public const string TTLDateProperty = "ttl";
        public const string LastUpdatedOnProperty = "uon";
        public const string RecordCreatedByProperty = "rcby";
        public const string RecordCreatedProperty = "rc";
        public const string RecordCreatedOnProperty = "rcon";

        [BsonId]
        [BsonElement(IdProperty)]
        public ObjectId Id { get; set; }

        [BsonId]
        [BsonElement(ReferralIdProperty)]
        public ObjectId ReferralId { get; set; }

        [BsonId]
        [BsonElement(PatientIdProperty)]
        public ObjectId PatientId { get; set; }
        
        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }

        [BsonElement(VersionProperty)]
        [BsonDefaultValue(1.0)]
        public double Version { get; set; }

        [BsonElement(UpdatedByProperty)]
        public ObjectId? UpdatedBy { get; set; }

        [BsonElement(UpdatedProperty)]
        [BsonIgnoreIfNull(true)]
        public string Updated { get; set; }

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

        [BsonElement(RecordCreatedProperty)]
        [BsonIgnoreIfNull(true)]
        public string RecordCreated { get; private set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedOnProperty)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime RecordCreatedOn { get; private set; }
    }
}
