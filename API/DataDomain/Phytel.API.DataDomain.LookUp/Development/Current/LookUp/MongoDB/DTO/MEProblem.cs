using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    [BsonIgnoreExtraElements(false)]
    public class MEProblem : LookUpBase
    {
        public const string ActiveProperty = "act";
        public const string CodeSystemProperty = "cs";
        public const string CodeProperty = "c";
        public const string TypeProperty = "type";
        public const string DefaultLevelProperty = "dl";
        public const string DefaultFeaturedProperty = "df";

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
    }
}