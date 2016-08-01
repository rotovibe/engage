using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Services.Mongo.Linq;
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

        //for index
        public const string DescriptionProperty = "desc";
        public const string QuestionProperty = "q";

        public const string IdProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }

        public const string TypeProperty = "ty";
        [BsonElement(TypeProperty)]
        [BsonIgnoreIfNull(true)]
        //[BsonRequired]
        public StepType Type { get; set; }

        public const string StatusProperty = "st";
        [BsonElement(StatusProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDefaultValue(1.0)]
        public Status Status { get; set; }

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

    }

    public enum StepType
    {
        YesNo = 1,
        Text = 2,
        Input = 3,
        Single = 4,
        Multi = 5,
        Date = 6,
        Complete = 7,
        DateTime = 8,
        Time = 9,
        InputMultiline = 10
    }

    public enum Status
    {
        Active = 1,
        Inactive = 2,
        InReview = 3,
        Met = 4,
        NotMet = 5
    } 
}
