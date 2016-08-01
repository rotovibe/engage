using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Services.Mongo.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    [MongoIndex(Keys = new string[] { DeleteFlagProperty }, Unique = false)]
    public class MEGoal : GoalBase, IMongoEntity<ObjectId>, IMEEntity
    {
        public MEGoal(string userId)
        {
            Id = ObjectId.GenerateNewId();
            Version = 1.0;
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = DateTime.UtcNow;
        }

        public const string IdProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }
        
        public const string FocusAreaProperty = "focs";
        [BsonElement(FocusAreaProperty)]
        [BsonIgnoreIfNull(true)]
        public List<ObjectId> FocusAreaIds { get; set; }

        public const string SourceProperty = "src";        
        [BsonElement(SourceProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? SourceId { get; set; }

        public const string TypeProperty = "type";
        [BsonElement(TypeProperty)]
        [BsonIgnoreIfNull(true)]
        public GoalType Type { get; set; }

        public const string StatusProperty = "sts";
        [BsonElement(StatusProperty)]
        [BsonIgnoreIfNull(true)]
        public GoalTaskStatus Status { get; set; }

        public const string EndDateProperty = "ed";
        [BsonElement(EndDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? EndDate { get; set; }

        public const string TargetValueProperty = "tv";
        [BsonElement(TargetValueProperty)]
        [BsonIgnoreIfNull(true)]
        public string TargetValue { get; set; }

        public const string TargetDateProperty = "td";
        [BsonElement(TargetDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? TargetDate { get; set; }

        public const string TargetDateRangeProperty = "tdr";
        [BsonElement(TargetDateRangeProperty)]
        [BsonIgnoreIfNull(true)]
        public int TargetDateRange { get; set; }

        public const string AttributesProperty = "attr";        
        [BsonElement(AttributesProperty)]
        [BsonIgnoreIfNull(false)]
        public List<MAttribute> Attributes { get; set; }

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
