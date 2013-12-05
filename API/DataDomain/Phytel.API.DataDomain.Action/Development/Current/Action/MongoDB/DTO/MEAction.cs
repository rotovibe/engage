using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.Action.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty, }, TimeToLive = 0)]
    [MongoIndex(Keys = new string[] { NameProperty }, Unique = true)]
    public class MEAction : IMongoEntity<ObjectId>
    {
        public MEAction() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        public const string NameProperty = "n";
        public const string DescriptionProperty = "desc";
        public const string CompletedByProperty = "cby";
        public const string ObjectivesProperty = "oi";
        public const string StatusProperty = "st";

        public const string ExtraElementsProperty = "ex";
        public const string VersionProperty = "v";
        public const string UpdatedByProperty = "uby";
        public const string DeleteFlagProperty = "del";
        public const string TTLDateProperty = "ttl";
        public const string LastUpdatedOnProperty = "uon";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(NameProperty)]
        [BsonIgnoreIfNull(true)]
        public string Name { get; set; }

        [BsonElement(DescriptionProperty)]
        [BsonIgnoreIfNull(true)]
        public string Description { get; set; }

        [BsonElement(CompletedByProperty)]
        [BsonIgnoreIfNull(true)]
        public string CompletedBy { get; set; }

        [BsonElement(ObjectivesProperty)]
        [BsonIgnoreIfNull(true)]
        public List<ObjectiveInfo> ObjectivesInfo { get; set; }

        [BsonElement(StatusProperty)]
        [BsonIgnoreIfNull(true)]
        public Status Status { get; set; }

        [BsonElement(ExtraElementsProperty)]
        [BsonExtraElements()]
        [BsonIgnoreIfNull(true)]
        public Dictionary<string, object> ExtraElements { get; set; }

        [BsonElement(VersionProperty)]
        [BsonDefaultValue("v1")]
        public string Version { get; set; }

        [BsonElement(UpdatedByProperty)]
        [BsonDefaultValue("-100")]
        public string UpdatedBy { get; set; }

        [BsonElement(DeleteFlagProperty)]
        [BsonDefaultValue(false)]
        public bool DeleteFlag { get; set; }

        [BsonElement(TTLDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public System.DateTime? TTLDate { get; set; }

        [BsonElement(LastUpdatedOnProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public System.DateTime? LastUpdatedOn { get; set; }
    }

    public class ObjectiveInfo
    {
        public const string IDProperty = "_id";
        public const string ValueProperty = "vl";
        public const string MeasurementProperty = "ms";
        public const string StatusProperty = "st";

        [BsonElement(IDProperty)]
        public ObjectId ID { get; set; }

        [BsonElement(ValueProperty)]
        public string Value { get; set; }

        [BsonElement(MeasurementProperty)]
        public string Measurement { get; set; }

        [BsonElement(StatusProperty)]
        public Status Status { get; set; }
    }
}
