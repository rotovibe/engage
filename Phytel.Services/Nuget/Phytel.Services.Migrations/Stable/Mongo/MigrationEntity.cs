using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Services.Mongo.Linq;
using Phytel.Services.Versions;
using System;

namespace Phytel.Services.Migrations.Mongo
{
    [MongoCollection(CollectionName = "MigrationLog")]
    [MongoIndex(Keys = new string[] { FieldNames.Product, FieldNames.MigrationVersion }, Unique=true)]
    [MongoIndex(Keys = new string[] { FieldNames.TTLDate }, TimeToLive = 0)]
    public class MigrationLogEntity : ITransMongoEntity<ObjectId>
    {
        [BsonElement(FieldNames.DeleteFlag)]
        public bool DeleteFlag { get; set; }

        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }

        public ObjectId Id { get; set; }

        [BsonElement(FieldNames.LastUpdatedBy)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? LastUpdatedBy { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonElement(FieldNames.LastUpdatedOn)]
        [BsonIgnoreIfNull(true)]
        public DateTime? LastUpdatedOn { get; set; }

        [BsonElement(FieldNames.MigrationVersion)]
        public SemanticVersion MigrationVersion { get; set; }

        [BsonElement(FieldNames.Product)]
        public string Product { get; set; }

        [BsonElement(FieldNames.RecordCreatedBy)]
        [BsonIgnoreIfNull(true)]
        public ObjectId RecordCreatedBy { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonElement(FieldNames.RecordCreatedOn)]
        [BsonIgnoreIfNull(true)]
        public DateTime RecordCreatedOn { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonElement(FieldNames.TTLDate)]
        [BsonIgnoreIfNull(true)]
        public DateTime? TTLDate { get; set; }

        [BsonElement(FieldNames.Version)]
        public double Version { get; set; }

        public static class FieldNames
        {
            public const string MigrationVersion = "mv";
            public const string Product = "product";
            public const string TTLDate = "ttl";
            public const string Version = "v";
            public const string RecordCreatedOn = "rcon";
            public const string RecordCreatedBy = "rcby";
            public const string LastUpdatedOn = "uon";
            public const string LastUpdatedBy = "uby";
            public const string DeleteFlag = "del";

        }
    }
}