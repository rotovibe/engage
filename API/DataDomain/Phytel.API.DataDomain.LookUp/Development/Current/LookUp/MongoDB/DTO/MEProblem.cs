using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    [BsonIgnoreExtraElements(true)]
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

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(NameProperty)]
        [BsonIgnoreIfNull(true)]
        public string Name { get; set; }

        [BsonElement(ActiveProperty)]
        [BsonIgnoreIfNull(true)]
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
        public int DefaultLevel { get; set; }

        [BsonElement(DefaultFeaturedProperty)]
        [BsonIgnoreIfNull(true)]
        public bool DefaultFeatured { get; set; }
    }
}