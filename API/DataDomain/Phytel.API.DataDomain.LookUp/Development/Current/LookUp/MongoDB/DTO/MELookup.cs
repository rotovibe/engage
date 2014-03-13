using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;
using System.Collections.Generic;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    [MongoIndex(Keys = new string[] { TypeProperty, DeleteFlagProperty }, Unique = true)]
    public class MELookup : IMongoEntity<ObjectId>, IMEEntity
    {
        public MELookup(string userId)
        {
            Id = ObjectId.GenerateNewId();
            Version = 1.0;
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = System.DateTime.UtcNow;
        }

        public const string IdProperty = "_id";
        public const string TypeProperty = "type";
        public const string DataProperty = "d";

        public const string ExtraElementsProperty = "ex";
        public const string VersionProperty = "v";
        public const string UpdatedByProperty = "uby";
        public const string DeleteFlagProperty = "del";
        public const string TTLDateProperty = "ttl";
        public const string LastUpdatedOnProperty = "uon";
        public const string RecordCreatedByProperty = "rcby";
        public const string RecordCreatedOnProperty = "rcon";

        [BsonId]
        public ObjectId Id { get; set; }
    
        [BsonElement(TypeProperty)]
        [BsonIgnoreIfNull(true)]
        public LookUpType Type { get; set; }

        [BsonElement(DataProperty)]
        [BsonIgnoreIfNull(true)]
        public List<LookUpBase> Data { get; set; }

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

        [BsonElement(RecordCreatedByProperty)]
        public ObjectId RecordCreatedBy { get; set; }

        [BsonElement(RecordCreatedOnProperty)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime RecordCreatedOn { get; set; }
    }

    public enum LookUpType
    { 
        Problem = 1,
        Objective = 2,
        ObjectiveCategory = 3,
        CommMode = 4,
        CommType = 5,
        State = 6,
        TimesOfDay = 7,
        TimeZone = 8,
        Language = 9,
        FocusArea = 10,
        Source = 11,
        BarrierCategory = 12,
        InterventionCategory = 13,
        ObservationType = 14,
        CareMemberType = 15,
        CodingSystem = 16
    }
}