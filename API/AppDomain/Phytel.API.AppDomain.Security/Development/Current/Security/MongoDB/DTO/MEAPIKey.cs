using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;

namespace Phytel.API.AppDomain.Security
{
    [BsonIgnoreExtraElements(true)]
    [MongoIndex(Keys = new string[] { ApiKeyProperty, ProductProperty, IsActiveProperty })]
    public class MEAPIKey : IMongoEntity<ObjectId>
    {
        public MEAPIKey() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        public const string ApiKeyProperty = "apikey";
        public const string ProductProperty = "product";
        public const string IsActiveProperty = "isactive";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(ApiKeyProperty)]
        [BsonIgnoreIfNull(true)]
        public string ApiKey { get; set; }

        [BsonElement(ProductProperty)]
        [BsonIgnoreIfNull(true)]
        public string Product { get; set; }

        [BsonElement(IsActiveProperty)]
        [BsonIgnoreIfNull(true)]
        public bool IsActive { get; set; }
    }
}
