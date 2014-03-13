using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using Phytel.API.Common;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Action.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty, }, TimeToLive = 0)]
    public class MEAction : IMongoEntity<ObjectId>, IMEEntity
    {
        public MEAction(string userId)
        {
            Id = ObjectId.GenerateNewId();
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = System.DateTime.UtcNow;
            Version = 1.0;
        }

        public const string IdProperty = "_id";
        public const string NameProperty = "nm";
        public const string DescriptionProperty = "desc";
        public const string CompletedByProperty = "cby";
        public const string ObjectivesProperty = "obj";
        public const string StatusProperty = "sts";

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
        public ObjectId CompletedBy { get; set; }

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

        [BsonElement("rcby")]
        public ObjectId RecordCreatedBy { get; set; }

        [BsonElement("rcon")]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime RecordCreatedOn { get; set; }
    }

    public class ObjectiveInfo
    {
        public const string IDProperty = "_id";
        public const string ValueProperty = "val";
        public const string MeasurementProperty = "m";
        public const string StatusProperty = "sts";

        [BsonElement(IDProperty)]
        public ObjectId Id { get; set; }

        [BsonElement(ValueProperty)]
        public string Value { get; set; }

        [BsonElement(MeasurementProperty)]
        public string Measurement { get; set; }

        [BsonElement(StatusProperty)]
        public Status Status { get; set; }
    }
}
