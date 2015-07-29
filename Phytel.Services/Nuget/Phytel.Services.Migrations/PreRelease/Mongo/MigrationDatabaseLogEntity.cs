using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Services.Mongo.Linq;
using Phytel.Services.Versions;
using System;

namespace Phytel.Services.Migrations.Mongo
{
    [MongoCollection(CollectionName = "MigrationDatabaseLog")]
    [MongoIndex(Keys = new string[] { FieldNames.Product, FieldNames.MigrationVersionMajor, FieldNames.Database + "." + Database.FieldNames.Name })]
    [MongoIndex(Keys = new string[] { FieldNames.TTLDate }, TimeToLive = 0)]
    public class MigrationDatabaseLogEntity : ITransMongoEntity<ObjectId>
    {
        public ObjectId Id { get; set; }
        
        [BsonElement(FieldNames.MigrationVersion)]
        public SemanticVersion MigrationVersion { get; set; }
        
        [BsonElement(FieldNames.Product)]
        public string Product { get; set; }

        [BsonElement(FieldNames.Database)]
        public Database Database { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement(FieldNames.Error)]
        public Error Error { get; set; }

        [BsonElement(FieldNames.DeleteFlag)]
        public bool DeleteFlag { get; set; }

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

        [BsonElement(FieldNames.LastUpdatedBy)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? LastUpdatedBy { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [BsonElement(FieldNames.LastUpdatedOn)]
        [BsonIgnoreIfNull(true)]
        public DateTime? LastUpdatedOn { get; set; }

        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }

        public static class FieldNames
        {
            public const string Database = "db";
            public const string Error = "err";
            public const string DeleteFlag = "del";
            public const string RecordCreatedBy = "rcby";
            public const string RecordCreatedOn = "rcon";
            public const string TTLDate = "ttl";
            public const string Version = "v";
            public const string LastUpdatedBy = "uby";
            public const string LastUpdatedOn = "uon";
            public const string MigrationVersion = "mv";
            public const string MigrationVersionMajor = "mv.Major";
            public const string MigrationVersionMinor = "mv.Minor";
            public const string MigrationVersionPatch = "mv.Patch";
            public const string MigrationVersionStep = "mv.Step";
            public const string MigrationVersionStage = "mv.Stage";
            public const string Product = "product";
        }
    }
}