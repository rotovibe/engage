using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Services.Mongo.Linq;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class Problem : LookUpBase
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
        [BsonDefaultValue(1.0)]
        public int? DefaultLevel { get; set; }

        [BsonElement(DefaultFeaturedProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDefaultValue(true)]
        public bool DefaultFeatured { get; set; }
    }
}