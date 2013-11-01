using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;

namespace Phytel.API.AppDomain.Security
{
    [BsonIgnoreExtraElements(true)]
    [MongoIndex(Keys = new string[] { ProductProperty  })]
    public class MEAPISession : IMongoEntity<ObjectId>
    {
        [BsonId]
        public ObjectId Id { get; set; } // token

        public const string IndexProperty = "i";
        [BsonElement(IndexProperty)]
        [BsonIgnoreIfNull(true)]
        public string Index { get; set; }

        public const string ProductProperty = "prd";
        [BsonElement(ProductProperty)]
        [BsonIgnoreIfNull(true)]
        public string Product { get; set; }
    }
}
