using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class AllergyType : LookUpBase
    {
        public const string ActiveProperty = "act";
        public const string CodeSystemProperty = "cs";
        public const string CodeProperty = "c";

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
    }
}