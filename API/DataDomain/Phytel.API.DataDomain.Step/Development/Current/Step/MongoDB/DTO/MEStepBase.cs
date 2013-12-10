using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.Step.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty, }, TimeToLive = 0)]
    [MongoIndex(Keys = new string[] { TypeProperty, DescriptionProperty, QuestionProperty }, Unique = true)]
    public class MEStepBase : IMongoEntity<ObjectId>
    {
        public MEStepBase() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        public const string TypeProperty = "ty";
        public const string StatusProperty = "st";

        //for index
        public const string DescriptionProperty = "desc";
        public const string QuestionProperty = "q";

        public const string ExtraElementsProperty = "ex";
        public const string VersionProperty = "v";
        public const string UpdatedByProperty = "uby";
        public const string DeleteFlagProperty = "del";
        public const string TTLDateProperty = "ttl";
        public const string LastUpdatedOnProperty = "uon";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(TypeProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonRequired]
        public Type Type { get; set; }

        [BsonElement(StatusProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDefaultValue(1)]
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

    public enum Type
    { 
        YesNo  = 1,
        Text = 2
    }
}
