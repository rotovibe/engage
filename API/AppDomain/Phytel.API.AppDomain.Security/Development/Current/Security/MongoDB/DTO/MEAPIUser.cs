using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;

namespace Phytel.API.DataDomain.Security
{
    public class MEAPIUser : IMongoEntity<ObjectId>
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public const string IndexProperty = "i";
        [BsonElement(IndexProperty)]
        [BsonIgnoreIfNull(true)]
        public string Index { get; set; }

        [BsonElement("usr")]
        [BsonIgnoreIfNull(true)]
        public string UserName { get; set; }

        [BsonElement("pw")]
        [BsonIgnoreIfNull(true)]
        public string Password { get; set; }

        [BsonElement("apikey")]
        [BsonIgnoreIfNull(true)]
        public string ApiKey { get; set; }

        [BsonElement("prd")]
        [BsonIgnoreIfNull(true)]
        public string Product { get; set; }
    }
}
