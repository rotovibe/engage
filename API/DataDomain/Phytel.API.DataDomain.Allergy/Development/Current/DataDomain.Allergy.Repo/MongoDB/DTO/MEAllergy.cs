using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;

namespace Phytel.API.DataDomain.Allergy.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { IdProperty, DescriptionProperty, TTLDateProperty }, TimeToLive = 0)]
    public class MEAllergy : IMongoEntity<ObjectId>, IMEEntity
    {
        public MEAllergy(string userId)
        {
            Id = ObjectId.GenerateNewId();
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = System.DateTime.UtcNow;
            Version = 1.0;
        }

        public const string IdProperty = "_id";
        [BsonId]
        public ObjectId Id { get; private set; }

        public const string VersionProperty = "v";
        [BsonElement(VersionProperty)]
        [BsonDefaultValue(1.0)]
        public double Version { get; set; }

        public const string SubTypeProperty = "subtype";
        [BsonElement(SubTypeProperty)]
        public List<ObjectId> SubType { get; set; }

        public const string DescriptionProperty = "desc";
        [BsonElement(DescriptionProperty)]
        public string Description { get; set; }

        public const string CodingSystemProperty = "csid";
        [BsonElement(CodingSystemProperty)]        
        public ObjectId CodingSystem { get; set; }

        public const string CodingSystemCodeProperty = "csc";
        [BsonElement(CodingSystemCodeProperty)]        
        public string CodingSystemCode { get; set; }

        public const string StatusProperty = "sts";
        [BsonElement(StatusProperty)]
        public int Status { get; set; }

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

        public const string ExtraElementsProperty = "ex";
        [BsonElement(ExtraElementsProperty)]
        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }
    }
}
