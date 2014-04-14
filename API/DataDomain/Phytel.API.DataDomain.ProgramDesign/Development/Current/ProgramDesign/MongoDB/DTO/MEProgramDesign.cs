using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Phytel.API.DataDomain.ProgramDesign.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    public class MEProgramDesign : IMongoEntity<ObjectId>, IMEEntity
    {
        public MEProgramDesign(string userId)
        {
            Id = ObjectId.GenerateNewId();
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = System.DateTime.UtcNow;
            Version = 1.0;
        }

        public const string IdProperty = "_id";

        #region Standard IMongoEntity Constants
        public const string ExtraElementsProperty = "ex";
        public const string VersionProperty = "v";
        public const string UpdatedByProperty = "uby";
        public const string DeleteFlagProperty = "del";
        public const string TTLDateProperty = "ttl";
        public const string LastUpdatedOnProperty = "uon";
        public const string RecordCreatedByProperty = "rcby";
        public const string RecordCreatedOnProperty = "rcon";
        #endregion

        [BsonId]
        public ObjectId Id { get; private set; }

        #region Standard IMongoEntity Implementation
        [BsonElement(ExtraElementsProperty)]
        [BsonExtraElements()]
        [BsonIgnoreIfNull(true)]
        public Dictionary<string, object> ExtraElements { get; set; }

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
