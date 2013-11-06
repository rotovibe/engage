using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;

namespace Phytel.API.AppDomain.Security
{
    [BsonIgnoreExtraElements(true)]
    [MongoIndex(Keys = new string[] { ApiKeyProperty, ProductProperty, IsActiveProperty })]
    public class MEAPIUser : IMongoEntity<ObjectId>
    {
        public MEAPIUser() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";

        public const string UserNameProperty = "un";
        public const string PasswordProperty = "pwd";

        public const string ApiKeyProperty = "apikey";
        public const string ProductProperty = "product";
        public const string IsActiveProperty = "isactive";
        public const string SessionTimeoutProperty = "timeout";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(UserNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string UserName { get; set; }

        [BsonElement(PasswordProperty)]
        [BsonIgnoreIfNull(true)]
        public string Password { get; set; }

        [BsonElement(ApiKeyProperty)]
        [BsonIgnoreIfNull(true)]
        public string ApiKey { get; set; }

        [BsonElement(ProductProperty)]
        [BsonIgnoreIfNull(true)]
        public string Product { get; set; }

        [BsonElement(IsActiveProperty)]
        [BsonIgnoreIfNull(true)]
        public bool IsActive { get; set; }

        [BsonElement(SessionTimeoutProperty)]
        [BsonDefaultValue(30)]
        public int SessionTimeout { get; set; }
        
    }
}
