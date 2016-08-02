using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Services.Mongo.Linq;
using System;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Program.MongoDB.DTO
{
    [MongoIndex(Keys = new string[] { ResponseBase.StepIdProperty }, Unique = false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    public class MEPatientProgramResponse : ResponseBase, IMongoEntity<ObjectId>
    {
        public MEPatientProgramResponse(string userId)
        {
            Id = ObjectId.GenerateNewId();
            Version = 1.0;
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = DateTime.UtcNow;
        }

        public const string IDProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }

        public const string ActionIdProperty = "actid";
        [BsonElement(ActionIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? ActionId { get; set; }

        #region standard
        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }

        public const string VersionProperty = "v";
        [BsonElement(VersionProperty)]
        [BsonDefaultValue(1.0)]
        public double Version { get; set; }

        public const string UpdatedByProperty = "uby";
        [BsonElement(UpdatedByProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? UpdatedBy { get; set; }

        public const string DeleteFlagProperty = "del";
        [BsonElement(DeleteFlagProperty)]
        [BsonDefaultValue(false)]
        public bool DeleteFlag { get; set; }

        public const string TTLDateProperty = "ttl";
        [BsonElement(TTLDateProperty)]
        [BsonDefaultValue(null)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public DateTime? TTLDate { get; set; }

        public const string LastUpdatedOnProperty = "uon";
        [BsonIgnoreIfNull(true)]
        [BsonElement(LastUpdatedOnProperty)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public DateTime? LastUpdatedOn { get; set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement("rcby")]
        public ObjectId RecordCreatedBy { get; set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement("rcon")]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public DateTime RecordCreatedOn { get; set; }
        #endregion
    }
}
