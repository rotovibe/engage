using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    [BsonIgnoreExtraElements(false)]
    public class MEProblem : IMongoEntity<ObjectId>
    {
        public MEProblem() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        public const string NameProperty = "n";
        public const string ActiveProperty = "a";
        public const string CodeSystemProperty = "cs";
        public const string CodeProperty = "c";
        public const string TypeProperty = "t";
        public const string DefaultLevelProperty = "dl";
        public const string DefaultFeaturedProperty = "df";
        public const string VersionProperty = "v";
        public const string ExtraElementsProperty = "ex";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(NameProperty)]
        [BsonIgnoreIfNull(true)]
        public string Name { get; set; }

        [BsonElement(ActiveProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDefaultValue(true)]
        public bool Active { get; set; }

        [BsonElement(CodeSystemProperty)]
        [BsonIgnoreIfNull(true)]
        public string CodeSystem { get; set; }

        [BsonElement(CodeProperty)]
        [BsonIgnoreIfNull(true)]
        public string Code { get; set; }

        [BsonElement(TypeProperty)]
        [BsonIgnoreIfNull(true)]
        public string Type { get; set; }

        [BsonElement(DefaultLevelProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDefaultValue(1)]
        public int? DefaultLevel { get; set; }

        [BsonElement(DefaultFeaturedProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDefaultValue(true)]
        public bool DefaultFeatured { get; set; }

        [BsonElement(VersionProperty)]
        [BsonDefaultValue("v1")]
        public string Version { get; set; }

        [BsonExtraElements]
        [BsonIgnoreIfNull(true)]
        [BsonElement(ExtraElementsProperty)]
        Dictionary<string, object> ExtraElements { get; set; }
    }
}